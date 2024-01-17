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
            PictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)PictureBox).BeginInit();
            SuspendLayout();
            // 
            // StartStopBtn
            // 
            StartStopBtn.Location = new Point(1075, 777);
            StartStopBtn.Name = "StartStopBtn";
            StartStopBtn.Size = new Size(188, 58);
            StartStopBtn.TabIndex = 0;
            StartStopBtn.Text = "Start";
            StartStopBtn.UseVisualStyleBackColor = true;
            // 
            // PictureBox
            // 
            PictureBox.Location = new Point(51, 38);
            PictureBox.Name = "PictureBox";
            PictureBox.Size = new Size(750, 532);
            PictureBox.TabIndex = 1;
            PictureBox.TabStop = false;
            // 
            // Form1
            // 
            ClientSize = new Size(1335, 886);
            Controls.Add(PictureBox);
            Controls.Add(StartStopBtn);
            Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)PictureBox).EndInit();
            ResumeLayout(false);
        }

        private Button StartStopBtn;
        private PictureBox PictureBox;

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
    }
}