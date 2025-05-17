using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;


namespace FilmKiralama
{
    public partial class AnaForm : Form
    {
        
        ComboBox turCombo, YonetmenCombo, yilCombo;
        Panel scrollPanel;

        kullaniciProfil kullaniciProfil = new kullaniciProfil();

        public void FilmleriYenile()
        {
            this.Controls.Clear();
            AnaForm_Load(null, null);
        }

        public AnaForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void SetDoubleBuffered(Control c)
        {
            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty(
                "DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        private string GetUserNameFromDatabase(int userId)
        {
            string username = string.Empty;
            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                try
                {
                    baglanti.Open();
                    string query = "SELECT KullaniciAdi FROM Kullanicilar WHERE KullaniciID = @UserID";
                    SqlCommand komut = new SqlCommand(query, baglanti);
                    komut.Parameters.AddWithValue("@UserID", userId);
                    username = komut.ExecuteScalar()?.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}");
                }
            }
            return username;
        }

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Size = new Size(this.ClientSize.Width, 50);
            headerPanel.BackColor = ColorTranslator.FromHtml("#2a2a3b");
            this.Controls.Add(headerPanel);

            string username = CurrentUser.UserName;
            CurrentUser.UserName = username;

            Label welcomeLabel = new Label();
            welcomeLabel.Text = "Merhaba, " + username;
            welcomeLabel.ForeColor = Color.White;
            welcomeLabel.BackColor = ColorTranslator.FromHtml("#2a2a3b");
            welcomeLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold); // FontStyle.SemiBold yerine Bold
            welcomeLabel.Location = new Point(10, 15);
            welcomeLabel.Size = new Size(500, 30);
            headerPanel.Controls.Add(welcomeLabel);

            Button profilButon = new Button();
            profilButon.Text = "Profil";
            profilButon.Size = new Size(100, 30);
            profilButon.Location = new Point(headerPanel.Width - 230, 10);
            profilButon.BackColor = Color.LightSeaGreen;
            profilButon.ForeColor = Color.White;
            profilButon.FlatStyle = FlatStyle.Flat;
            profilButon.Click += (s, args) => {
                kullaniciProfil.ShowDialog();
                FilmleriYenile();
            };
            headerPanel.Controls.Add(profilButon);

            Button cikisButon = new Button();
            cikisButon.Text = "Çıkış Yap";
            cikisButon.Size = new Size(100, 30);
            cikisButon.Location = new Point(headerPanel.Width - 110, 10);
            cikisButon.BackColor = Color.IndianRed;
            cikisButon.ForeColor = Color.White;
            cikisButon.FlatStyle = FlatStyle.Flat;
            cikisButon.Click += (s, args) =>
            {
                if (MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Close();
                    loginForm loginForm = new loginForm();
                    loginForm.Show();
                }
            };
            headerPanel.Controls.Add(cikisButon);

            // Filtre Paneli
            Panel filtrePanel = new Panel();
            filtrePanel.Size = new Size(this.ClientSize.Width, 60);
            filtrePanel.Location = new Point(0, 50);
            filtrePanel.BackgroundImage = Properties.Resources.background;
            this.Controls.Add(filtrePanel);

            Label TurLabel = new Label()
            {
                Text = "Tür",
                Location = new Point(20, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold), // FontStyle.SemiBold yerine Bold
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label YonetmenLabel = new Label()
            {
                Text = "Yönetmen",
                Location = new Point(190, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold), // FontStyle.SemiBold yerine Bold
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            Label YilLabel = new Label()
            {
                Text = "Yıl",
                Location = new Point(360, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold), // FontStyle.SemiBold yerine Bold
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };


            filtrePanel.Controls.AddRange(new Control[] {
                TurLabel,YonetmenLabel,YilLabel
            });

            turCombo = new ComboBox() { Location = new Point(20, 20), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            YonetmenCombo = new ComboBox() { Location = new Point(190, 20), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            yilCombo = new ComboBox() { Location = new Point(360, 20), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            turCombo.Items.Add("Tümü"); YonetmenCombo.Items.Add("Tümü"); yilCombo.Items.Add("Tümü");


            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT Tur FROM Filmler", baglanti);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) turCombo.Items.Add(reader.GetString(0));
                }

                cmd = new SqlCommand("SELECT DISTINCT Yonetmen FROM Filmler", baglanti);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) YonetmenCombo.Items.Add(reader.GetString(0));
                }

                cmd = new SqlCommand("SELECT DISTINCT Yil FROM Filmler", baglanti);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yilCombo.Items.Add(reader.GetInt32(0).ToString()); // int olarak alıp string'e çeviriyoruz
                }

            }

            turCombo.SelectedIndex = 0;
            YonetmenCombo.SelectedIndex = 0;
            yilCombo.SelectedIndex = 0;

            turCombo.SelectedIndexChanged += (s, eventargs) => FiltreleVeListele();
            YonetmenCombo.SelectedIndexChanged += (s, eventargs) => FiltreleVeListele();
            yilCombo.SelectedIndexChanged += (s, eventargs) => FiltreleVeListele();

            filtrePanel.Controls.AddRange(new Control[] { turCombo, YonetmenCombo, yilCombo });

            // Scroll Panel
            int spacing = 10;
            filtrePanel.Location = new Point(0, headerPanel.Bottom + spacing);

