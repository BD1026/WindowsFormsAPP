using System.Linq.Expressions;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "圖像文件(JPeg,Gif,Bmp,etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*tif;*tiff;*.png|所有文件(*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap MyBitmap = new Bitmap(openFileDialog.FileName);
                    this.pictureBox1.Image = MyBitmap;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息顯示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int Height = this.pictureBox1.Image.Height;
                int Width = this.pictureBox1.Image.Width;
                Bitmap newBitmap = new Bitmap(Width, Height);
                Bitmap newBitmap0 = new Bitmap(Width, Height);
                Bitmap newBitmap1 = new Bitmap(Width, Height);
                Bitmap newBitmap2 = new Bitmap(Width, Height);
                Bitmap newBitmap3 = new Bitmap(Width, Height);
                Bitmap newBitmap4 = new Bitmap(Width, Height);
                Bitmap newBitmap5 = new Bitmap(Width, Height);
                Bitmap newBitmap6 = new Bitmap(Width, Height);
                Bitmap newBitmap7 = new Bitmap(Width, Height);
                Bitmap oldBitmap = (Bitmap)this.pictureBox1.Image;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
                Color pixel;
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        pixel = oldBitmap.GetPixel(x, y);
                        int r, g, b, Result = 0;
                        int tmp;
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;
                        Result = (299 * r + 587 * g + 114 * b) / 1000;
                        newBitmap.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                        tmp = Result & 0x00;
                        if (tmp != 0) newBitmap0.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap0.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        tmp = Result & 0x00;
                        if (tmp != 0) newBitmap1.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap1.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        tmp = Result & 0x00;
                        if (tmp != 0) newBitmap2.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap2.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        tmp = Result & 0x00;
                        if (tmp != 0) newBitmap3.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap3.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        tmp = Result & 0x10;
                        if (tmp != 0) newBitmap4.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap4.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        tmp = Result & 0x20;
                        if (tmp != 0) newBitmap5.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap5.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        tmp = Result & 0x40;
                        if (tmp != 0) newBitmap6.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap6.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        tmp = Result & 0x80;
                        if (tmp != 0) newBitmap7.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        else newBitmap7.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                this.pictureBox2.Image = newBitmap;
                this.pictureBox3.Image = newBitmap0;
                this.pictureBox4.Image = newBitmap1;
                this.pictureBox5.Image = newBitmap2;
                this.pictureBox6.Image = newBitmap3;
                this.pictureBox7.Image = newBitmap4;
                this.pictureBox8.Image = newBitmap5;
                this.pictureBox9.Image = newBitmap6;
                this.pictureBox10.Image = newBitmap7;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息提示");
            }
        }


        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}