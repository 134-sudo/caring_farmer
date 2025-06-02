using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace caring_farmer
{
    public partial class MainForm : Form
    {
        public static NpgsqlConnection conn;
        NpgsqlDataAdapter adapter;
        DataTable dataTable;
        string currentTable;
        Dictionary<string, Dictionary<int, string>> lookupData = new Dictionary<string, Dictionary<int, string>>();

        Dictionary<string, Dictionary<string, string>> foreignKeysMap = new Dictionary<string, Dictionary<string, string>>
        {
            ["продажи"] = new Dictionary<string, string>
            {
                ["ин_списка_проданных_товаров"] = "список_продажи"
            },
            ["список_продажи"] = new Dictionary<string, string>
            {
                ["ин_товара"] = "товары"
            },
            ["поставки"] = new Dictionary<string, string>
            {
                ["ин_списка_проданных_товаров"] = "список_поставки"
            }
        };

        public MainForm()
        {
            InitializeComponent();
            try
            {
                conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=caring_farmer_2;Username=postgres;Password=1111;");
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                currentTable = comboBox1.Text;
                LoadLookupTables();

                adapter = new NpgsqlDataAdapter($"SELECT * FROM {currentTable};", conn);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns["ин"].Visible = false;

                ReplaceForeignKeyColumns();

                dataGridView1.DefaultValuesNeeded -= dataGridView1_DefaultValuesNeeded;
                dataGridView1.DefaultValuesNeeded += dataGridView1_DefaultValuesNeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                NpgsqlCommandBuilder npgsqlCommandBuilder = new NpgsqlCommandBuilder(adapter);
                adapter.Update(dataTable);
                MessageBox.Show("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void LoadLookupTables()
        {
            try
            {
                var requiredLookups = foreignKeysMap.Values
                    .SelectMany(d => d.Values)
                    .Distinct()
                    .ToList();

                foreach (var table in requiredLookups)
                {
                    string keyColumn = "ин";
                    string valueColumn = table == "товары" ? "наименование_товара" :
                                        table == "список_поставки" ? "номер_списка" : "ин";
                    lookupData[table] = LoadLookup(table, keyColumn, valueColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочных данных: {ex.Message}");
            }
        }

        private Dictionary<int, string> LoadLookup(string table, string key, string column)
        {
            var dict = new Dictionary<int, string>();
            try
            {
                using (var cmd = new NpgsqlCommand($"select {key}, {column} from {table};", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dict.Add(reader.GetInt32(0), reader.IsDBNull(1) ? "NULL" : reader.GetValue(1).ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки справочника {table}: {ex.Message}");
            }
            return dict;
        }

        private void ReplaceForeignKeyColumns()
        {
            try
            {
                if (!foreignKeysMap.TryGetValue(currentTable, out var fkColumns))
                    return;

                foreach (var fk in fkColumns)
                {
                    string columnName = fk.Key;
                    string lookupTable = fk.Value;

                    if (!dataTable.Columns.Contains(columnName))
                        continue;

                    var combo = new DataGridViewComboBoxColumn
                    {
                        Name = columnName,
                        DataPropertyName = columnName,
                        DataSource = lookupData[lookupTable].ToList(),
                        DisplayMember = "Value",
                        ValueMember = "Key",
                        FlatStyle = FlatStyle.Flat
                    };

                    int index = dataGridView1.Columns[columnName].Index;
                    dataGridView1.Columns.Remove(columnName);
                    dataGridView1.Columns.Insert(index, combo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка замены столбцов: {ex.Message}");
            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (!foreignKeysMap.TryGetValue(currentTable, out var fkColumns))
                    return;

                foreach (var fk in fkColumns)
                {
                    string columnName = fk.Key;
                    string lookupTable = fk.Value;

                    if (lookupData.TryGetValue(lookupTable, out var lookup) && lookup.Count > 0)
                    {
                        e.Row.Cells[columnName].Value = lookup.First().Key;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка установки значений по умолчанию: {ex.Message}");
            }
        }

        private void goods_Click(object sender, EventArgs e)
        {
            try
            {
                Form form = new GoodsForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия формы товаров: {ex.Message}");
            }
        }

        private void basket_Click(object sender, EventArgs e)
        {
            try
            {
                Form form = new BasketForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия формы корзины: {ex.Message}");
            }
        }
    }
}