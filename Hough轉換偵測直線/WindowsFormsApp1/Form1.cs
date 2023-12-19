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
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息顯示");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Bitmap Image|*.bmp";
                saveFileDialog1.Title = "儲存圖片";
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            this.pictureBox3.Image.Save(fs,System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }
                    fs.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息提示");
            }
        }

        struct XYPoint
        {
            public short X;
            public short Y;
        };
        struct LineParameters
        {
            public int Angle;
            public int Distance;
        };

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int Height = this.pictureBox2.Image.Height;
                int Width = this.pictureBox2.Image.Width;
                Bitmap oldBitmap = (Bitmap)this.pictureBox2.Image;
                int EdgeNum = 0;
                XYPoint[] EdgePoint = new XYPoint[Width * Height];
                LineParameters[] Line = new LineParameters[Width * Height];
                for(short x=0;x<Width;x++)
                    for(short y=0;y<Height;y++)
                        if (oldBitmap.GetPixel(x, y).G == 255)
                        {
                            EdgePoint[EdgeNum].X = x;
                            EdgePoint[EdgeNum].Y = y;
                            EdgeNum++;
                        }
                int AngleNum = 360;
                int DistNum = (int)Math.Sqrt(Width * Width + Height * Height) * 2;
                int Threshold = Math.Min(Width, Height) / 5;
                int HoughSpaceMax = 0;
                Bitmap newBitmap = new Bitmap(AngleNum, DistNum);
                int pixH;
                double DeltaAngle, DeltaDist;
                double MaxDist, MinDist;
                double Angle, Dist;
                int LineCount;
                int[,] HoughSpace = new int[AngleNum, DistNum];
                MaxDist = Math.Sqrt(Width * Width + Height * Height);
                MinDist = (double)-Width;
                DeltaAngle = Math.PI / AngleNum;
                DeltaDist = (MaxDist - MinDist) / DistNum;
                for (int i = 0; i < AngleNum; i++)
                    for (int j = 0; j < DistNum; j++)
                        HoughSpace[i, j] = 0;
                for(int i = 0;i < EdgeNum; i++)
                    for(int j = 0;j < AngleNum; j++)
                    {
                        Angle = j * DeltaAngle;
                        Dist = EdgePoint[i].X * Math.Cos(Angle) + EdgePoint[i].Y * Math.Sin(Angle);
                        HoughSpace[j, (int)((Dist - MinDist) / DeltaDist)]++;
                    }
                LineCount = 0;
                for (int i = 0;i<AngleNum;i++)
                    for (int j = 0;j<DistNum;j++)
                    {
                        if (HoughSpace[i, j] > HoughSpaceMax) HoughSpaceMax = HoughSpace[i, j];
                        if (HoughSpace[i, j] >= Threshold)
                        {
                            Line[LineCount].Angle = i;
                            Line[LineCount].Distance = j;
                            LineCount++;
                        }
                    }
                for (int x = 0; x<AngleNum;x++)
                    for(int y = 0; y<DistNum;y++)
                    {
                        pixH = 255 - (HoughSpaceMax - HoughSpace[x, y]) * 255 / HoughSpaceMax;
                        if (HoughSpace[x, y] > Threshold)
                            newBitmap.SetPixel(x, y, Color.FromArgb(pixH, 0, 0));
                        else
                            newBitmap.SetPixel(x, y, Color.FromArgb(pixH, pixH, pixH));
                    }
                this.pictureBox3.Image = newBitmap;
                for(int i = 0;i<LineCount&i<Width*Height;i++)
                {
                    for(int x = 0; x < Width; x++)
                    {
                        int y = (int)((Line[i].Distance * DeltaDist + MinDist - x * Math.Cos(Line[i].Angle * DeltaAngle)) / Math.Sin(Line[i].Angle * DeltaAngle));
                        if(y >= 0 & y < Height)
                        {
                            pixH = oldBitmap.GetPixel(x, y).G;
                            oldBitmap.SetPixel(x, y, Color.FromArgb(pixH ^ 255, pixH, pixH));
                        }
                    }
                    for(int y =0; y < Height; y++)
                    {
                        int x = (int)((Line[i].Distance * DeltaDist + MinDist - y * Math.Sin(Line[i].Angle * DeltaAngle)) / Math.Cos(Line[i].Angle * DeltaAngle));
                        if (x >= 0 & x < Width)
                        {
                            pixH = oldBitmap.GetPixel(x, y).G;
                            oldBitmap.SetPixel(x, y, Color.FromArgb(pixH ^ 255, pixH, pixH));
                        }
                    }
                }
                this.pictureBox2.Image = oldBitmap;
                this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                //this.label1.Text = "Hough transform 完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int Height = this.pictureBox1.Image.Height;
                int Width = this.pictureBox1.Image.Width;
                Bitmap oldBitmap = (Bitmap)this.pictureBox1.Image;

                

                // 邊緣檢測
                Bitmap edgeDetectedBitmap = ApplySobelEdgeDetection(oldBitmap);

                // 在pictureBox2中顯示邊緣檢測後的圖像
                this.pictureBox2.Image = edgeDetectedBitmap;
                this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤");
            }
        }


        // 應用Sobel邊緣檢測
        private Bitmap ApplySobelEdgeDetection(Bitmap inputImage)
        {
            int Width = inputImage.Width;
            int Height = inputImage.Height;
            Bitmap edgeDetectedBitmap = new Bitmap(Width, Height);

            int[,] sobelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] sobelY = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    int pixelX = 0;
                    int pixelY = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Color pixel = inputImage.GetPixel(x + i, y + j);
                            int grayValue = pixel.R;

                            pixelX += sobelX[i + 1, j + 1] * grayValue;
                            pixelY += sobelY[i + 1, j + 1] * grayValue;
                        }
                    }

                    int edgeValue = (int)Math.Sqrt(pixelX * pixelX + pixelY * pixelY);

                    // 確保像素值在 0 到 255 之間
                    edgeValue = Math.Max(0, Math.Min(255, edgeValue));

                    edgeDetectedBitmap.SetPixel(x, y, Color.FromArgb(edgeValue, edgeValue, edgeValue));
                }
            }

            return edgeDetectedBitmap;
        }


    }
}
