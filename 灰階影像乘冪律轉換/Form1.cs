namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image image;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = image;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int Height = this.pictureBox1.Image.Height;
                int Width = this.pictureBox1.Image.Width;
                Bitmap newBitmap = new Bitmap(Width, Height);
                Bitmap oldBitmap = (Bitmap)this.pictureBox1.Image;
                Color pixel;
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        pixel = oldBitmap.GetPixel(x, y);
                        int r, g, b, Result = 0;
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;
                        Result = (299 * r + 587 * g + 114 * b) / 1000;
                        newBitmap.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                    }
                this.pictureBox2.Image = newBitmap;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "訊息提示"); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int Height = this.pictureBox2.Image.Height;
                int Width = this.pictureBox2.Image.Width;
                Bitmap newBitmap = new Bitmap(Width, Height);
                Bitmap oldBitmap = (Bitmap)this.pictureBox2.Image;
                int[] xferFunc = new int[256];
                double pow255, gamma;
                gamma = Convert.ToDouble(textBox1.Text);
                if (gamma < 0) gamma = -gamma;
                else if (gamma > 100) gamma = 100;
                pow255 = Math.Pow(255.0, gamma);
                for (int x = 0; x < 255; x++)
                    xferFunc[x] = (int)(Math.Pow((double)x, gamma) / pow255 * 255 + 0.5);
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        int Result = xferFunc[oldBitmap.GetPixel(x, y).G];
                        newBitmap.SetPixel(x, y, Color.FromArgb(Result, Result, Result));

                    }
                this.pictureBox3.Image = newBitmap;
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息提示");
            }
        }
    }
}