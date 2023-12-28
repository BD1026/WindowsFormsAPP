namespace WinFormsApp5
{
    public partial class Form1 : Form
    {

        private const double focalLength = 12.07; // 焦距，單位：mm
        private const double sensorWidth = 7.6; // 感測器寬度，單位：mm
        private const double sensorHeight = 5.7; // 感測器高度，單位：mm
        private const int imageWidth = 2272; // 影像寬度，單位：像素
        private const int imageHeight = 1704; // 影像高度，單位：像素
        private const double pixelSize = 0.0033450704225352; // 兩像素間的距離，單位：mm
        private double baseline = 0;
        private Point? redPoint1 = null;
        private Point? redPoint2 = null;

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
                    Bitmap myBitmap = new Bitmap(openFileDialog.FileName);
                    this.pictureBox1.Image = myBitmap;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    redPoint1 = FindRedPoint(myBitmap);
                    if (redPoint1 != null)
                        label1.Text = $"紅點座標：{redPoint1.Value}";

                    CalculateDepth();
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
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "圖像文件(JPeg,Gif,Bmp,etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*tif;*tiff;*.png|所有文件(*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap myBitmap = new Bitmap(openFileDialog.FileName);
                    this.pictureBox2.Image = myBitmap;
                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

                    redPoint2 = FindRedPoint(myBitmap);
                    if (redPoint2 != null)
                        label2.Text = $"紅點座標：{redPoint2.Value}";

                    CalculateDepth();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "訊息顯示");
            }
        }

        private Point? FindRedPoint(Bitmap bitmap)
        {
            Color redColor = Color.FromArgb(255, 245, 0, 0); // 紅色的 ARGB 值
            int threshold = 63; // 色差閾值，用於判斷紅色像素

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    // 判斷像素是否是紅色
                    if (Math.Abs(pixelColor.R - redColor.R) < threshold &&
                        Math.Abs(pixelColor.G - redColor.G) < threshold &&
                        Math.Abs(pixelColor.B - redColor.B) < threshold)
                    {
                        return new Point(x, y);
                    }
                }
            }

            return null;
        }

        private void CalculateDepth()
        {
            if (redPoint1 != null && redPoint2 != null)
            {
                double disparity = Math.Abs(redPoint1.Value.X - redPoint2.Value.X) * pixelSize; // 觀察兩紅點的視差，單位：mm
                double depth = (focalLength * baseline) / disparity; // 深度，單位：mm

                label5.Text = $"深度(Depth)：{depth:F2} 公分"; // 顯示深度，單位：公分
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double inputBaseline))
            {
                baseline = inputBaseline; // 更新相機移動距離
                CalculateDepth(); // 重新計算深度
            }
            else
            {
                MessageBox.Show("請輸入有效數字", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}