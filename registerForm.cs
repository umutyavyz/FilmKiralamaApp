using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilmKiralama
{
    public partial class registerForm : Form
    {
        public registerForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void registerForm_Load(object sender, EventArgs e)
        {

        }

        private void btnKayit_Click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO Kullanicilar (KullaniciAdi, Sifre, Eposta, Telefon) VALUES (@kadi, @sifre, @mail, @telefon)";
            string checkQuery = "SELECT KullaniciAdi, Eposta, Telefon FROM Kullanicilar WHERE KullaniciAdi = @kadi OR Eposta = @mail OR Telefon = @telefon";

            // Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtMail.Text) ||
                string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtUsername.BackColor = string.IsNullOrWhiteSpace(txtUsername.Text) ? Color.LightPink : Color.White;
                txtPassword.BackColor = string.IsNullOrWhiteSpace(txtPassword.Text) ? Color.LightPink : Color.White;
                txtMail.BackColor = string.IsNullOrWhiteSpace(txtMail.Text) ? Color.LightPink : Color.White;
                txtTelefon.BackColor = string.IsNullOrWhiteSpace(txtTelefon.Text) ? Color.LightPink : Color.White;

                return;
            }

            // Admin kullanıcı adına izin verme
            if (txtUsername.Text.Trim().ToLower() == "admin")
            {
                MessageBox.Show("Bu kullanıcı adı rezerve edilmiştir. Lütfen başka bir kullanıcı adı seçin.", "Geçersiz Kullanıcı Adı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.BackColor = Color.LightPink;
                return;
            }

            // Alanlar doluysa arka planları beyaz yap
            txtUsername.BackColor = Color.White;
            txtPassword.BackColor = Color.White;
            txtMail.BackColor = Color.White;
            txtTelefon.BackColor = Color.White;

            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, baglanti))
            {
                checkCmd.Parameters.AddWithValue("@kadi", txtUsername.Text.Trim());
                checkCmd.Parameters.AddWithValue("@mail", txtMail.Text.Trim());
                checkCmd.Parameters.AddWithValue("@telefon", txtTelefon.Text.Trim());

                try
                {
                    baglanti.Open();
                    using (SqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        bool kullaniciVar = false;
                        bool mailVar = false;
                        bool telefonVar = false;

                        while (reader.Read())
                        {
                            if (reader["KullaniciAdi"].ToString() == txtUsername.Text.Trim())
                                kullaniciVar = true;
                            if (reader["Eposta"].ToString() == txtMail.Text.Trim())
                                mailVar = true;
                            if (reader["Telefon"].ToString() == txtTelefon.Text.Trim())
                                telefonVar = true;
                        }

                        if (kullaniciVar || mailVar || telefonVar)
                        {
                            string mesaj = "Kayıt yapılamadı. Lütfen aşağıdaki hataları düzeltin:\n";
                            if (kullaniciVar) mesaj += "- Bu kullanıcı adı zaten mevcut.\n";
                            if (mailVar) mesaj += "- Bu e-posta adresi zaten kullanılıyor.\n";
                            if (telefonVar) mesaj += "- Bu telefon numarası zaten kayıtlı.\n";

                            MessageBox.Show(mesaj, "Kayıt Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Kayıt yoksa veritabanına ekle
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, baglanti))
                    {
                        insertCmd.Parameters.AddWithValue("@kadi", txtUsername.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@sifre", txtPassword.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@mail", txtMail.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@telefon", txtTelefon.Text.Trim());

                        int result = insertCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Kayıt başarılı!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Kayıt başarısız.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void txtTelefon_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakamlara izin ver ve kontrol tuşları (backspace gibi)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // karakteri engelle
            }
        }

        private void txtTelefon_TextChanged(object sender, EventArgs e)
        {
            string raw = new string(txtTelefon.Text.Where(char.IsDigit).ToArray()); // Sadece rakamları al

            if (raw.Length > 10)
                raw = raw.Substring(0, 10); // En fazla 10 hane

            string formatted = "";

            if (raw.Length >= 1)
                formatted = raw.Substring(0, Math.Min(3, raw.Length));
            if (raw.Length >= 4)
                formatted += " " + raw.Substring(3, Math.Min(3, raw.Length - 3));
            if (raw.Length >= 7)
                formatted += " " + raw.Substring(6, raw.Length - 6);

            // Cursor pozisyonunu bozmamak için geçici olarak event'i kaldır
            txtTelefon.TextChanged -= txtTelefon_TextChanged;
            txtTelefon.Text = formatted;
            txtTelefon.SelectionStart = txtTelefon.Text.Length;
            txtTelefon.TextChanged += txtTelefon_TextChanged;
        }
    }
}

