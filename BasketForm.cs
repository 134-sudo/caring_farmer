using System.Windows.Forms;
using Npgsql;

namespace caring_farmer
{
    public partial class BasketForm : Form
    {

        NpgsqlCommand cmd;
        NpgsqlDataReader reader;
        string productIds;
        decimal sum;
        public BasketForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximumSize = this.Size;
            LoadGoods();
        }

        public void LoadGoods()
        {
            if (Buff.goodsDict == null || Buff.goodsDict.Count == 0)
            {
                MessageBox.Show("Товар куплен!");
                return;
            }
            productIds = string.Join(",", Buff.goodsDict.Keys);
            string query = $"select *, sum(цена) over() сумма from товары where ин in ({productIds})";

            cmd = new NpgsqlCommand(query, MainForm.conn);
            reader = cmd.ExecuteReader();
         
            while (reader.Read())
            {
                string productId = reader["ин"].ToString();
                string[] productInfo = Buff.goodsDict[productId];
                int q = int.Parse(productInfo[0]);
                decimal p = decimal.Parse(productInfo[1]);
                decimal tp = q * p;

                Label name = new Label();
                name.AutoSize = true;
                name.Location = new Point(22, 10);
                name.Name = "name";
                name.Size = new Size(51, 19);
                name.TabIndex = 0;
                name.Text = "Наименование: " + reader["наименование_товара"].ToString();

                Label category = new Label();
                category.AutoSize = true;
                category.Location = new Point(22, 35);
                category.Name = "category";
                category.Size = new Size(51, 19);
                category.TabIndex = 1;
                category.Text = "Категория: " + reader["категория"].ToString();

                Label package = new Label();
                package.AutoSize = true;
                package.Location = new Point(22, 60);
                package.Name = "package";
                package.Size = new Size(51, 19);
                package.TabIndex = 2;
                package.Text = "Упаковка: " + reader["упаковка"].ToString();

                Label size = new Label();
                size.AutoSize = true;
                size.Location = new Point(22, 85);
                size.Name = "size";
                size.Size = new Size(51, 19);
                size.TabIndex = 3;
                size.Text = "Размер: " + reader["размер"].ToString();

                Label price = new Label();
                price.AutoSize = true;
                price.Location = new Point(365, 85);
                price.Name = "price";
                price.Size = new Size(51, 19);
                price.TabIndex = 4;
                price.Text = $"Цена: {q} * {p} = {tp}"; 

                Panel productPanel = new Panel();
                productPanel.Controls.Add(price);
                productPanel.Controls.Add(size);
                productPanel.Controls.Add(package);
                productPanel.Controls.Add(category);
                productPanel.Controls.Add(name);
                productPanel.Location = new Point(3, 3);
                productPanel.Name = "productPanel";
                productPanel.Size = new Size(560, 120);
                productPanel.TabIndex = 0;
                productPanel.BorderStyle = BorderStyle.FixedSingle;
                productPanel.Margin = new Padding(2);

                goodsList.Controls.Add(productPanel);
                sum += tp;
            }

            summ.Text = "Сумма: " + sum.ToString();
            cmd.Dispose();
            cmd.Cancel();
            reader.Close();
        }

        private void regAnOrder_Click(object sender, EventArgs e)
        {
            if (Buff.goodsDict == null || Buff.goodsDict.Count == 0)
            {
                MessageBox.Show("Товар куплен!");
                return;
            }
            int num = 0, listNum = 0;
            using (var cmd = new NpgsqlCommand("select ин, max(номер_списка) from список_продажи where номер_списка = (select max(номер_списка) from список_продажи) group by ин order by ин desc limit 1;", MainForm.conn))
            {
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        num = Convert.ToInt32(reader[0]) + 1;
                        listNum = Convert.ToInt32(reader[1]) + 1;
                    }
                }
            }

            int count = 0;
            string query = "insert into список_продажи (номер_списка, ин_товара, количество) values ";
            foreach (var item in Buff.goodsDict)
            {
                int productId = int.Parse(item.Key);
                int quantity = int.Parse(item.Value[0]);

                count += int.Parse(item.Value[0]);
                query += $"({listNum}, {productId}, {quantity}),";
            }
            query = query.Remove(query.Length - 1);
            query += ";";
            query += $"insert into продажи (дата_продажи, ин_списка_проданных_товаров, количество, сумма) values ('{DateTime.UtcNow}', (select ин from список_продажи where номер_списка = (select max(номер_списка) from список_продажи) order by ин desc limit 1) , {count.ToString()}, {sum.ToString().Replace(',', '.')});";

            cmd = new NpgsqlCommand(query, MainForm.conn);
            cmd.ExecuteNonQuery();

            Buff.goodsDict.Clear();
        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
