using Emgu.CV;
using System.Text.RegularExpressions;
using System.Threading;

namespace UR1_Alyssa_Bloomfield
{
    public partial class Form1 : Form
    {

        //Main capture object
        VideoCapture mCapture;

        //Video thread that allows multi-threading
        Thread mCaptureThread;

        //Allows for thread termination
        CancellationTokenSource mCancellationToken = new();

        //Indicator for the capturing state
        bool mIsCapturing = false;

        private Button StartStopBtn;
        private PictureBox VideoPictureBox;
        private PictureBox GrayPictureBox;


        public Form1()
        {
            InitializeComponent();
        }

        //Backend Setup given by MSVS
        private void InitializeComponent()
        {
            StartStopBtn = new Button();
            VideoPictureBox = new PictureBox();
            GrayPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)VideoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GrayPictureBox).BeginInit();
            SuspendLayout();
            // 
            // StartStopBtn
            // 
            StartStopBtn.Location = new Point(313, 683);
            StartStopBtn.Name = "StartStopBtn";
            StartStopBtn.Size = new Size(207, 108);
            StartStopBtn.TabIndex = 0;
            StartStopBtn.Text = "Start";
            StartStopBtn.UseVisualStyleBackColor = true;
            StartStopBtn.Click += StartStopBtn_Click;
            // 
            // VideoPictureBox
            // 
            VideoPictureBox.Location = new Point(37, 46);
            VideoPictureBox.Name = "VideoPictureBox";
            VideoPictureBox.Size = new Size(789, 560);
            VideoPictureBox.TabIndex = 1;
            VideoPictureBox.TabStop = false;
            // 
            // GrayPictureBox
            // 
            GrayPictureBox.Location = new Point(916, 46);
            GrayPictureBox.Name = "GrayPictureBox";
            GrayPictureBox.Size = new Size(789, 560);
            GrayPictureBox.TabIndex = 2;
            GrayPictureBox.TabStop = false;
            // 
            // Form1
            // 
            ClientSize = new Size(1757, 886);
            Controls.Add(GrayPictureBox);
            Controls.Add(VideoPictureBox);
            Controls.Add(StartStopBtn);
            Name = "Form1";
            Load += Form1_Load_1;
            ((System.ComponentModel.ISupportInitialize)VideoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)GrayPictureBox).EndInit();
            ResumeLayout(false);
        }

        private void Form1_Load_1(object sender, EventArgs e)
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
                mCaptureThread = new(() => DisplayWebcam(mCancellationToken.Token));
                mCaptureThread.Start();//start the feed
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
                int newHeight = (frame.Size.Height * VideoPictureBox.Size.Width) / frame.Size.Width;
                Size newSize = new Size(VideoPictureBox.Size.Width, newHeight);
                CvInvoke.Resize(frame, frame, newSize);

                //Create a 60 fps frame rate
                Task.Delay(16);

                //Display the current frame
                VideoPictureBox.Image = frame.ToBitmap();
            }
        }

        private void DisplayGrayWebcam(CancellationToken token)
        {
            while (!token.IsCancellationRequested) //While no requested cancellation
            {
                Mat frame = mCapture.QueryFrame(); //grab a new frame

                //resize picture box
                int newHeight = (frame.Size.Height * VideoPictureBox.Size.Width) / frame.Size.Width;
                Size newSize = new Size(VideoPictureBox.Size.Width, newHeight);
                CvInvoke.Resize(frame, frame, newSize);

                CvInvoke.CvtColor(frame, frame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                CvInvoke.Threshold(frame, frame, 150, 255, Emgu.CV.CvEnum.ThreasholdType.Binary);

                //Create a 60 fps frame rate
                Task.Delay(16);

                //Display the current frame
                VideoPictureBox.Image = frame.ToBitmap();
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

    } //public partial class Form1 parathensis
} //namespace parathensis