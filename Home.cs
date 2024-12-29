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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private string videoPath = string.Empty;
        private string referenceImagePath = string.Empty;
        private List<Tuple<string, Bitmap>> precomputedLBPImages = new List<Tuple<string, Bitmap>>();

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

        private void btnImageLBP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(referenceImagePath))
            {
                MessageBox.Show("Vui lòng tải ảnh tham chiếu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển ảnh gốc sang ảnh Bitmap
            Bitmap originalBitmap = new Bitmap(picBoxOriginal.Image);

            // Chuyển ảnh thành grayscale
            Bitmap grayImage = ConvertToGrayScale(originalBitmap);

            // Tính toán LBP cho ảnh grayscale
            Bitmap lbpImage = ComputeLBP(grayImage);

            // Hiển thị ảnh LBP trong picBox1
            picBox2.Image = lbpImage;
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

        // Phương thức tính LBP cho một ảnh
        public static int GetPixel(Bitmap img, int center, int x, int y)
        {
            int newValue = 0;
            try
            {
                if (img.GetPixel(x, y).R >= center)
                    newValue = 1;
            }
            catch { }

            return newValue;
        }

        public static int LbpCalculatedPixel(Bitmap img, int x, int y)
        {
            int center = img.GetPixel(x, y).R;
            int[] valAr = new int[8];

            // Tính các pixel lân cận
            valAr[0] = GetPixel(img, center, x - 1, y - 1); // top-left
            valAr[1] = GetPixel(img, center, x - 1, y);     // top
            valAr[2] = GetPixel(img, center, x - 1, y + 1); // top-right
            valAr[3] = GetPixel(img, center, x, y + 1);     // right
            valAr[4] = GetPixel(img, center, x + 1, y + 1); // bottom-right
            valAr[5] = GetPixel(img, center, x + 1, y);     // bottom
            valAr[6] = GetPixel(img, center, x + 1, y - 1); // bottom-left
            valAr[7] = GetPixel(img, center, x, y - 1);     // left

            int lbpValue = 0;
            int[] powerVal = new int[] { 1, 2, 4, 8, 16, 32, 64, 128 };

            for (int i = 0; i < valAr.Length; i++)
            {
                lbpValue += valAr[i] * powerVal[i];
            }

            return lbpValue;
        }

        // Phương thức tính toàn bộ ảnh LBP
        public static Bitmap ComputeLBP(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;
            Bitmap lbpImage = new Bitmap(width, height);

            for (int i = 1; i < height - 1; i++) // Tránh biên
            {
                for (int j = 1; j < width - 1; j++)
                {
                    // Tính toán LBP cho pixel tại (i, j) và chuyển thành màu xám
                    int lbpValue = LbpCalculatedPixel(img, j, i); // LBP cho pixel tại (j, i)
                    lbpImage.SetPixel(j, i, Color.FromArgb(lbpValue, lbpValue, lbpValue)); // Lưu giá trị LBP vào ảnh LBP
                }
            }

            return lbpImage;
        }

        private void btnProcessLBP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(videoPath) || string.IsNullOrEmpty(referenceImagePath))
            {
                MessageBox.Show("Vui lòng tải cả video và ảnh tham chiếu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiểm tra xem thư mục frames có tồn tại và có ảnh để xử lý không
            string framesFolder = Path.Combine(Application.StartupPath, "frames");
            if (!Directory.Exists(framesFolder))
            {
                MessageBox.Show("Thư mục frames không tồn tại hoặc không có ảnh nào.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Trích chọn đặc trưng (LBP) của tất cả các ảnh trong thư mục frames nếu chưa thực hiện
            if (precomputedLBPImages.Count == 0)
            {
                PrecomputeLBPForFrames(framesFolder);
            }

            // Tính toán LBP cho ảnh gốc
            Bitmap originalBitmap = new Bitmap(picBoxOriginal.Image);
            Bitmap grayImage = ConvertToGrayScale(originalBitmap);
            Bitmap lbpOriginalImage = ComputeLBP(grayImage);

            // Lưu trữ các ảnh trong thư mục frames
            string[] frameFiles = Directory.GetFiles(framesFolder, "*.jpg");
            List<Tuple<string, double>> lbpComparisons = new List<Tuple<string, double>>();

            foreach (var precomputedLbp in precomputedLBPImages)
            {
                // Tính toán độ tương tự (Hamming Distance) giữa LBP của ảnh gốc và ảnh trong frames
                double similarity = CalculateLBPSimilarity(lbpOriginalImage, precomputedLbp.Item2);
                lbpComparisons.Add(new Tuple<string, double>(precomputedLbp.Item1, similarity));
            }

            // Sắp xếp danh sách theo độ tương tự (từ cao đến thấp)
            var top4SimilarImages = lbpComparisons.OrderByDescending(x => x.Item2).Take(4).ToList();

            // Hiển thị 4 ảnh có độ tương tự cao nhất
            DisplayTop4Images(top4SimilarImages);

            MessageBox.Show("Quá trình trích xuất và so sánh hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Phương thức để trích chọn đặc trưng (LBP) cho tất cả các ảnh trong thư mục frames
        private void PrecomputeLBPForFrames(string framesFolder)
        {
            string[] frameFiles = Directory.GetFiles(framesFolder, "*.jpg");

            foreach (string frameFile in frameFiles)
            {
                Bitmap frameBitmap = new Bitmap(frameFile);
                Bitmap grayFrame = ConvertToGrayScale(frameBitmap);
                Bitmap lbpFrame = ComputeLBP(grayFrame);

                // Lưu trữ kết quả LBP của ảnh trong danh sách
                precomputedLBPImages.Add(new Tuple<string, Bitmap>(frameFile, lbpFrame));
            }
        }

        // Phương thức tính độ tương tự (Hamming Distance) giữa hai ảnh LBP
        private double CalculateLBPSimilarity(Bitmap lbpImage1, Bitmap lbpImage2)
        {
            int width = lbpImage1.Width;
            int height = lbpImage1.Height;
            int matchingPixels = 0;
            int totalPixels = width * height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Lấy giá trị LBP của mỗi pixel
                    Color pixel1 = lbpImage1.GetPixel(i, j);
                    Color pixel2 = lbpImage2.GetPixel(i, j);

                    // So sánh các giá trị LBP
                    if (pixel1.R == pixel2.R) // Chỉ so sánh kênh R (do LBP là ảnh xám)
                    {
                        matchingPixels++;
                    }
                }
            }

            // Tính toán độ tương tự (tỉ lệ matching pixels trên tổng số pixels)
            return (double)matchingPixels / totalPixels;
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
    }
}
