using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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
                    this.pictureBox1.Image = newBitmap;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "訊息提示"); }
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
                int[] pixel_mask = new int[9];//影像遮罩
                int pixS;
                //                     [1 1 1]
                //Smoothing Filter Mask[1 1 1]平滑(平均)遮罩
                //                     [1 1 1]
                int[]Smoothing= new int[] {1,1,1,1,1,1,1,1,1};//smoothing mask 平滑(平均)遮罩
                for (int x = 1;x < Width-1;x++)
                    for (int y = 1; y < Height - 1; y++)
                    {
                        pixel_mask[0]=oldBitmap.GetPixel(x-1, y-1).G;
                        pixel_mask[1] = oldBitmap.GetPixel(x , y - 1).G;
                        pixel_mask[2] = oldBitmap.GetPixel(x + 1, y - 1).G;
                        pixel_mask[3] = oldBitmap.GetPixel(x - 1, y ).G;
                        pixel_mask[4] = oldBitmap.GetPixel(x , y).G;
                        pixel_mask[5] = oldBitmap.GetPixel(x + 1, y ).G;
                        pixel_mask[6] = oldBitmap.GetPixel(x - 1, y+ 1).G;
                        pixel_mask[7] = oldBitmap.GetPixel(x , y + 1).G;
                        pixel_mask[8] = oldBitmap.GetPixel(x + 1, y + 1).G;
                        pixS = 0;
                        for (int i = 0; i < 9; i++)
                            pixS += (pixel_mask[i] * Smoothing[i]);
                        pixS /= 9;
                        newBitmap.SetPixel(x,y,Color.FromArgb(pixS, pixS, pixS));
                    }
               
                this.pictureBox2.Image = newBitmap;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "訊息提示"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                int Height = this.pictureBox1.Image.Height;
                int Width = this.pictureBox1.Image.Width;
                Bitmap newBitmap = new Bitmap(Width, Height);
                Bitmap oldBitmap = (Bitmap)this.pictureBox1.Image;
                int[] pixel_mask = new int[9];
                int pixS;                            
                int[] Smoothing = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                for (int x = 1; x < Width - 1; x++)
                    for (int y = 1; y < Height - 1; y++)
                    {
                        pixel_mask[0] = oldBitmap.GetPixel(x - 1, y - 1).G;
                        pixel_mask[1] = oldBitmap.GetPixel(x, y - 1).G;
                        pixel_mask[2] = oldBitmap.GetPixel(x + 1, y - 1).G;
                        pixel_mask[3] = oldBitmap.GetPixel(x - 1, y).G;
                        pixel_mask[4] = oldBitmap.GetPixel(x, y).G;
                        pixel_mask[5] = oldBitmap.GetPixel(x + 1, y).G;
                        pixel_mask[6] = oldBitmap.GetPixel(x - 1, y + 1).G;
                        pixel_mask[7] = oldBitmap.GetPixel(x, y + 1).G;
                        pixel_mask[8] = oldBitmap.GetPixel(x + 1, y + 1).G;
                        pixS = 0;
                        for (int i = 0; i < 9; i++)
                            pixS += (pixel_mask[i] * Smoothing[i]);
                        
                        Array.Sort(pixel_mask);
                        pixS = pixel_mask[4];
                        newBitmap.SetPixel(x, y, Color.FromArgb(pixS, pixS, pixS));
                    }

                this.pictureBox2.Image = newBitmap;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "訊息提示"); }
        }
    }
}
