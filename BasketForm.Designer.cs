namespace caring_farmer
{
    partial class BasketForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasketForm));
            back = new Button();
            goodsList = new FlowLayoutPanel();
            summ = new Label();
            regAnOrder = new Button();
            SuspendLayout();
            // 
            // back
            // 
            back.BackColor = Color.FromArgb(57, 121, 61);
            back.Font = new Font("Times New Roman", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            back.ForeColor = Color.White;
            back.Location = new Point(12, 306);
            back.Name = "back";
            back.Size = new Size(168, 30);
            back.TabIndex = 9;
            back.Text = "Назад";
            back.UseVisualStyleBackColor = false;
            back.Click += back_Click;
            // 
            // goodsList
            // 
            goodsList.AutoScroll = true;
            goodsList.BackColor = Color.FromArgb(255, 255, 128);
            goodsList.Font = new Font("Times New Roman", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            goodsList.Location = new Point(12, 12);
            goodsList.Name = "goodsList";
            goodsList.Size = new Size(675, 288);
            goodsList.TabIndex = 8;
            // 
            // summ
            // 
            summ.AutoSize = true;
            summ.Location = new Point(347, 311);
            summ.Name = "summ";
            summ.Size = new Size(62, 20);
            summ.TabIndex = 3;
            summ.Text = "Сумма: ";
            // 
            // regAnOrder
            // 
            regAnOrder.BackColor = Color.FromArgb(57, 121, 61);
            regAnOrder.Font = new Font("Times New Roman", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            regAnOrder.ForeColor = Color.White;
            regAnOrder.Location = new Point(510, 306);
            regAnOrder.Name = "regAnOrder";
            regAnOrder.Size = new Size(168, 30);
            regAnOrder.TabIndex = 10;
            regAnOrder.Text = "Выполнить заказ";
            regAnOrder.UseVisualStyleBackColor = false;
            regAnOrder.Click += regAnOrder_Click;
            // 
            // BasketForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(708, 341);
            Controls.Add(regAnOrder);
            Controls.Add(back);
            Controls.Add(summ);
            Controls.Add(goodsList);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "BasketForm";
            Text = "Корзина";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button back;
        private FlowLayoutPanel goodsList;
        private Label summ;
        private Button regAnOrder;
    }
}