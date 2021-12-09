using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace WindowsForms
{
	public partial class Form1 : Form
	{
		Form2 newForm = new Form2();
		public void Initil(string a, string b)
		{
			NpgsqlConnection connect = new NpgsqlConnection("Server=localhost;Port=5432;Database=warehouse;User id=postgres;Password=1217112171dz;");
			connect.Open();

			NpgsqlCommand command = new NpgsqlCommand();
			command.Connection = connect;
			command.CommandType = CommandType.Text;
			command.CommandText = $@"Select {a} from product {b}";
			NpgsqlDataReader data_reader = command.ExecuteReader();
			if (data_reader.HasRows)
			{
				DataTable data_table = new DataTable();
				data_table.Load(data_reader);
				dataGridView1.DataSource = data_table;
			}
			connect.Dispose();
			connect.Close();
		}

		public void Initil2(string a)
		{
			NpgsqlConnection connect = new NpgsqlConnection("Server=localhost;Port=5432;Database=warehouse;User id=postgres;Password=1217112171dz;");
			connect.Open();

			NpgsqlCommand command = new NpgsqlCommand();
			command.Connection = connect;
			command.CommandType = CommandType.Text;
			command.CommandText = a;
			NpgsqlDataReader data_reader = command.ExecuteReader();
			if (data_reader.HasRows)
			{
				DataTable data_table = new DataTable();
				data_table.Load(data_reader);
				newForm.dataGridView2.DataSource = data_table;
			}
			connect.Dispose();
			connect.Close();
		}


		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			comboBox1.Items.Add("Бытовая химия");
			comboBox1.Items.Add("Пищевые продукты");
			
			comboBox2.Items.Add("Поиск товара по названию");
			comboBox2.Items.Add("Поиск товара по артиклю");
			comboBox2.Items.Add("Поиск товара по цене");
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0)
			{
				Initil("product_name, item_number, remains, barcode, price_per_piece", " where number_of_sector = 1");
			}
			if (comboBox1.SelectedIndex == 1)
			{
				Initil("product_name, item_number, remains, barcode, price_per_piece" , " where number_of_sector = 2");
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{}
		
		List<string> item_number = new List<string>();
		List<int> Price = new List<int>();
		List<int> Count = new List<int>();
		
		private void button1_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex != 0 && comboBox1.SelectedIndex != 1 &&comboBox2.SelectedIndex != 0 && comboBox2.SelectedIndex != 1 && comboBox2.SelectedIndex != 2)
			{
			 MessageBox.Show("Товар не выбран");
			}
			else { 
				int index = dataGridView1.CurrentCell.RowIndex;
				int check = (int)dataGridView1[2, index].Value;
				string check1 = textBox3.Text;
				if (check1 != "") 
				{
					int help = Convert.ToInt32(check1);
					if (check >= help)
					{
						int price = help * (int)dataGridView1[4, index].Value;
						string item = dataGridView1[1, index].Value.ToString();
						item_number.Add(item);
						Count.Add(help);
						Price.Add(price);
					}
					else MessageBox.Show("Товара недостаточно");
				}	else MessageBox.Show("Количество товара не выбрано");
			}	
		}

		private void button2_Click(object sender, EventArgs e)
		{
			string Str = "";
			int price = 0;
			int count = 1;		
			int count1 = item_number.Count;
			int count2 = 0;
			if (count1 > 0)
			{
				foreach (string p in item_number)
				{
					if (count < count1)
					{
						count++;
						Str += $@"Select product_name, price_per_piece , {Count[count2]} as Count from product where item_number = {p} union ";
					}
					else Str += $@"Select product_name, price_per_piece, {Count[count2]} as Count from product where item_number = {p}";
					count2++;
				}
				foreach (int p in Price)
				{	
					price += p;
				}
				Initil2(Str);
				newForm.textBox2.Text = price.ToString();
				newForm.Show();
			} else MessageBox.Show("Корзина пуста");
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			textBox1.Clear();
			
			if (comboBox2.SelectedIndex == 0)
			{
				textBox1.Visible = true;
				textBox4.Visible = false;
				textBox5.Visible = false;
				textBox6.Visible = false;
				textBox7.Visible = false;
			}
			if (comboBox2.SelectedIndex == 1)
			{
				textBox1.Visible = true;
				textBox4.Visible = false;
				textBox5.Visible = false;
				textBox6.Visible = false;
				textBox7.Visible = false;
			}if (comboBox2.SelectedIndex == 2)
			{
			textBox1.Visible = false;
			textBox4.Visible = true;
			textBox5.Visible = true;
			textBox6.Visible = true;
			textBox7.Visible = true;
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			string search = textBox1.Text;
			if (comboBox2.SelectedIndex != 0 && comboBox2.SelectedIndex != 1 && comboBox2.SelectedIndex != 2) MessageBox.Show("Категория поиска не выбрана");
			else if (comboBox2.SelectedIndex == 0 && search != "") Initil("product_name, item_number, remains, barcode, price_per_piece", $@" where product_name LIKE '%{search}%'");
			else if (comboBox2.SelectedIndex == 1 && search != "") Initil("product_name, item_number, remains, barcode, price_per_piece", $@" where item_number = {search}");
			else if (comboBox2.SelectedIndex == 2 )
			{
				
				string check1 = textBox5.Text; string check2 = textBox7.Text;
				Initil("*", $@" where price_per_piece >= {check1} and price_per_piece <= {check2} ");
			}						
						else if (search == "") MessageBox.Show("Поле поиска пусто");
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
		}
	}
}
