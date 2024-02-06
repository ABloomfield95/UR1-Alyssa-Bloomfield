using Emgu.CV;
using Emgu.CV.CvEnum;

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

        CancellationTokenSource _CancellationToken = new();

        //For the black and white picture box
        private PictureBox GrayPictureBox;

        private int minGrayValue = 10;
        private int maxGrayValue = 255;

        private TrackBar MinTrackBar;
        private TrackBar MaxTrackBar;

        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e) //min
        {
            if (MinTrackBar.Value > MaxTrackBar.Value)
            {
                MaxTrackBar.Value = MinTrackBar.Value;
            }

            minGrayValue = MaxTrackBar.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e) //max
        {
            if (MinTrackBar.Value > MaxTrackBar.Value)
            {
                MinTrackBar.Value = MaxTrackBar.Value;
            }
            maxGrayValue = MaxTrackBar.Value;
        }


        //Backend Setup given by MSVS
        private void InitializeComponent()
        {
            StartStopBtn = new Button();
            VideoPictureBox = new PictureBox();
            GrayPictureBox = new PictureBox();
            MinTrackBar = new TrackBar();
            MaxTrackBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)VideoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GrayPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MinTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MaxTrackBar).BeginInit();
            SuspendLayout();
            // 
            // StartStopBtn
            // 
            StartStopBtn.Location = new Point(332, 715);
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
            // MinTrackBar
            // 
            MinTrackBar.Location = new Point(1033, 655);
            MinTrackBar.Maximum = 255;
            MinTrackBar.Minimum = 100;
            MinTrackBar.Name = "MinTrackBar";
            MinTrackBar.Size = new Size(545, 114);
            MinTrackBar.TabIndex = 3;
            MinTrackBar.Value = 100;
            MinTrackBar.Scroll += trackBar1_Scroll;
            // 
            // MaxTrackBar
            // 
            MaxTrackBar.Location = new Point(1033, 821);
            MaxTrackBar.Maximum = 255;
            MaxTrackBar.Minimum = 100;
            MaxTrackBar.Name = "MaxTrackBar";
            MaxTrackBar.Size = new Size(545, 114);
            MaxTrackBar.TabIndex = 4;
            MaxTrackBar.Value = 100;
            MaxTrackBar.Scroll += trackBar2_Scroll;
            // 
            // Form1
            // 
            ClientSize = new Size(1882, 984);
            Controls.Add(MaxTrackBar);
            Controls.Add(MinTrackBar);
            Controls.Add(GrayPictureBox);
            Controls.Add(VideoPictureBox);
            Controls.Add(StartStopBtn);
            Name = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load_1;
            ((System.ComponentModel.ISupportInitialize)VideoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)GrayPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)MinTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)MaxTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {
                //Initailize with the plugged camera
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

                //resize gray picture box
                int newGHeight = (frame.Size.Height * GrayPictureBox.Size.Width) / frame.Size.Width;
                Size newGSize = new Size(GrayPictureBox.Size.Width, newHeight);

                //Create a 60 fps frame rate
                Task.Delay(16);

                //Display the current frame
                VideoPictureBox.Image = frame.ToBitmap();

                CvInvoke.CvtColor(frame, frame, ColorConversion.Bgr2Gray);
                CvInvoke.Threshold(frame, frame, minGrayValue, maxGrayValue, Emgu.CV.CvEnum.ThresholdType.Binary);
                GrayPictureBox.Image = frame.ToBitmap();


            } //while loop closer
        } //private void closer

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //mCaptureThread.Abort();

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