using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_cuoi_ky
{
    public partial class Gabor : Form
    {
        public Gabor()
        {
            InitializeComponent();
        }
        private string videoPath = string.Empty;
        private string referenceImagePath = string.Empty;
        private List<Tuple<string, Bitmap>> precomputedGaborImages = new List<Tuple<string, Bitmap>>();

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

        private void btnImageGABOR_Click(object sender, EventArgs e)
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

            // Áp dụng bộ lọc Gabor cho ảnh grayscale
            // Các tham số: theta (hướng), sigma (độ lệch chuẩn), frequency (tần số)
            double theta = Math.PI / 4; // Góc 45 độ
            double sigma = 4.0;         // Độ lệch chuẩn
            double frequency = 0.1;     // Tần số

            Bitmap gaborImage = ApplyGaborFilter(grayImage, theta, sigma, frequency);

            // Hiển thị ảnh Gabor trong picBox
            picBox2.Image = gaborImage;
        }

        // Hàm giảm kích thước ảnh
        private Bitmap ResizeImage(Bitmap original, int width, int height)
        {
            return new Bitmap(original, new Size(width, height));
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

        // Phương thức tạo bộ lọc Gabor
        public static Bitmap ApplyGaborFilter(Bitmap img, double theta, double sigma, double frequency)
        {
            int kernelSize = 31; // Kích thước kernel
            double[,] gaborKernel = GenerateGaborKernel(kernelSize, theta, sigma, frequency);

            Bitmap filteredImage = new Bitmap(img.Width, img.Height);
            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);

            // Lock bits for direct memory access
            BitmapData imgData = img.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData resultData = filteredImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int offset = kernelSize / 2;
            unsafe
            {
                byte* srcPtr = (byte*)imgData.Scan0;
                byte* destPtr = (byte*)resultData.Scan0;
                int stride = imgData.Stride;

                for (int y = offset; y < img.Height - offset; y++)
                {
                    for (int x = offset; x < img.Width - offset; x++)
                    {
                        double newValue = 0;

                        for (int ky = -offset; ky <= offset; ky++)
                        {
                            for (int kx = -offset; kx <= offset; kx++)
                            {
                                int srcX = x + kx;
                                int srcY = y + ky;

                                byte intensity = srcPtr[srcY * stride + srcX * 3]; // Lấy giá trị grayscale
                                newValue += intensity * gaborKernel[kx + offset, ky + offset];
                            }
                        }

                        newValue = Math.Max(0, Math.Min(255, newValue)); // Giới hạn giá trị [0, 255]
                        destPtr[y * stride + x * 3] = (byte)newValue;   // Ghi giá trị mới
                        destPtr[y * stride + x * 3 + 1] = (byte)newValue;
                        destPtr[y * stride + x * 3 + 2] = (byte)newValue;
                    }
                }
            }

            img.UnlockBits(imgData);
            filteredImage.UnlockBits(resultData);

            return filteredImage;
        }

        // Hàm tạo kernel Gabor
        private static double[,] GenerateGaborKernel(int kernelSize, double theta, double sigma, double frequency)
        {
            double[,] kernel = new double[kernelSize, kernelSize];
            int offset = kernelSize / 2;
            double cosTheta = Math.Cos(theta);
            double sinTheta = Math.Sin(theta);

            for (int x = -offset; x <= offset; x++)
            {
                for (int y = -offset; y <= offset; y++)
                {
                    double xTheta = x * cosTheta + y * sinTheta;
                    double yTheta = -x * sinTheta + y * cosTheta;

                    kernel[x + offset, y + offset] = Math.Exp(-0.5 * (Math.Pow(xTheta / sigma, 2) + Math.Pow(yTheta / sigma, 2)))
                                                      * Math.Cos(2 * Math.PI * frequency * xTheta);
                }
            }

            return kernel;
        }

        // Phương thức tiền xử lý các đặc trưng Gabor cho frames
        private void PrecomputeGaborForFrames(string framesFolder)
        {
            string[] frameFiles = Directory.GetFiles(framesFolder, "*.jpg");

            foreach (string frameFile in frameFiles)
            {
                Bitmap frameBitmap = new Bitmap(frameFile);
                Bitmap grayFrame = ConvertToGrayScale(frameBitmap);

                // Áp dụng bộ lọc Gabor
                Bitmap gaborFrame = ApplyGaborFilter(grayFrame, theta: Math.PI / 4, sigma: 4.0, frequency: 0.1);

                // Lưu trữ kết quả Gabor của ảnh trong danh sách
                precomputedGaborImages.Add(new Tuple<string, Bitmap>(frameFile, gaborFrame));
            }
        }

        // Phương thức tính độ tương tự giữa hai ảnh Gabor
        private double CalculateGaborSimilarity(Bitmap gaborImage1, Bitmap gaborImage2)
        {
            int width = gaborImage1.Width;
            int height = gaborImage1.Height;
            int matchingPixels = 0;
            int totalPixels = width * height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixel1 = gaborImage1.GetPixel(i, j);
                    Color pixel2 = gaborImage2.GetPixel(i, j);

                    if (pixel1.R == pixel2.R) // So sánh kênh R (ảnh grayscale)
                    {
                        matchingPixels++;
                    }
                }
            }

            return (double)matchingPixels / totalPixels; // Tỷ lệ pixel khớp
        }

        private void btnProcessGABOR_Click(object sender, EventArgs e)
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

            if (precomputedGaborImages.Count == 0)
            {
                PrecomputeGaborForFrames(framesFolder);
            }

            Bitmap originalBitmap = new Bitmap(picBoxOriginal.Image);
            Bitmap grayImage = ConvertToGrayScale(originalBitmap);
            Bitmap gaborOriginalImage = ApplyGaborFilter(grayImage, theta: Math.PI / 4, sigma: 4.0, frequency: 0.1);

            List<Tuple<string, double>> gaborComparisons = new List<Tuple<string, double>>();

            foreach (var precomputedGabor in precomputedGaborImages)
            {
                double similarity = CalculateGaborSimilarity(gaborOriginalImage, precomputedGabor.Item2);
                gaborComparisons.Add(new Tuple<string, double>(precomputedGabor.Item1, similarity));
            }

            var top4SimilarImages = gaborComparisons.OrderByDescending(x => x.Item2).Take(4).ToList();
            DisplayTop4Images(top4SimilarImages);

            MessageBox.Show("Quá trình xử lý và so sánh hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
