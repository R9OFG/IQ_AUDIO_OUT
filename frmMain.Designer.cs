namespace IQ_AUDIO_OUT
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            CmbOutputDevices = new ComboBox();
            label1 = new Label();
            LblFrequency = new Label();
            TbrFrequency = new TrackBar();
            BtnStart = new Button();
            BtnStop = new Button();
            LblLevel = new Label();
            TbrLevel = new TrackBar();
            CtxLevel = new ContextMenuStrip(components);
            s1ToolStripMenuItem = new ToolStripMenuItem();
            s2ToolStripMenuItem = new ToolStripMenuItem();
            s3ToolStripMenuItem = new ToolStripMenuItem();
            s4ToolStripMenuItem = new ToolStripMenuItem();
            s5ToolStripMenuItem = new ToolStripMenuItem();
            s6ToolStripMenuItem = new ToolStripMenuItem();
            s7ToolStripMenuItem = new ToolStripMenuItem();
            s8ToolStripMenuItem = new ToolStripMenuItem();
            s9ToolStripMenuItem = new ToolStripMenuItem();
            s9Plus10ToolStripMenuItem = new ToolStripMenuItem();
            s9Plus20ToolStripMenuItem = new ToolStripMenuItem();
            s9Plus30ToolStripMenuItem = new ToolStripMenuItem();
            s9Plus40ToolStripMenuItem = new ToolStripMenuItem();
            ChkSwapIQ = new CheckBox();
            TbrIQBalance = new TrackBar();
            LblIQBalance = new Label();
            LblIQPhase = new Label();
            TbrIQPhase = new TrackBar();
            ChkMuteI = new CheckBox();
            ChkMuteQ = new CheckBox();
            LblSampleRate = new Label();
            LblBitDepth = new Label();
            label2 = new Label();
            LnkR9OFG = new LinkLabel();
            LblDcI = new Label();
            TbrDcI = new TrackBar();
            LblDcQ = new Label();
            TbrDcQ = new TrackBar();
            BtnDeviceSettings = new Button();
            ChkNoise = new CheckBox();
            ChkDualTone = new CheckBox();
            LblSecondTone = new Label();
            TbrSecondTone = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)TbrFrequency).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TbrLevel).BeginInit();
            CtxLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TbrIQBalance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TbrIQPhase).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TbrDcI).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TbrDcQ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TbrSecondTone).BeginInit();
            SuspendLayout();
            // 
            // CmbOutputDevices
            // 
            CmbOutputDevices.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbOutputDevices.FormattingEnabled = true;
            CmbOutputDevices.Location = new Point(146, 9);
            CmbOutputDevices.Name = "CmbOutputDevices";
            CmbOutputDevices.Size = new Size(330, 23);
            CmbOutputDevices.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(118, 15);
            label1.TabIndex = 0;
            label1.Text = "Output audio device:";
            // 
            // LblFrequency
            // 
            LblFrequency.AutoSize = true;
            LblFrequency.Location = new Point(14, 74);
            LblFrequency.Name = "LblFrequency";
            LblFrequency.Size = new Size(109, 15);
            LblFrequency.TabIndex = 5;
            LblFrequency.Text = "Frequency: 1000 Hz";
            // 
            // TbrFrequency
            // 
            TbrFrequency.LargeChange = 1000;
            TbrFrequency.Location = new Point(138, 71);
            TbrFrequency.Maximum = 20000;
            TbrFrequency.Name = "TbrFrequency";
            TbrFrequency.Size = new Size(339, 45);
            TbrFrequency.SmallChange = 100;
            TbrFrequency.TabIndex = 6;
            TbrFrequency.TickFrequency = 1000;
            TbrFrequency.Value = 20000;
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(13, 345);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(118, 23);
            BtnStart.TabIndex = 25;
            BtnStart.Text = "Start generate IQ";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // BtnStop
            // 
            BtnStop.Location = new Point(360, 345);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(118, 23);
            BtnStop.TabIndex = 27;
            BtnStop.Text = "Stop generate IQ";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // LblLevel
            // 
            LblLevel.AutoSize = true;
            LblLevel.Location = new Point(14, 138);
            LblLevel.Name = "LblLevel";
            LblLevel.Size = new Size(63, 15);
            LblLevel.TabIndex = 10;
            LblLevel.Text = "Level: 0 dB";
            // 
            // TbrLevel
            // 
            TbrLevel.ContextMenuStrip = CtxLevel;
            TbrLevel.LargeChange = 10;
            TbrLevel.Location = new Point(138, 135);
            TbrLevel.Maximum = 0;
            TbrLevel.Minimum = -100;
            TbrLevel.Name = "TbrLevel";
            TbrLevel.Size = new Size(339, 45);
            TbrLevel.TabIndex = 11;
            TbrLevel.TickFrequency = 10;
            // 
            // CtxLevel
            // 
            CtxLevel.ImageScalingSize = new Size(20, 20);
            CtxLevel.Items.AddRange(new ToolStripItem[] { s1ToolStripMenuItem, s2ToolStripMenuItem, s3ToolStripMenuItem, s4ToolStripMenuItem, s5ToolStripMenuItem, s6ToolStripMenuItem, s7ToolStripMenuItem, s8ToolStripMenuItem, s9ToolStripMenuItem, s9Plus10ToolStripMenuItem, s9Plus20ToolStripMenuItem, s9Plus30ToolStripMenuItem, s9Plus40ToolStripMenuItem });
            CtxLevel.Name = "CtxLevel";
            CtxLevel.Size = new Size(107, 290);
            // 
            // s1ToolStripMenuItem
            // 
            s1ToolStripMenuItem.Name = "s1ToolStripMenuItem";
            s1ToolStripMenuItem.Size = new Size(106, 22);
            s1ToolStripMenuItem.Text = "S1";
            // 
            // s2ToolStripMenuItem
            // 
            s2ToolStripMenuItem.Name = "s2ToolStripMenuItem";
            s2ToolStripMenuItem.Size = new Size(106, 22);
            s2ToolStripMenuItem.Text = "S2";
            // 
            // s3ToolStripMenuItem
            // 
            s3ToolStripMenuItem.Name = "s3ToolStripMenuItem";
            s3ToolStripMenuItem.Size = new Size(106, 22);
            s3ToolStripMenuItem.Text = "S3";
            // 
            // s4ToolStripMenuItem
            // 
            s4ToolStripMenuItem.Name = "s4ToolStripMenuItem";
            s4ToolStripMenuItem.Size = new Size(106, 22);
            s4ToolStripMenuItem.Text = "S4";
            // 
            // s5ToolStripMenuItem
            // 
            s5ToolStripMenuItem.Name = "s5ToolStripMenuItem";
            s5ToolStripMenuItem.Size = new Size(106, 22);
            s5ToolStripMenuItem.Text = "S5";
            // 
            // s6ToolStripMenuItem
            // 
            s6ToolStripMenuItem.Name = "s6ToolStripMenuItem";
            s6ToolStripMenuItem.Size = new Size(106, 22);
            s6ToolStripMenuItem.Text = "S6";
            // 
            // s7ToolStripMenuItem
            // 
            s7ToolStripMenuItem.Name = "s7ToolStripMenuItem";
            s7ToolStripMenuItem.Size = new Size(106, 22);
            s7ToolStripMenuItem.Text = "S7";
            // 
            // s8ToolStripMenuItem
            // 
            s8ToolStripMenuItem.Name = "s8ToolStripMenuItem";
            s8ToolStripMenuItem.Size = new Size(106, 22);
            s8ToolStripMenuItem.Text = "S8";
            // 
            // s9ToolStripMenuItem
            // 
            s9ToolStripMenuItem.Name = "s9ToolStripMenuItem";
            s9ToolStripMenuItem.Size = new Size(106, 22);
            s9ToolStripMenuItem.Text = "S9";
            // 
            // s9Plus10ToolStripMenuItem
            // 
            s9Plus10ToolStripMenuItem.Name = "s9Plus10ToolStripMenuItem";
            s9Plus10ToolStripMenuItem.Size = new Size(106, 22);
            s9Plus10ToolStripMenuItem.Text = "S9+10";
            // 
            // s9Plus20ToolStripMenuItem
            // 
            s9Plus20ToolStripMenuItem.Name = "s9Plus20ToolStripMenuItem";
            s9Plus20ToolStripMenuItem.Size = new Size(106, 22);
            s9Plus20ToolStripMenuItem.Text = "S9+20";
            // 
            // s9Plus30ToolStripMenuItem
            // 
            s9Plus30ToolStripMenuItem.Name = "s9Plus30ToolStripMenuItem";
            s9Plus30ToolStripMenuItem.Size = new Size(106, 22);
            s9Plus30ToolStripMenuItem.Text = "S9+30";
            // 
            // s9Plus40ToolStripMenuItem
            // 
            s9Plus40ToolStripMenuItem.Name = "s9Plus40ToolStripMenuItem";
            s9Plus40ToolStripMenuItem.Size = new Size(106, 22);
            s9Plus40ToolStripMenuItem.Text = "S9+40";
            // 
            // ChkSwapIQ
            // 
            ChkSwapIQ.AutoSize = true;
            ChkSwapIQ.Location = new Point(18, 308);
            ChkSwapIQ.Name = "ChkSwapIQ";
            ChkSwapIQ.Size = new Size(74, 19);
            ChkSwapIQ.TabIndex = 20;
            ChkSwapIQ.Text = "Swap I/Q";
            ChkSwapIQ.UseVisualStyleBackColor = true;
            // 
            // TbrIQBalance
            // 
            TbrIQBalance.ContextMenuStrip = CtxLevel;
            TbrIQBalance.LargeChange = 10;
            TbrIQBalance.Location = new Point(138, 168);
            TbrIQBalance.Maximum = 0;
            TbrIQBalance.Minimum = -100;
            TbrIQBalance.Name = "TbrIQBalance";
            TbrIQBalance.Size = new Size(339, 45);
            TbrIQBalance.TabIndex = 13;
            TbrIQBalance.TickFrequency = 10;
            // 
            // LblIQBalance
            // 
            LblIQBalance.AutoSize = true;
            LblIQBalance.Location = new Point(15, 169);
            LblIQBalance.Name = "LblIQBalance";
            LblIQBalance.Size = new Size(84, 15);
            LblIQBalance.TabIndex = 12;
            LblIQBalance.Text = "IQ Balance: 1.0";
            // 
            // LblIQPhase
            // 
            LblIQPhase.AutoSize = true;
            LblIQPhase.Location = new Point(15, 201);
            LblIQPhase.Name = "LblIQPhase";
            LblIQPhase.Size = new Size(79, 15);
            LblIQPhase.TabIndex = 14;
            LblIQPhase.Text = "IQ Phase: 0.0°";
            // 
            // TbrIQPhase
            // 
            TbrIQPhase.ContextMenuStrip = CtxLevel;
            TbrIQPhase.LargeChange = 10;
            TbrIQPhase.Location = new Point(138, 199);
            TbrIQPhase.Maximum = 0;
            TbrIQPhase.Minimum = -100;
            TbrIQPhase.Name = "TbrIQPhase";
            TbrIQPhase.Size = new Size(339, 45);
            TbrIQPhase.TabIndex = 15;
            TbrIQPhase.TickFrequency = 10;
            // 
            // ChkMuteI
            // 
            ChkMuteI.AutoSize = true;
            ChkMuteI.Location = new Point(323, 308);
            ChkMuteI.Name = "ChkMuteI";
            ChkMuteI.Size = new Size(62, 19);
            ChkMuteI.TabIndex = 22;
            ChkMuteI.Text = "Mute-I";
            ChkMuteI.UseVisualStyleBackColor = true;
            // 
            // ChkMuteQ
            // 
            ChkMuteQ.AutoSize = true;
            ChkMuteQ.Location = new Point(406, 308);
            ChkMuteQ.Name = "ChkMuteQ";
            ChkMuteQ.Size = new Size(68, 19);
            ChkMuteQ.TabIndex = 23;
            ChkMuteQ.Text = "Mute-Q";
            ChkMuteQ.UseVisualStyleBackColor = true;
            // 
            // LblSampleRate
            // 
            LblSampleRate.AutoSize = true;
            LblSampleRate.Location = new Point(12, 44);
            LblSampleRate.Name = "LblSampleRate";
            LblSampleRate.Size = new Size(75, 15);
            LblSampleRate.TabIndex = 2;
            LblSampleRate.Text = "Sample Rate:";
            // 
            // LblBitDepth
            // 
            LblBitDepth.AutoSize = true;
            LblBitDepth.Location = new Point(149, 44);
            LblBitDepth.Name = "LblBitDepth";
            LblBitDepth.Size = new Size(84, 15);
            LblBitDepth.TabIndex = 3;
            LblBitDepth.Text = "Device setting:";
            // 
            // label2
            // 
            label2.BackColor = SystemColors.ControlDark;
            label2.Location = new Point(14, 331);
            label2.Name = "label2";
            label2.Size = new Size(463, 2);
            label2.TabIndex = 24;
            // 
            // LnkR9OFG
            // 
            LnkR9OFG.AutoSize = true;
            LnkR9OFG.Location = new Point(182, 349);
            LnkR9OFG.Name = "LnkR9OFG";
            LnkR9OFG.Size = new Size(116, 15);
            LnkR9OFG.TabIndex = 26;
            LnkR9OFG.TabStop = true;
            LnkR9OFG.Text = "By R9OFG 30.01.2026";
            // 
            // LblDcI
            // 
            LblDcI.AutoSize = true;
            LblDcI.Location = new Point(14, 234);
            LblDcI.Name = "LblDcI";
            LblDcI.Size = new Size(50, 15);
            LblDcI.TabIndex = 16;
            LblDcI.Text = "DC I: 0.0";
            // 
            // TbrDcI
            // 
            TbrDcI.ContextMenuStrip = CtxLevel;
            TbrDcI.LargeChange = 10;
            TbrDcI.Location = new Point(137, 230);
            TbrDcI.Maximum = 0;
            TbrDcI.Minimum = -100;
            TbrDcI.Name = "TbrDcI";
            TbrDcI.Size = new Size(339, 45);
            TbrDcI.TabIndex = 17;
            TbrDcI.TickFrequency = 10;
            // 
            // LblDcQ
            // 
            LblDcQ.AutoSize = true;
            LblDcQ.Location = new Point(14, 266);
            LblDcQ.Name = "LblDcQ";
            LblDcQ.Size = new Size(56, 15);
            LblDcQ.TabIndex = 18;
            LblDcQ.Text = "DC Q: 0.0";
            // 
            // TbrDcQ
            // 
            TbrDcQ.ContextMenuStrip = CtxLevel;
            TbrDcQ.LargeChange = 10;
            TbrDcQ.Location = new Point(137, 262);
            TbrDcQ.Maximum = 0;
            TbrDcQ.Minimum = -100;
            TbrDcQ.Name = "TbrDcQ";
            TbrDcQ.Size = new Size(339, 45);
            TbrDcQ.TabIndex = 19;
            TbrDcQ.TickFrequency = 10;
            // 
            // BtnDeviceSettings
            // 
            BtnDeviceSettings.Location = new Point(358, 36);
            BtnDeviceSettings.Name = "BtnDeviceSettings";
            BtnDeviceSettings.Size = new Size(118, 23);
            BtnDeviceSettings.TabIndex = 4;
            BtnDeviceSettings.Text = "Device setting";
            BtnDeviceSettings.UseVisualStyleBackColor = true;
            // 
            // ChkNoise
            // 
            ChkNoise.AutoSize = true;
            ChkNoise.Location = new Point(114, 308);
            ChkNoise.Name = "ChkNoise";
            ChkNoise.Size = new Size(90, 19);
            ChkNoise.TabIndex = 21;
            ChkNoise.Text = "Noise Mode";
            ChkNoise.UseVisualStyleBackColor = true;
            // 
            // ChkDualTone
            // 
            ChkDualTone.AutoSize = true;
            ChkDualTone.Location = new Point(18, 107);
            ChkDualTone.Name = "ChkDualTone";
            ChkDualTone.Size = new Size(78, 19);
            ChkDualTone.TabIndex = 7;
            ChkDualTone.Text = "Dual Tone";
            ChkDualTone.UseVisualStyleBackColor = true;
            // 
            // LblSecondTone
            // 
            LblSecondTone.AutoSize = true;
            LblSecondTone.Location = new Point(101, 108);
            LblSecondTone.Name = "LblSecondTone";
            LblSecondTone.Size = new Size(102, 15);
            LblSecondTone.TabIndex = 8;
            LblSecondTone.Text = "2nd Tone: 1500 Hz";
            // 
            // TbrSecondTone
            // 
            TbrSecondTone.LargeChange = 1000;
            TbrSecondTone.Location = new Point(209, 103);
            TbrSecondTone.Maximum = 20000;
            TbrSecondTone.Name = "TbrSecondTone";
            TbrSecondTone.Size = new Size(268, 45);
            TbrSecondTone.SmallChange = 100;
            TbrSecondTone.TabIndex = 9;
            TbrSecondTone.TickFrequency = 1000;
            TbrSecondTone.Value = 20000;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(490, 377);
            Controls.Add(TbrDcQ);
            Controls.Add(TbrDcI);
            Controls.Add(TbrIQPhase);
            Controls.Add(TbrIQBalance);
            Controls.Add(TbrLevel);
            Controls.Add(TbrSecondTone);
            Controls.Add(LblSecondTone);
            Controls.Add(ChkDualTone);
            Controls.Add(ChkNoise);
            Controls.Add(BtnDeviceSettings);
            Controls.Add(LblDcQ);
            Controls.Add(LblDcI);
            Controls.Add(LnkR9OFG);
            Controls.Add(label2);
            Controls.Add(LblBitDepth);
            Controls.Add(LblSampleRate);
            Controls.Add(ChkMuteQ);
            Controls.Add(ChkMuteI);
            Controls.Add(LblIQPhase);
            Controls.Add(LblIQBalance);
            Controls.Add(ChkSwapIQ);
            Controls.Add(LblLevel);
            Controls.Add(BtnStop);
            Controls.Add(BtnStart);
            Controls.Add(TbrFrequency);
            Controls.Add(LblFrequency);
            Controls.Add(label1);
            Controls.Add(CmbOutputDevices);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "I/Q Tone Generator for SDR Software Testing";
            ((System.ComponentModel.ISupportInitialize)TbrFrequency).EndInit();
            ((System.ComponentModel.ISupportInitialize)TbrLevel).EndInit();
            CtxLevel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)TbrIQBalance).EndInit();
            ((System.ComponentModel.ISupportInitialize)TbrIQPhase).EndInit();
            ((System.ComponentModel.ISupportInitialize)TbrDcI).EndInit();
            ((System.ComponentModel.ISupportInitialize)TbrDcQ).EndInit();
            ((System.ComponentModel.ISupportInitialize)TbrSecondTone).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox CmbOutputDevices;
        private Label label1;
        private Label LblFrequency;
        private TrackBar TbrFrequency;
        private Button BtnStart;
        private Button BtnStop;
        private Label LblLevel;
        private TrackBar TbrLevel;
        private CheckBox ChkSwapIQ;
        private ContextMenuStrip CtxLevel;
        private ToolStripMenuItem s1ToolStripMenuItem;
        private ToolStripMenuItem s2ToolStripMenuItem;
        private ToolStripMenuItem s3ToolStripMenuItem;
        private ToolStripMenuItem s4ToolStripMenuItem;
        private ToolStripMenuItem s5ToolStripMenuItem;
        private ToolStripMenuItem s6ToolStripMenuItem;
        private ToolStripMenuItem s7ToolStripMenuItem;
        private ToolStripMenuItem s8ToolStripMenuItem;
        private ToolStripMenuItem s9ToolStripMenuItem;
        private ToolStripMenuItem s9Plus10ToolStripMenuItem;
        private ToolStripMenuItem s9Plus20ToolStripMenuItem;
        private ToolStripMenuItem s9Plus30ToolStripMenuItem;
        private ToolStripMenuItem s9Plus40ToolStripMenuItem;
        private TrackBar TbrIQBalance;
        private Label LblIQBalance;
        private Label LblIQPhase;
        private TrackBar TbrIQPhase;
        private CheckBox ChkMuteI;
        private CheckBox ChkMuteQ;
        private Label LblSampleRate;
        private Label LblBitDepth;
        private Label label2;
        private LinkLabel LnkR9OFG;
        private Label LblDcI;
        private TrackBar TbrDcI;
        private Label LblDcQ;
        private TrackBar TbrDcQ;
        private Button BtnDeviceSettings;
        private CheckBox ChkNoise;
        private CheckBox ChkDualTone;
        private Label LblSecondTone;
        private TrackBar TbrSecondTone;
    }
}
