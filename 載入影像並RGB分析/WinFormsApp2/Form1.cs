namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                Bitmap newBitmap2 = new Bitmap(Width, Height);
                Bitmap newBitmap3 = new Bitmap(Width, Height);
                Bitmap newBitmap4 = new Bitmap(Width, Height);
                Bitmap newBitmap5 = new Bitmap(Width, Height);
                Bitmap oldBitmap = (Bitmap)this.pictureBox1.Image;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                Color pixel;
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        pixel = oldBitmap.GetPixel(x, y);
                        int r, g, b,Result = 0;
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;

                        newBitmap2.SetPixel(x, y, Color.FromArgb(r, 0, 0));
                        newBitmap3.SetPixel(x, y, Color.FromArgb(0, g, 0));
                        newBitmap4.SetPixel(x, y, Color.FromArgb(0, 0, b));
                        Result = (299 * r + 587 * g + 114 * b) / 1000;
                        newBitmap5.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                    }
                this.pictureBox2.Image = newBitmap2;
                this.pictureBox3.Image = newBitmap3;
                this.pictureBox4.Image = newBitmap4;
                this.pictureBox5.Image = newBitmap5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息顯示");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}