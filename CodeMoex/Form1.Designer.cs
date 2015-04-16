namespace CodeMoex
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.search_btn = new System.Windows.Forms.Button();
            this.ll1 = new System.Windows.Forms.LinkLabel();
            this.ll2 = new System.Windows.Forms.LinkLabel();
            this.ll3 = new System.Windows.Forms.LinkLabel();
            this.ll4 = new System.Windows.Forms.LinkLabel();
            this.ll5 = new System.Windows.Forms.LinkLabel();
            this.ok_btn = new System.Windows.Forms.Button();
            this.fileUP = new System.Windows.Forms.LinkLabel();
            this.Close_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // search_btn
            // 
            this.search_btn.Location = new System.Drawing.Point(12, 21);
            this.search_btn.Name = "search_btn";
            this.search_btn.Size = new System.Drawing.Size(101, 28);
            this.search_btn.TabIndex = 0;
            this.search_btn.Text = "Обзор";
            this.search_btn.UseVisualStyleBackColor = true;
            this.search_btn.Click += new System.EventHandler(this.search_btn_Click);
            // 
            // ll1
            // 
            this.ll1.AutoSize = true;
            this.ll1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ll1.Location = new System.Drawing.Point(12, 94);
            this.ll1.Name = "ll1";
            this.ll1.Size = new System.Drawing.Size(16, 13);
            this.ll1.TabIndex = 1;
            this.ll1.TabStop = true;
            this.ll1.Text = "...";
            // 
            // ll2
            // 
            this.ll2.AutoSize = true;
            this.ll2.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ll2.Location = new System.Drawing.Point(12, 64);
            this.ll2.Name = "ll2";
            this.ll2.Size = new System.Drawing.Size(16, 13);
            this.ll2.TabIndex = 2;
            this.ll2.TabStop = true;
            this.ll2.Text = "...";
            // 
            // ll3
            // 
            this.ll3.AutoSize = true;
            this.ll3.Location = new System.Drawing.Point(26, 107);
            this.ll3.Name = "ll3";
            this.ll3.Size = new System.Drawing.Size(0, 13);
            this.ll3.TabIndex = 3;
            // 
            // ll4
            // 
            this.ll4.AutoSize = true;
            this.ll4.Location = new System.Drawing.Point(26, 131);
            this.ll4.Name = "ll4";
            this.ll4.Size = new System.Drawing.Size(0, 13);
            this.ll4.TabIndex = 4;
            // 
            // ll5
            // 
            this.ll5.AutoSize = true;
            this.ll5.Location = new System.Drawing.Point(26, 156);
            this.ll5.Name = "ll5";
            this.ll5.Size = new System.Drawing.Size(0, 13);
            this.ll5.TabIndex = 5;
            // 
            // ok_btn
            // 
            this.ok_btn.Location = new System.Drawing.Point(156, 21);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(47, 28);
            this.ok_btn.TabIndex = 6;
            this.ok_btn.Text = "Start";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // fileUP
            // 
            this.fileUP.AutoSize = true;
            this.fileUP.LinkColor = System.Drawing.Color.Black;
            this.fileUP.Location = new System.Drawing.Point(10, 5);
            this.fileUP.Name = "fileUP";
            this.fileUP.Size = new System.Drawing.Size(16, 13);
            this.fileUP.TabIndex = 7;
            this.fileUP.TabStop = true;
            this.fileUP.Text = "...";
            // 
            // Close_btn
            // 
            this.Close_btn.Location = new System.Drawing.Point(156, 85);
            this.Close_btn.Name = "Close_btn";
            this.Close_btn.Size = new System.Drawing.Size(47, 35);
            this.Close_btn.TabIndex = 8;
            this.Close_btn.Text = "Close";
            this.Close_btn.UseVisualStyleBackColor = true;
            this.Close_btn.Click += new System.EventHandler(this.Close_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 118);
            this.Controls.Add(this.Close_btn);
            this.Controls.Add(this.fileUP);
            this.Controls.Add(this.ok_btn);
            this.Controls.Add(this.ll5);
            this.Controls.Add(this.ll4);
            this.Controls.Add(this.ll3);
            this.Controls.Add(this.ll2);
            this.Controls.Add(this.ll1);
            this.Controls.Add(this.search_btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button search_btn;
        private System.Windows.Forms.LinkLabel ll1;
        private System.Windows.Forms.LinkLabel ll2;
        private System.Windows.Forms.LinkLabel ll3;
        private System.Windows.Forms.LinkLabel ll4;
        private System.Windows.Forms.LinkLabel ll5;
        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.LinkLabel fileUP;
        private System.Windows.Forms.Button Close_btn;
    }
}

