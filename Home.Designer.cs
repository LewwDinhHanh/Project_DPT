namespace Project_cuoi_ky
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            panel1 = new Panel();
            panel4 = new Panel();
            btnImage = new Button();
            btnImageLBP = new Button();
            btnExtractFrames = new Button();
            label3 = new Label();
            picBox2 = new PictureBox();
            label2 = new Label();
            btnLoadImage = new Button();
            picBoxOriginal = new PictureBox();
            panel3 = new Panel();
            btnProcess = new Button();
            btnProcessLBP = new Button();
            lblDTT4 = new Label();
            lblDTT3 = new Label();
            lblDTT2 = new Label();
            lblDTT1 = new Label();
            picBox6 = new PictureBox();
            picBox5 = new PictureBox();
            picBox4 = new PictureBox();
            picBox3 = new PictureBox();
            panel2 = new Panel();
            btnLoadVideo = new Button();
            txtVideoPath = new TextBox();
            label1 = new Label();
            wmpPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBoxOriginal).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox3).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)wmpPlayer).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(1, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(930, 596);
            panel1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(btnImage);
            panel4.Controls.Add(btnImageLBP);
            panel4.Controls.Add(btnExtractFrames);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(picBox2);
            panel4.Controls.Add(label2);
            panel4.Controls.Add(btnLoadImage);
            panel4.Controls.Add(picBoxOriginal);
            panel4.Location = new Point(3, 359);
            panel4.Name = "panel4";
            panel4.Size = new Size(498, 232);
            panel4.TabIndex = 2;
            // 
            // btnImage
            // 
            btnImage.Location = new Point(381, 188);
            btnImage.Name = "btnImage";
            btnImage.Size = new Size(85, 39);
            btnImage.TabIndex = 7;
            btnImage.Text = "Biến đổi ảnh";
            btnImage.UseVisualStyleBackColor = true;
            // 
            // btnImageLBP
            // 
            btnImageLBP.Location = new Point(381, 133);
            btnImageLBP.Name = "btnImageLBP";
            btnImageLBP.Size = new Size(85, 39);
            btnImageLBP.TabIndex = 6;
            btnImageLBP.Text = "Biến đổi ảnh LBP";
            btnImageLBP.UseVisualStyleBackColor = true;
            btnImageLBP.Click += btnImageLBP_Click;
            // 
            // btnExtractFrames
            // 
            btnExtractFrames.Location = new Point(381, 79);
            btnExtractFrames.Name = "btnExtractFrames";
            btnExtractFrames.Size = new Size(85, 34);
            btnExtractFrames.TabIndex = 5;
            btnExtractFrames.Text = "Tách Frames";
            btnExtractFrames.UseVisualStyleBackColor = true;
            btnExtractFrames.Click += btnExtractFrames_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Red;
            label3.Location = new Point(190, 6);
            label3.Name = "label3";
            label3.Size = new Size(134, 17);
            label3.TabIndex = 4;
            label3.Text = "* Ảnh sau khi biến đổi ";
            // 
            // picBox2
            // 
            picBox2.BorderStyle = BorderStyle.FixedSingle;
            picBox2.Location = new Point(190, 26);
            picBox2.Name = "picBox2";
            picBox2.Size = new Size(167, 201);
            picBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox2.TabIndex = 3;
            picBox2.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Red;
            label2.Location = new Point(3, 6);
            label2.Name = "label2";
            label2.Size = new Size(101, 17);
            label2.TabIndex = 2;
            label2.Text = "* Ảnh gốc tải lên";
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new Point(381, 26);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(85, 34);
            btnLoadImage.TabIndex = 1;
            btnLoadImage.Text = "Chọn Ảnh";
            btnLoadImage.UseVisualStyleBackColor = true;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // picBoxOriginal
            // 
            picBoxOriginal.BorderStyle = BorderStyle.FixedSingle;
            picBoxOriginal.Location = new Point(3, 26);
            picBoxOriginal.Name = "picBoxOriginal";
            picBoxOriginal.Size = new Size(167, 201);
            picBoxOriginal.SizeMode = PictureBoxSizeMode.StretchImage;
            picBoxOriginal.TabIndex = 0;
            picBoxOriginal.TabStop = false;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(btnProcess);
            panel3.Controls.Add(btnProcessLBP);
            panel3.Controls.Add(lblDTT4);
            panel3.Controls.Add(lblDTT3);
            panel3.Controls.Add(lblDTT2);
            panel3.Controls.Add(lblDTT1);
            panel3.Controls.Add(picBox6);
            panel3.Controls.Add(picBox5);
            panel3.Controls.Add(picBox4);
            panel3.Controls.Add(picBox3);
            panel3.Location = new Point(503, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(422, 588);
            panel3.TabIndex = 1;
            // 
            // btnProcess
            // 
            btnProcess.Location = new Point(314, 544);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(85, 39);
            btnProcess.TabIndex = 10;
            btnProcess.Text = "Xử lý ...";
            btnProcess.UseVisualStyleBackColor = true;
            // 
            // btnProcessLBP
            // 
            btnProcessLBP.Location = new Point(218, 542);
            btnProcessLBP.Name = "btnProcessLBP";
            btnProcessLBP.Size = new Size(85, 39);
            btnProcessLBP.TabIndex = 9;
            btnProcessLBP.Text = "Xử lý LBP";
            btnProcessLBP.UseVisualStyleBackColor = true;
            btnProcessLBP.Click += btnProcessLBP_Click;
            // 
            // lblDTT4
            // 
            lblDTT4.AutoSize = true;
            lblDTT4.Location = new Point(232, 489);
            lblDTT4.Name = "lblDTT4";
            lblDTT4.Size = new Size(71, 15);
            lblDTT4.TabIndex = 8;
            lblDTT4.Text = "Độ tương tự";
            // 
            // lblDTT3
            // 
            lblDTT3.AutoSize = true;
            lblDTT3.Location = new Point(26, 489);
            lblDTT3.Name = "lblDTT3";
            lblDTT3.Size = new Size(71, 15);
            lblDTT3.TabIndex = 7;
            lblDTT3.Text = "Độ tương tự";
            // 
            // lblDTT2
            // 
            lblDTT2.AutoSize = true;
            lblDTT2.Location = new Point(232, 230);
            lblDTT2.Name = "lblDTT2";
            lblDTT2.Size = new Size(71, 15);
            lblDTT2.TabIndex = 6;
            lblDTT2.Text = "Độ tương tự";
            // 
            // lblDTT1
            // 
            lblDTT1.AutoSize = true;
            lblDTT1.Location = new Point(26, 230);
            lblDTT1.Name = "lblDTT1";
            lblDTT1.Size = new Size(71, 15);
            lblDTT1.TabIndex = 5;
            lblDTT1.Text = "Độ tương tự";
            // 
            // picBox6
            // 
            picBox6.BorderStyle = BorderStyle.FixedSingle;
            picBox6.Location = new Point(232, 268);
            picBox6.Name = "picBox6";
            picBox6.Size = new Size(167, 201);
            picBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox6.TabIndex = 4;
            picBox6.TabStop = false;
            // 
            // picBox5
            // 
            picBox5.BorderStyle = BorderStyle.FixedSingle;
            picBox5.Location = new Point(26, 268);
            picBox5.Name = "picBox5";
            picBox5.Size = new Size(167, 201);
            picBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox5.TabIndex = 3;
            picBox5.TabStop = false;
            // 
            // picBox4
            // 
            picBox4.BorderStyle = BorderStyle.FixedSingle;
            picBox4.Location = new Point(232, 17);
            picBox4.Name = "picBox4";
            picBox4.Size = new Size(167, 201);
            picBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox4.TabIndex = 2;
            picBox4.TabStop = false;
            // 
            // picBox3
            // 
            picBox3.BorderStyle = BorderStyle.FixedSingle;
            picBox3.Location = new Point(26, 17);
            picBox3.Name = "picBox3";
            picBox3.Size = new Size(167, 201);
            picBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox3.TabIndex = 1;
            picBox3.TabStop = false;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(btnLoadVideo);
            panel2.Controls.Add(txtVideoPath);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(wmpPlayer);
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(498, 350);
            panel2.TabIndex = 0;
            // 
            // btnLoadVideo
            // 
            btnLoadVideo.Location = new Point(404, 317);
            btnLoadVideo.Name = "btnLoadVideo";
            btnLoadVideo.Size = new Size(52, 23);
            btnLoadVideo.TabIndex = 3;
            btnLoadVideo.Text = "...";
            btnLoadVideo.UseVisualStyleBackColor = true;
            btnLoadVideo.Click += btnLoadVideo_Click;
            // 
            // txtVideoPath
            // 
            txtVideoPath.BorderStyle = BorderStyle.FixedSingle;
            txtVideoPath.Location = new Point(92, 317);
            txtVideoPath.Name = "txtVideoPath";
            txtVideoPath.Size = new Size(288, 23);
            txtVideoPath.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 321);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 1;
            label1.Text = "Đang phát";
            // 
            // wmpPlayer
            // 
            wmpPlayer.Enabled = true;
            wmpPlayer.Location = new Point(3, 3);
            wmpPlayer.Name = "wmpPlayer";
            wmpPlayer.OcxState = (AxHost.State)resources.GetObject("wmpPlayer.OcxState");
            wmpPlayer.Size = new Size(490, 300);
            wmpPlayer.TabIndex = 0;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(932, 599);
            Controls.Add(panel1);
            Name = "Home";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Home";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBoxOriginal).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox3).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)wmpPlayer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private AxWMPLib.AxWindowsMediaPlayer wmpPlayer;
        private Button btnLoadVideo;
        private TextBox txtVideoPath;
        private Label label1;
        private Panel panel3;
        private Panel panel4;
        private PictureBox picBoxOriginal;
        private Label label2;
        private Button btnLoadImage;
        private PictureBox picBox6;
        private PictureBox picBox5;
        private PictureBox picBox4;
        private PictureBox picBox3;
        private Button btnImage;
        private Button btnImageLBP;
        private Button btnExtractFrames;
        private Label label3;
        private PictureBox picBox2;
        private Button btnProcess;
        private Button btnProcessLBP;
        private Label lblDTT4;
        private Label lblDTT3;
        private Label lblDTT2;
        private Label lblDTT1;
    }
}