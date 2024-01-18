using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace arackiralama
{
    public partial class frmMüşteriEkle : Form
    {
        private MySqlConnection mySqlConnection;
        private string mysqlcon = "server=localhost;user=root;database=arackiralama;password=";


        public frmMüşteriEkle()
        {
            InitializeComponent();

        }

        private void InitializeConnection()
        {
            mySqlConnection = new MySqlConnection(mysqlcon);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanıcı tarafından girilen verileri al
            string tc = textBox1.Text;
            string adSoyad = textBox2.Text;
            string telefon = textBox3.Text;
            string adres = textBox4.Text;
            string email = textBox5.Text;

            // Veritabanı bağlantısını başlat
            InitializeConnection();

            try
            {
                // SQL sorgusu oluştur
                string insertQuery = "INSERT INTO müsteri (TC, AdSoyad, Telefon, Adres, Email) VALUES (@TC, @AdSoyad, @Telefon, @Adres, @Email)";

                // Parametreleri ekleyerek SQL komutunu oluştur
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@TC", tc);
                    cmd.Parameters.AddWithValue("@AdSoyad", adSoyad);
                    cmd.Parameters.AddWithValue("@Telefon", telefon);
                    cmd.Parameters.AddWithValue("@Adres", adres);
                    cmd.Parameters.AddWithValue("@Email", email);

                    // Bağlantıyı aç
                    mySqlConnection.Open();

                    // Komutu çalıştır
                    cmd.ExecuteNonQuery();

                    // Başarılı mesajı göster
                    MessageBox.Show("Müşteri başarıyla eklendi.");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını göster
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapat
                mySqlConnection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
