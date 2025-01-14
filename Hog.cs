using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_cuoi_ky
{
    public partial class Hog : Form
    {
        public Hog()
        {
            InitializeComponent();
        }

        private string videoPath = string.Empty;
        private string referenceImagePath = string.Empty;
        private List<Tuple<string, float[]>> precomputedHOGFeatures = new List<Tuple<string, float[]>>();

        private void btnLoadVideo_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video Files|*.mp4;*.avi;*.mkv;*.wmv",
                Title = "Chọn video"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                videoPath = openFileDialog.FileName;
                wmpPlayer.URL = videoPath;
                wmpPlayer.Ctlcontrols.play();
                txtVideoPath.Text = Path.GetFileName(videoPath);
                MessageBox.Show("Video đã được tải và hiển thị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Chọn ảnh tham chiếu"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    referenceImagePath = openFileDialog.FileName;
                    picBoxOriginal.Image = Image.FromFile(referenceImagePath);

                    // Reset các PictureBox khác (picBox2 đến picBox6) thành ảnh trắng
                    picBox2.Image = new Bitmap(picBox2.Width, picBox2.Height);
                    picBox3.Image = new Bitmap(picBox3.Width, picBox3.Height);
                    picBox4.Image = new Bitmap(picBox4.Width, picBox4.Height);
                    picBox5.Image = new Bitmap(picBox5.Width, picBox5.Height);
                    picBox6.Image = new Bitmap(picBox6.Width, picBox6.Height);

                    lblDTT1.Text = "Độ tương tự";
                    lblDTT2.Text = "Độ tương tự";
                    lblDTT3.Text = "Độ tương tự";
                    lblDTT4.Text = "Độ tương tự";

                    MessageBox.Show("Ảnh đã được tải và hiển thị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tải ảnh. Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExtractFrames_Click(object sender, EventArgs e)
        {
            string framesFolder = Path.Combine(Application.StartupPath, "frames");
            // Kiểm tra nếu thư mục đã tồn tại, xóa tất cả các tệp trong đó
            if (Directory.Exists(framesFolder))
            {
                // Xóa tất cả các tệp trong thư mục frames
                string[] files = Directory.GetFiles(framesFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            else
            {
                // Nếu thư mục không tồn tại, tạo mới thư mục frames
                Directory.CreateDirectory(framesFolder);
            }
            ExtractFrames(videoPath, framesFolder);
            MessageBox.Show("Xử lý hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExtractFrames(string videoPath, string outputFolder)
        {
            if (string.IsNullOrEmpty(videoPath))
            {
                MessageBox.Show("Vui lòng tải video tham chiếu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ffmpegPath = @"C:\\ffmpeg-2024-12-23-git-6c9218d748-essentials_build\\bin\\ffmpeg.exe";
            string arguments = $"-i \"{videoPath}\" -vf \"fps=2\" \"{Path.Combine(outputFolder, "f_%d.jpg")}\"";
            ProcessStartInfo processInfo = new ProcessStartInfo(ffmpegPath, arguments)
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process process = Process.Start(processInfo);
            process.WaitForExit();
        }

        private void btnImageHOG_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(referenceImagePath))
            {
                MessageBox.Show("Vui lòng tải ảnh tham chiếu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển ảnh gốc sang ảnh Bitmap
            using (Bitmap originalBitmap = new Bitmap(picBoxOriginal.Image))
            {
                // Tính HOG cho ảnh
                float[] hogFeatures = ComputeHOG(originalBitmap);

                // Hiển thị visualization của HOG (tùy chọn)
                Bitmap hogVisualization = VisualizeHOG(originalBitmap, hogFeatures);
                picBox2.Image = hogVisualization;
            }
        }

        // Phương thức chuyển ảnh màu sang ảnh grayscale
        public static Bitmap ConvertToGrayScale(Bitmap original)
        {
            Bitmap grayImage = new Bitmap(original.Width, original.Height);
            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color pixelColor = original.GetPixel(i, j);
                    int grayValue = (int)(0.2989 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    grayImage.SetPixel(i, j, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }
            return grayImage;
        }

        private void btnProcessHOG_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(videoPath) || string.IsNullOrEmpty(referenceImagePath))
            {
                MessageBox.Show("Vui lòng tải cả video và ảnh tham chiếu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string framesFolder = Path.Combine(Application.StartupPath, "frames");
            if (!Directory.Exists(framesFolder))
            {
                MessageBox.Show("Thư mục frames không tồn tại hoặc không có ảnh nào.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Bitmap originalBitmap = new Bitmap(picBoxOriginal.Image))
            {
                float[] hogOriginal = ComputeHOG(originalBitmap);

                if (precomputedHOGFeatures.Count == 0)
                {
                    PrecomputeHOGForFrames(framesFolder);
                }

                List<Tuple<string, double>> hogComparisons = new List<Tuple<string, double>>();

                foreach (var precomputedHog in precomputedHOGFeatures)
                {
                    double similarity = CalculateHOGSimilarity(hogOriginal, precomputedHog.Item2);
                    hogComparisons.Add(new Tuple<string, double>(precomputedHog.Item1, similarity));
                }

                var top4SimilarImages = hogComparisons.OrderByDescending(x => x.Item2).Take(4).ToList();
                DisplayTop4Images(top4SimilarImages);
            }

            MessageBox.Show("Quá trình trích xuất và so sánh hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Phương thức để hiển thị 4 ảnh có độ tương tự cao nhất vào các PictureBox
        private void DisplayTop4Images(List<Tuple<string, double>> top4SimilarImages)
        {
            // Hiển thị 4 ảnh có độ tương tự cao nhất trong các picBox
            if (top4SimilarImages.Count >= 4)
            {
                picBox3.Image = Image.FromFile(top4SimilarImages[0].Item1);
                picBox4.Image = Image.FromFile(top4SimilarImages[1].Item1);
                picBox5.Image = Image.FromFile(top4SimilarImages[2].Item1);
                picBox6.Image = Image.FromFile(top4SimilarImages[3].Item1);

                // Cập nhật độ tương tự vào các Label
                lblDTT1.Text = $"Độ tương tự: {top4SimilarImages[0].Item2:F2}";
                lblDTT2.Text = $"Độ tương tự: {top4SimilarImages[1].Item2:F2}";
                lblDTT3.Text = $"Độ tương tự: {top4SimilarImages[2].Item2:F2}";
                lblDTT4.Text = $"Độ tương tự: {top4SimilarImages[3].Item2:F2}";
            }
            else
            {
                MessageBox.Show("Không có đủ ảnh để hiển thị!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //private void btnChucnang_Click(object sender, EventArgs e)
        //{
        //    ChucNang chucNang = new ChucNang();
        //    chucNang.Show();
        //    this.Close();
        //}

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Kiểm tra phản hồi từ người dùng
            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Thoát chương trình
            }
        }

        // Phương thức tính HOG cho một ảnh
        public float[] ComputeHOG(Bitmap image)
        {
            try
            {
                // Các tham số HOG
                int cellSize = 8;
                int blockSize = 2;
                int binCount = 9;

                // Chuyển ảnh sang grayscale
                using (Bitmap grayImage = ConvertToGrayScale(image))
                {
                    // Đảm bảo kích thước ảnh hợp lệ
                    int width = grayImage.Width;
                    int height = grayImage.Height;

                    // Tính số cells và blocks
                    int cellsX = width / cellSize;
                    int cellsY = height / cellSize;

                    // Điều chỉnh width và height để chia hết cho cellSize
                    width = cellsX * cellSize;
                    height = cellsY * cellSize;

                    Console.WriteLine($"Width: {width}, Height: {height}"); // Debug info

                    // Tính gradient
                    double[,] gradientMagnitudes = new double[height, width];
                    double[,] gradientAngles = new double[height, width];

                    // Tính gradient cho các pixel (bỏ qua viền)
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            try
                            {
                                Color c1 = grayImage.GetPixel(x + 1, y);
                                Color c2 = grayImage.GetPixel(x - 1, y);
                                Color c3 = grayImage.GetPixel(x, y + 1);
                                Color c4 = grayImage.GetPixel(x, y - 1);

                                double dx = c1.R - c2.R;
                                double dy = c3.R - c4.R;

                                gradientMagnitudes[y, x] = Math.Sqrt(dx * dx + dy * dy);
                                gradientAngles[y, x] = (Math.Atan2(dy, dx) * 180 / Math.PI + 180) % 180;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi khi tính gradient tại pixel ({x}, {y}): {ex.Message}");
                                gradientMagnitudes[y, x] = 0;
                                gradientAngles[y, x] = 0;
                            }
                        }
                    }

                    // Tính histogram cho mỗi cell
                    double[,,] cellHistograms = new double[cellsY, cellsX, binCount];

                    // Tính histogram cho mỗi cell
                    for (int cy = 0; cy < cellsY; cy++)
                    {
                        for (int cx = 0; cx < cellsX; cx++)
                        {
                            for (int y = cy * cellSize; y < (cy + 1) * cellSize && y < height - 1; y++)
                            {
                                for (int x = cx * cellSize; x < (cx + 1) * cellSize && x < width - 1; x++)
                                {
                                    double magnitude = gradientMagnitudes[y, x];
                                    double angle = gradientAngles[y, x];

                                    // Tính bin index và interpolation
                                    double binSize = 180.0 / binCount;
                                    int binIndex = (int)(angle / binSize);
                                    if (binIndex >= binCount) binIndex = binCount - 1;

                                    cellHistograms[cy, cx, binIndex] += magnitude;
                                }
                            }
                        }
                    }

                    // Tính features cho mỗi block và normalize
                    List<float> hogFeatures = new List<float>();

                    // Block normalization
                    for (int by = 0; by <= cellsY - blockSize; by++)
                    {
                        for (int bx = 0; bx <= cellsX - blockSize; bx++)
                        {
                            double[] blockVector = new double[blockSize * blockSize * binCount];
                            int vectorIndex = 0;

                            // Collect all histograms in this block
                            for (int y = by; y < by + blockSize; y++)
                            {
                                for (int x = bx; x < bx + blockSize; x++)
                                {
                                    for (int b = 0; b < binCount; b++)
                                    {
                                        blockVector[vectorIndex++] = cellHistograms[y, x, b];
                                    }
                                }
                            }

                            // L2 normalization
                            double norm = 0;
                            for (int i = 0; i < blockVector.Length; i++)
                            {
                                norm += blockVector[i] * blockVector[i];
                            }
                            norm = Math.Sqrt(norm + 1e-6);

                            // Add normalized values to final feature vector
                            for (int i = 0; i < blockVector.Length; i++)
                            {
                                hogFeatures.Add((float)(blockVector[i] / norm));
                            }
                        }
                    }

                    Console.WriteLine($"HOG Features Length: {hogFeatures.Count}"); // Debug info
                    return hogFeatures.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong ComputeHOG: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        // Phương thức tính độ tương tự giữa hai vector HOG
        private double CalculateHOGSimilarity(float[] hog1, float[] hog2)
        {
            if (hog1.Length != hog2.Length)
                return 0;

            double dotProduct = 0;
            double norm1 = 0;
            double norm2 = 0;

            for (int i = 0; i < hog1.Length; i++)
            {
                dotProduct += hog1[i] * hog2[i];
                norm1 += hog1[i] * hog1[i];
                norm2 += hog2[i] * hog2[i];
            }

            // Cosine similarity
            return dotProduct / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
        }

        // Cập nhật phương thức PrecomputeHOGForFrames
        private void PrecomputeHOGForFrames(string framesFolder)
        {
            string[] frameFiles = Directory.GetFiles(framesFolder, "*.jpg");

            foreach (string frameFile in frameFiles)
            {
                using (Bitmap frameBitmap = new Bitmap(frameFile))
                {
                    float[] hogFeatures = ComputeHOG(frameBitmap);
                    precomputedHOGFeatures.Add(new Tuple<string, float[]>(frameFile, hogFeatures));
                }
            }
        }

        // Thêm phương thức để visualization HOG (để người dùng có thể thấy được kết quả)
        private Bitmap VisualizeHOG(Bitmap original, float[] hogFeatures)
        {
            Bitmap visualization = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(visualization))
            {
                g.DrawImage(original, 0, 0);

                using (Pen pen = new Pen(Color.Yellow, 1))
                {
                    int cellSize = 8;
                    int blockSize = 2;
                    int binCount = 9;

                    // Tính số cells
                    int cellsX = original.Width / cellSize;
                    int cellsY = original.Height / cellSize;

                    // Tính số blocks
                    int blocksX = cellsX - blockSize + 1;
                    int blocksY = cellsY - blockSize + 1;

                    // Số features cho mỗi block
                    int featuresPerBlock = blockSize * blockSize * binCount;

                    // Vẽ một vector cho mỗi block
                    for (int by = 0; by < blocksY; by++)
                    {
                        for (int bx = 0; bx < blocksX; bx++)
                        {
                            int blockIndex = (by * blocksX + bx) * featuresPerBlock;

                            // Kiểm tra xem có đủ dữ liệu cho block này không
                            if (blockIndex + featuresPerBlock <= hogFeatures.Length)
                            {
                                // Tính góc và magnitude trung bình cho block
                                float sumX = 0, sumY = 0;
                                float maxMagnitude = 0;

                                // Xử lý từng bin trong block
                                for (int bin = 0; bin < binCount; bin++)
                                {
                                    float binAngle = (float)(bin * Math.PI / binCount);
                                    float magnitude = hogFeatures[blockIndex + bin];

                                    sumX += magnitude * (float)Math.Cos(binAngle);
                                    sumY += magnitude * (float)Math.Sin(binAngle);
                                    maxMagnitude = Math.Max(maxMagnitude, magnitude);
                                }

                                // Tính vị trí trung tâm của block
                                int centerX = bx * cellSize + cellSize;
                                int centerY = by * cellSize + cellSize;

                                // Tính hướng chính của gradient
                                float gradientAngle = (float)Math.Atan2(sumY, sumX);
                                float length = cellSize * maxMagnitude;

                                // Vẽ vector
                                g.DrawLine(pen,
                                    centerX,
                                    centerY,
                                    centerX + length * (float)Math.Cos(gradientAngle),
                                    centerY + length * (float)Math.Sin(gradientAngle));
                            }
                        }
                    }
                }
            }
            return visualization;
        }

        private void Hog_Load(object sender, EventArgs e)
        {

        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
