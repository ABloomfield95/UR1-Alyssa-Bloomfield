using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.IO.Ports;


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
        private TextBox H_Label;
        private TextBox S_Label;
        private TextBox V_Label;

        //HSV Trackbar Values
        private int hMin = 0;
        private int hMax = 179;
        private int sMin = 0;
        private int sMax = 255;
        private int vMin = 0;
        private int vMax = 255;

        //HSV2 Modification Decleration Values
        private TextBox V2_Label;
        private TextBox S2_Label;
        private TextBox H2_Label;
        private TrackBar VVal2Max;
        private TrackBar VVal2Min;
        private TrackBar SVal2Max;
        private TrackBar SVal2Min;
        private TrackBar HVal2Max;
        private TrackBar HVal2Min;
        private PictureBox V2PictureBox;
        private PictureBox S2PictureBox;
        private PictureBox H2PictureBox;

        //HSV Trackbar Values
        private int hMin2 = 0;
        private int hMax2 = 179;
        private int sMin2 = 0;
        private int sMax2 = 255;
        private int vMin2 = 0;
        private int vMax2 = 255;

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
        int whitePixelsFarLeftHSV = 0;
        int whitePixelsMidLeftHSV = 0;
        int whitePixelsMidHSV = 0;
        int whitePixelsMidRightHSV = 0;
        int whitePixelsFarRightHSV = 0;

        //HSV2 Pixel Counting
        private Label ColorBox_Alt;
        private TextBox HSV2_FR_Label;
        private TextBox HSV2_MR_Label;
        private TextBox HSV2_M_Label;
        private TextBox HSV2_ML_Label;
        private TextBox HSV2_FL_Label;
        private Label HSV2_FR;
        private Label HSV2_MR;
        private Label HSV2_M;
        private Label HSV2_ML;
        private Label HSV2_FL;
        private TextBox HSV2_Text;
        private PictureBox HSV2PictureBox;
        int whitePixelsFarLeftHSV2 = 0;
        int whitePixelsMidLeftHSV2 = 0;
        int whitePixelsMidHSV2 = 0;
        int whitePixelsMidRightHSV2 = 0;
        int whitePixelsFarRightHSV2 = 0;

        // Serial 
        SerialPort sPort;
        int serialTransmission = 0;
        private TextBox Serial;

        //Red Pixels
        int redPixels = 0;
        private TextBox RedPixelCount;
        private Label RPC;

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
            ColorBox_Alt = new Label();
            HSV2_FR_Label = new TextBox();
            HSV2_MR_Label = new TextBox();
            HSV2_M_Label = new TextBox();
            HSV2_ML_Label = new TextBox();
            HSV2_FL_Label = new TextBox();
            HSV2_FR = new Label();
            HSV2_MR = new Label();
            HSV2_M = new Label();
            HSV2_ML = new Label();
            HSV2_FL = new Label();
            HSV2_Text = new TextBox();
            HSV2PictureBox = new PictureBox();
            V2_Label = new TextBox();
            S2_Label = new TextBox();
            H2_Label = new TextBox();
            VVal2Max = new TrackBar();
            VVal2Min = new TrackBar();
            SVal2Max = new TrackBar();
            SVal2Min = new TrackBar();
            HVal2Max = new TrackBar();
            HVal2Min = new TrackBar();
            V2PictureBox = new PictureBox();
            S2PictureBox = new PictureBox();
            H2PictureBox = new PictureBox();
            Serial = new TextBox();
            RedPixelCount = new TextBox();
            RPC = new Label();
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
            ((System.ComponentModel.ISupportInitialize)HSV2PictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VVal2Max).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VVal2Min).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SVal2Max).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SVal2Min).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HVal2Max).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HVal2Min).BeginInit();
            ((System.ComponentModel.ISupportInitialize)V2PictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)S2PictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)H2PictureBox).BeginInit();
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
            // 
            // MidLeft
            // 
            MidLeft.AutoSize = true;
            MidLeft.Location = new Point(1810, 216);
            MidLeft.Name = "MidLeft";
            MidLeft.Size = new Size(119, 41);
            MidLeft.TabIndex = 7;
            MidLeft.Text = "MidLeft";
            // 
            // Middle
            // 
            Middle.AutoSize = true;
            Middle.Location = new Point(1810, 330);
            Middle.Name = "Middle";
            Middle.Size = new Size(111, 41);
            Middle.TabIndex = 9;
            Middle.Text = "Middle";
            // 
            // MidRight
            // 
            MidRight.AutoSize = true;
            MidRight.Location = new Point(1810, 454);
            MidRight.Name = "MidRight";
            MidRight.Size = new Size(140, 41);
            MidRight.TabIndex = 11;
            MidRight.Text = "MidRight";
            // 
            // FarRight
            // 
            FarRight.AutoSize = true;
            FarRight.Location = new Point(1802, 565);
            FarRight.Name = "FarRight";
            FarRight.Size = new Size(127, 41);
            FarRight.TabIndex = 13;
            FarRight.Text = "FarRight";
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
            // 
            // SPictureBox
            // 
            SPictureBox.Location = new Point(2502, 538);
            SPictureBox.Name = "SPictureBox";
            SPictureBox.Size = new Size(436, 281);
            SPictureBox.TabIndex = 20;
            SPictureBox.TabStop = false;
            // 
            // VPictureBox
            // 
            VPictureBox.Location = new Point(2502, 1031);
            VPictureBox.Name = "VPictureBox";
            VPictureBox.Size = new Size(436, 281);
            VPictureBox.TabIndex = 21;
            VPictureBox.TabStop = false;
            // 
            // HValMin
            // 
            HValMin.Location = new Point(2516, 330);
            HValMin.Maximum = 179;
            HValMin.Name = "HValMin";
            HValMin.Size = new Size(338, 114);
            HValMin.TabIndex = 22;
            HValMin.Value = 10;
            HValMin.Scroll += HValueMin_Scroll;
            // 
            // HValMax
            // 
            HValMax.Location = new Point(2502, 418);
            HValMax.Maximum = 179;
            HValMax.Name = "HValMax";
            HValMax.Size = new Size(338, 114);
            HValMax.TabIndex = 23;
            HValMax.Value = 40;
            HValMax.Scroll += HValMax_Scroll;
            // 
            // SValMin
            // 
            SValMin.Location = new Point(2502, 825);
            SValMin.Maximum = 255;
            SValMin.Name = "SValMin";
            SValMin.Size = new Size(338, 114);
            SValMin.TabIndex = 24;
            SValMin.Value = 95;
            SValMin.Scroll += SValMin_Scroll;
            // 
            // SValMax
            // 
            SValMax.Location = new Point(2502, 911);
            SValMax.Maximum = 255;
            SValMax.Name = "SValMax";
            SValMax.Size = new Size(338, 114);
            SValMax.TabIndex = 25;
            SValMax.Value = 245;
            SValMax.Scroll += SValMax_Scroll;
            // 
            // VValMin
            // 
            VValMin.Location = new Point(2502, 1318);
            VValMin.Maximum = 255;
            VValMin.Name = "VValMin";
            VValMin.Size = new Size(338, 114);
            VValMin.TabIndex = 26;
            VValMin.Value = 150;
            VValMin.Scroll += VValMin_Scroll;
            // 
            // VValMax
            // 
            VValMax.Location = new Point(2502, 1401);
            VValMax.Maximum = 255;
            VValMax.Name = "VValMax";
            VValMax.Size = new Size(338, 114);
            VValMax.TabIndex = 27;
            VValMax.Value = 250;
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
            // 
            // S_Label
            // 
            S_Label.Location = new Point(2502, 538);
            S_Label.Name = "S_Label";
            S_Label.Size = new Size(29, 47);
            S_Label.TabIndex = 33;
            S_Label.Text = "S";
            // 
            // V_Label
            // 
            V_Label.Location = new Point(2502, 1031);
            V_Label.Name = "V_Label";
            V_Label.Size = new Size(29, 47);
            V_Label.TabIndex = 34;
            V_Label.Text = "V";
            // 
            // HSV_FR_Label
            // 
            HSV_FR_Label.Location = new Point(893, 1509);
            HSV_FR_Label.Name = "HSV_FR_Label";
            HSV_FR_Label.Size = new Size(283, 47);
            HSV_FR_Label.TabIndex = 44;
            HSV_FR_Label.Text = "Far Right Pixel Count";
            // 
            // HSV_MR_Label
            // 
            HSV_MR_Label.Location = new Point(901, 1389);
            HSV_MR_Label.Name = "HSV_MR_Label";
            HSV_MR_Label.Size = new Size(342, 47);
            HSV_MR_Label.TabIndex = 43;
            HSV_MR_Label.Text = "Middle Right Pixel Count";
            // 
            // HSV_M_Label
            // 
            HSV_M_Label.Location = new Point(901, 1276);
            HSV_M_Label.Name = "HSV_M_Label";
            HSV_M_Label.Size = new Size(263, 47);
            HSV_M_Label.TabIndex = 42;
            HSV_M_Label.Text = "Middle Pixel Count";
            // 
            // HSV_ML_Label
            // 
            HSV_ML_Label.Location = new Point(901, 1151);
            HSV_ML_Label.Name = "HSV_ML_Label";
            HSV_ML_Label.Size = new Size(317, 47);
            HSV_ML_Label.TabIndex = 41;
            HSV_ML_Label.Text = "Middle Left Pixel Count";
            // 
            // HSV_FL_Label
            // 
            HSV_FL_Label.Location = new Point(901, 1031);
            HSV_FL_Label.Name = "HSV_FL_Label";
            HSV_FL_Label.Size = new Size(263, 47);
            HSV_FL_Label.TabIndex = 40;
            HSV_FL_Label.Text = "Far Left Pixel Count";
            // 
            // HSV_FR
            // 
            HSV_FR.AutoSize = true;
            HSV_FR.Location = new Point(893, 1550);
            HSV_FR.Name = "HSV_FR";
            HSV_FR.Size = new Size(127, 41);
            HSV_FR.TabIndex = 39;
            HSV_FR.Text = "FarRight";
            // 
            // HSV_MR
            // 
            HSV_MR.AutoSize = true;
            HSV_MR.Location = new Point(901, 1439);
            HSV_MR.Name = "HSV_MR";
            HSV_MR.Size = new Size(181, 41);
            HSV_MR.TabIndex = 38;
            HSV_MR.Text = "MiddleRight";
            // 
            // HSV_M
            // 
            HSV_M.AutoSize = true;
            HSV_M.Location = new Point(901, 1315);
            HSV_M.Name = "HSV_M";
            HSV_M.Size = new Size(111, 41);
            HSV_M.TabIndex = 37;
            HSV_M.Text = "Middle";
            // 
            // HSV_ML
            // 
            HSV_ML.AutoSize = true;
            HSV_ML.Location = new Point(901, 1201);
            HSV_ML.Name = "HSV_ML";
            HSV_ML.Size = new Size(160, 41);
            HSV_ML.TabIndex = 36;
            HSV_ML.Text = "MiddleLeft";
            // 
            // HSV_FL
            // 
            HSV_FL.AutoSize = true;
            HSV_FL.Location = new Point(901, 1081);
            HSV_FL.Name = "HSV_FL";
            HSV_FL.Size = new Size(106, 41);
            HSV_FL.TabIndex = 35;
            HSV_FL.Text = "FarLeft";
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
            ColorBox_R.Size = new Size(120, 41);
            ColorBox_R.TabIndex = 46;
            ColorBox_R.Text = "(Yellow)";
            // 
            // ColorBox_Alt
            // 
            ColorBox_Alt.AutoSize = true;
            ColorBox_Alt.Location = new Point(1529, 1603);
            ColorBox_Alt.Name = "ColorBox_Alt";
            ColorBox_Alt.Size = new Size(87, 41);
            ColorBox_Alt.TabIndex = 59;
            ColorBox_Alt.Text = "(Red)";
            // 
            // HSV2_FR_Label
            // 
            HSV2_FR_Label.Location = new Point(2140, 1509);
            HSV2_FR_Label.Name = "HSV2_FR_Label";
            HSV2_FR_Label.Size = new Size(283, 47);
            HSV2_FR_Label.TabIndex = 58;
            HSV2_FR_Label.Text = "Far Right Pixel Count";
            // 
            // HSV2_MR_Label
            // 
            HSV2_MR_Label.Location = new Point(2148, 1389);
            HSV2_MR_Label.Name = "HSV2_MR_Label";
            HSV2_MR_Label.Size = new Size(342, 47);
            HSV2_MR_Label.TabIndex = 57;
            HSV2_MR_Label.Text = "Middle Right Pixel Count";
            // 
            // HSV2_M_Label
            // 
            HSV2_M_Label.Location = new Point(2148, 1276);
            HSV2_M_Label.Name = "HSV2_M_Label";
            HSV2_M_Label.Size = new Size(263, 47);
            HSV2_M_Label.TabIndex = 56;
            HSV2_M_Label.Text = "Middle Pixel Count";
            // 
            // HSV2_ML_Label
            // 
            HSV2_ML_Label.Location = new Point(2148, 1151);
            HSV2_ML_Label.Name = "HSV2_ML_Label";
            HSV2_ML_Label.Size = new Size(317, 47);
            HSV2_ML_Label.TabIndex = 55;
            HSV2_ML_Label.Text = "Middle Left Pixel Count";
            // 
            // HSV2_FL_Label
            // 
            HSV2_FL_Label.Location = new Point(2148, 1031);
            HSV2_FL_Label.Name = "HSV2_FL_Label";
            HSV2_FL_Label.Size = new Size(263, 47);
            HSV2_FL_Label.TabIndex = 54;
            HSV2_FL_Label.Text = "Far Left Pixel Count";
            // 
            // HSV2_FR
            // 
            HSV2_FR.AutoSize = true;
            HSV2_FR.Location = new Point(2140, 1550);
            HSV2_FR.Name = "HSV2_FR";
            HSV2_FR.Size = new Size(127, 41);
            HSV2_FR.TabIndex = 53;
            HSV2_FR.Text = "FarRight";
            // 
            // HSV2_MR
            // 
            HSV2_MR.AutoSize = true;
            HSV2_MR.Location = new Point(2148, 1439);
            HSV2_MR.Name = "HSV2_MR";
            HSV2_MR.Size = new Size(181, 41);
            HSV2_MR.TabIndex = 52;
            HSV2_MR.Text = "MiddleRight";
            // 
            // HSV2_M
            // 
            HSV2_M.AutoSize = true;
            HSV2_M.Location = new Point(2148, 1315);
            HSV2_M.Name = "HSV2_M";
            HSV2_M.Size = new Size(111, 41);
            HSV2_M.TabIndex = 51;
            HSV2_M.Text = "Middle";
            // 
            // HSV2_ML
            // 
            HSV2_ML.AutoSize = true;
            HSV2_ML.Location = new Point(2148, 1201);
            HSV2_ML.Name = "HSV2_ML";
            HSV2_ML.Size = new Size(160, 41);
            HSV2_ML.TabIndex = 50;
            HSV2_ML.Text = "MiddleLeft";
            // 
            // HSV2_FL
            // 
            HSV2_FL.AutoSize = true;
            HSV2_FL.Location = new Point(2148, 1081);
            HSV2_FL.Name = "HSV2_FL";
            HSV2_FL.Size = new Size(106, 41);
            HSV2_FL.TabIndex = 49;
            HSV2_FL.Text = "FarLeft";
            // 
            // HSV2_Text
            // 
            HSV2_Text.Location = new Point(1284, 1597);
            HSV2_Text.Name = "HSV2_Text";
            HSV2_Text.Size = new Size(239, 47);
            HSV2_Text.TabIndex = 48;
            HSV2_Text.Text = "HSV Modification";
            // 
            // HSV2PictureBox
            // 
            HSV2PictureBox.Location = new Point(1284, 1031);
            HSV2PictureBox.Name = "HSV2PictureBox";
            HSV2PictureBox.Size = new Size(789, 560);
            HSV2PictureBox.TabIndex = 47;
            HSV2PictureBox.TabStop = false;
            // 
            // V2_Label
            // 
            V2_Label.Location = new Point(2993, 1031);
            V2_Label.Name = "V2_Label";
            V2_Label.Size = new Size(29, 47);
            V2_Label.TabIndex = 71;
            V2_Label.Text = "V";
            // 
            // S2_Label
            // 
            S2_Label.Location = new Point(2993, 538);
            S2_Label.Name = "S2_Label";
            S2_Label.Size = new Size(29, 47);
            S2_Label.TabIndex = 70;
            S2_Label.Text = "S";
            // 
            // H2_Label
            // 
            H2_Label.Location = new Point(2993, 46);
            H2_Label.Name = "H2_Label";
            H2_Label.Size = new Size(29, 47);
            H2_Label.TabIndex = 69;
            H2_Label.Text = "H";
            // 
            // VVal2Max
            // 
            VVal2Max.Location = new Point(2993, 1401);
            VVal2Max.Maximum = 255;
            VVal2Max.Name = "VVal2Max";
            VVal2Max.Size = new Size(338, 114);
            VVal2Max.TabIndex = 68;
            VVal2Max.Value = 100;
            VVal2Max.Scroll += VVal2Max_Scroll;
            // 
            // VVal2Min
            // 
            VVal2Min.Location = new Point(2993, 1318);
            VVal2Min.Maximum = 255;
            VVal2Min.Name = "VVal2Min";
            VVal2Min.Size = new Size(338, 114);
            VVal2Min.TabIndex = 67;
            VVal2Min.Value = 100;
            VVal2Min.Scroll += VVal2Min_Scroll;
            // 
            // SVal2Max
            // 
            SVal2Max.Location = new Point(2993, 911);
            SVal2Max.Maximum = 255;
            SVal2Max.Name = "SVal2Max";
            SVal2Max.Size = new Size(338, 114);
            SVal2Max.TabIndex = 66;
            SVal2Max.Value = 255;
            SVal2Max.Scroll += SVal2Max_Scroll;
            // 
            // SVal2Min
            // 
            SVal2Min.Location = new Point(2993, 825);
            SVal2Min.Maximum = 255;
            SVal2Min.Name = "SVal2Min";
            SVal2Min.Size = new Size(338, 114);
            SVal2Min.TabIndex = 65;
            SVal2Min.Value = 180;
            SVal2Min.Scroll += SVal2Min_Scroll;
            // 
            // HVal2Max
            // 
            HVal2Max.Location = new Point(2993, 418);
            HVal2Max.Maximum = 179;
            HVal2Max.Name = "HVal2Max";
            HVal2Max.Size = new Size(338, 114);
            HVal2Max.TabIndex = 64;
            HVal2Max.Value = 178;
            HVal2Max.Scroll += HVal2Max_Scroll;
            // 
            // HVal2Min
            // 
            HVal2Min.Location = new Point(2993, 330);
            HVal2Min.Maximum = 179;
            HVal2Min.Name = "HVal2Min";
            HVal2Min.Size = new Size(338, 114);
            HVal2Min.TabIndex = 63;
            HVal2Min.Value = 160;
            HVal2Min.Scroll += HVal2Min_Scroll;
            // 
            // V2PictureBox
            // 
            V2PictureBox.Location = new Point(2993, 1031);
            V2PictureBox.Name = "V2PictureBox";
            V2PictureBox.Size = new Size(436, 281);
            V2PictureBox.TabIndex = 62;
            V2PictureBox.TabStop = false;
            // 
            // S2PictureBox
            // 
            S2PictureBox.Location = new Point(2993, 538);
            S2PictureBox.Name = "S2PictureBox";
            S2PictureBox.Size = new Size(436, 281);
            S2PictureBox.TabIndex = 61;
            S2PictureBox.TabStop = false;
            // 
            // H2PictureBox
            // 
            H2PictureBox.Location = new Point(2993, 46);
            H2PictureBox.Name = "H2PictureBox";
            H2PictureBox.Size = new Size(436, 281);
            H2PictureBox.TabIndex = 60;
            H2PictureBox.TabStop = false;
            // 
            // Serial
            // 
            Serial.Location = new Point(3024, 1562);
            Serial.Name = "Serial";
            Serial.Size = new Size(272, 47);
            Serial.TabIndex = 72;
            Serial.Text = "Serial Value Display";
            // 
            // RedPixelCount
            // 
            RedPixelCount.Location = new Point(2528, 1544);
            RedPixelCount.Name = "RedPixelCount";
            RedPixelCount.Size = new Size(312, 47);
            RedPixelCount.TabIndex = 73;
            RedPixelCount.Text = "Total Red Pixels";
            // 
            // RPC
            // 
            RPC.AutoSize = true;
            RPC.Location = new Point(2528, 1594);
            RPC.Name = "RPC";
            RPC.Size = new Size(69, 41);
            RPC.TabIndex = 74;
            RPC.Text = "Red";
            // 
            // Form1
            // 
            ClientSize = new Size(3498, 1775);
            Controls.Add(RPC);
            Controls.Add(RedPixelCount);
            Controls.Add(Serial);
            Controls.Add(V2_Label);
            Controls.Add(S2_Label);
            Controls.Add(H2_Label);
            Controls.Add(VVal2Max);
            Controls.Add(VVal2Min);
            Controls.Add(SVal2Max);
            Controls.Add(SVal2Min);
            Controls.Add(HVal2Max);
            Controls.Add(HVal2Min);
            Controls.Add(V2PictureBox);
            Controls.Add(S2PictureBox);
            Controls.Add(H2PictureBox);
            Controls.Add(ColorBox_Alt);
            Controls.Add(HSV2_FR_Label);
            Controls.Add(HSV2_MR_Label);
            Controls.Add(HSV2_M_Label);
            Controls.Add(HSV2_ML_Label);
            Controls.Add(HSV2_FL_Label);
            Controls.Add(HSV2_FR);
            Controls.Add(HSV2_MR);
            Controls.Add(HSV2_M);
            Controls.Add(HSV2_ML);
            Controls.Add(HSV2_FL);
            Controls.Add(HSV2_Text);
            Controls.Add(HSV2PictureBox);
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
            ((System.ComponentModel.ISupportInitialize)HSV2PictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)VVal2Max).EndInit();
            ((System.ComponentModel.ISupportInitialize)VVal2Min).EndInit();
            ((System.ComponentModel.ISupportInitialize)SVal2Max).EndInit();
            ((System.ComponentModel.ISupportInitialize)SVal2Min).EndInit();
            ((System.ComponentModel.ISupportInitialize)HVal2Max).EndInit();
            ((System.ComponentModel.ISupportInitialize)HVal2Min).EndInit();
            ((System.ComponentModel.ISupportInitialize)V2PictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)S2PictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)H2PictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {
                //Initailize with the plugged camera
                mCapture = new VideoCapture(1);

                if (mCapture.Height == 0)
                    throw new Exception("No Cameras Found");

                //Serial Port Setup
                if (sPort == null)
                {
                    sPort = new SerialPort("COM5", 9600);
                    sPort.Open();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
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

        //The 6 trackbars for the HSV2 Modifications
        private void HVal2Min_Scroll(object sender, EventArgs e)
        {
            if (HVal2Min.Value > HVal2Max.Value)
            {
                HVal2Max.Value = HVal2Min.Value;
            }

            hMin2 = HVal2Min.Value;
        }

        private void HVal2Max_Scroll(object sender, EventArgs e)
        {
            if (HVal2Min.Value > HVal2Max.Value)
            {
                HVal2Min.Value = HVal2Max.Value;
            }
            hMax2 = HVal2Max.Value;
        }

        private void SVal2Min_Scroll(object sender, EventArgs e)
        {
            if (SVal2Min.Value > SVal2Max.Value)
            {
                SVal2Max.Value = SVal2Min.Value;
            }

            sMin2 = SVal2Min.Value;
        }

        private void SVal2Max_Scroll(object sender, EventArgs e)
        {
            if (SVal2Min.Value > SVal2Max.Value)
            {
                SVal2Min.Value = SVal2Max.Value;
            }
            sMax2 = SVal2Max.Value;
        }

        private void VVal2Min_Scroll(object sender, EventArgs e)
        {
            if (VVal2Min.Value > VVal2Max.Value)
            {
                VVal2Max.Value = VVal2Min.Value;
            }

            vMin2 = VVal2Min.Value;
        }

        private void VVal2Max_Scroll(object sender, EventArgs e)
        {
            if (VVal2Min.Value > VVal2Max.Value)
            {
                VVal2Min.Value = VVal2Max.Value;
            }
            vMax2 = VVal2Max.Value;
        }

        private void DisplayWebcam(CancellationToken token)
        {
            while (!token.IsCancellationRequested) //While no requested cancellation
            {
                Mat mOriginalImage = mCapture.QueryFrame(); //grab a new frame
                Mat GrayFrame = mOriginalImage.Clone();
                Mat HSVFrame = mOriginalImage.Clone();
                Mat HSV2Frame = mOriginalImage.Clone();

                if (mOriginalImage == null)
                {
                    continue;
                }

                //resize picture box
                int newHeight = (mOriginalImage.Size.Height * VideoPictureBox.Size.Width) / mOriginalImage.Size.Width;
                Size newSize = new(VideoPictureBox.Size.Width, newHeight);
                CvInvoke.Resize(mOriginalImage, mOriginalImage, newSize);

                //Helps to make sure that the pixel count doesnt add/become to large
                whitePixelsFarLeftHSV = 0;
                whitePixelsMidLeftHSV = 0;
                whitePixelsMidHSV = 0;
                whitePixelsMidRightHSV = 0;
                whitePixelsFarRightHSV = 0;
                whitePixelsFarLeftHSV2 = 0;
                whitePixelsMidLeftHSV2 = 0;
                whitePixelsMidHSV2 = 0;
                whitePixelsMidRightHSV2 = 0;
                whitePixelsFarRightHSV2 = 0;


                //Create a 60 fps frame rate
                _ = Task.Delay(16);


                //Display the current frame
                VideoPictureBox.Image = mOriginalImage.ToBitmap();

                //Modifications to change the second frame
                CvInvoke.Resize(GrayFrame, GrayFrame, newSize);
                CvInvoke.CvtColor(GrayFrame, GrayFrame, ColorConversion.Bgr2Gray);
                CvInvoke.Threshold(GrayFrame, GrayFrame, minGrayValue, maxGrayValue, Emgu.CV.CvEnum.ThresholdType.Binary);
                GrayPictureBox.Image = GrayFrame.ToBitmap();

                //Far Left Pixels - Binary
                int whitePixelsFarLeft = 0;
                Image<Gray, byte> img = GrayFrame.ToImage<Gray, byte>();
                for (int x = 0; x < mOriginalImage.Width / 5; x++)
                {
                    for (int y = 0; mOriginalImage.Height > y; y++)
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
                for (int x = mOriginalImage.Width / 5; x < 2 * (mOriginalImage.Width / 5); x++)
                {
                    for (int y = 0; mOriginalImage.Height > y; y++)
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
                for (int x = 2 * (mOriginalImage.Width / 5); x < 3 * (mOriginalImage.Width / 5); x++)
                {
                    for (int y = 0; mOriginalImage.Height > y; y++)
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
                for (int x = 3 * (mOriginalImage.Width / 5); x < 4 * (mOriginalImage.Width / 5); x++)
                {
                    for (int y = 0; mOriginalImage.Height > y; y++)
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
                for (int x = 5 * (mOriginalImage.Width / 5); x < mOriginalImage.Width; x++)
                {
                    for (int y = 0; mOriginalImage.Height > y; y++)
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
                CvInvoke.CvtColor(mOriginalImage, HSVFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

                Mat[] hsvChannels = HSVFrame.Split();

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
                //int whitePixelsFarLeftHSV = 0;
                Image<Gray, byte> imgHSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 0; x < HSVFrame.Width / 5; x++)
                {
                    for (int y = 0; HSVFrame.Height > y; y++)
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
                //int whitePixelsMidLeftHSV = 0;
                Image<Gray, byte> img2HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = HSVFrame.Width / 5; x < 2 * (HSVFrame.Width / 5); x++)
                {
                    for (int y = 0; HSVFrame.Height > y; y++)
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
                //int whitePixelsMidHSV = 0;
                Image<Gray, byte> img3HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 2 * (HSVFrame.Width / 5); x < 3 * (HSVFrame.Width / 5); x++)
                {
                    for (int y = 0; HSVFrame.Height > y; y++)
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
                //int whitePixelsMidRightHSV = 0;
                Image<Gray, byte> img4HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 3 * (HSVFrame.Width / 5); x < 4 * (HSVFrame.Width / 5); x++)
                {
                    for (int y = 0; HSVFrame.Height > y; y++)
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
                //int whitePixelsFarRightHSV = 0;
                Image<Gray, byte> img5HSV = mergedImage.ToImage<Gray, byte>();
                for (int x = 4 * (HSVFrame.Width / 5); x < 5*(HSVFrame.Width/5); x++)
                {
                    for (int y = 0; HSVFrame.Height > y; y++)
                    {
                        if (img5HSV.Data[y, x, 0] == 255)
                            whitePixelsFarRightHSV++;
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV_FR.Text = $"{whitePixelsFarRightHSV} White Pixels";
                }));

                //HSV2 Code

                Mat hsvFrame2 = new Mat();
                CvInvoke.CvtColor(HSV2Frame, hsvFrame2, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

                Mat[] hsvChannels2 = hsvFrame2.Split();

                Mat hueFilter2 = new Mat();
                CvInvoke.InRange(hsvChannels2[0], new ScalarArray(hMin2), new ScalarArray(hMax2), hueFilter2);
                Invoke(new Action(() => { H2PictureBox.Image = hueFilter2.ToBitmap(); }));

                Mat saturationFilter2 = new Mat();
                CvInvoke.InRange(hsvChannels2[1], new ScalarArray(sMin2), new ScalarArray(sMax2), saturationFilter2);
                Invoke(new Action(() => { S2PictureBox.Image = saturationFilter2.ToBitmap(); }));

                Mat valueFilter2 = new Mat();
                CvInvoke.InRange(hsvChannels2[2], new ScalarArray(vMin2), new ScalarArray(vMax2), valueFilter2);
                Invoke(new Action(() => { V2PictureBox.Image = valueFilter2.ToBitmap(); }));

                Mat mergedImage2 = new();
                CvInvoke.BitwiseAnd(hueFilter2, saturationFilter2, mergedImage2);
                CvInvoke.BitwiseAnd(mergedImage2, valueFilter2, mergedImage2);
                CvInvoke.Resize(mergedImage2, mergedImage2, newSize);
                Invoke(new Action(() => { HSV2PictureBox.Image = mergedImage2.ToBitmap(); }));

                //HSV2 White Pixel Counting

                //Far Left Pixels - HSV2
                //int whitePixelsFarLeftHSV2 = 0;
                Image<Gray, byte> imgHSV2 = mergedImage2.ToImage<Gray, byte>();
                for (int x = 0; x < HSV2Frame.Width / 5; x++)
                {
                    for (int y = 0; HSV2Frame.Height > y; y++)
                    {
                        if (imgHSV2.Data[y, x, 0] == 255)
                            whitePixelsFarLeftHSV2++;
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV2_FL.Text = $"{whitePixelsFarLeftHSV2} White Pixels";
                }));


                //Mid Left Pixels - HSV
                //int whitePixelsMidLeftHSV2 = 0;
                Image<Gray, byte> img2HSV2 = mergedImage2.ToImage<Gray, byte>();
                for (int x = HSV2Frame.Width / 5; x < 2 * (HSV2Frame.Width / 5); x++)
                {
                    for (int y = 0; HSV2Frame.Height > y; y++)
                    {
                        if (img2HSV2.Data[y, x, 0] == 255)
                            whitePixelsMidLeftHSV2++;
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV2_ML.Text = $"{whitePixelsMidLeftHSV2} White Pixels";
                }));


                //Middle Pixels  - HSV
                //int whitePixelsMidHSV2 = 0;
                Image<Gray, byte> img3HSV2 = mergedImage2.ToImage<Gray, byte>();
                for (int x = 2 * (HSV2Frame.Width / 5); x < 3 * (HSV2Frame.Width / 5); x++)
                {
                    for (int y = 0; HSV2Frame.Height > y; y++)
                    {
                        if (img3HSV2.Data[y, x, 0] == 255)
                        { whitePixelsMidHSV2++; }
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV2_M.Text = $"{whitePixelsMidHSV2} White Pixels";
                }));


                //Mid Right Pixels  - HSV
                //int whitePixelsMidRightHSV2 = 0;
                Image<Gray, byte> img4HSV2 = mergedImage2.ToImage<Gray, byte>();
                for (int x = 3 * (HSV2Frame.Width / 5); x < 4 * (HSV2Frame.Width / 5); x++)
                {
                    for (int y = 0; HSV2Frame.Height > y; y++)
                    {
                        if (img4HSV2.Data[y, x, 0] == 255)
                        { whitePixelsMidRightHSV2++; }
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV2_MR.Text = $"{whitePixelsMidRightHSV2} White Pixels";
                }));


                //Far Right Pixels  - HSV
                //int whitePixelsFarRightHSV2 = 0;
                Image<Gray, byte> img5HSV2 = mergedImage2.ToImage<Gray, byte>();
                for (int x = 4 * (HSV2Frame.Width / 5); x < 5 * (HSV2Frame.Width/5); x++)
                {
                    for (int y = 0; HSV2Frame.Height > y; y++)
                    {
                        if (img5HSV2.Data[y, x, 0] == 255)
                            whitePixelsFarRightHSV2++;
                    }
                }

                Invoke(new Action(() =>
                {
                    HSV2_FR.Text = $"{whitePixelsFarRightHSV2} White Pixels";
                }));

                //Displays serial information
                Invoke(new Action(() =>
                {
                    int serialTransmission = SerialFunction();
                    Serial.Text = $"SValue: {serialTransmission}";
                    SendSerial(serialTransmission);
                }));

                redPixels = whitePixelsFarLeftHSV2 + whitePixelsMidLeftHSV2 + whitePixelsMidHSV2
                                + whitePixelsMidRightHSV2 + whitePixelsFarRightHSV2;

                Invoke(new Action(() =>
                {
                    RPC.Text = $"{redPixels} White Pixels";
                }));

            } //while loop closer
        } //private void closer

        //Function tells what value to send over the serial monitor
        int SerialFunction()
        {
            //Mid Left
            if (      whitePixelsMidLeftHSV > whitePixelsFarLeftHSV && whitePixelsMidLeftHSV > whitePixelsMidHSV &&
                     whitePixelsMidLeftHSV > whitePixelsFarRightHSV && whitePixelsMidLeftHSV > whitePixelsMidRightHSV)
                serialTransmission = 2;
            //Mid Right
            else if (whitePixelsMidRightHSV > whitePixelsFarLeftHSV  && whitePixelsMidRightHSV > whitePixelsMidLeftHSV &&
                     whitePixelsMidRightHSV > whitePixelsFarRightHSV && whitePixelsMidRightHSV > whitePixelsMidHSV)
                serialTransmission = 4;

            //Far Left
            else if (whitePixelsFarLeftHSV > whitePixelsMidLeftHSV  && whitePixelsFarLeftHSV > whitePixelsMidHSV &&
                     whitePixelsFarLeftHSV > whitePixelsFarRightHSV && whitePixelsFarLeftHSV > whitePixelsMidRightHSV)
                serialTransmission = 1;
            //Far Right
            else if (whitePixelsFarRightHSV > whitePixelsFarLeftHSV && whitePixelsFarRightHSV > whitePixelsMidLeftHSV &&
                     whitePixelsFarRightHSV > whitePixelsMidHSV     && whitePixelsFarRightHSV > whitePixelsMidRightHSV)
                serialTransmission = 5;

            //Red
            else if (redPixels > 8000)
                serialTransmission = 6;

            //Middle
            else
                serialTransmission = 3;

            return serialTransmission;
        }

        private void SendSerial(int data)
        {
            byte[] buffer = new byte[1]
            {
                Convert.ToByte(data),
            };
            sPort.Write(buffer, 0, buffer.Length);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Dispose all processing threats to avoid orphanded processes
            if (mIsCapturing)
            {
                mCancellationToken.Cancel();
            }

            mCapture.Dispose();
            mCancellationToken.Dispose();

            if (sPort != null && sPort.IsOpen)
            {
                sPort.Close();
            }
        }
    } //public partial class Form1 parathensis
} //namespace parathensis