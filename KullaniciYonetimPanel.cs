using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FilmKiralama
{
    public partial class KullaniciYonetimPanel : Form
    {
        private int secilenSatirIndex = -1;

        public KullaniciYonetimPanel()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void VerileriTemizle()
        {
            txtID.Clear();
            txtKullaniciAdi.Clear();
            txtSifre.Clear();
            txtEposta.Clear();
            txtTelefon.Clear();
            cmbRol.SelectedIndex = -1;
        }

        private void KullaniciVerileriniYukle()
        {
            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                string query = "SELECT * FROM Kullanicilar";
                SqlDataAdapter da = new SqlDataAdapter(query, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void KullaniciYonetimPanel_Load(object sender, EventArgs e)
        {
            cmbRol.Items.AddRange(new string[] { "Admin", "Müşteri" });
            cmbRol.SelectedIndex = -1;

            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);
                KullaniciVerileriniYukle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                secilenSatirIndex = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Burada KullaniciID sütununun doğru isme sahip olduğundan emin olun
                txtID.Text = row.Cells["KullaniciID"].Value?.ToString() ?? "";
                txtKullaniciAdi.Text = row.Cells["KullaniciAdi"].Value?.ToString() ?? "";
                txtSifre.Text = row.Cells["Sifre"].Value?.ToString() ?? "";
                txtEposta.Text = row.Cells["Eposta"].Value?.ToString() ?? "";
                txtTelefon.Text = row.Cells["Telefon"].Value?.ToString() ?? "";
                cmbRol.Text = row.Cells["Rol"].Value?.ToString() ?? "";
            }
        }

        private void btnPanelCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool AlanlarBosMu(bool idGerekli)
        {
            if (idGerekli && string.IsNullOrWhiteSpace(txtID.Text))
                return true;

            return string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) ||
                   string.IsNullOrWhiteSpace(txtSifre.Text) ||
                   string.IsNullOrWhiteSpace(txtEposta.Text) ||
                   string.IsNullOrWhiteSpace(txtTelefon.Text) ||
                   cmbRol.SelectedIndex == -1;
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (AlanlarBosMu(true))  // ID zorunlu
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }


            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                baglanti.Open();

                string kontrolQuery = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi AND KullaniciID <> @KullaniciID";
                SqlCommand kontrolCmd = new SqlCommand(kontrolQuery, baglanti);
                kontrolCmd.Parameters.AddWithValue("@KullaniciAdi", txtKullaniciAdi.Text);
                kontrolCmd.Parameters.AddWithValue("@KullaniciID", txtID.Text);

                if ((int)kontrolCmd.ExecuteScalar() > 0)
                {
                    MessageBox.Show("Bu kullanıcı adı başka bir kullanıcıya ait!");
                    return;
                }

                string updateQuery = @"UPDATE Kullanicilar SET 
                                       KullaniciAdi = @KullaniciAdi, 
                                       Sifre = @Sifre, 
                                       Eposta = @Eposta, 
                                       Telefon = @Telefon, 
                                       Rol = @Rol 
                                       WHERE KullaniciID = @KullaniciID";

                SqlCommand cmd = new SqlCommand(updateQuery, baglanti);
                cmd.Parameters.AddWithValue("@KullaniciAdi", txtKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                cmd.Parameters.AddWithValue("@Eposta", txtEposta.Text);
                cmd.Parameters.AddWithValue("@Telefon", txtTelefon.Text.Replace(" ", ""));
                cmd.Parameters.AddWithValue("@Rol", cmbRol.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@KullaniciID", txtID.Text);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Kullanıcı başarıyla güncellendi.");
                    KullaniciVerileriniYukle();
                    VerileriTemizle();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız. Lütfen tekrar deneyin.");
                }
            }
        }

        private void btnKullaniciSil_Click(object sender, EventArgs e)
        {
            if (secilenSatirIndex == -1)
            {
                MessageBox.Show("Lütfen silinecek bir kullanıcı seçin.");
                return;
            }

            string kullaniciTuru = dataGridView1.Rows[secilenSatirIndex].Cells["Rol"].Value?.ToString();
            if (kullaniciTuru == "Admin")
            {
                MessageBox.Show("Yönetici rolündeki kullanıcılar silinemez.", "İşlem Engellendi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bu kullanıcıyı silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (SqlConnection baglanti = ConnectionManager.GetConnection())
                {
                    string deleteQuery = "DELETE FROM Kullanicilar WHERE KullaniciID = @KullaniciID";
                    SqlCommand cmd = new SqlCommand(deleteQuery, baglanti);
                    cmd.Parameters.AddWithValue("@KullaniciID", txtID.Text);

                    try
                    {
                        baglanti.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Kullanıcı başarıyla silindi.");
                            KullaniciVerileriniYukle();
                            VerileriTemizle();
                            secilenSatirIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("Silme işlemi başarısız.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }
        }

        private void btnKullaniciEkle_Click(object sender, EventArgs e)
        {

            if (AlanlarBosMu(false)) // ID gerekli değil
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                baglanti.Open();

                string[] kontroller = {
                    "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @KullaniciAdi",
                    "SELECT COUNT(*) FROM Kullanicilar WHERE Eposta = @Eposta",
                    "SELECT COUNT(*) FROM Kullanicilar WHERE Telefon = @Telefon"
                };

                SqlParameter[] parametreler = {
                    new SqlParameter("@KullaniciAdi", txtKullaniciAdi.Text),
                    new SqlParameter("@Eposta", txtEposta.Text),
                    new SqlParameter("@Telefon", txtTelefon.Text.Replace(" ", ""))
                };

                for (int i = 0; i < kontroller.Length; i++)
                {
                    SqlCommand kontrolCmd = new SqlCommand(kontroller[i], baglanti);
                    kontrolCmd.Parameters.Add(parametreler[i]);

                    if ((int)kontrolCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show($"{parametreler[i].ParameterName.Substring(1)} zaten kullanımda!");
                        return;
                    }
                }

                string insertQuery = @"INSERT INTO Kullanicilar 
                                      (KullaniciAdi, Sifre, Eposta, Telefon, Rol) 
                                       VALUES (@KullaniciAdi, @Sifre, @Eposta, @Telefon, @Rol)";

                SqlCommand insertCmd = new SqlCommand(insertQuery, baglanti);
                insertCmd.Parameters.AddWithValue("@KullaniciAdi", txtKullaniciAdi.Text);
                insertCmd.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                insertCmd.Parameters.AddWithValue("@Eposta", txtEposta.Text);
                insertCmd.Parameters.AddWithValue("@Telefon", txtTelefon.Text.Replace(" ", ""));
                insertCmd.Parameters.AddWithValue("@Rol", cmbRol.SelectedItem.ToString());

                if (insertCmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Kullanıcı başarıyla eklendi.");
                    KullaniciVerileriniYukle();
                    VerileriTemizle();
                }
                else
                {
                    MessageBox.Show("Kullanıcı eklenirken bir hata oluştu.");
                }
            }
        }

        private void txtTelefon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void txtTelefon_TextChanged(object sender, EventArgs e)
        {
            string telefon = new string(txtTelefon.Text.Where(char.IsDigit).ToArray());
            if (telefon.Length > 10) telefon = telefon.Substring(0, 10);

            if (telefon.Length == 10)
                txtTelefon.Text = $"{telefon.Substring(0, 3)} {telefon.Substring(3, 3)} {telefon.Substring(6, 4)}";
            else if (telefon.Length >= 4)
                txtTelefon.Text = $"{telefon.Substring(0, 3)} {telefon.Substring(3)}";
            else
                txtTelefon.Text = telefon;

            txtTelefon.SelectionStart = txtTelefon.Text.Length;
        }
    }
}
