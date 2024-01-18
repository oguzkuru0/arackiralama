using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace arackiralama
{
    public partial class frmSatış : Form
    {
        private MySqlConnection mySqlConnection;
        private string mysqlcon = "server=localhost;user=root;database=arackiralama;password=";

        public frmSatış()
        {
            InitializeComponent();
            InitializeConnection();
            LoadDataToGridView();
            UpdateTotalKiraUcreti();
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

                // Araç tablosundan plaka, marka, seri ve kiraucreti bilgilerini çek
                string query = "SELECT plaka, marka, seri, kiraucreti FROM araç";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridViewKiraUcreti için verileri yükle
                dataGridViewKiraUcreti.DataSource = dataTable;
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

        private void UpdateTotalKiraUcreti()
        {
            try
            {
                mySqlConnection.Open();

                // Araç tablosundaki tüm kiraucreti değerlerini topla
                string sumQuery = "SELECT SUM(kiraucreti) FROM araç";
                MySqlCommand sumCmd = new MySqlCommand(sumQuery, mySqlConnection);

                // Toplam değeri al
                object totalKiraUcreti = sumCmd.ExecuteScalar();

                // Toplam değeri label1'in yanındaki bir yerde göster
                label1.Text = "Toplam Kira Ücreti: " + (totalKiraUcreti is DBNull ? "0" : totalKiraUcreti.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Toplam kira ücreti hesaplama hatası: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewKiraUcreti.SelectedRows.Count > 0)
            {
                // Seçili satırın DataGridView üzerindeki indeksini al
                int rowIndex = dataGridViewKiraUcreti.SelectedRows[0].Index;

                // DataGridView'de seçili satırdaki plaka değerini al
                string selectedPlaka = dataGridViewKiraUcreti.Rows[rowIndex].Cells["plaka"].Value.ToString();

                // Araç tablosundan seçili aracı kaldır
                DeleteArac(selectedPlaka);

                // DataGridView'deki satırı kaldır
                dataGridViewKiraUcreti.Rows.RemoveAt(rowIndex);

                // Toplam kira ücretini güncelle
                UpdateTotalKiraUcreti();

                MessageBox.Show("Araç Teslim Edildi ve Kaldırıldı!");
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

                // Araç tablosundan seçili aracı kaldır
                string deleteQuery = "DELETE FROM araç WHERE plaka = @plaka";
                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@plaka", plaka);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Araç kaldırma hatası: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
    }
}
