using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dai;
using Emgu.CV.Structure;

namespace UR1_Alyssa_Bloomfield
{
    public partial class Form1 : Form
    {
        private Mat frame;

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

        //CancellationTokenSource _CancellationToken = new();

        //For the black and white picture box
        private PictureBox GrayPictureBox;

        private int minGrayValue = 10;
        private int maxGrayValue = 255;

        private TrackBar MinTrackBar;
        private TrackBar MaxTrackBar;

        private Label FarLeft;
        private Label MidLeft;
        private Label Middle;
        private Label MidRight;
        private Label FarRight;

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
            MinTrackBar = new TrackBar();
            MaxTrackBar = new TrackBar();
            FarLeft = new Label();
            MidLeft = new Label();
            Middle = new Label();
            MidRight = new Label();
            FarRight = new Label();
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
            MinTrackBar.Location = new Point(916, 653);
            MinTrackBar.Maximum = 255;
            MinTrackBar.Minimum = 100;
            MinTrackBar.Name = "MinTrackBar";
            MinTrackBar.Size = new Size(789, 114);
            MinTrackBar.TabIndex = 3;
            MinTrackBar.Value = 100;
            MinTrackBar.Scroll += TrackBar1_Scroll;
            // 
            // MaxTrackBar
            // 
            MaxTrackBar.Location = new Point(916, 821);
            MaxTrackBar.Maximum = 255;
            MaxTrackBar.Minimum = 100;
            MaxTrackBar.Name = "MaxTrackBar";
            MaxTrackBar.Size = new Size(789, 114);
            MaxTrackBar.TabIndex = 4;
            MaxTrackBar.Value = 100;
            MaxTrackBar.Scroll += TrackBar2_Scroll;
            // 
            // FarLeft
            // 
            FarLeft.AutoSize = true;
            FarLeft.Location = new Point(1810, 46);
            FarLeft.Name = "FarLeft";
            FarLeft.Size = new Size(106, 41);
            FarLeft.TabIndex = 5;
            FarLeft.Text = "FarLeft";
            FarLeft.Click += FarLeft_Click;
            // 
            // MidLeft
            // 
            MidLeft.AutoSize = true;
            MidLeft.Location = new Point(1810, 160);
            MidLeft.Name = "MidLeft";
            MidLeft.Size = new Size(119, 41);
            MidLeft.TabIndex = 7;
            MidLeft.Text = "MidLeft";
            MidLeft.Click += MidLeft_Click;
            // 
            // Middle
            // 
            Middle.AutoSize = true;
            Middle.Location = new Point(1810, 271);
            Middle.Name = "Middle";
            Middle.Size = new Size(111, 41);
            Middle.TabIndex = 9;
            Middle.Text = "Middle";
            Middle.Click += Middle_Click;
            // 
            // MidRight
            // 
            MidRight.AutoSize = true;
            MidRight.Location = new Point(1810, 376);
            MidRight.Name = "MidRight";
            MidRight.Size = new Size(140, 41);
            MidRight.TabIndex = 11;
            MidRight.Text = "MidRight";
            MidRight.Click += MidRight_Click;
            // 
            // FarRight
            // 
            FarRight.AutoSize = true;
            FarRight.Location = new Point(1810, 485);
            FarRight.Name = "FarRight";
            FarRight.Size = new Size(127, 41);
            FarRight.TabIndex = 13;
            FarRight.Text = "FarRight";
            FarRight.Click += FarRight_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(2332, 984);
            Controls.Add(FarRight);
            Controls.Add(MidRight);
            Controls.Add(Middle);
            Controls.Add(MidLeft);
            Controls.Add(FarLeft);
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

        private void TrackBar1_Scroll(object sender, EventArgs e) //min
        {
            if (MinTrackBar.Value > MaxTrackBar.Value)
            {
                MaxTrackBar.Value = MinTrackBar.Value;
            }

            minGrayValue = MaxTrackBar.Value;
        }

