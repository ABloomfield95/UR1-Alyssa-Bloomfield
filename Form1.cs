using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

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

        //CancellationTokenSource _CancellationToken = new();

        //For the black and white picture box
        private PictureBox GrayPictureBox;

        private int minGrayValue = 10;
        private int maxGrayValue = 255;

        private TrackBar MinTrackBar;
        private TrackBar MaxTrackBar;

        //Textbox headers for the pixel counts
        private TextBox FL_Text_Label1;
        private TextBox ML_Text_Label;
        private TextBox M_Text_Label;
        private TextBox MR_Text_Label;
        private TextBox FR_Text_Label;

        //Labels that will display the pixel count
        private Label FL_Text_Label;
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
            FL_Text_Label = new Label();
            MidLeft = new Label();
            Middle = new Label();
            MidRight = new Label();
            FarRight = new Label();
            FL_Text_Label1 = new TextBox();
            ML_Text_Label = new TextBox();
            M_Text_Label = new TextBox();
            MR_Text_Label = new TextBox();
            FR_Text_Label = new TextBox();
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
            // FL_Text_Label
            // 
            FL_Text_Label.AutoSize = true;
            FL_Text_Label.Location = new Point(1810, 96);
            FL_Text_Label.Name = "FL_Text_Label";
            FL_Text_Label.Size = new Size(106, 41);
            FL_Text_Label.TabIndex = 5;
            FL_Text_Label.Text = "FarLeft";
            FL_Text_Label.Click += FarLeft_Click;
            // 
            // MidLeft
            // 
            MidLeft.AutoSize = true;
            MidLeft.Location = new Point(1810, 216);
            MidLeft.Name = "MidLeft";
            MidLeft.Size = new Size(119, 41);
            MidLeft.TabIndex = 7;
            MidLeft.Text = "MidLeft";
            MidLeft.Click += MidLeft_Click;
            // 
            // Middle
            // 
            Middle.AutoSize = true;
            Middle.Location = new Point(1810, 330);
            Middle.Name = "Middle";
            Middle.Size = new Size(111, 41);
            Middle.TabIndex = 9;
            Middle.Text = "Middle";
            Middle.Click += Middle_Click;
            // 
            // MidRight
            // 
            MidRight.AutoSize = true;
            MidRight.Location = new Point(1810, 454);
            MidRight.Name = "MidRight";
            MidRight.Size = new Size(140, 41);
            MidRight.TabIndex = 11;
            MidRight.Text = "MidRight";
            MidRight.Click += MidRight_Click;
            // 
            // FarRight
            // 
            FarRight.AutoSize = true;
            FarRight.Location = new Point(1802, 565);
            FarRight.Name = "FarRight";
            FarRight.Size = new Size(127, 41);
            FarRight.TabIndex = 13;
            FarRight.Text = "FarRight";
            FarRight.Click += FarRight_Click;
            // 
            // FL_Text_Label1
            // 
            FL_Text_Label1.Location = new Point(1810, 46);
            FL_Text_Label1.Name = "FL_Text_Label1";
            FL_Text_Label1.Size = new Size(263, 47);
            FL_Text_Label1.TabIndex = 14;
            FL_Text_Label1.Text = "Far Left Pixel Count";
            // 
            // ML_Text_Label
            // 
            ML_Text_Label.Location = new Point(1810, 166);
            ML_Text_Label.Name = "ML_Text_Label";
            ML_Text_Label.Size = new Size(317, 47);
            ML_Text_Label.TabIndex = 15;
            ML_Text_Label.Text = "Middle Left Pixel Count";
            // 
            // M_Text_Label
            // 
            M_Text_Label.Location = new Point(1810, 291);
            M_Text_Label.Name = "M_Text_Label";
            M_Text_Label.Size = new Size(263, 47);
            M_Text_Label.TabIndex = 16;
            M_Text_Label.Text = "Middle Pixel Count";
            // 
            // MR_Text_Label
            // 
            MR_Text_Label.Location = new Point(1810, 404);
            MR_Text_Label.Name = "MR_Text_Label";
            MR_Text_Label.Size = new Size(342, 47);
            MR_Text_Label.TabIndex = 17;
            MR_Text_Label.Text = "Middle Right Pixel Count";
            // 
            // FR_Text_Label
            // 
            FR_Text_Label.Location = new Point(1802, 524);
            FR_Text_Label.Name = "FR_Text_Label";
            FR_Text_Label.Size = new Size(283, 47);
            FR_Text_Label.TabIndex = 18;
            FR_Text_Label.Text = "Far Right Pixel Count";
            // 
            // Form1
            // 
            ClientSize = new Size(2332, 984);
            Controls.Add(FR_Text_Label);
            Controls.Add(MR_Text_Label);
            Controls.Add(M_Text_Label);
            Controls.Add(ML_Text_Label);
            Controls.Add(FL_Text_Label1);
            Controls.Add(FarRight);
            Controls.Add(MidRight);
            Controls.Add(Middle);
            Controls.Add(MidLeft);
            Controls.Add(FL_Text_Label);
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

                if (frame == null)
                {
                    continue;
                }

                //resize picture box
                int newHeight = (frame.Size.Height * VideoPictureBox.Size.Width) / frame.Size.Width;
                Size newSize = new(VideoPictureBox.Size.Width, newHeight);
                CvInvoke.Resize(frame, frame, newSize);

                //Create a 60 fps frame rate
                _ = Task.Delay(16);

                //Display the current frame
                VideoPictureBox.Image = frame.ToBitmap();

                //Modifications to change the second frame
                CvInvoke.CvtColor(frame, frame, ColorConversion.Bgr2Gray);
                CvInvoke.Threshold(frame, frame, minGrayValue, maxGrayValue, Emgu.CV.CvEnum.ThresholdType.Binary);
                GrayPictureBox.Image = frame.ToBitmap();

                //Far Left Pixels
                int whitePixelsFarLeft = 0;
                Image<Gray, byte> img = frame.ToImage<Gray, byte>();
                for (int x = 0; x < frame.Width / 5; x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img.Data[y, x, 0] == 255)
                            whitePixelsFarLeft++;
                    }
                }

                Invoke(new Action(() =>
                {
                    FL_Text_Label.Text = $"{whitePixelsFarLeft} White Pixels";
                }));


                //Mid Left Pixels
                int whitePixelsMidLeft = 0;
                Image<Gray, byte> img2 = frame.ToImage<Gray, byte>();
                for (int x = frame.Width / 5; x < 2 * (frame.Width / 5); x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img2.Data[y, x, 0] == 255)
                            whitePixelsMidLeft++;
                    }
                }

                Invoke(new Action(() =>
                {
                    MidLeft.Text = $"{whitePixelsMidLeft} White Pixels";
                }));


                //Middle Pixels
                int whitePixelsMid = 0;
                Image<Gray, byte> img3 = frame.ToImage<Gray, byte>();
                for (int x = 2 * (frame.Width / 5); x < 3 * (frame.Width / 5); x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img3.Data[y, x, 0] == 255)
                        { whitePixelsMid++; }
                    }
                }

                Invoke(new Action(() =>
                {
                    Middle.Text = $"{whitePixelsMid} White Pixels";
                }));


                //Mid Right Pixels
                int whitePixelsMidRight = 0;
                Image<Gray, byte> img4 = frame.ToImage<Gray, byte>();
                for (int x = 3 * (frame.Width / 5); x < 4 * (frame.Width / 5); x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img4.Data[y, x, 0] == 255)
                        { whitePixelsMidRight++; }
                    }
                }

                Invoke(new Action(() =>
                {
                    MidRight.Text = $"{whitePixelsMidRight} White Pixels";
                }));


                //Far Right Pixels
                int whitePixelsFarRight = 0;
                Image<Gray, byte> img5 = frame.ToImage<Gray, byte>();
                for (int x = 5 * (frame.Width / 5); x < frame.Width; x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img5.Data[y, x, 0] == 255)
                            whitePixelsFarRight++;
                    }
                }

                Invoke(new Action(() =>
                {
                    FarRight.Text = $"{whitePixelsFarRight} White Pixels";
                }));
            } //while loop closer
        } //private void closer

        private void FarLeft_Click(object sender, EventArgs e)
        {

        }//FarLeft_Click parathensis

        private void MidLeft_Click(object sender, EventArgs e)
        {

        }//MidLeft Parathensis

        private void Middle_Click(object sender, EventArgs e)
        {

        }//Middle Parathensis

        private void MidRight_Click(object sender, EventArgs e)
        {

        }//MidRight Click parathensis

        private void FarRight_Click(object sender, EventArgs e)
        {

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