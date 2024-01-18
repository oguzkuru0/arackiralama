using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace arackiralama
{
    public partial class frmMüşteriListele : Form
    {
        private MySqlConnection mySqlConnection;
        private string mysqlcon = "server=localhost;user=root;database=arackiralama;password=";

        public frmMüşteriListele()
        {
            InitializeComponent();
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            mySqlConnection = new MySqlConnection(mysqlcon);

            try
            {
                

                // Verileri DataGridView'e yükle
                LoadDataToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDataToGridView()
        {
            // SQL sorgusu ile müsteri tablosundaki verileri çek
            string query = "SELECT * FROM müsteri";
            MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();

            dataAdapter.Fill(dataTable);

            // DataGridView'e verileri yükle
            dataGridView1.DataSource = dataTable;

            // Bağlantıyı kapat
            mySqlConnection.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // DataGridView hücre içeriği tıklandığında seçili satırın bilgilerini TextBox'lara aktar
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Satırın her bir hücresini alarak TextBox'lara aktar
                textBox1.Text = row.Cells["TC"].Value.ToString();
                textBox2.Text = row.Cells["AdSoyad"].Value.ToString();
                textBox3.Text = row.Cells["Telefon"].Value.ToString();
                textBox4.Text = row.Cells["Adres"].Value.ToString();
                textBox5.Text = row.Cells["Email"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TextBox'lardan alınan verilerle güncelleme işlemi gerçekleştir
            try
            {
                mySqlConnection.Open();

                string updateQuery = "UPDATE müsteri SET AdSoyad = @AdSoyad, Telefon = @Telefon, Adres = @Adres, Email = @Email WHERE TC = @TC";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@TC", textBox1.Text);
                    cmd.Parameters.AddWithValue("@AdSoyad", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Telefon", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Adres", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Email", textBox5.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Veri Güncellendi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            // Güncelleme işlemi sonrasında DataGridView'i yeniden yükle
            LoadDataToGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // TextBox6'da herhangi bir değişiklik olduğunda bu olay tetiklensin

            // Veritabanı bağlantısını aç
            mySqlConnection.Open();

            // SQL sorgusu oluştur
            string selectQuery = "SELECT * FROM müsteri WHERE TC LIKE @TC";
            MySqlCommand cmd = new MySqlCommand(selectQuery, mySqlConnection);

            // Parametre ekleyerek TC'ye göre filtreleme yap
            cmd.Parameters.AddWithValue("@TC", "%" + textBox6.Text + "%");

            // MySQL veri adaptörü ve veri tablosunu oluştur
            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
            {
                DataTable dataTable = new DataTable();

                // Veri adaptörü ile verileri tabloya doldur
                adapter.Fill(dataTable);

                // DataGridView'e filtrelenmiş verileri ata
                dataGridView1.DataSource = dataTable;
            }

            // Bağlantıyı kapat
            mySqlConnection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // DataGridView'den seçili satırı kontrol et
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Kullanıcıya onay mesajı göster
                DialogResult result = MessageBox.Show("Seçili müşteriyi silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Kullanıcı onay verirse
                if (result == DialogResult.Yes)
                {
                    // Veritabanı bağlantısını aç
                    mySqlConnection.Open();

                    // Seçili satırın TC'sini al
                    string selectedTC = dataGridView1.SelectedRows[0].Cells["TC"].Value.ToString();

                    try
                    {
                        // SQL sorgusu oluştur ve müşteriyi sil
                        string deleteQuery = "DELETE FROM müsteri WHERE TC = @TC";
                        using (MySqlCommand cmd = new MySqlCommand(deleteQuery, mySqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@TC", selectedTC);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Müşteri Silindi!");
                        }

                        // Silme işlemi sonrasında DataGridView'i güncelle
                        LoadDataToGridView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Bağlantıyı kapat
                        mySqlConnection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz müşteriyi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
