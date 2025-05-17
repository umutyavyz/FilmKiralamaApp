using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FilmKiralama
{
    public partial class kullaniciProfil : Form
    {
        public kullaniciProfil()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private Panel headerPanel;
        private Panel scrollPanel;
        private Label lblKullaniciAdi;

        private ComboBox turCombo, YonetmenCombo, yilCombo;

        private void kullaniciProfil_Load(object sender, EventArgs e)
        {
            HeaderPanelOlustur();
            KiralananFilmleriGoster();
            FiltreleVeListele();
        }

        private void HeaderPanelOlustur()
        {
            if (headerPanel != null && this.Controls.Contains(headerPanel))
                return; // Zaten oluşturulmuş, tekrar oluşturma

            headerPanel = new Panel();
            headerPanel.Size = new Size(this.ClientSize.Width, 80);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.BackColor = Color.SteelBlue;
            this.Controls.Add(headerPanel);

            lblKullaniciAdi = new Label();
            lblKullaniciAdi.Text = $"Hoş geldin, {CurrentUser.UserName}";
            lblKullaniciAdi.Font = new Font("Arial", 14, FontStyle.Bold);
            lblKullaniciAdi.ForeColor = Color.White;
            lblKullaniciAdi.Location = new Point(20, 25);
            lblKullaniciAdi.AutoSize = true;
            headerPanel.Controls.Add(lblKullaniciAdi);

            Button btnSifreDegistir = new Button();
            btnSifreDegistir.Text = "Şifre Değiştir";
            btnSifreDegistir.Size = new Size(120, 35);
            btnSifreDegistir.Location = new Point(headerPanel.Width - 140, 22);
            btnSifreDegistir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSifreDegistir.BackColor = Color.White;
            btnSifreDegistir.ForeColor = Color.SteelBlue;
            btnSifreDegistir.FlatStyle = FlatStyle.Flat;
            btnSifreDegistir.Click += BtnSifreDegistir_Click;
            headerPanel.Controls.Add(btnSifreDegistir);
        }

        private void BtnSifreDegistir_Click(object sender, EventArgs e)
        {
            sifreDegistirmeForm sifreDegistirmeForm = new sifreDegistirmeForm();
            sifreDegistirmeForm.ShowDialog();
        }

        private void KiralananFilmleriGoster()
        {
            scrollPanel = new Panel();
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.AutoScroll = true;
            scrollPanel.BackgroundImage = Properties.Resources.background;
            scrollPanel.Padding = new Padding(0, 10, 0, 0); // Header panelden sonra 10px boşluk

            this.Controls.Add(scrollPanel);
            this.Controls.SetChildIndex(scrollPanel, 1); // HeaderPanel'in altına gelsin

            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                baglanti.Open();

                string query = @"
                 SELECT k.KiralamaID, k.FilmID, k.AlisTarihi, k.IadeTarihi, k.Ucret,
                        a.Ad, a.Tur, a.Yil, a.Yonetmen, a.Image
                 FROM Kiralama k
                 INNER JOIN Filmler a ON k.FilmID = a.FilmID
                 WHERE k.KullaniciID = @kullaniciId";

                SqlCommand cmd = new SqlCommand(query, baglanti);
                cmd.Parameters.AddWithValue("@kullaniciId", CurrentUser.UserId);

                SqlDataReader reader = cmd.ExecuteReader();

                int x = 20;
                int y = headerPanel.Bottom + 20;

                scrollPanel.AutoScroll = true;

                while (reader.Read())
                {
                    Panel kiralamaPanel = new Panel();
                    kiralamaPanel.Size = new Size(600, 210);
                    kiralamaPanel.Location = new Point(x, y);
                    kiralamaPanel.BackColor = Color.FromArgb(230, 230, 230);

                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(150, 210);
                    pic.Location = new Point(0, 0);
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    string imgUrl = reader["Image"].ToString();
                    if (!string.IsNullOrEmpty(imgUrl))
                        pic.Load(imgUrl);
                    kiralamaPanel.Controls.Add(pic);

                    Label detay = new Label();
                    detay.Location = new Point(180, 20);
                    detay.Size = new Size(230, 140);
                    detay.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    detay.Text = $"Film: {reader["Ad"]} - {reader["Tur"]}\n\n" +
                                 $"Alış Tarihi: {Convert.ToDateTime(reader["AlisTarihi"]).ToShortDateString()}\n\n" +
                                 $"İade Tarihi: {Convert.ToDateTime(reader["IadeTarihi"]).ToShortDateString()}\n\n" +
                                 $"Toplam Ücret: {reader["Ucret"]} TL";
                    kiralamaPanel.Controls.Add(detay);

                    Button iptalBtn = new Button();
                    iptalBtn.Text = "Kiralamayı İptal Et";
                    iptalBtn.Size = new Size(150, 40);
                    iptalBtn.Location = new Point(420, 140);
                    iptalBtn.BackColor = Color.IndianRed;
                    iptalBtn.ForeColor = Color.White;
                    iptalBtn.FlatStyle = FlatStyle.Flat;

                    int kiralamaId = Convert.ToInt32(reader["KiralamaID"]);
                    int FilmID = Convert.ToInt32(reader["FilmID"]);

                    iptalBtn.Click += (s, args) =>
                    {
                        IptalEtVeYenile(kiralamaId, FilmID);
                    };

                    kiralamaPanel.Controls.Add(iptalBtn);
                    scrollPanel.Controls.Add(kiralamaPanel);

                    // Panelin altından + boşlukla bir sonrakini yerleştir
                    y = kiralamaPanel.Bottom + 20;
                }


                reader.Close();
            }
        }

        private void IptalEtVeYenile(int kiralamaId, int FilmID)
        {
            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                try
                {
                    baglanti.Open();

                    string deleteQuery = "DELETE FROM Kiralama WHERE KiralamaID = @kid";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, baglanti);
                    deleteCmd.Parameters.AddWithValue("@kid", kiralamaId);
                    deleteCmd.ExecuteNonQuery();

                    string updateQuery = "UPDATE Filmler SET Durum = 'Müsait' WHERE FilmID = @fid";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, baglanti);
                    updateCmd.Parameters.AddWithValue("@fid", FilmID);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Kiralama başarıyla iptal edildi.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    scrollPanel.Controls.Clear();
                    KiralananFilmleriGoster();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "İşlem Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FiltreleVeListele()
        {
            // Create a filtering panel for vehicles
            Panel filtrePanel = new Panel();
            filtrePanel.Size = new Size(this.ClientSize.Width, 60);
            filtrePanel.Location = new Point(0, headerPanel.Bottom + 20);
            filtrePanel.BackgroundImage = Properties.Resources.background;

            this.Controls.Add(filtrePanel);

            Label TurLabel = new Label()
            {
                Text = "Tür",
                Location = new Point(20, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label YonetmenLabel = new Label()
            {
                Text = "Yönetmen",
                Location = new Point(190, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label YilLabel = new Label()
            {
                Text = "Yıl",
                Location = new Point(360, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            filtrePanel.Controls.AddRange(new Control[] { TurLabel, YonetmenLabel, YilLabel });
            {
                turCombo = new ComboBox() { Location = new Point(20, 20), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
                YonetmenCombo = new ComboBox() { Location = new Point(190, 20), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
                yilCombo = new ComboBox() { Location = new Point(360, 20), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

                turCombo.Items.Add("Tümü");
                YonetmenCombo.Items.Add("Tümü");
                yilCombo.Items.Add("Tümü");
            }
        }
    }
}

