
namespace Zoo
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.calc_Button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // calc_Button
            // 
            this.calc_Button.Location = new System.Drawing.Point(233, 240);
            this.calc_Button.Name = "calc_Button";
            this.calc_Button.Size = new System.Drawing.Size(238, 99);
            this.calc_Button.TabIndex = 0;
            this.calc_Button.Text = "Калькулятор";
            this.calc_Button.UseVisualStyleBackColor = true;
            this.calc_Button.Click += new System.EventHandler(this.calc_Button_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(606, 240);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(238, 99);
            this.button1.TabIndex = 1;
            this.button1.Text = "Касса";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 961);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.calc_Button);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Зоотель \"Шоколад\"";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button calc_Button;
        private System.Windows.Forms.Button button1;
    }
}

