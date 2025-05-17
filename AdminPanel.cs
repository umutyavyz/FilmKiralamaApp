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
using FilmKiralama;


namespace FilmKiralama
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }
        private void TextBoxTemizle()
        {
            txtID.Clear();
            txtAd.Clear();
            txtTur.Clear();
            cmbYil.SelectedIndex = -1; // Yıl combobox'ını temizle
            txtYonetmen.Clear();
            txtUcret.Clear();
            txtDurum.Clear();
            txtLink.Clear();
            txtHakkinda.Clear();
            pictureBox1.Image = null; // Resmi temizle
        }
        private bool PlakaZatenVar(string plaka, int? aracID = null)
        {
            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                baglanti.Open();
                string sql = "SELECT COUNT(*) FROM Filmler WHERE Plaka = @Plaka";

                if (aracID.HasValue)
                {
                    // Güncellemede aynı ID'li kaydı hariç tut
                    sql += " AND AracID != @AracID";
                }

                SqlCommand komut = new SqlCommand(sql, baglanti);
                komut.Parameters.AddWithValue("@Plaka", plaka);

                if (aracID.HasValue)
                {
                    komut.Parameters.AddWithValue("@AracID", aracID.Value);
                }

                int count = (int)komut.ExecuteScalar();
                return count > 0;
            }
        }

        private void btnAracDurum_Click(object sender, EventArgs e)
        {
            try
            {
                // Bağlantıyı aç
                using (SqlConnection baglanti = ConnectionManager.GetConnection())
                {
                    baglanti.Open();

                    // Filmler tablosundaki tüm verileri çekecek sorgu
                    SqlCommand komut = new SqlCommand("SELECT * FROM Filmler", baglanti);

                    // Verileri çekmek için SqlDataAdapter kullanıyoruz
                    SqlDataAdapter da = new SqlDataAdapter(komut);

                    // DataTable oluştur
                    DataTable dt = new DataTable();

                    // Verileri DataTable'a doldur
                    da.Fill(dt);

                    // DataGridView'e veri atama
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            for (int yil = DateTime.Now.Year; yil >= 1900; yil--)
            {
                cmbYil.Items.Add(yil.ToString());
            }

            dataGridView1.ClearSelection(); // Form yüklendiğinde seçimi kaldır
            btnGuncelle.Enabled = false; // İlk başta pasif başlasın



            try
            {
                // Bağlantıyı aç
                using (SqlConnection baglanti = ConnectionManager.GetConnection())
                {   
                    baglanti.Open();

                    // Filmler tablosundaki tüm verileri çekecek sorgu
                    SqlCommand komut = new SqlCommand("SELECT * FROM Filmler", baglanti);

                    // Verileri çekmek için SqlDataAdapter kullanıyoruz
                    SqlDataAdapter da = new SqlDataAdapter(komut);

                    // DataTable oluştur
                    DataTable dt = new DataTable();

                    // Verileri DataTable'a doldur
                    da.Fill(dt);

                    // DataGridView'e veri atama
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ReadOnly = true;  // Hücreleri sadece okunur yapar
            dataGridView1.AllowUserToAddRows = false;  // Kullanıcıların yeni satır eklemesini engeller
            dataGridView1.AllowUserToDeleteRows = false;  // Kullanıcıların satır silmesini engeller

        }

        private void btnKullaniciPanel_Click(object sender, EventArgs e)
        {
            KullaniciYonetimPanel kullaniciYonetimPanel = new KullaniciYonetimPanel();
            kullaniciYonetimPanel.ShowDialog();
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void VeriGetir()
        {
            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Filmler", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Butonu aktif et
                btnGuncelle.Enabled = true;

                // Verileri aktar
                txtID.Text = row.Cells["FilmID"].Value.ToString();
                txtAd.Text = row.Cells["Ad"].Value.ToString();
                txtTur.Text = row.Cells["Tur"].Value.ToString();
                cmbYil.Text = row.Cells["Yil"].Value.ToString();
                txtYonetmen.Text = row.Cells["Yonetmen"].Value.ToString();
                txtUcret.Text = row.Cells["GunlukUcret"].Value.ToString();
                txtDurum.Text = row.Cells["Durum"].Value.ToString();
                txtLink.Text = row.Cells["Image"].Value.ToString();
                txtHakkinda.Text = row.Cells["Hakkinda"].Value.ToString();

                try
                {
                    pictureBox1.Load(row.Cells["Image"].Value.ToString());
                }
                catch
                {
                    pictureBox1.Image = null;
                    MessageBox.Show("Resim yüklenemedi.");
                }
            }


        }
        

        
            private void btnGuncelle_Click(object sender, EventArgs e)
        {
           
        }

        private void btnGuncelle_Click_1(object sender, EventArgs e)
        {
            // txtID boşsa güncelleme işlemi yapılmasın
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Lütfen güncellemek için bir araç seçin.");
                return;
            }
            // TextBox'lardan verileri al
            int FilmID = Convert.ToInt32(txtID.Text); // FilmID, düzenlenecek filmin ID'sini tutuyor
            string ad = txtAd.Text;
            string tur = txtTur.Text;
            string yil = cmbYil.Text;
            string yonetmen = txtYonetmen.Text;
            decimal ucret = Convert.ToDecimal(txtUcret.Text);
            string durum = txtDurum.Text;
            string link = txtLink.Text;
            string hakkinda = txtHakkinda.Text;

            try
            {
                using (SqlConnection baglanti = ConnectionManager.GetConnection())
                {           
                    baglanti.Open();

                    // SQL UPDATE komutu
                    string sql = "UPDATE Filmler SET Ad = @Ad, Tur = @Tur, Yil = @Yil, Yonetmen = @Yonetmen, GunlukUcret = @GunlukUcret, Durum = @Durum, Image = @Image, Hakkinda = @Hakkinda WHERE FilmID = @FilmID";

                    SqlCommand komut = new SqlCommand(sql, baglanti);

                    // Parametreleri ekleyelim
                    komut.Parameters.AddWithValue("@Ad", ad);
                    komut.Parameters.AddWithValue("@Tur", tur);
                    komut.Parameters.AddWithValue("@Yil", yil);
                    komut.Parameters.AddWithValue("@Yonetmen", yonetmen);
                    komut.Parameters.AddWithValue("@GunlukUcret", ucret);
                    komut.Parameters.AddWithValue("@Durum", durum);
                    komut.Parameters.AddWithValue("@Image", link);
                    komut.Parameters.AddWithValue("@Hakkinda", hakkinda);
                    komut.Parameters.AddWithValue("@FilmID", FilmID); // Hangi filmin güncelleneceğini belirliyoruz

                   
                    // Güncellemeyi gerçekleştir
                    int rowsAffected = komut.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Film başarıyla güncellendi!");

                        // DataGridView'i güncellemek için tekrar veriyi al
                        VeriGetir();
                    }
                    else
                    {
                        MessageBox.Show("Bir hata oluştu, aracın güncellenmesi başarısız!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void btnAracEkle_Click(object sender, EventArgs e)
        { // ErrorProvider'ı temizle
            errorProvider1.Clear();

            bool alanlarDoluMu = true;

            // Eğer DataGridView'de bir hücre seçiliyse, ekleme yapılmasın
            if (dataGridView1.SelectedCells.Count > 0)
            {
                dataGridView1.ClearSelection(); // Seçili hücre/satır varsa temizler
                TextBoxTemizle(); // Seçili hücre varsa, TextBox'ları temizle
                MessageBox.Show("Bir kayıt seçili durumda. Yeni araç eklemek için seçimler kaldırıldı. Şimdi ekleyebilirsiniz.");

                for (int yil = DateTime.Now.Year; yil >= 1900; yil--)
                {
                    cmbYil.Items.Add(yil.ToString());
                }
                return; // Fonksiyondan çık
            }

            // Eğer ad TextBox'ı boşsa
            if (string.IsNullOrWhiteSpace(txtAd.Text))
            {
                errorProvider1.SetError(txtAd, "Ad boş bırakılamaz.");
                alanlarDoluMu = false;
            }

            // Eğer tür TextBox'ı boşsa
            if (string.IsNullOrWhiteSpace(txtTur.Text))
            {
                errorProvider1.SetError(txtTur, "Tür boş bırakılamaz.");
                alanlarDoluMu = false;
            }
            // Yıl kontrolü
            if (string.IsNullOrWhiteSpace(cmbYil.Text))
            {
                errorProvider1.SetError(cmbYil, "Yıl boş bırakılamaz.");
                alanlarDoluMu = false;
            }

            // Yönetmen kontrolü
            if (string.IsNullOrWhiteSpace(txtYonetmen.Text))
            {
                errorProvider1.SetError(txtYonetmen, "Yönetmen boş bırakılamaz.");
                alanlarDoluMu = false;
            }

            // Ücret kontrolü
            if (string.IsNullOrWhiteSpace(txtUcret.Text))
            {
                errorProvider1.SetError(txtUcret, "Ücret boş bırakılamaz.");
                alanlarDoluMu = false;
            }

            // Link kontrolü
            if (string.IsNullOrWhiteSpace(txtLink.Text))
            {
                errorProvider1.SetError(txtLink, "Image linki boş bırakılamaz.");
                alanlarDoluMu = false;
            }

            // Link kontrolü
            if (string.IsNullOrWhiteSpace(txtHakkinda.Text))
            {
                errorProvider1.SetError(txtHakkinda, "Hakkında kısmı boş bırakılamaz.");
                alanlarDoluMu = false;
            }

            // Eğer alanlar eksikse, işlem yapılmasın
            if (!alanlarDoluMu)
            {
                MessageBox.Show("Lütfen gerekli alanları doldurun.");
                return;
            }

            try
            {
                using (SqlConnection baglanti = ConnectionManager.GetConnection())
                {   
                    baglanti.Open();

                    string sql = @"INSERT INTO Filmler (Ad, Tur, Yil, Yonetmen, GunlukUcret, Durum, Image, Hakkinda)
                       VALUES (@Ad, @Tur, @Yil, @Yonetmen, @GunlukUcret, @Durum, @Image , @Hakkinda)";

                    SqlCommand komut = new SqlCommand(sql, baglanti);
                    komut.Parameters.AddWithValue("@Ad", txtAd.Text);
                    komut.Parameters.AddWithValue("@Tur", txtTur.Text);
                    komut.Parameters.AddWithValue("@Yil", cmbYil.Text);
                    komut.Parameters.AddWithValue("@Yonetmen", txtYonetmen.Text);
                    komut.Parameters.AddWithValue("@GunlukUcret", txtUcret.Text);
                    komut.Parameters.AddWithValue("@Durum", "Müsait");
                    komut.Parameters.AddWithValue("@Image", txtLink.Text);
                    komut.Parameters.AddWithValue("@Hakkinda", txtHakkinda.Text);

                    komut.ExecuteNonQuery();
                    MessageBox.Show("Film başarıyla eklendi.");

                    VeriGetir(); // DataGridView’i güncelleyen fonksiyonun adı
                    TextBoxTemizle(); // TextBox'ları temizle (varsa)
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hata oluştu: Aynı plakadan birden fazla olamaz." );
            }
        }


        private void btnAracSil_Click(object sender, EventArgs e)
        {
            // Önce ID'yi kontrol edelim
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Lütfen silmek için bir araç seçin.");
                return;
            }

            DialogResult sonuc = MessageBox.Show("Seçili filmi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (sonuc == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection baglanti = ConnectionManager.GetConnection())
                    {
                        baglanti.Open();

                        SqlCommand komut = new SqlCommand("DELETE FROM Filmler WHERE FilmID = @id", baglanti);
                        komut.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                        komut.ExecuteNonQuery();
                    }

                    MessageBox.Show("Film başarıyla silindi.");
                    VeriGetir();   // datagridview verisini güncelle
                    TextBoxTemizle();   // textboxları ve resmi temizle
                    dataGridView1.ClearSelection(); // seçimi temizle
                }
                catch (Exception )
                {
                    MessageBox.Show("Aynı filmden birden fazla bulunamaz.");
                }
            }
        }

        private void btnPanelCikis_Click(object sender, EventArgs e)
        {
            this.Close(); // AdminPanel'i kapat
            // LoginForm örneği oluştur
            loginForm loginForm = new loginForm();
            loginForm.Show(); // LoginForm'u göster
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            btnGuncelle.Enabled = dataGridView1.SelectedCells.Count > 0;
        }

        private void txtYil_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtUcret_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtKm_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void btnKiralanlar_Click(object sender, EventArgs e)
        {
            KiralananFilmlerPanel kiralananAraclarPanel = new KiralananFilmlerPanel();
            kiralananAraclarPanel.ShowDialog();
        }

        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}



