using System;
using System.Data;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace arackiralama
{
    public partial class frmAraçListele : Form
    {
        private MySqlConnection mySqlConnection;
        private string mysqlcon = "server=localhost;user=root;database=arackiralama;password=";

        public frmAraçListele()
        {
            InitializeComponent();
            InitializeConnection();
            LoadDataToGridView();
        }

        private void InitializeConnection()
        {
            mySqlConnection = new MySqlConnection(mysqlcon);
        }

        private void LoadDataToGridView()
        {
            try
            {
                mySqlConnection.Open();

                string query = "SELECT * FROM araç";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yükleme hatası: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            // TextBox'lardan alınan verilerle güncelleme işlemi gerçekleştir
            try
            {
                mySqlConnection.Open();

                string updateQuery = "UPDATE araç SET marka = @marka, seri = @seri, yil = @yil, renk = @renk, km = @km, yakit = @yakit, kiraucreti = @kiraucreti WHERE plaka = @plaka";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@plaka", textBox1.Text);
                    cmd.Parameters.AddWithValue("@marka", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@seri", comboBox2.Text);
                    cmd.Parameters.AddWithValue("@yil", Convert.ToInt32(textBox2.Text));
                    cmd.Parameters.AddWithValue("@renk", textBox3.Text);
                    cmd.Parameters.AddWithValue("@km", Convert.ToInt32(textBox4.Text));
                    cmd.Parameters.AddWithValue("@yakit", comboBox3.Text);
                    cmd.Parameters.AddWithValue("@kiraucreti", Convert.ToDouble(textBox5.Text));
                   
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçili satırın DataGridView üzerindeki indeksini al
                int rowIndex = dataGridView1.SelectedRows[0].Index;

                // DataGridView'de seçili satırdaki plaka değerini al
                string selectedPlaka = dataGridView1.Rows[rowIndex].Cells["plaka"].Value.ToString();

                // Veritabanından seçili aracı sil
                DeleteArac(selectedPlaka);

                // DataGridView'deki satırı sil
                dataGridView1.Rows.RemoveAt(rowIndex);

                MessageBox.Show("Araç Başarıyla Silindi!");
            }
            else
            {
                MessageBox.Show("Lütfen bir araç seçin.");
            }
        }

        private void DeleteArac(string plaka)
        {
            try
            {
                mySqlConnection.Open();

                string deleteQuery = "DELETE FROM araç WHERE plaka = @plaka";
                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@plaka", plaka);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Araç silme hatası: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // DataGridView'de seçilen satırdaki bilgileri TextBox'lara aktar
                textBox1.Text = row.Cells["plaka"].Value.ToString();
                comboBox1.Text = row.Cells["marka"].Value.ToString();
                comboBox2.Text = row.Cells["seri"].Value.ToString();
                textBox2.Text = row.Cells["yil"].Value.ToString();
                textBox3.Text = row.Cells["renk"].Value.ToString();
                textBox4.Text = row.Cells["km"].Value.ToString();
                comboBox3.Text = row.Cells["yakit"].Value.ToString();
                textBox5.Text = row.Cells["kiraucreti"].Value.ToString();

                try
                {
                    // DataGridView'de seçilen satırdaki resmi al
                    if (row.Cells["resim"].Value != null && row.Cells["resim"].Value != DBNull.Value)
                    {
                        string base64String = row.Cells["resim"].Value.ToString();
                        byte[] imageData = Convert.FromBase64String(base64String);

                        // PictureBox'a resmi yükle
                        LoadImageToPictureBox(imageData);
                    }
                    else
                    {
                        // Resim yoksa PictureBox'ı temizle
                        pictureBox1.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Resim alınırken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void LoadImageToPictureBox(byte[] imageData)
        {
            try
            {
                if (imageData != null && imageData.Length > 0)
                {
                    // Byte dizisini MemoryStream'e çevir
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        // MemoryStream'deki resmi PictureBox'a yükle
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    // Resim yoksa PictureBox'ı temizle
                    pictureBox1.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Resim yükleme hatası: " + ex.Message);
            }
        }
    }
}
