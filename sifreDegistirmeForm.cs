using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FilmKiralama
{
    public partial class sifreDegistirmeForm : Form
    {
        public sifreDegistirmeForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void btnSifreDegistir_Click(object sender, EventArgs e)
        {
            // Kullanıcı bilgilerini al
            string mevcutSifre = txtMevcutSifre.Text.Trim();
            string yeniSifre = txtYeniSifre.Text.Trim();
            string yeniSifreTekrar = txtYeniSifreTekrar.Text.Trim();

            // Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(mevcutSifre) ||
                string.IsNullOrWhiteSpace(yeniSifre) ||
                string.IsNullOrWhiteSpace(yeniSifreTekrar))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Yeni şifrelerin eşleşip eşleşmediğini kontrol et
            if (yeniSifre != yeniSifreTekrar)
            {
                MessageBox.Show("Yeni şifreler birbirini tutmuyor. Lütfen aynı şifreyi girin.", "Şifre Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mevcut kullanıcıyı CurrentUser sınıfından al
            string kullaniciAdi = CurrentUser.UserName;

            string checkQuery = "SELECT Sifre FROM Kullanicilar WHERE KullaniciAdi = @kadi";
            string updateQuery = "UPDATE Kullanicilar SET Sifre = @yeniSifre WHERE KullaniciAdi = @kadi";

            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                // Mevcut şifreyi kontrol et
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, baglanti))
                {
                    checkCmd.Parameters.AddWithValue("@kadi", kullaniciAdi);

                    try
                    {
                        baglanti.Open();
                        string dbMevcutSifre = "";

                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dbMevcutSifre = reader["Sifre"].ToString().Trim(); // Veritabanındaki şifreyi da temizle
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        // Mevcut şifre doğruysa, şifreyi güncelle
                        if (dbMevcutSifre == mevcutSifre)
                        {
                            // Şifreyi güncelle
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, baglanti))
                            {
                                updateCmd.Parameters.AddWithValue("@kadi", kullaniciAdi);
                                updateCmd.Parameters.AddWithValue("@yeniSifre", yeniSifre);

                                int result = updateCmd.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    MessageBox.Show("Şifre başarıyla değiştirildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Şifre değiştirilemedi. Lütfen tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Mevcut şifreniz yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }
        }
    }
}

