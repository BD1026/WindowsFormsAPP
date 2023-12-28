namespace WinFormsApp4
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

        struct XYPoint
        {
            public short X;
            public short Y;
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

                for (short x = 0; x < Width; x++)
                {
                    for (short y = 0; y < Height; y++)
                    {
                        if (oldBitmap.GetPixel(x, y).G == 255)
                        {
                            EdgePoint[EdgeNum].X = x;
                            EdgePoint[EdgeNum].Y = y;
                            EdgeNum++;
                        }
                    }
                }

                // 圓形霍夫變換參數
                int RadiusNum = Math.Min(Width, Height) / 2; // 最大半徑
                int[,,] HoughSpace = new int[Width, Height, RadiusNum];
                int Threshold = 200; // 您可以根據需要調整此閾值
                double CircularityThreshold = 0.3; // 這是圓形度的閾值，可以根據需要調整
                int MinRadius = 10; // 最小半徑
                int MaxRadius = 100; // 最大半徑

                // 初始化 HoughSpace 數組
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int r = 0; r < RadiusNum; r++)
                        {
                            HoughSpace[x, y, r] = 0;
                        }
                    }
                }

                // 執行圓形霍夫變換
                for (int i = 0; i < EdgeNum; i++)
                {
                    for (int r = 1; r < RadiusNum; r++) // 半徑從1開始，以避免除以零
                    {
                        for (int theta = 0; theta < 360; theta++)
                        {
                            int centerX = (int)(EdgePoint[i].X - r * Math.Cos(theta * Math.PI / 180));
                            int centerY = (int)(EdgePoint[i].Y - r * Math.Sin(theta * Math.PI / 180));

                            if (centerX >= 0 && centerX < Width && centerY >= 0 && centerY < Height)
                            {
                                HoughSpace[centerX, centerY, r]++;
                            }
                        }
                    }
                }

                // 查找具有足夠投票的圓
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int r = 1; r < RadiusNum; r++) // 半徑從1開始，以避免除以零
                        {
                            if (IsCircular(HoughSpace, x, y, r, Threshold, CircularityThreshold, MinRadius, MaxRadius))
                            {
                                // 在原始影像上繪製檢測到的圓形
                                DrawCircle(oldBitmap, x, y, r, 0.5);
                            }
                        }
                    }
                }

                this.pictureBox3.Image = oldBitmap;
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息提示");
            }
        }

        private void DrawCircle(Bitmap bitmap, int centerX, int centerY, int radius, double circularityThreshold)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Pen pen = new Pen(Color.Red, 2); // 這裡的 2 代表線條寬度，你可以根據需要調整

                int diameter = radius * 2;
                int x = centerX - radius;
                int y = centerY - radius;

                g.DrawEllipse(pen, x, y, diameter, diameter);
            }
        }

        private bool IsCircular(int[,,] houghSpace, int x, int y, int radius, int threshold, double circularityThreshold, int minRadius, int maxRadius)
        {
            int voteCount = houghSpace[x, y, radius];

            // 檢查投票點是否是局部最大值
            bool isLocalMax = IsLocalMaximum(houghSpace, x, y, radius);

            // 檢查圓形度
            double expectedCircumference = 2 * Math.PI * radius;
            double actualCircumference = voteCount;
            double circularity = actualCircumference / expectedCircumference;

            // 檢查額外的條件，例如半徑範圍
            bool isReasonableRadius = IsReasonableRadius(radius, minRadius, maxRadius);

            // 最終條件檢查
            return (voteCount >= threshold && circularity >= circularityThreshold && isLocalMax && isReasonableRadius);
        }

        private bool IsLocalMaximum(int[,,] houghSpace, int x, int y, int radius)
        {
            int currentValue = houghSpace[x, y, radius];

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < houghSpace.GetLength(0) && j >= 0 && j < houghSpace.GetLength(1))
                    {
                        if (houghSpace[i, j, radius] > currentValue)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool IsReasonableRadius(int radius, int minRadius, int maxRadius)
        {
            return radius >= minRadius && radius <= maxRadius;
        }


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
                            this.pictureBox3.Image.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息提示");
            }
        }
    }
}