            scrollPanel = new Panel();
            SetDoubleBuffered(scrollPanel);
            scrollPanel.BackgroundImage = Properties.Resources.background;
            scrollPanel.BackgroundImageLayout = ImageLayout.Stretch;
            scrollPanel.Location = new Point(0, filtrePanel.Bottom + spacing);
            scrollPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - scrollPanel.Location.Y);
            scrollPanel.AutoScroll = true;

            this.Controls.Add(scrollPanel);

            FiltreleVeListele(); // İlk yükleme
        }

        private void FiltreleVeListele()
        {

            scrollPanel.Controls.Clear();

            List<string> kosullar = new List<string> { "Durum != 'Kirada'" };
            List<SqlParameter> parametreler = new List<SqlParameter>();

            if (turCombo.SelectedIndex > 0)
            {
                kosullar.Add("Tur = @Tur");
                parametreler.Add(new SqlParameter("@Tur", turCombo.SelectedItem));
            }

            if (YonetmenCombo.SelectedIndex > 0)
            {
                kosullar.Add("Yonetmen = @Yonetmen");
                parametreler.Add(new SqlParameter("@Yonetmen", YonetmenCombo.SelectedItem));
            }

            if (yilCombo.SelectedIndex > 0)
            {
                kosullar.Add("Yil = @Yil");
                parametreler.Add(new SqlParameter("@Yil", yilCombo.SelectedItem));
            }

            string whereClause = string.Join(" AND ", kosullar);
            string query = $"SELECT * FROM Filmler WHERE {whereClause}";

            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, baglanti);
                cmd.Parameters.AddRange(parametreler.ToArray());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                int y = 10; // Starting point vertically
                int sayac = 0;

                // Total width of the panel for centering
                int panelWidth = this.ClientSize.Width;
                int totalCardWidth = 240; // Width of one card
                int spacing = 40; // Spacing between cards
                int cardsPerRow = 4; // Number of cards per row

                // Karteksin yüksekliğini belirleyin
                int cardHeight = 480; // Kart yüksekliği
                int cardSpacing = 30; // Kartlar arası boşluk
                int totalRowHeight = cardHeight + cardSpacing; // Satırın toplam yüksekliği

                foreach (DataRow row in dt.Rows)
                {
                    Panel panel = new Panel()
                    {
                        Size = new Size(totalCardWidth, cardHeight), // Kart boyutu
                        BackColor = ColorTranslator.FromHtml("#2a2a3b"),
                        Padding = new Padding(0)
                    };

                    GraphicsPath path = new GraphicsPath();
                    int r = 20;
                    path.AddArc(0, 0, r, r, 180, 90);
                    path.AddArc(panel.Width - r, 0, r, r, 270, 90);
                    path.AddArc(panel.Width - r, panel.Height - r, r, r, 0, 90);
                    path.AddArc(0, panel.Height - r, r, r, 90, 90);
                    panel.Region = new Region(path);

                    // 🎬 Poster
                    PictureBox pic = new PictureBox()
                    {
                        Size = new Size(240, 320),
                        Location = new Point(0, 0),
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };

                    string img = row["Image"].ToString();
                    if (!string.IsNullOrEmpty(img))
                    {
                        try
                        {
                            pic.Load(img);
                        }
                        catch
                        {
                            // Resim yüklenemezse varsayılan bir resim göster
                            pic.Image = Properties.Resources.placeholder; // placeholder: Projeye ekleyeceğin bir varsayılan görsel
                        }
                    }
                    else
                    {
                        pic.Image = Properties.Resources.placeholder;
                    }
                    panel.Controls.Add(pic);

                    // 📝 Information area
                    Label lbl = new Label()
                    {
                        Size = new Size(220, 85),
                        Location = new Point(10, 325),
                        Text = $"{row["Ad"]}\nYönetmen: {row["Yonetmen"]}\n{row["Yil"]} | {row["Tur"]} | {row["GunlukUcret"]} TL",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.White
                    };
                    panel.Controls.Add(lbl);

                    // 🟩 Rent Button
                    Button btn = new Button()
                    {
                        Text = "Kirala",
                        Size = new Size(120, 36),
                        Location = new Point((panel.Width - 120) / 2, 420),
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        BackColor = ColorTranslator.FromHtml("#3cb371"),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Padding = new Padding(0)
                    };
                    btn.FlatAppearance.BorderSize = 0;

                    GraphicsPath buttonPath = new GraphicsPath();
                    buttonPath.AddArc(0, 0, 20, 20, 180, 90); // Top left
                    buttonPath.AddArc(btn.Width - 20, 0, 20, 20, 270, 90); // Top right
                    buttonPath.AddArc(btn.Width - 20, btn.Height - 20, 20, 20, 0, 90); // Bottom right
                    buttonPath.AddArc(0, btn.Height - 20, 20, 20, 90, 90); // Bottom left
                    btn.Region = new Region(buttonPath);

                    int FilmID = Convert.ToInt32(row["FilmID"]);
                    btn.Click += (s, args) =>
                    {
                        KiralamaForm kiralamaForm = new KiralamaForm(FilmID);
                        if (kiralamaForm.ShowDialog() == DialogResult.OK)
                            FiltreleVeListele();
                    };
                    panel.Controls.Add(btn);

                    // Yatay hizalama için x hesaplama
                    int totalWidthForRow = cardsPerRow * (totalCardWidth + spacing) - spacing;
                    int x = (panelWidth - totalWidthForRow) / 2 + (sayac % cardsPerRow) * (totalCardWidth + spacing);

                    panel.Location = new Point(x, y);
                    scrollPanel.Controls.Add(panel);

                    sayac++;

                    // Satır geçişi, her 5 kartta bir
                    if (sayac % cardsPerRow == 0)
                    {
                        y += totalRowHeight; // Kart yüksekliği + boşluk kadar
                    }
                }

            }

        }

    }
}