        private void TrackBar2_Scroll(object sender, EventArgs e) //max
        {
            if (MinTrackBar.Value > MaxTrackBar.Value)
            {
                MinTrackBar.Value = MaxTrackBar.Value;
            }
            maxGrayValue = MaxTrackBar.Value;
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
                //int newGHeight = (frame.Size.Height * GrayPictureBox.Size.Width) / frame.Size.Width;
                //Size newGSize = new Size(GrayPictureBox.Size.Width, newHeight);

                //Create a 60 fps frame rate
                //Task.Delay(16);

                //Display the current frame
                VideoPictureBox.Image = frame.ToBitmap();

                CvInvoke.CvtColor(frame, frame, ColorConversion.Bgr2Gray);
                CvInvoke.Threshold(frame, frame, minGrayValue, maxGrayValue, Emgu.CV.CvEnum.ThresholdType.Binary);
                GrayPictureBox.Image = frame.ToBitmap();


            } //while loop closer
        } //private void closer

        private void FarLeft_Click(object sender, EventArgs e)
        {
            int whitePixelsExtended = 0;
            Image<Gray, byte> img = frame.ToImage<Gray, byte>();
            for (int x = 0; x < img.Width / 5; x++)
            {
                for (int y = 0; img.Height > y; y++)
                {
                    if (img.Data[y, x, 0] == 255)
                    { whitePixelsExtended++; }
                }
            }

            Invoke(new Action(() =>
            {
                FarLeft.Text = $"{whitePixelsExtended} White Pixels";
            }
            ));
        }//FarLeft_Click parathensis

        private void MidLeft_Click(object sender, EventArgs e)
        {
            int whitePixelsExtended = 0;
            Image<Gray, byte> img = frame.ToImage<Gray, byte>();
            for (int x = img.Width / 5; x < 2 * (img.Width / 5); x++)
            {
                for (int y = 0; img.Height > y; y++)
                {
                    if (img.Data[y, x, 0] == 255)
                    { whitePixelsExtended++; }
                }
            }

            Invoke(new Action(() =>
            {
                MidLeft.Text = $"{whitePixelsExtended} White Pixels";
            }
            ));
        }//MidLeft Parathensis

        private void Middle_Click(object sender, EventArgs e)
        {
            int whitePixelsExtended = 0;
            Image<Gray, byte> img = frame.ToImage<Gray, byte>();
            for (int x = 2 * (img.Width / 5); x < 3 * (img.Width / 5); x++)
            {
                for (int y = 0; img.Height > y; y++)
                {
                    if (img.Data[y, x, 0] == 255)
                    { whitePixelsExtended++; }
                }
            }

            Invoke(new Action(() =>
            {
                Middle.Text = $"{whitePixelsExtended} White Pixels";
            }
            ));
        }//Middle Parathensis

        private void MidRight_Click(object sender, EventArgs e)
        {
            int whitePixelsExtended = 0;
            Image<Gray, byte> img = frame.ToImage<Gray, byte>();
            for (int x = 3 * (img.Width / 5); x < 4 * (img.Width / 5); x++)
            {
                for (int y = 0; img.Height > y; y++)
                {
                    if (img.Data[y, x, 0] == 255)
                    { whitePixelsExtended++; }
                }
            }

            Invoke(new Action(() =>
            {
                MidRight.Text = $"{whitePixelsExtended} White Pixels";
            }
            ));
        }//MidRight Click parathensis

        private void FarRight_Click(object sender, EventArgs e)
        {
            int whitePixelsExtended = 0;
            Image<Gray, byte> img = frame.ToImage<Gray, byte>();
            for (int x = 5 * (img.Width / 5); x < img.Width; x++)
            {
                for (int y = 0; img.Height > y; y++)
                {
                    if (img.Data[y, x, 0] == 255)
                    { whitePixelsExtended++; }
                }
            }

            Invoke(new Action(() =>
            {
                FarRight.Text = $"{whitePixelsExtended} White Pixels";
            }
            ));
        }//FarRight Click Parathensis


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