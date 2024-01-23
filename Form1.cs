using Emgu.CV;
using System.Text.RegularExpressions;
using System.Threading;

namespace UR1_Alyssa_Bloomfield
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            StartStopBtn = new Button();
            VideoPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)VideoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // StartStopBtn
            // 
            StartStopBtn.Location = new Point(967, 698);
            StartStopBtn.Name = "StartStopBtn";
            StartStopBtn.Size = new Size(207, 108);
            StartStopBtn.TabIndex = 0;
            StartStopBtn.Text = "Start";
            StartStopBtn.UseVisualStyleBackColor = true;
            StartStopBtn.Click += StartStopBtn_Click_1;
            // 
            // VideoPictureBox
            // 
            VideoPictureBox.Location = new Point(109, 55);
            VideoPictureBox.Name = "VideoPictureBox";
            VideoPictureBox.Size = new Size(744, 510);
            VideoPictureBox.TabIndex = 1;
            VideoPictureBox.TabStop = false;
            VideoPictureBox.Click += VideoPictureBox_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(1335, 886);
            Controls.Add(VideoPictureBox);
            Controls.Add(StartStopBtn);
            Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)VideoPictureBox).EndInit();
            ResumeLayout(false);
        }

        public partial class Form1 : Form
        {
            VideoCapture mCapture;

            Thread mCaptureThread;

            CancellationTokenSource mCallellationToken = new();

            bool mIsCapturing = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                mCapture = new VideoCapture(0);
                if (mCapture.Height == 0)
                    throw new Exception("No Cameras Found");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void StartStopBtn_Click(object sender, EventArgs e)
        {
            if (mIsCapturing)
            {
                mCancellationToken.Cancel();
                mIsCapturing = false;
                StartStopBtn.Text = "Start";
            }
            else
            {
                mCancellationToken = new();
                mCaptureThread = new(() => DisplayWebCam(mCancellationToken.Token));
                mCapture.Thread.start();
                mIsCapturing = true;
                StartStopBtn.Text = "Stop";
            }
        }
        private void DisplayWebcam(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Mat frame = mCapture.QueryFrame();

                int newHeight = frame(frame.Size.Height * PictureBox.Size.Width) / frame.Size.Width;
                Size newSize = new Size(PictureBox.Size.Width, newHeight);
                CVInvoke.Resize(frame, frame, newSize);

                Task.Delay(16);

                PictureBox.Image = frame.ToBitmap();
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mIsCapturing)
            {
                mCancellationToken.Cancel();
            }
            else
            {
                mCapture.Dispose();
                mCancellationToken.Dispose();
            }
        }

        private Button StartStopBtn;
        private PictureBox VideoPictureBox;

        private void VideoPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void StartStopBtn_Click_1(object sender, EventArgs e)
        {

        }
    }
}