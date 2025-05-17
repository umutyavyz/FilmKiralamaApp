using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using FilmKiralama;


namespace FilmKiralama
{
    public partial class loginForm : Form
    {
        // Buraya veritabanı dosyanın tam yolunu yaz

        SqlConnection baglanti = ConnectionManager.GetConnection();


        public loginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void txtKullanici_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginForm_Load(object sender, EventArgs e)
        {

            var images = new[]
          {
                Properties.Resources.wicked, 
                Properties.Resources.fight_club,
                Properties.Resources.breaking_bad,
                Properties.Resources.godfather,
                Properties.Resources.lalaland,
            };
            Random random = new Random();
            int randomIndex = random.Next(images.Length);

            // PictureBox'a rastgele seçilen resmi ata
            pictureBox1.Image = images[randomIndex];
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;


            this.FormBorderStyle = FormBorderStyle.None;

            // Çıkış ve diğer düğmeleri gizle
            this.ControlBox = false;
            this.MaximizeBox = false; // Maksimize butonunu gizle
            this.MinimizeBox = false; // Minimize butonunu gizle

            Color butonRenk = ColorTranslator.FromHtml("#ea2842");
            btnOlustur2.ForeColor = butonRenk;
            btnGiris2.PrimaryColor = butonRenk;
            btnGiris2.Font = new Font(btnGiris2.Font.FontFamily, 9, FontStyle.Bold);
            // Butonun kenarlarını kaldır
        }

        private void btnHesapOlustur_Click(object sender, EventArgs e)
        {
            registerForm kayitFormu = new registerForm();
            kayitFormu.StartPosition = FormStartPosition.CenterParent;
            kayitFormu.ShowDialog(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void loginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnGiris2_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullanici.Text.Trim();
            string sifre = txtSifre2.Text.Trim();

            try
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("SELECT * FROM Kullanicilar WHERE KullaniciAdi = @adi AND Sifre = @sifre", baglanti);
                komut.Parameters.AddWithValue("@adi", kullaniciAdi);
                komut.Parameters.AddWithValue("@sifre", sifre);

                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read())
                {
                    CurrentUser.UserId = (int)dr["KullaniciID"];
                    CurrentUser.UserName = dr["KullaniciAdi"].ToString();
                    string rol = dr["Rol"].ToString();

                    if (rol == "Admin")
                    {
                        AdminPanel adminPanel = new AdminPanel();
                        adminPanel.Show();
                        this.Hide();
                    }
                    else
                    {
                        AnaForm ana = new AnaForm();
                        ana.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void foxLabel4_MouseEnter(object sender, EventArgs e)
        {
            btnOlustur2.Cursor = Cursors.Hand; // El şeklinde imleç
        }

        private void foxLabel4_MouseLeave(object sender, EventArgs e)
        {
            btnOlustur2.Cursor = Cursors.Default; // Eski haline döner
        }

        private void btnOlustur2_Click(object sender, EventArgs e)
        {
            registerForm kayitFormu = new registerForm();
            kayitFormu.StartPosition = FormStartPosition.CenterParent;
            kayitFormu.ShowDialog(this);
        }

        private void lostButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lostButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}

