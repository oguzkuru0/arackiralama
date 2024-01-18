using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace arackiralama
{
    public partial class frmAraçKayıt : Form
    {
        private MySqlConnection mySqlConnection;
        private string mysqlcon = "server=localhost;user=root;database=arackiralama;password=";

        public frmAraçKayıt()
        {
            InitializeComponent();
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            mySqlConnection = new MySqlConnection(mysqlcon);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Resim seçme iletişim kutusunu görüntüle
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
              
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();
                if (comboBox1.SelectedIndex == 0)
                {
                    comboBox2.Items.Add("astra");
                    comboBox2.Items.Add("vectra");
                    comboBox2.Items.Add("corsa");
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    comboBox2.Items.Add("Megane");
                    comboBox2.Items.Add("Clio");
                    comboBox2.Items.Add("Symbol");
                }

                else if (comboBox1.SelectedIndex == 2)
                {
                    comboBox2.Items.Add("Linea");
                    comboBox2.Items.Add("Egea");
                    comboBox2.Items.Add("124 Spider");
                }
                else if (comboBox1.SelectedIndex == 3)
                {
                    comboBox2.Items.Add("Fiesta");
                    comboBox2.Items.Add("Focus");
                    comboBox2.Items.Add("Mustang");
                }
                else if (comboBox1.SelectedIndex == 4)
                {
                    comboBox2.Items.Add("S-Class");
                    comboBox2.Items.Add("G-63");
                    comboBox2.Items.Add("AMG");
                }
                else if (comboBox1.SelectedIndex == 5)
                {
                    comboBox2.Items.Add("Corolla");
                    comboBox2.Items.Add("SUPRAAA");
                    comboBox2.Items.Add("Hilux");
                }
                else if (comboBox1.SelectedIndex == 6)
                {
                    comboBox2.Items.Add("Qashaqi");
                    comboBox2.Items.Add("Micra");
                    comboBox2.Items.Add("GTR");
                }
                else if (comboBox1.SelectedIndex == 8)
                {
                    comboBox2.Items.Add("M5 competitions");
                    comboBox2.Items.Add("520 Xdrive");
                    comboBox2.Items.Add("E60");
                }
                else if (comboBox1.SelectedIndex == 7)
                {
                    comboBox2.Items.Add("Rs-6");
                    comboBox2.Items.Add("Q8");
                    comboBox2.Items.Add("Q7");
                }
                else if (comboBox1.SelectedIndex == 9)
                {
                    comboBox2.Items.Add("Passat");
                    comboBox2.Items.Add("Jetta");
                    comboBox2.Items.Add("Golf-R");
                }

            }
            catch
            {
                ;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                mySqlConnection.Open();

                // ComboBox'lardan seçilen verileri al
                string marka = comboBox1.SelectedItem.ToString();
                string seri = comboBox2.SelectedItem.ToString();

                // TextBox'lardan girilen verileri al
                string plaka = textBox1.Text;
                int yil = Convert.ToInt32(textBox2.Text);
                string renk = textBox3.Text;
                int km = Convert.ToInt32(textBox4.Text);
                string yakit = comboBox3.Text;
                double kiraUcreti = Convert.ToDouble(textBox5.Text);

                // Resim yolunu al
                string resimYolu = "";

                if (pictureBox1.Image != null)
                {
                    string resimAdi = Guid.NewGuid().ToString() + ".png"; // Resmin adını oluştur
                    resimYolu = Application.StartupPath + "\\Resimler\\" + resimAdi; // Resmin kaydedileceği yeri belirle

                    pictureBox1.Image.Save(resimYolu); // Resmi kaydet
                }

                // SQL sorgusu oluştur ve veritabanına ekle
                string insertQuery = "INSERT INTO araç (plaka, marka, seri, yil, renk, km, yakit, kiraucreti, resim) VALUES (@plaka, @marka, @seri, @yil, @renk, @km, @yakit, @kiraucreti, @resim)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@plaka", plaka);
                    cmd.Parameters.AddWithValue("@marka", marka);
                    cmd.Parameters.AddWithValue("@seri", seri);
                    cmd.Parameters.AddWithValue("@yil", yil);
                    cmd.Parameters.AddWithValue("@renk", renk);
                    cmd.Parameters.AddWithValue("@km", km);
                    cmd.Parameters.AddWithValue("@yakit", yakit);
                    cmd.Parameters.AddWithValue("@kiraucreti", kiraUcreti);
                    cmd.Parameters.AddWithValue("@resim", resimYolu);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Araç Başarıyla Kaydedildi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        private void btnResim_Click(object sender, EventArgs e)
        {
            // Resim seçme iletişim kutusunu görüntüle
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Resim Dosyaları | *.jpg; *.jpeg; *.png; *.gif; *.bmp";
            openFileDialog1.Title = "Resim Seç";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Klasörü kontrol et ve oluştur
                string resimKlasorYolu = Application.StartupPath + "\\Resimler\\";
                if (!System.IO.Directory.Exists(resimKlasorYolu))
                {
                    System.IO.Directory.CreateDirectory(resimKlasorYolu);
                }

                // Seçilen resmi pictureBox1 kontrolüne at
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }
    }
}
