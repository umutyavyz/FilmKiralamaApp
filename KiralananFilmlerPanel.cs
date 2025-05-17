using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FilmKiralama
{
    public partial class KiralananFilmlerPanel : Form
    {
        public KiralananFilmlerPanel()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private Panel filtrePanel;
        private ComboBox cbTur, cbYil, cbYonetmen;
        private string secilenTur = "Tümü";
        private string secilenYil = "Tümü";
        private string secilenYonetmen = "Tümü";

        private void KiralananAraclarPanel_Load(object sender, EventArgs e)
        {
            FiltrePaneliOlustur();
            KiralananFilmleriGoster();
        }

        private void FiltrePaneliOlustur()
        {
            filtrePanel = new Panel();
            filtrePanel.Size = new Size(this.Width, 60);
            filtrePanel.Location = new Point(0, 0);
            filtrePanel.BackColor = Color.FromArgb(245, 245, 245);
            filtrePanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(filtrePanel);

            Label lbl1 = new Label() { Text = "Tür:", Location = new Point(10, 20) };
            lbl1.Size = new Size(50, 15);
            cbTur = new ComboBox() { Location = new Point(60, 15), Width = 120 };
            cbTur.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lbl2 = new Label() { Text = "Yıl:", Location = new Point(200, 20) };
            lbl2.Size = new Size(58, 15);
            cbYil = new ComboBox() { Location = new Point(260, 15), Width = 120 };
            cbYil.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lbl3 = new Label() { Text = "Yönetmen:", Location = new Point(410, 20) };
            lbl3.Size = new Size(80, 15);
            cbYonetmen = new ComboBox() { Location = new Point(490, 15), Width = 120 };
            cbYonetmen.DropDownStyle = ComboBoxStyle.DropDownList;

            Button filtreleBtn = new Button()
            {
                Text = "Filtrele",
                Location = new Point(640, 12),
                Size = new Size(100, 30),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            filtreleBtn.Click += (s, e) =>
            {
                secilenTur = cbTur.SelectedItem?.ToString() ?? "Tümü";
                secilenYil = cbYil.SelectedItem?.ToString() ?? "Tümü";
                secilenYonetmen = cbYonetmen.SelectedItem?.ToString() ?? "Tümü" ;
                KiralananFilmleriGoster();
            };

            filtrePanel.Controls.AddRange(new Control[] { lbl1, cbTur, lbl2, cbYil, lbl3, cbYonetmen, filtreleBtn });
            TurYilYonetmenDoldur();
        }

        private void TurYilYonetmenDoldur()
        {
            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
                baglanti.Open();

                void Doldur(string sorgu, ComboBox cb)
                {
                    SqlCommand cmd = new SqlCommand(sorgu, baglanti);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    cb.Items.Clear();
                    cb.Items.Add("Tümü"); // boş seçenek
                    while (rdr.Read()) cb.Items.Add(rdr[0].ToString());
                    rdr.Close();
                }

                Doldur("SELECT DISTINCT Tur FROM Filmler", cbTur);
                Doldur("SELECT DISTINCT Yil FROM Filmler", cbYil);
                Doldur("SELECT DISTINCT Yonetmen FROM Filmler", cbYonetmen);
            }
        }

        private void KiralananFilmleriGoster()
        {
             Panel panel = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Dock == DockStyle.Fill);
    if (panel == null)
    {
        panel = new Panel();
        panel.Dock = DockStyle.Fill;
        panel.AutoScroll = true;
        this.Controls.Add(panel);
    }
    else
    {
        panel.Controls.Clear();
    }

            using (SqlConnection baglanti = ConnectionManager.GetConnection())
            {
        baglanti.Open();

        string query = @"
SELECT k.KiralamaID, k.FilmID, k.AlisTarihi, k.IadeTarihi, k.Ucret,
       a.Ad, a.Tur, a.Image, a.Yil, a.Yonetmen,
       ku.KullaniciID, ku.KullaniciAdi
FROM Kiralama k
INNER JOIN Filmler a ON k.FilmID = a.FilmID
INNER JOIN Kullanicilar ku ON k.KullaniciID = ku.KullaniciID
WHERE 1 = 1";

        if (secilenTur != "Tümü")
            query += " AND a.Ad = @Ad";
        if (secilenYil != "Tümü")
            query += " AND a.Tur = @Tur";
        if (secilenYonetmen != "Tümü")
            query += " AND a.Yonetmen = @Yonetmen";

        SqlCommand cmd = new SqlCommand(query, baglanti);

        if (secilenTur != "Tümü")
            cmd.Parameters.AddWithValue("@Ad", secilenTur);
        if (secilenYil != "Tümü")
            cmd.Parameters.AddWithValue("@Tur", secilenYil);
        if (secilenYonetmen != "Tümü")
            cmd.Parameters.AddWithValue("@Yonetmen", secilenYonetmen);

        SqlDataReader reader = cmd.ExecuteReader();

        int x = 20;
        int y = filtrePanel.Bottom+20;
        int count = 0;

        while (reader.Read())
        {
            Panel kiralamaPanel = new Panel();
            kiralamaPanel.Size = new Size(600, 220);
            kiralamaPanel.Location = new Point(x, y);
            kiralamaPanel.BackColor = Color.FromArgb(230, 230, 230);

            PictureBox pic = new PictureBox();
            pic.Size = new Size(150, 220);
            pic.Location = new Point(0, 0);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            string imgUrl = reader["Image"].ToString();
            if (!string.IsNullOrEmpty(imgUrl))
                pic.Load(imgUrl);
            kiralamaPanel.Controls.Add(pic);

                    Label detay = new Label();
                    detay.Location = new Point(180, 20);
                    detay.Size = new Size(225, 190);
                    detay.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    detay.Text = $"Ad: {reader["Ad"]}\n\n" +
                                 $"Alış Tarihi: {Convert.ToDateTime(reader["AlisTarihi"]).ToShortDateString()}\n\n" +
                                 $"İade Tarihi: {Convert.ToDateTime(reader["IadeTarihi"]).ToShortDateString()}\n\n" +
                                 $"Toplam Ücret: {reader["Ucret"]} TL\n\n" +
                                 $"Kiracı: {reader["KullaniciAdi"]} (ID: {reader["KullaniciID"]})";

                    kiralamaPanel.Controls.Add(detay);


                    Button iptalBtn = new Button();
            iptalBtn.Text = "Kiralamayı İptal Et";
            iptalBtn.Size = new Size(150, 40);
            iptalBtn.Location = new Point(420, 150);
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
            panel.Controls.Add(kiralamaPanel);

            count++;

            if (count % 2 == 0)
            {
                x = 20;
                y += 200;
            }
            else
            {
                x = 650;
            }
        }

        reader.Close();
    }

    // Filtre panelini en üste getir
    if (filtrePanel != null)
        this.Controls.SetChildIndex(filtrePanel, 0);
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

                    string updateQuery = "UPDATE Filmler SET Durum = 'Müsait' WHERE FilmID = @aid";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, baglanti);
                    updateCmd.Parameters.AddWithValue("@aid", FilmID);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Kiralama başarıyla iptal edildi.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Controls.Clear();
                    KiralananAraclarPanel_Load(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }
        }
    }
}

