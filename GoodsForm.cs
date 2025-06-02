using Npgsql;

namespace caring_farmer
{
  
    public partial class GoodsForm : Form
    {
        NpgsqlCommand cmd;
        NpgsqlDataReader reader;

        public GoodsForm()
        {
            InitializeComponent(); 
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximumSize = this.Size;
            LoadSales();
        }

        void LoadSales()
        {
            cmd = new NpgsqlCommand("select ин, наименование_товара назв, категория, цена, дата_изготовления дата_изг, дата_истечения_срока_годности дата_ист, дата_истечения_срока_годности -  date(now()) срок_годности from товары;", MainForm.conn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["срок_годности"]) < 0)
                    continue;

                int productId = Convert.ToInt32(reader["ин"]);
                decimal price = Convert.ToDecimal(reader["цена"]);
                int daysToExpire = Convert.ToInt32(reader["срок_годности"]);

                if (daysToExpire == 1) 
                    price *= 0.8m;
                else if (daysToExpire < 4) 
                    price *= 0.9m;

                Label name = new Label();
                name.AutoSize = true;
                name.Location = new Point(31, 16);
                name.Name = "name";
                name.Size = new Size(51, 19);
                name.TabIndex = 1;
                name.Text = "Наименование товара: " + reader["назв"].ToString();

                Label category = new Label();
                category.AutoSize = true;
                category.Location = new Point(31, 46);
                category.Name = "category";
                category.Size = new Size(51, 19);
                category.TabIndex = 2;
                category.Text = "Категория: " + reader["категория"].ToString();

                Label expirationDate = new Label();
                expirationDate.AutoSize = true;
                expirationDate.Location = new Point(31, 76);
                expirationDate.Name = "expirationDate";
                expirationDate.Size = new Size(51, 19);
                expirationDate.TabIndex = 3;
                expirationDate.Text = "Срок годности: c " + reader["дата_изг"].ToString() + " по " + reader["дата_ист"].ToString();

                Label daysTillExpiry = new Label();
                daysTillExpiry.AutoSize = true;
                daysTillExpiry.Location = new Point(31, 106);
                daysTillExpiry.Name = "daysTillExpiry";
                daysTillExpiry.Size = new Size(51, 19);
                daysTillExpiry.TabIndex = 4;
                daysTillExpiry.Text = "Дней до истечения срока годности: " + daysToExpire;

                Label sale = new Label();
                sale.AutoSize = true;
                sale.Location = new Point(480, 65);
                sale.Name = "sale";
                sale.Size = new Size(51, 19);
                sale.TabIndex = 6;
                sale.ForeColor = Color.Red;
                sale.Text = "";

                if (daysToExpire == 1)
                    sale.Text = "Скидка 20%";
                else if (daysToExpire < 4)
                    sale.Text = "Скидка 10%";

                NumericUpDown quantityInput = new NumericUpDown();
                quantityInput.Minimum = 1;
                quantityInput.Maximum = 100;
                quantityInput.Value = 1;
                quantityInput.Location = new Point(480, 35);
                quantityInput.Width = 50;

                Button AddToBacket = new Button();
                AddToBacket.BackColor = Color.FromArgb(57, 121, 61);
                AddToBacket.Font = new Font("Times New Roman", 10.2F);
                AddToBacket.ForeColor = Color.White;
                AddToBacket.Location = new Point(480, 105);
                AddToBacket.Name = productId.ToString();
                AddToBacket.Size = new Size(160, 30);
                AddToBacket.TabIndex = 7;
                AddToBacket.Text = "В корзину";
                AddToBacket.UseVisualStyleBackColor = false;
                AddToBacket.Click += (sender, e) => AddToBacket_Click(productId, (int)quantityInput.Value, price);

                Panel panel = new Panel();
                panel.Controls.Add(quantityInput);
                panel.Controls.Add(AddToBacket);
                panel.Controls.Add(sale);
                panel.Controls.Add(daysTillExpiry);
                panel.Controls.Add(expirationDate);
                panel.Controls.Add(category);
                panel.Controls.Add(name);
                panel.Location = new Point(3, 3);
                panel.Name = "panel";
                panel.Size = new Size(650, 140);
                panel.TabIndex = 1;
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Margin = new Padding(2);

                goodsList.Controls.Add(panel);
            }
            cmd.Dispose();
            reader.Close();
        }

        private void AddToBacket_Click(int productId, int quantity, decimal price)
        {
            if (Buff.goodsDict == null)
                Buff.goodsDict = new Dictionary<string, string[]>();

            string[] productInfo = { quantity.ToString(), price.ToString("0.00") };
            Buff.goodsDict[productId.ToString()] = productInfo;
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    public static class Buff
    {
        public static Dictionary<string, string[]> goodsDict;
    }
}
