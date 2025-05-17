
namespace FilmKiralama
   
{
    partial class loginForm
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            this.dungeonLabel1 = new ReaLTaiizor.Controls.DungeonLabel();
            this.txtKullanici = new ReaLTaiizor.Controls.HopeTextBox();
            this.txtSifre2 = new ReaLTaiizor.Controls.HopeTextBox();
            this.foxLabel1 = new ReaLTaiizor.Controls.FoxLabel();
            this.foxLabel2 = new ReaLTaiizor.Controls.FoxLabel();
            this.btnGiris2 = new ReaLTaiizor.Controls.HopeButton();
            this.foxLabel3 = new ReaLTaiizor.Controls.FoxLabel();
            this.btnOlustur2 = new ReaLTaiizor.Controls.FoxLabel();
            this.button1 = new ReaLTaiizor.Controls.Button();
            this.button2 = new ReaLTaiizor.Controls.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::FilmKiralama.Properties.Resources.lalaland;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(545, 737);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // bigLabel1
            // 
            this.bigLabel1.AutoSize = true;
            this.bigLabel1.BackColor = System.Drawing.Color.Transparent;
            this.bigLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.bigLabel1.ForeColor = System.Drawing.Color.White;
            this.bigLabel1.Location = new System.Drawing.Point(592, 65);
            this.bigLabel1.Name = "bigLabel1";
            this.bigLabel1.Size = new System.Drawing.Size(285, 45);
            this.bigLabel1.TabIndex = 8;
            this.bigLabel1.Text = "Tekrar hoş geldin,";
            // 
            // dungeonLabel1
            // 
            this.dungeonLabel1.AutoSize = true;
            this.dungeonLabel1.BackColor = System.Drawing.Color.Transparent;
            this.dungeonLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dungeonLabel1.ForeColor = System.Drawing.Color.DarkGray;
            this.dungeonLabel1.Location = new System.Drawing.Point(596, 126);
            this.dungeonLabel1.Name = "dungeonLabel1";
            this.dungeonLabel1.Size = new System.Drawing.Size(158, 20);
            this.dungeonLabel1.TabIndex = 9;
            this.dungeonLabel1.Text = "Hesabınıza giriş yapın";
            // 
            // txtKullanici
            // 
            this.txtKullanici.BackColor = System.Drawing.Color.White;
            this.txtKullanici.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.txtKullanici.BorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.txtKullanici.BorderColorB = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.txtKullanici.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtKullanici.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.txtKullanici.Hint = "";
            this.txtKullanici.Location = new System.Drawing.Point(641, 242);
            this.txtKullanici.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKullanici.MaxLength = 32767;
            this.txtKullanici.Multiline = false;
            this.txtKullanici.Name = "txtKullanici";
            this.txtKullanici.PasswordChar = '\0';
            this.txtKullanici.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtKullanici.SelectedText = "";
            this.txtKullanici.SelectionLength = 0;
            this.txtKullanici.SelectionStart = 0;
            this.txtKullanici.Size = new System.Drawing.Size(351, 38);
            this.txtKullanici.TabIndex = 11;
            this.txtKullanici.TabStop = false;
            this.txtKullanici.UseSystemPasswordChar = false;
            // 
            // txtSifre2
            // 
            this.txtSifre2.BackColor = System.Drawing.Color.White;
            this.txtSifre2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.txtSifre2.BorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.txtSifre2.BorderColorB = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.txtSifre2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSifre2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.txtSifre2.Hint = "";
            this.txtSifre2.Location = new System.Drawing.Point(641, 335);
            this.txtSifre2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSifre2.MaxLength = 32767;
            this.txtSifre2.Multiline = false;
            this.txtSifre2.Name = "txtSifre2";
            this.txtSifre2.PasswordChar = '\0';
            this.txtSifre2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSifre2.SelectedText = "";
            this.txtSifre2.SelectionLength = 0;
            this.txtSifre2.SelectionStart = 0;
            this.txtSifre2.Size = new System.Drawing.Size(351, 38);
            this.txtSifre2.TabIndex = 12;
            this.txtSifre2.TabStop = false;
            this.txtSifre2.UseSystemPasswordChar = true;
            // 
            // foxLabel1
            // 
            this.foxLabel1.BackColor = System.Drawing.Color.Transparent;
            this.foxLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.foxLabel1.ForeColor = System.Drawing.Color.White;
            this.foxLabel1.Location = new System.Drawing.Point(641, 212);
            this.foxLabel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.foxLabel1.Name = "foxLabel1";
            this.foxLabel1.Size = new System.Drawing.Size(113, 25);
            this.foxLabel1.TabIndex = 13;
            this.foxLabel1.Text = "Kullanıcı Adı";
            // 
            // foxLabel2
            // 
            this.foxLabel2.BackColor = System.Drawing.Color.Transparent;
            this.foxLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.foxLabel2.ForeColor = System.Drawing.Color.White;
            this.foxLabel2.Location = new System.Drawing.Point(641, 303);
            this.foxLabel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.foxLabel2.Name = "foxLabel2";
            this.foxLabel2.Size = new System.Drawing.Size(92, 25);
            this.foxLabel2.TabIndex = 14;
            this.foxLabel2.Text = "Şifre";
            // 
            // btnGiris2
            // 
            this.btnGiris2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnGiris2.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            this.btnGiris2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGiris2.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnGiris2.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnGiris2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGiris2.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnGiris2.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnGiris2.Location = new System.Drawing.Point(635, 420);
            this.btnGiris2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGiris2.Name = "btnGiris2";
            this.btnGiris2.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnGiris2.Size = new System.Drawing.Size(357, 39);
            this.btnGiris2.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnGiris2.TabIndex = 15;
            this.btnGiris2.Text = "Giriş Yap";
            this.btnGiris2.TextColor = System.Drawing.Color.White;
            this.btnGiris2.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnGiris2.Click += new System.EventHandler(this.btnGiris2_Click);
            // 
            // foxLabel3
            // 
            this.foxLabel3.BackColor = System.Drawing.Color.Transparent;
            this.foxLabel3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.foxLabel3.ForeColor = System.Drawing.Color.White;
            this.foxLabel3.Location = new System.Drawing.Point(660, 498);
            this.foxLabel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.foxLabel3.Name = "foxLabel3";
            this.foxLabel3.Size = new System.Drawing.Size(155, 31);
            this.foxLabel3.TabIndex = 17;
            this.foxLabel3.Text = "Hesabın yok mu?";
            // 
            // btnOlustur2
            // 
            this.btnOlustur2.BackColor = System.Drawing.Color.Transparent;
            this.btnOlustur2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOlustur2.ForeColor = System.Drawing.Color.White;
            this.btnOlustur2.Location = new System.Drawing.Point(813, 498);
            this.btnOlustur2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOlustur2.Name = "btnOlustur2";
            this.btnOlustur2.Size = new System.Drawing.Size(173, 31);
            this.btnOlustur2.TabIndex = 18;
            this.btnOlustur2.Text = "Hesap Oluştur";
            this.btnOlustur2.Click += new System.EventHandler(this.btnOlustur2_Click);
            this.btnOlustur2.MouseEnter += new System.EventHandler(this.foxLabel4_MouseEnter);
            this.btnOlustur2.MouseLeave += new System.EventHandler(this.foxLabel4_MouseLeave);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.button1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.button1.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.Image = null;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.button1.Location = new System.Drawing.Point(1043, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.button1.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.button1.Size = new System.Drawing.Size(77, 33);
            this.button1.TabIndex = 21;
            this.button1.Text = "X";
            this.button1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.button2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.EnteredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.button2.EnteredColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button2.Image = null;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.button2.Location = new System.Drawing.Point(949, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.PressedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.button2.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.button2.Size = new System.Drawing.Size(96, 33);
            this.button2.TabIndex = 22;
            this.button2.Text = "-";
            this.button2.TextAlignment = System.Drawing.StringAlignment.Center;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // loginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::FilmKiralama.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(1096, 737);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOlustur2);
            this.Controls.Add(this.foxLabel3);
            this.Controls.Add(this.btnGiris2);
            this.Controls.Add(this.foxLabel2);
            this.Controls.Add(this.foxLabel1);
            this.Controls.Add(this.txtKullanici);
            this.Controls.Add(this.dungeonLabel1);
            this.Controls.Add(this.bigLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtSifre2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "loginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Film Kiralama";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.loginForm_FormClosing);
            this.Load += new System.EventHandler(this.loginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel1;
        private ReaLTaiizor.Controls.HopeTextBox txtKullanici;
        private ReaLTaiizor.Controls.HopeTextBox txtSifre2;
        private ReaLTaiizor.Controls.FoxLabel foxLabel1;
        private ReaLTaiizor.Controls.FoxLabel foxLabel2;
        private ReaLTaiizor.Controls.HopeButton btnGiris2;
        private ReaLTaiizor.Controls.FoxLabel foxLabel3;
        private ReaLTaiizor.Controls.FoxLabel btnOlustur2;
        private ReaLTaiizor.Controls.Button button1;
        private ReaLTaiizor.Controls.Button button2;
    }
}



