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
            //Main capture object
            VideoCapture mCapture;

            //Video thread that allows multi-threading
            Thread mCaptureThread;

            //Allows for thread termination
            CancellationTokenSource mCallellationToken = new();

            //Indicator for the capturing state
            bool mIsCapturing = false;
        }

        private Button StartStopBtn;
        private PictureBox VideoPictureBox;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //Initailize with ifany plugged camera
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
            //Stop capturing if live 
            if (mIsCapturing)
            {
                mCancellationToken.Cancel(); //requesting for a new stop
                mIsCapturing = false; //indicate a new state
                StartStopBtn.Text = "Start"; //inform accordingly
            }
            //Start capturing if dead
            else
            {
                mCancellationToken = new(); // reinitialize
                
                //Create a new thread(inilitize)
                mCaptureThread = new(() => DisplayWebCam(mCancellationToken.Token)); 
                mCapture.Thread.start();//start the feed
                mIsCapturing = true; //indicate the state
                StartStopBtn.Text = "Stop"; //infrom accordingly
            }
        }
        private void DisplayWebcam(CancellationToken token)
        {
            while (!token.IsCancellationRequested) //While no requested cancellation
            {
                Mat frame = mCapture.QueryFrame(); //grab a new frame

                //resize picture box
                int newHeight = frame(frame.Size.Height * PictureBox.Size.Width) / frame.Size.Width; 
                Size newSize = new Size(PictureBox.Size.Width, newHeight);
                CVInvoke.Resize(frame, frame, newSize);

                //Create a 60 fps frame rate
                Task.Delay(16);

                //Display the current frame
                PictureBox.Image = frame.ToBitmap();
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Dispose all processing threats to avoid orphanded processes
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

        private void StartStopBtn_Click_1(object sender, EventArgs e)
        {

        }
    }
}