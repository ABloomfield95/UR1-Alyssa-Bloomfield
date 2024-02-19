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

        //Info for HSV Modifications
        private PictureBox HPictureBox;
        private PictureBox SPictureBox;
        private PictureBox VPictureBox;
        private TrackBar HValMin;
        private TrackBar HValMax;
        private TrackBar SValMin;
        private TrackBar SValMax;
        private TrackBar VValMin;
        private TrackBar VValMax;
        private TextBox RI_Text;
        private TextBox BM_Text;
        private PictureBox HSVPictureBox;
        private TextBox HSV_Text;

        //HSV Trackbar Values
        private int hMin = 0;
        private int hMax = 179;
        private int sMin = 0;
        private int sMax = 255;
        private int vMin = 0;
        private int vMax = 255;

        private TextBox H_Label;
        private TextBox S_Label;
        private TextBox V_Label;

        //HSV Pixel Counting
        private TextBox HSV_FR_Label;
        private TextBox HSV_MR_Label;
        private TextBox HSV_M_Label;
        private TextBox HSV_ML_Label;
        private TextBox HSV_FL_Label;
        private Label HSV_FR;
        private Label HSV_MR;
        private Label HSV_M;
        private Label HSV_ML;
        private Label HSV_FL;
        private Label ColorBox_W;
        private Label ColorBox_R;

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
            HPictureBox = new PictureBox();
            SPictureBox = new PictureBox();
            VPictureBox = new PictureBox();
            HValMin = new TrackBar();
            HValMax = new TrackBar();
            SValMin = new TrackBar();
            SValMax = new TrackBar();
            VValMin = new TrackBar();
            VValMax = new TrackBar();
            RI_Text = new TextBox();
            BM_Text = new TextBox();
            HSVPictureBox = new PictureBox();
            HSV_Text = new TextBox();
            H_Label = new TextBox();
            S_Label = new TextBox();
            V_Label = new TextBox();
            HSV_FR_Label = new TextBox();
            HSV_MR_Label = new TextBox();
            HSV_M_Label = new TextBox();
            HSV_ML_Label = new TextBox();
            HSV_FL_Label = new TextBox();
            HSV_FR = new Label();
            HSV_MR = new Label();
            HSV_M = new Label();
            HSV_ML = new Label();
            HSV_FL = new Label();
            ColorBox_W = new Label();
            ColorBox_R = new Label();
            ((System.ComponentModel.ISupportInitialize)VideoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GrayPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MinTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MaxTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HValMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HValMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SValMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SValMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VValMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VValMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HSVPictureBox).BeginInit();
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
            MinTrackBar.Location = new Point(916, 674);
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
            // HPictureBox
            // 
            HPictureBox.Location = new Point(2502, 46);
            HPictureBox.Name = "HPictureBox";
            HPictureBox.Size = new Size(436, 281);
            HPictureBox.TabIndex = 19;
            HPictureBox.TabStop = false;
            HPictureBox.Click += HPictureBox_Click;
            // 
            // SPictureBox
            // 
            SPictureBox.Location = new Point(2502, 538);
            SPictureBox.Name = "SPictureBox";
            SPictureBox.Size = new Size(436, 281);
            SPictureBox.TabIndex = 20;
            SPictureBox.TabStop = false;
            SPictureBox.Click += SPictureBox_Click;
            // 
            // VPictureBox
            // 
            VPictureBox.Location = new Point(2502, 1031);
            VPictureBox.Name = "VPictureBox";
            VPictureBox.Size = new Size(436, 281);
            VPictureBox.TabIndex = 21;
            VPictureBox.TabStop = false;
            VPictureBox.Click += VPictureBox_Click;
            // 
            // HValMin
            // 
            HValMin.Location = new Point(2502, 330);
            HValMin.Maximum = 179;
            HValMin.Name = "HValMin";
            HValMin.Size = new Size(338, 114);
            HValMin.TabIndex = 22;
            HValMin.Value = 100;
            HValMin.Scroll += HValueMin_Scroll;
            // 
            // HValMax
            // 
            HValMax.Location = new Point(2502, 418);
            HValMax.Maximum = 179;
            HValMax.Name = "HValMax";
            HValMax.Size = new Size(338, 114);
            HValMax.TabIndex = 23;
            HValMax.Value = 100;
            HValMax.Scroll += HValMax_Scroll;
            // 
            // SValMin
            // 
            SValMin.Location = new Point(2502, 825);
            SValMin.Maximum = 255;
            SValMin.Name = "SValMin";
            SValMin.Size = new Size(338, 114);
            SValMin.TabIndex = 24;
            SValMin.Value = 100;
            SValMin.Scroll += SValMin_Scroll;
            // 
            // SValMax
            // 
            SValMax.Location = new Point(2502, 911);
            SValMax.Maximum = 255;
            SValMax.Name = "SValMax";
            SValMax.Size = new Size(338, 114);
            SValMax.TabIndex = 25;
            SValMax.Value = 100;
            SValMax.Scroll += SValMax_Scroll;
            // 
            // VValMin
            // 
            VValMin.Location = new Point(2502, 1318);
            VValMin.Maximum = 255;
            VValMin.Name = "VValMin";
            VValMin.Size = new Size(338, 114);
            VValMin.TabIndex = 26;
            VValMin.Value = 100;
            VValMin.Scroll += VValMin_Scroll;
            // 
            // VValMax
            // 
            VValMax.Location = new Point(2502, 1401);
            VValMax.Maximum = 255;
            VValMax.Name = "VValMax";
            VValMax.Size = new Size(338, 114);
            VValMax.TabIndex = 27;
            VValMax.Value = 100;
            VValMax.Scroll += VValMax_Scroll;
            // 
            // RI_Text
            // 
            RI_Text.Location = new Point(37, 612);
            RI_Text.Name = "RI_Text";
            RI_Text.Size = new Size(155, 47);
            RI_Text.TabIndex = 28;
            RI_Text.Text = "Raw Image";
            // 
            // BM_Text
            // 
            BM_Text.Location = new Point(916, 612);
            BM_Text.Name = "BM_Text";
            BM_Text.Size = new Size(275, 47);
            BM_Text.TabIndex = 29;
            BM_Text.Text = "Binary Modification";
            // 
            // HSVPictureBox
            // 
            HSVPictureBox.Location = new Point(37, 1031);
            HSVPictureBox.Name = "HSVPictureBox";
            HSVPictureBox.Size = new Size(789, 560);
            HSVPictureBox.TabIndex = 30;
            HSVPictureBox.TabStop = false;
            HSVPictureBox.Click += HSVPictureBox_Click;
            // 
            // HSV_Text
            // 
            HSV_Text.Location = new Point(37, 1597);
            HSV_Text.Name = "HSV_Text";
            HSV_Text.Size = new Size(239, 47);
            HSV_Text.TabIndex = 31;
            HSV_Text.Text = "HSV Modification";
            // 
            // H_Label
            // 
            H_Label.Location = new Point(2502, 46);
            H_Label.Name = "H_Label";
            H_Label.Size = new Size(29, 47);
            H_Label.TabIndex = 32;
            H_Label.Text = "H";
            H_Label.TextChanged += H_Label_TextChanged;
            // 
            // S_Label
            // 
            S_Label.Location = new Point(2502, 538);
            S_Label.Name = "S_Label";
            S_Label.Size = new Size(29, 47);
            S_Label.TabIndex = 33;
            S_Label.Text = "S";
            S_Label.TextChanged += S_Label_TextChanged;
            // 
            // V_Label
            // 
            V_Label.Location = new Point(2502, 1031);
            V_Label.Name = "V_Label";
            V_Label.Size = new Size(29, 47);
            V_Label.TabIndex = 34;
            V_Label.Text = "V";
            V_Label.TextChanged += V_Label_TextChanged;
            // 
            // HSV_FR_Label
            // 
            HSV_FR_Label.Location = new Point(893, 1509);
            HSV_FR_Label.Name = "HSV_FR_Label";
            HSV_FR_Label.Size = new Size(283, 47);
            HSV_FR_Label.TabIndex = 44;
            HSV_FR_Label.Text = "Far Right Pixel Count";
            HSV_FR_Label.TextChanged += HSV_FR_Label_TextChanged;
            // 
            // HSV_MR_Label
            // 
            HSV_MR_Label.Location = new Point(901, 1389);
            HSV_MR_Label.Name = "HSV_MR_Label";
            HSV_MR_Label.Size = new Size(342, 47);
            HSV_MR_Label.TabIndex = 43;
            HSV_MR_Label.Text = "Middle Right Pixel Count";
            HSV_MR_Label.TextChanged += HSV_MR_Label_TextChanged;
            // 
            // HSV_M_Label
            // 
            HSV_M_Label.Location = new Point(901, 1276);
            HSV_M_Label.Name = "HSV_M_Label";
            HSV_M_Label.Size = new Size(263, 47);
            HSV_M_Label.TabIndex = 42;
            HSV_M_Label.Text = "Middle Pixel Count";
            HSV_M_Label.TextChanged += HSV_M_Label_TextChanged;
            // 
            // HSV_ML_Label
            // 
            HSV_ML_Label.Location = new Point(901, 1151);
            HSV_ML_Label.Name = "HSV_ML_Label";
            HSV_ML_Label.Size = new Size(317, 47);
            HSV_ML_Label.TabIndex = 41;
            HSV_ML_Label.Text = "Middle Left Pixel Count";
            HSV_ML_Label.TextChanged += HSV_ML_Label_TextChanged;
            // 
            // HSV_FL_Label
            // 
            HSV_FL_Label.Location = new Point(901, 1031);
            HSV_FL_Label.Name = "HSV_FL_Label";
            HSV_FL_Label.Size = new Size(263, 47);
            HSV_FL_Label.TabIndex = 40;
            HSV_FL_Label.Text = "Far Left Pixel Count";
            HSV_FL_Label.TextChanged += HSV_FL_Label_TextChanged;
            // 
            // HSV_FR
            // 
            HSV_FR.AutoSize = true;
            HSV_FR.Location = new Point(893, 1550);
            HSV_FR.Name = "HSV_FR";
            HSV_FR.Size = new Size(127, 41);
            HSV_FR.TabIndex = 39;
            HSV_FR.Text = "FarRight";
            HSV_FR.Click += HSV_FR_Click;
            // 
            // HSV_MR
            // 
            HSV_MR.AutoSize = true;
            HSV_MR.Location = new Point(901, 1439);
            HSV_MR.Name = "HSV_MR";
            HSV_MR.Size = new Size(181, 41);
            HSV_MR.TabIndex = 38;
            HSV_MR.Text = "MiddleRight";
            HSV_MR.Click += HSV_MR_Click;
            // 
            // HSV_M
            // 
            HSV_M.AutoSize = true;
            HSV_M.Location = new Point(901, 1315);
            HSV_M.Name = "HSV_M";
            HSV_M.Size = new Size(111, 41);
            HSV_M.TabIndex = 37;
            HSV_M.Text = "Middle";
            HSV_M.Click += HSV_M_Click;
            // 
            // HSV_ML
            // 
            HSV_ML.AutoSize = true;
            HSV_ML.Location = new Point(901, 1201);
            HSV_ML.Name = "HSV_ML";
            HSV_ML.Size = new Size(160, 41);
            HSV_ML.TabIndex = 36;
            HSV_ML.Text = "MiddleLeft";
            HSV_ML.Click += HSV_ML_Click;
            // 
            // HSV_FL
            // 
            HSV_FL.AutoSize = true;
            HSV_FL.Location = new Point(901, 1081);
            HSV_FL.Name = "HSV_FL";
            HSV_FL.Size = new Size(106, 41);
            HSV_FL.TabIndex = 35;
            HSV_FL.Text = "FarLeft";
            HSV_FL.Click += HSV_FL_Click;
            // 
            // ColorBox_W
            // 
            ColorBox_W.AutoSize = true;
            ColorBox_W.Location = new Point(1187, 618);
            ColorBox_W.Name = "ColorBox_W";
            ColorBox_W.Size = new Size(114, 41);
            ColorBox_W.TabIndex = 45;
            ColorBox_W.Text = "(White)";
            // 
            // ColorBox_R
            // 
            ColorBox_R.AutoSize = true;
            ColorBox_R.Location = new Point(282, 1603);
            ColorBox_R.Name = "ColorBox_R";
            ColorBox_R.Size = new Size(87, 41);
            ColorBox_R.TabIndex = 46;
            ColorBox_R.Text = "(Red)";
            // 
            // Form1
            // 
            ClientSize = new Size(3168, 1775);
            Controls.Add(ColorBox_R);
            Controls.Add(ColorBox_W);
            Controls.Add(HSV_FR_Label);
            Controls.Add(HSV_MR_Label);
            Controls.Add(HSV_M_Label);
            Controls.Add(HSV_ML_Label);
            Controls.Add(HSV_FL_Label);
            Controls.Add(HSV_FR);
            Controls.Add(HSV_MR);
            Controls.Add(HSV_M);
            Controls.Add(HSV_ML);
            Controls.Add(HSV_FL);
            Controls.Add(V_Label);
            Controls.Add(S_Label);
            Controls.Add(H_Label);
            Controls.Add(HSV_Text);
            Controls.Add(HSVPictureBox);
            Controls.Add(BM_Text);
            Controls.Add(RI_Text);
            Controls.Add(VValMax);
            Controls.Add(VValMin);
            Controls.Add(SValMax);
            Controls.Add(SValMin);
            Controls.Add(HValMax);
            Controls.Add(HValMin);
            Controls.Add(VPictureBox);
            Controls.Add(SPictureBox);
            Controls.Add(HPictureBox);
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
            ((System.ComponentModel.ISupportInitialize)HPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)SPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)VPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)HValMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)HValMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)SValMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)SValMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)VValMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)VValMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)HSVPictureBox).EndInit();
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

            minGrayValue = MinTrackBar.Value;
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

        //The 6 trackbars for the HSV Modifications
        private void HValueMin_Scroll(object sender, EventArgs e)
        {
            if (HValMin.Value > HValMax.Value)
            {
                HValMax.Value = HValMin.Value;
            }

            hMin = HValMin.Value;
        }

        private void HValMax_Scroll(object sender, EventArgs e)
        {
            if (HValMin.Value > HValMax.Value)
            {
                HValMin.Value = HValMax.Value;
            }
            hMax = HValMax.Value;
        }

        private void SValMin_Scroll(object sender, EventArgs e)
        {
            if (SValMin.Value > SValMax.Value)
            {
                SValMax.Value = SValMin.Value;
            }

            sMin = SValMin.Value;
        }

        private void SValMax_Scroll(object sender, EventArgs e)
        {
            if (SValMin.Value > SValMax.Value)
            {
                SValMin.Value = SValMax.Value;
            }
            sMax = SValMax.Value;
        }

        private void VValMin_Scroll(object sender, EventArgs e)
        {
            if (VValMin.Value > VValMax.Value)
            {
                VValMax.Value = VValMin.Value;
            }

            vMin = VValMin.Value;
        }

        private void VValMax_Scroll(object sender, EventArgs e)
        {
            if (VValMin.Value > VValMax.Value)
            {
                VValMin.Value = VValMax.Value;
            }
            vMax = VValMax.Value;
        }

        private void DisplayWebcam(CancellationToken token)
        {
            while (!token.IsCancellationRequested) //While no requested cancellation
            {
                Mat frame = mCapture.QueryFrame(); //grab a new frame
                Mat GrayFrame = frame.Clone();

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
                CvInvoke.Resize(GrayFrame, GrayFrame, newSize);
                CvInvoke.CvtColor(GrayFrame, GrayFrame, ColorConversion.Bgr2Gray);
                CvInvoke.Threshold(GrayFrame, GrayFrame, minGrayValue, maxGrayValue, Emgu.CV.CvEnum.ThresholdType.Binary);
                GrayPictureBox.Image = GrayFrame.ToBitmap();

                //Far Left Pixels - Binary
                int whitePixelsFarLeft = 0;
                Image<Gray, byte> img = GrayFrame.ToImage<Gray, byte>();
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


                //Mid Left Pixels - Binary
                int whitePixelsMidLeft = 0;
                Image<Gray, byte> img2 = GrayFrame.ToImage<Gray, byte>();
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


                //Middle Pixels  - Binary
                int whitePixelsMid = 0;
                Image<Gray, byte> img3 = GrayFrame.ToImage<Gray, byte>();
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


                //Mid Right Pixels  - Binary
                int whitePixelsMidRight = 0;
                Image<Gray, byte> img4 = GrayFrame.ToImage<Gray, byte>();
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


                //Far Right Pixels  - Binary
                int whitePixelsFarRight = 0;
                Image<Gray, byte> img5 = GrayFrame.ToImage<Gray, byte>();
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

                //HSV Code

                Mat hsvFrame = new Mat();
                CvInvoke.CvtColor(frame, hsvFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

                Mat[] hsvChannels = hsvFrame.Split();

                Mat hueFilter = new Mat();
                CvInvoke.InRange(hsvChannels[0], new ScalarArray(hMin), new ScalarArray(hMax), hueFilter);
                Invoke(new Action(() => { HPictureBox.Image = hueFilter.ToBitmap(); }));

                Mat saturationFilter = new Mat();
                CvInvoke.InRange(hsvChannels[1], new ScalarArray(sMin), new ScalarArray(sMax), saturationFilter);
                Invoke(new Action(() => { SPictureBox.Image = saturationFilter.ToBitmap(); }));

                Mat valueFilter = new Mat();
                CvInvoke.InRange(hsvChannels[2], new ScalarArray(vMin), new ScalarArray(vMax), valueFilter);
                Invoke(new Action(() => { VPictureBox.Image = valueFilter.ToBitmap(); }));

                Mat mergedImage = new Mat();
                CvInvoke.BitwiseAnd(hueFilter, saturationFilter, mergedImage);
                CvInvoke.BitwiseAnd(mergedImage, valueFilter, mergedImage);
                CvInvoke.Resize(mergedImage, mergedImage, newSize);
                Invoke(new Action(() => { HSVPictureBox.Image = mergedImage.ToBitmap(); }));

                //HSV White Pixel Counting

                //Far Left Pixels - HSV
                int whitePixelsFarLeftHSV = 0;
                Image<Gray, byte> imgHSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 0; x < frame.Width / 5; x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (imgHSV.Data[y, x, 0] == 255)
                            whitePixelsFarLeftHSV++;
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV_FL.Text = $"{whitePixelsFarLeftHSV} White Pixels";
                }));


                //Mid Left Pixels - HSV
                int whitePixelsMidLeftHSV = 0;
                Image<Gray, byte> img2HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = frame.Width / 5; x < 2 * (frame.Width / 5); x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img2HSV.Data[y, x, 0] == 255)
                            whitePixelsMidLeftHSV++;
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV_ML.Text = $"{whitePixelsMidLeftHSV} White Pixels";
                }));


                //Middle Pixels  - HSV
                int whitePixelsMidHSV = 0;
                Image<Gray, byte> img3HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 2 * (frame.Width / 5); x < 3 * (frame.Width / 5); x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img3HSV.Data[y, x, 0] == 255)
                        { whitePixelsMidHSV++; }
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV_M.Text = $"{whitePixelsMidHSV} White Pixels";
                }));


                //Mid Right Pixels  - HSV
                int whitePixelsMidRightHSV = 0;
                Image<Gray, byte> img4HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 3 * (frame.Width / 5); x < 4 * (frame.Width / 5); x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img4HSV.Data[y, x, 0] == 255)
                        { whitePixelsMidRightHSV++; }
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV_MR.Text = $"{whitePixelsMidRightHSV} White Pixels";
                }));


                //Far Right Pixels  - HSV
                int whitePixelsFarRightHSV = 0;
                Image<Gray, byte> img5HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 5 * (frame.Width / 5); x < frame.Width; x++)
                {
                    for (int y = 0; frame.Height > y; y++)
                    {
                        if (img5HSV.Data[y, x, 0] == 255)
                            whitePixelsFarRightHSV++;
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV_FR.Text = $"{whitePixelsFarRightHSV} White Pixels";
                }));
            } //while loop closer

            //Delete used mats to free up space
            /*hsvFrame.Dispose();
            hueFilter.Dispose();
            saturationFilter.Dispose();
            valueFilter.Dispose();
            mergedImage.Dispose();*/
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

        private void HSVPictureBox_Click(object sender, EventArgs e)
        {

        }
        private void HPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void SPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void VPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void H_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void S_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void V_Label_TextChanged(object sender, EventArgs e)
        {

        }
        private void HSV_FL_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void HSV_ML_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void HSV_M_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void HSV_MR_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void HSV_FR_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void HSV_FL_Click(object sender, EventArgs e)
        {

        }

        private void HSV_ML_Click(object sender, EventArgs e)
        {

        }

        private void HSV_M_Click(object sender, EventArgs e)
        {

        }

        private void HSV_MR_Click(object sender, EventArgs e)
        {

        }

        private void HSV_FR_Click(object sender, EventArgs e)
        {

        }
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