using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMoex
{
   public partial class Zayavka
    {

        public int num;//номер строки
        public char bs;//buy/sell
        public char lm;//limit/market
        public int volume;// объём сделки
        public int price;// цена за единицу объёма





        public Zayavka Read(string[] args)//входной параметр - массив строк
        {
            Zayavka p = new Zayavka();
            num = Convert.ToInt32(args[0]);// подразумевается, что с номером то уж не накосячат
            //обезопасить бс и лм от кривых данных------------------------------------------------------------------------------------------------
            bs = Convert.ToChar(args[1]);// проверка еще при считывании
            if ((Convert.ToChar(args[2]) == 76 || Convert.ToChar(args[2]) == 77))
            {
                lm = Convert.ToChar(args[2]);
            }
            else//если не LM тогда выкидвыаем
            {
                lm = Convert.ToChar(-1);
            }
            if ((Convert.ToInt32(args[3]) < 0) || (Convert.ToInt32(args[4]) < 0))//если отрицательные числа в цене или объёме
                lm = Convert.ToChar(-1);
            volume = Convert.ToInt32(args[3]);
            price = Convert.ToInt32(args[4]);
            return p;
        }
       public  Zayavka copy()
        {
            Zayavka p = new Zayavka();
            p.num = num;
            p.bs = bs;
            p.lm = lm;
            p.price = price;
            p.volume = volume;

            return p;
        }
    }

   struct Deal
   {
       public int buyOrderNo;//номер заявки на покупку
       public int sellOrderNo;//номер заявки на продажу
       public int volume;//объём сделки
       public int value;//стоимость сделки (объём*цена)


       public Deal(int buy, int sell, int vol, int val)
       {
           buyOrderNo = buy;
           sellOrderNo = sell;
           volume = vol;
           value = val;
       }
   }

   public partial class Work//класс для распараллеивания копирования листов
   {
       private List<Zayavka> toList;
       private List<Zayavka> fromList;

       public Work(List<Zayavka> toList, List<Zayavka> fromList)
       {
           this.toList = toList;
           this.fromList = fromList;
       }


       public void try_copy()// копируем из листа в лист по зачениям и удаляем маркеты
       {

           for (int i = 0; i < fromList.Count; i++)
               toList.Add(fromList[i].copy());
           toList.RemoveAll(x => x.lm == 77);


       }
         public void try_sortB()//сортировка списка по цене и маркету/лимиту для покупки
         {
             List<Zayavka> zlB = Form1.zlB;
              zlB = zlB.OrderByDescending(x => x.lm).ToList();// так, сначала маркет/лимит
             List<Zayavka> zlbM = new List<Zayavka>();
             zlbM.AddRange(zlB.FindAll(x => x.lm == 77).ToList());//Записываем маркет заявки в один список
             List<Zayavka> zlBL = new List<Zayavka>();
             zlBL.AddRange(zlB.FindAll(x => x.lm == 76).ToList());//Записываем лимит заявки в дпугой список
             //сортируем лимит заявки по цене, потом по номеру
             zlBL = zlBL.OrderByDescending(x => x.price).ThenBy(x => x.num).ToList();//сортируем лимит заявки по убыванию цены и возрастанию номера
             Form1.zlB.Clear();
             Form1.zlB.AddRange(zlbM);
             Form1.zlB.AddRange(zlBL);
             
         }

         public void try_sortS()//сортировка списка по цене и маркету/лимиту для продажи
         {
             List<Zayavka> zlS = Form1.zlS;
             zlS = zlS.OrderByDescending(x => x.lm).ToList();// так, сначала маркет/лимит
             List<Zayavka> zlSM = new List<Zayavka>();
             zlSM.AddRange(zlS.FindAll(x => x.lm == 77).ToList());//Записываем маркет заявки в один список
             List<Zayavka> zlSL = new List<Zayavka>();
             zlSL.AddRange(zlS.FindAll(x => x.lm == 76).ToList());//Записываем лимит заявки в дпугой список
             //сортируем лимит заявки по цене, потом по номеру
             zlSL = zlSL.OrderBy(x => x.price).ThenBy(x => x.num).ToList();//сортируем лимит заявки по возрастанию цены и возрастанию номера
            Form1.zlS.Clear();
             Form1.zlS.AddRange(zlSM);
             Form1.zlS.AddRange(zlSL);
         }

   }

}
