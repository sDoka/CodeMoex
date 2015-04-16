using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CodeMoex
{
    public partial class Form1 : Form
    {
        //допилить всякие проверки
        int minS, maxB, price;
        Zayavka z; public  static List<Zayavka> zlB, zlS;//zlB, zlS -исходные
        Deal d; List<Deal> dD;
        long dvolume = 0, dvalue = 0;//Объём и стоимость сделки
        Stopwatch sw = new Stopwatch();

        string input = "";
        string output = "";
        public Form1()
        {
            InitializeComponent();
        }




        private void ok_btn_Click(object sender, EventArgs e)
        {
            sw.Start();

                this.read_BSsort();
                this.ml_sort();
                this.check_price();
                this.deal();
            

            sw.Stop();
            ll2.Text = ("Время выполнения "+sw.Elapsed.ToString("mm\\:ss\\.ff")+"");
            ll1.Text = ("Цена "+price.ToString()+"");
            this.SaveFileAs();
            sw.Reset();
            zlB.Clear();
            zlS.Clear();
        }



        public void read_BSsort()
        {
            //сразу при считывании записываем заявки в два списка BUY & SELL
            zlB = new List<Zayavka>();//список заявок на покупку
            zlS = new List<Zayavka>();//список заявок на продажу

           
            
            try
            {
                StreamReader sr = new StreamReader(input);
                string[] header = sr.ReadLine().Split(';', ',', '/', '|');//заголовок (нах не нужен)
                if (header.Length < 4)
                    return;

                //загоняем наши заявки в список
                while (!sr.EndOfStream)//пока не конец файла 
                {
                    try
                    {
                        string[] param = sr.ReadLine().Split(';', ',', '/', '|');
                        if (Convert.ToChar(param[1]) == 66)//если заявка на Buy
                        {
                            z = new Zayavka();
                            z.Read(param);
                            zlB.Add(z);//добавляем в список на покупку
                        }
                        else if ((Convert.ToChar(param[1]) == 83))
                        {
                            z = new Zayavka();
                            z.Read(param);
                            zlS.Add(z);//добавляем в список на продажу
                        }
                    }
                    catch//параллельно отсеиваем все косячные заявки
                    {
                    }
                   
                }
                sr.Dispose();
                sr.Close();
            }
            catch
            {
                MessageBox.Show("Input error ", "Lol");
                this.SaveFileAsFailed();
                Application.Exit();


            }
           
        }


        public void ml_sort()
        {
            try
            {   //в дальнейем при необходимости, можно запилить пользовательскую сортировки по типу поразрядной

                Work w = new Work(zlB, zlB);
                Thread t1 = new Thread(w.try_sortB);
                Work s = new Work(zlS, zlS);
                Thread t2 = new Thread(s.try_sortS);
                t1.Start();
                t2.Start();
                t1.Join();
                t2.Join();
                /*
                zlB = zlB.OrderByDescending(x => x.lm).ThenByDescending(x => x.price).ToList();//для покупки, сначала маркет, потом лимит и по убыванию цены
                zlS = zlS.OrderByDescending(x => x.lm).ThenBy(x => x.price).ToList();//для продажи, сначала маркет, потом лимит и по возрастанию цены  */
            }
            catch
            {

                MessageBox.Show("ML sorting problem ", "Lol");
                this.SaveFileAsFailed();
                Application.Exit();
            }
 

        }
        public void check_price()
        {
           

            //определяем диапазон цены. Это от minS до maxB
            //minS- первая лимитная заявка в zlS1, maxB - первая лимитная заявка в zlB1
            try
            {
                //вытаскиваем диапазон цен
                maxB = zlB.Find(x => x.lm == 76).price;//ищем первую лимитную заявку в Buy и вытаскиваем цену
                minS = zlS.Find(x => x.lm == 76).price; //ищем первую лимитную заявку в Sell
                if (minS > maxB)//если цены не пересекаются
                {
                    StreamWriter writer = new StreamWriter(output);
                    this.SaveFileAsFailed();
                    MessageBox.Show("Нет пересечения цен", "Lol");
                    Application.Exit();

                }
                //создаём списки для расчёта цены аукциона. Без маркета
                List<Zayavka> zlB1 = new List<Zayavka>();//новый пустой список
                List<Zayavka> zlS1 = new List<Zayavka>();

                //Добавляем в списки новые заявки, а не ссылки на старые

                Work w = new Work(zlB1, zlB);
                Thread t1 = new Thread(w.try_copy);
                Work s = new Work(zlS1, zlS);
                Thread t2 = new Thread(s.try_copy);
                t1.Start();
                t2.Start();
                t1.Join();
                t2.Join();
                //удаляем глобально жадные заявки
                zlB1.RemoveAll(x => x.price < minS);
                zlS1.RemoveAll(x => x.price > maxB);

                //Учитываются заявки маркета при подсчёте стоимости для выбора цены?????? сказали, что не учитываются
                long sMvolume = zlS.FindAll(x=>x.lm ==77).Sum(x => Convert.ToInt64(x.volume));//Объём маркет заявок на продажу
                 long bMvolume = zlB.FindAll(x => x.lm == 77).Sum(x => Convert.ToInt64(x.volume));//объём маркет сделок на покупку
                 long mValue = 0;//объём маркет сделок.
                if (sMvolume < bMvolume)
                     mValue = sMvolume;
                 else  
                     mValue = bMvolume;
                 
                //создать копии сортированных списков и сложить все заявки с одинаковыми ценами
                //чтобы подсчитать цену для максимальной стоимости
                List<Zayavka> a = new List<Zayavka>();
                List<Zayavka> b = new List<Zayavka>();

                //схлапываем списки
                //складываем объёмы с одинаковой ценой---------------------------------------------------------------------
                int k = 0, j = 0;
                while (k != zlB1.Count)//для списка buy
                {

                    if (k != zlB1.Count - 1 && zlB1[k].price == zlB1[k + 1].price)
                    {
                        zlB1[k + 1].volume = zlB1[k].volume + zlB1[k + 1].volume;
                        k++;
                    }
                    else
                    {
                        a.Add(zlB1[k]);//записываем в наш массив расчёта цены по Buy
                        k++;
                    }
                }

                while (j != zlS1.Count)//для списка buy
                {

                    if (j != zlS1.Count - 1 && zlS1[j].price == zlS1[j + 1].price)
                    {
                        zlS1[j + 1].volume = zlS1[j].volume + zlS1[j + 1].volume;//если цена этой заявки, равна цене следующей
                        j++;
                    }
                    else
                    {
                        b.Add(zlS1[j]);//записываем в наш массив расчёта цены по Sell
                        j++;
                    }
                }
                //определяем какие цены у нас есть, чтоб не брать все цены диапазона
                zlB1.Clear();
                zlS1.Clear();
                //создаем список имеющихся цен, чтобы не перебирать весь диапазон
                List<int> test = new List<int>();
                for (int i = 0; i < a.Count; i ++)
                    test.Add(a[i].price);
                for (int i = 0; i < b.Count ; i++)
                    test.Add(b[i].price);
                    

                
                test = test.Distinct().ToList();
                price = 0;
                List<KeyValuePair<int, long>> kvpl = new List<KeyValuePair<int, long>>();//для записи цена-стоимость

                for (j = test.Count - 1; j >= 0; j--)//наконец цикл вычисления цены, тупой перебор. Берём цены из пересечения и идем вверх по цене
                //подсчитывая итоговые стоимости
                {//берём первую цену из селл
                    price = test[j];
                    dvalue = 0;
                    //снова создаём независимые копии списков
                    //Добавляем в списки новые заявки, а не ссылки на старые
                    //копируем значения из схлопнутых списков в тестировочные
                    for (int i = 0; i < a.Count; i++)
                        zlB1.Add(a[i].copy());

                    for (int i = 0; i < b.Count; i++)
                        zlS1.Add(b[i].copy());
                    //отсеять все местные жадные заявки ( создать по новой списки)
                    zlB1.RemoveAll(x => x.price < price);// buy меньше цены - нахрен
                    zlS1.RemoveAll(x => x.price > price);//sell больше цены - нахрен
                    int c = 0, f = 0;//счётчики внутри цикла K-buy, f -sell----------------------------------проверить, что volume != 0
                    while (zlB1.Count != c && zlS1.Count != f)//пока не кончится один из списков
                    {
                        //прогоняем по ней аукцион, записываем получившуюся стоимость
                        //БЕЗ МАРКЕТОВ!!!
                        if (zlB1[c].volume < zlS1[f].volume)//если объём buy<sell
                        {
                            //вернуть оставшуются часть заявки в рынок
                            zlS1[f].volume = zlS1[f].volume - zlB1[c].volume;

                            dvalue = dvalue + zlB1[c].volume * price;
                            //убрать исполненную заявку
                            c++;
                        }
                        else if (zlB1[c].volume > zlS1[f].volume)//если объём buy>sell
                        {
                            //вернуть оставшуются часть заявки в рынок
                            zlB1[c].volume = zlB1[c].volume - zlS1[f].volume;//найти другой способ передачи по значению

                            dvalue = dvalue + zlS1[f].volume * price;
                            //убрать исполненную заявку
                            f++;
                        }
                        else//если равны
                        {
                            dvalue = dvalue + zlB1[c].volume * price;
                            //убрать исполненные заявки
                            k++;
                            f++;
                        }
                        //заносим значение цены и стоимости в список            
                    }
                    dvalue += mValue * price;
                    kvpl.Add(new KeyValuePair<int, long>(price, dvalue));
                    zlB1.Clear();
                    zlS1.Clear();

                }
                //теперь надо отсортировать получившийся массив и найти наибольшее значение стоимости dvalue

                kvpl = kvpl.OrderByDescending(x => x.Value).ToList();
                //берём первое значение из листа и делаем его ключ - ценой аукциона
                price = kvpl[0].Key;
                //после определения цены возвращаемся к исходным отсортированным спискам
                zlB.RemoveAll(x => (x.price < price && x.lm != 77));//убираем все Buy ниже цены
                zlS.RemoveAll(x => (x.price > price && x.lm != 77));//удалить все лимитные заявки Sell выше цены
                ll2.Text = ("Цена - " + price.ToString() + "");
            }
            catch
            {
                MessageBox.Show("Checking price problem", "Lol");
                this.SaveFileAsFailed();
                Application.Exit();
            }

           
        }



        public void deal()
        {
            try
            {

            
            int i = 0, j = 0;//счётчики
            //здесь нам надо взять исходные отсортированные листы и загнать в них цену
            dvalue = 0;
            dvolume = 0;
            dD = new List<Deal>();
            // price = 100;
            //Здесь нам необходимо свести заявки по очерёдно
            //и сформировать список в выходной файл
            while (zlB.Count != i && zlS.Count != j)//пока не кончится один из списков
            {
                if (zlB[i].volume < zlS[j].volume)//если объём buy<sell
                {
                    d = new Deal(zlB[i].num, zlS[j].num, zlB[i].volume, zlB[i].volume * price);//регистриуем сделку


                    //вернуть оставшуются часть заявки в рынок
                    zlS[j].volume = zlS[j].volume - zlB[i].volume;

                    dvalue = dvalue + zlB[i].volume * price;
                    dvolume = dvolume + zlB[i].volume;
                    //убрать исполненную заявку
                    i++;

                }
                else if (zlB[i].volume > zlS[j].volume)//если объём buy>sell
                {
                    d = new Deal(zlB[i].num, zlS[j].num, zlS[j].volume, zlS[j].volume * price);//регистрруем сделку
                    //вернуть оставшуются часть заявки в рынок
                    zlB[i].volume = zlB[i].volume - zlS[j].volume;
                    dvalue = dvalue + zlS[j].volume * price;
                    dvolume = dvolume + zlS[j].volume;
                    //убрать исполненную заявку
                    j++;

                }
                else//если равны
                {
                    d = new Deal(zlB[i].num, zlS[j].num, zlB[i].volume, zlB[i].volume * price);//регистрруем сделку
                    dvalue = dvalue + zlB[i].volume * price;
                    dvolume = dvolume + zlB[i].volume;
                    //убрать исполненные заявки
                    i++;
                    j++;


                }
                dD.Add(d);//заносим сделку в реестр
            }
            }
        catch
            {
                MessageBox.Show("Deal Problem", "Lol");
                this.SaveFileAsFailed();
                Application.Exit();
            }
    
        }


        public void write2file(string output)
        {

            StreamWriter writer = new StreamWriter(output);
            // вывод финального списка сведённых заявок в файл

            writer.WriteLine("OK;" + price.ToString() + ";" + dvalue.ToString() + "");
            for (int i = 0; i < dD.Count ; i++)
            {
                writer.WriteLine("" + dD[i].buyOrderNo.ToString() + ";" + dD[i].sellOrderNo.ToString() + ";" + dD[i].volume.ToString() + ";" + dD[i].value.ToString() + "");
            }

            writer.Close();
        }

        public void writeFailed (string output)
        {
            StreamWriter writer = new StreamWriter(output);
            writer.Write("FAILED");
            writer.Close();
        }

       

        private void search_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.FileName = "";
            sfd.Title = "Выберите файл";
            sfd.DefaultExt = "csv";
            sfd.Filter = "Файлы аукциона | *.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
                input = sfd.FileName.ToString();
            fileUP.Text = "Файл загружен";
                
        }
        private void SaveFileAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";

            sfd.DefaultExt = ".csv";
            sfd.Filter = "Файлы аукциона (*.csv)| *.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                
                write2file(sfd.FileName);
                MessageBox.Show("Файл сохранен", "Всё ок =)");
            }
                
               
        }
        private void SaveFileAsFailed()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";

            sfd.DefaultExt = ".csv";
            sfd.Filter = "Файлы аукциона (*.csv)| *.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {

                writeFailed(sfd.FileName);
                MessageBox.Show("Auction failed", "File saved");
            }
            else
            {
                Application.Exit();
            }


        }




      

        private void Close_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

 







    }
}
