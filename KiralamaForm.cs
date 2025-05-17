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
    public partial class KiralamaForm : Form
    {
        private int FilmID;
        private int gunSayisi = 1;
        private decimal gunlukUcret = 0;



        public KiralamaForm(int selectedFilmId)
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            FilmID = selectedFilmId;
            txtGunSayisi.Text = gunSayisi.ToString();  // varsayılan başlangıç değeri
            LoadFilmBilgileri();


           


            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 12);

            btnArtir.ForeColor = Color.Black;

            btnAzalt.ForeColor = Color.Black;
            btnOdemeyeGec.ForeColor = Color.Black;

            txtGunSayisi.TextAlign = HorizontalAlignment.Center;
            txtGunSayisi.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            txtGunSayisi.ReadOnly = false;

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;

        }
        private void ToplamTutarGuncelle()
        {
            decimal toplam = gunSayisi * gunlukUcret;
            lblToplamTutar.Text = $"Toplam Tutar: {toplam} TL";
        }
        private void LoadFilmBilgileri()
        {
            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                string query = "SELECT * FROM Filmler WHERE FilmID = @id";
                SqlCommand komut = new SqlCommand(query, baglanti);
                komut.Parameters.AddWithValue("@id", FilmID);

                baglanti.Open();
                SqlDataReader reader = komut.ExecuteReader();
                if (reader.Read())
                {
                    // Bilgileri uygun kontrol nesnelerine yaz
                    lblMarka.Text = reader["Ad"].ToString();
                    lblModel.Text = reader["Tur"].ToString();
                    lblYil.Text = reader["Yil"].ToString();
                    lblVites.Text = reader["Yonetmen"].ToString();
                    lblUcret.Text = reader["GunlukUcret"].ToString() + " TL";
                    gunlukUcret = Convert.ToDecimal(reader["GunlukUcret"]);
                    lblHakkinda.Text = reader["Hakkinda"].ToString();
                    try
                    {
                        pictureBox1.Load(reader["Image"].ToString());
                    }
                    catch
                    {
                        pictureBox1.Image = Properties.Resources.placeholder;
                    }
                   
                }
            }
            ToplamTutarGuncelle();

        }

        private void txtGunSayisi_TextChanged(object sender, EventArgs e)
        {
            // Sadece kullanıcı manuel olarak değiştiriyorsa bu çalışsın
            if (int.TryParse(txtGunSayisi.Text, out int sayi))
            {
                if (sayi >= 1)
                {
                    gunSayisi = sayi;
                }
                else
                {
                    gunSayisi = 1;
                    txtGunSayisi.Text = "1";
                }
            }
            else
            {
                gunSayisi = 1;
                txtGunSayisi.Text = "1";
            }

            ToplamTutarGuncelle();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnArtir_Click_1(object sender, EventArgs e)
        {
            lblToplamTutar.Show();
            gunSayisi++;
            txtGunSayisi.Text = gunSayisi.ToString();
            ToplamTutarGuncelle();
        }

        private void btnAzalt_Click_1(object sender, EventArgs e)
        {
            lblToplamTutar.Show();
            if (gunSayisi > 1)
            {
                gunSayisi--;
                txtGunSayisi.Text = gunSayisi.ToString();
                ToplamTutarGuncelle();
            }
        }

        private void txtGunSayisi_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakamlar ve kontrol karakterlerine izin ver (örn: Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void KiralamaForm_Load(object sender, EventArgs e)
        {
            lblHakkinda.AutoSize = false;          // Otomatik boyutlandırmayı kapat
            lblHakkinda.Width = 370;               // Genişlik belirle
            lblHakkinda.Height = 130;              // Yükseklik belirle
            lblHakkinda.MaximumSize = new Size(370, 0);  // Genişlik sabit, yükseklik esnek
            lblHakkinda.AutoSize = true;           // Yazıya göre yükseklik ayarlansın
            lblHakkinda.TextAlign = ContentAlignment.TopLeft; // Yazı hizalama



            pictureBox1.BorderStyle = BorderStyle.None;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Resmi kutuya sığdır

        }

        private void txtGunSayisi_Leave(object sender, EventArgs e)
        {
            txtGunSayisi_TextChanged(sender, e); // aynı kontrolü kullan
        }

        private void txtGunSayisi_Validating(object sender, CancelEventArgs e)
        {
            txtGunSayisi_TextChanged(sender, e); // aynı kontrolü kullan
        }

        private void txtGunSayisi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // ding sesi çıkmasın
                lblToplamTutar.Show();
                txtGunSayisi_TextChanged(sender, e);
            }
        }

        private void btnOdemeyeGec_Click_1(object sender, EventArgs e)
        {
            DateTime baslangicTarihi = DateTime.Today;
            DateTime bitisTarihi = baslangicTarihi.AddDays(gunSayisi);
            decimal toplam = gunSayisi * gunlukUcret;

            string mesaj = $"Toplam Tutar: {toplam} TL\n" +
                           $"Kiralama Başlangıç Tarihi: {baslangicTarihi.ToShortDateString()}\n" +
                           $"Kiralama Bitiş Tarihi: {bitisTarihi.ToShortDateString()}\n\n" +
                           "Kiralamayı onaylıyor musunuz?";

            DialogResult sonuc = MessageBox.Show(mesaj, "Kiralama Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                // Veritabanına kaydetme işlemi
                using (SqlConnection baglanti = ConnectionManager.GetConnection())
                {
                    string query = "INSERT INTO Kiralama (FilmID, AlisTarihi, IadeTarihi, Ucret, KullaniciID) " +
                                   "VALUES (@FilmID, @AlisTarihi, @IadeTarihi, @Ucret, @KullaniciID)";
                    SqlCommand komut = new SqlCommand(query, baglanti);

                    komut.Parameters.AddWithValue("@FilmID", FilmID); // FilmID, seçilen araç
                    komut.Parameters.AddWithValue("@AlisTarihi", baslangicTarihi);
                    komut.Parameters.AddWithValue("@IadeTarihi", bitisTarihi);
                    komut.Parameters.AddWithValue("@Ucret", toplam);

                    // Şu an giriş yapmış kullanıcı ID'si
                    komut.Parameters.AddWithValue("@KullaniciID", CurrentUser.UserId); // KullaniciID, oturum açmış kullanıcının ID'si

                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }

                // Kiralanan aracın durumunu "Kirada" olarak güncelleme
                using (SqlConnection baglanti = ConnectionManager.GetConnection())
                {
                    string queryDurum = "UPDATE Filmler SET Durum = 'Kirada' WHERE FilmID = @FilmID";
                    SqlCommand komutDurum = new SqlCommand(queryDurum, baglanti);
                    komutDurum.Parameters.AddWithValue("@FilmID", FilmID);

                    baglanti.Open();
                    komutDurum.ExecuteNonQuery();
                    baglanti.Close();
                }

                MessageBox.Show("Kiralama işlemi onaylandı ve kaydedildi.Anasayfaya yönlendiriliyorsunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                // Kiralama başarılıysa:
                this.DialogResult = DialogResult.OK; // ✅ Bu satır önemli
                this.Close(); // Formu kapatıyoruz, AnaForm'daki ShowDialog kontrolü çalışacak


            }
            else
            {
                MessageBox.Show("Kiralama iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}

