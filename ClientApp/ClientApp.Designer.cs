namespace ClientApp
{
    partial class ClientApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientApp));
            this.checkBoxProcessor = new System.Windows.Forms.CheckBox();
            this.trackBarProcessorLevel1 = new System.Windows.Forms.TrackBar();
            this.textBoxProcessorLevel1 = new System.Windows.Forms.TextBox();
            this.textBoxProcessorLevel2 = new System.Windows.Forms.TextBox();
            this.trackBarProcessorLevel2 = new System.Windows.Forms.TrackBar();
            this.textBoxProcessorLevel3 = new System.Windows.Forms.TextBox();
            this.trackBarProcessorLevel3 = new System.Windows.Forms.TrackBar();
            this.checkBoxMemory = new System.Windows.Forms.CheckBox();
            this.trackBarMemoryLevel1 = new System.Windows.Forms.TrackBar();
            this.textBoxMemoryLevel1 = new System.Windows.Forms.TextBox();
            this.trackBarMemoryLevel2 = new System.Windows.Forms.TrackBar();
            this.textBoxMemoryLevel2 = new System.Windows.Forms.TextBox();
            this.trackBarMemoryLevel3 = new System.Windows.Forms.TrackBar();
            this.textBoxMemoryLevel3 = new System.Windows.Forms.TextBox();
            this.checkBoxDisk = new System.Windows.Forms.CheckBox();
            this.trackBarDiskLevel1 = new System.Windows.Forms.TrackBar();
            this.textBoxDiskLevel1 = new System.Windows.Forms.TextBox();
            this.trackBarDiskLevel2 = new System.Windows.Forms.TrackBar();
            this.textBoxDiskLevel2 = new System.Windows.Forms.TextBox();
            this.trackBarDiskLevel3 = new System.Windows.Forms.TrackBar();
            this.textBoxDiskLevel3 = new System.Windows.Forms.TextBox();
            this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPathData = new System.Windows.Forms.TextBox();
            this.buttonPathData = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPathSummary = new System.Windows.Forms.TextBox();
            this.buttonPathSummary = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonModify = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProcessorLevel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProcessorLevel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProcessorLevel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemoryLevel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemoryLevel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemoryLevel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDiskLevel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDiskLevel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDiskLevel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxProcessor
            // 
            this.checkBoxProcessor.AutoSize = true;
            this.checkBoxProcessor.Location = new System.Drawing.Point(12, 68);
            this.checkBoxProcessor.Name = "checkBoxProcessor";
            this.checkBoxProcessor.Size = new System.Drawing.Size(167, 17);
            this.checkBoxProcessor.TabIndex = 0;
            this.checkBoxProcessor.Text = "Procesor - Czas procesora (%)";
            this.checkBoxProcessor.UseVisualStyleBackColor = true;
            this.checkBoxProcessor.CheckedChanged += new System.EventHandler(this.checkBoxProcessor_CheckedChanged);
            // 
            // trackBarProcessorLevel1
            // 
            this.trackBarProcessorLevel1.Enabled = false;
            this.trackBarProcessorLevel1.Location = new System.Drawing.Point(242, 68);
            this.trackBarProcessorLevel1.Maximum = 98;
            this.trackBarProcessorLevel1.Minimum = 1;
            this.trackBarProcessorLevel1.Name = "trackBarProcessorLevel1";
            this.trackBarProcessorLevel1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarProcessorLevel1.Size = new System.Drawing.Size(45, 104);
            this.trackBarProcessorLevel1.TabIndex = 1;
            this.trackBarProcessorLevel1.Value = 1;
            this.trackBarProcessorLevel1.ValueChanged += new System.EventHandler(this.trackBarProcessorLevel1_ValueChanged);
            // 
            // textBoxProcessorLevel1
            // 
            this.textBoxProcessorLevel1.Location = new System.Drawing.Point(242, 178);
            this.textBoxProcessorLevel1.Name = "textBoxProcessorLevel1";
            this.textBoxProcessorLevel1.ReadOnly = true;
            this.textBoxProcessorLevel1.Size = new System.Drawing.Size(45, 20);
            this.textBoxProcessorLevel1.TabIndex = 2;
            this.textBoxProcessorLevel1.Text = "1";
            // 
            // textBoxProcessorLevel2
            // 
            this.textBoxProcessorLevel2.Location = new System.Drawing.Point(293, 178);
            this.textBoxProcessorLevel2.Name = "textBoxProcessorLevel2";
            this.textBoxProcessorLevel2.ReadOnly = true;
            this.textBoxProcessorLevel2.Size = new System.Drawing.Size(45, 20);
            this.textBoxProcessorLevel2.TabIndex = 4;
            this.textBoxProcessorLevel2.Text = "2";
            // 
            // trackBarProcessorLevel2
            // 
            this.trackBarProcessorLevel2.Enabled = false;
            this.trackBarProcessorLevel2.Location = new System.Drawing.Point(293, 68);
            this.trackBarProcessorLevel2.Maximum = 99;
            this.trackBarProcessorLevel2.Minimum = 2;
            this.trackBarProcessorLevel2.Name = "trackBarProcessorLevel2";
            this.trackBarProcessorLevel2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarProcessorLevel2.Size = new System.Drawing.Size(45, 104);
            this.trackBarProcessorLevel2.TabIndex = 3;
            this.trackBarProcessorLevel2.Value = 2;
            this.trackBarProcessorLevel2.ValueChanged += new System.EventHandler(this.trackBarProcessorLevel2_ValueChanged);
            // 
            // textBoxProcessorLevel3
            // 
            this.textBoxProcessorLevel3.Location = new System.Drawing.Point(344, 178);
            this.textBoxProcessorLevel3.Name = "textBoxProcessorLevel3";
            this.textBoxProcessorLevel3.ReadOnly = true;
            this.textBoxProcessorLevel3.Size = new System.Drawing.Size(45, 20);
            this.textBoxProcessorLevel3.TabIndex = 6;
            this.textBoxProcessorLevel3.Text = "100";
            // 
            // trackBarProcessorLevel3
            // 
            this.trackBarProcessorLevel3.Enabled = false;
            this.trackBarProcessorLevel3.Location = new System.Drawing.Point(344, 68);
            this.trackBarProcessorLevel3.Maximum = 100;
            this.trackBarProcessorLevel3.Minimum = 1;
            this.trackBarProcessorLevel3.Name = "trackBarProcessorLevel3";
            this.trackBarProcessorLevel3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarProcessorLevel3.Size = new System.Drawing.Size(45, 104);
            this.trackBarProcessorLevel3.TabIndex = 5;
            this.trackBarProcessorLevel3.Value = 100;
            // 
            // checkBoxMemory
            // 
            this.checkBoxMemory.AutoSize = true;
            this.checkBoxMemory.Location = new System.Drawing.Point(12, 221);
            this.checkBoxMemory.Name = "checkBoxMemory";
            this.checkBoxMemory.Size = new System.Drawing.Size(230, 17);
            this.checkBoxMemory.TabIndex = 0;
            this.checkBoxMemory.Text = "Pamięć - Zadeklarowane bajty w użyciu (%)";
            this.checkBoxMemory.UseVisualStyleBackColor = true;
            this.checkBoxMemory.CheckedChanged += new System.EventHandler(this.checkBoxMemory_CheckedChanged);
            // 
            // trackBarMemoryLevel1
            // 
            this.trackBarMemoryLevel1.Enabled = false;
            this.trackBarMemoryLevel1.Location = new System.Drawing.Point(242, 221);
            this.trackBarMemoryLevel1.Maximum = 98;
            this.trackBarMemoryLevel1.Minimum = 1;
            this.trackBarMemoryLevel1.Name = "trackBarMemoryLevel1";
            this.trackBarMemoryLevel1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMemoryLevel1.Size = new System.Drawing.Size(45, 104);
            this.trackBarMemoryLevel1.TabIndex = 1;
            this.trackBarMemoryLevel1.Value = 1;
            this.trackBarMemoryLevel1.ValueChanged += new System.EventHandler(this.trackBarMemoryLevel1_ValueChanged);
            // 
            // textBoxMemoryLevel1
            // 
            this.textBoxMemoryLevel1.Location = new System.Drawing.Point(242, 331);
            this.textBoxMemoryLevel1.Name = "textBoxMemoryLevel1";
            this.textBoxMemoryLevel1.ReadOnly = true;
            this.textBoxMemoryLevel1.Size = new System.Drawing.Size(45, 20);
            this.textBoxMemoryLevel1.TabIndex = 2;
            this.textBoxMemoryLevel1.Text = "1";
            // 
            // trackBarMemoryLevel2
            // 
            this.trackBarMemoryLevel2.Enabled = false;
            this.trackBarMemoryLevel2.Location = new System.Drawing.Point(293, 221);
            this.trackBarMemoryLevel2.Maximum = 99;
            this.trackBarMemoryLevel2.Minimum = 2;
            this.trackBarMemoryLevel2.Name = "trackBarMemoryLevel2";
            this.trackBarMemoryLevel2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMemoryLevel2.Size = new System.Drawing.Size(45, 104);
            this.trackBarMemoryLevel2.TabIndex = 3;
            this.trackBarMemoryLevel2.Value = 2;
            this.trackBarMemoryLevel2.ValueChanged += new System.EventHandler(this.trackBarMemoryLevel2_ValueChanged);
            // 
            // textBoxMemoryLevel2
            // 
            this.textBoxMemoryLevel2.Location = new System.Drawing.Point(293, 331);
            this.textBoxMemoryLevel2.Name = "textBoxMemoryLevel2";
            this.textBoxMemoryLevel2.ReadOnly = true;
            this.textBoxMemoryLevel2.Size = new System.Drawing.Size(45, 20);
            this.textBoxMemoryLevel2.TabIndex = 4;
            this.textBoxMemoryLevel2.Text = "2";
            // 
            // trackBarMemoryLevel3
            // 
            this.trackBarMemoryLevel3.Enabled = false;
            this.trackBarMemoryLevel3.Location = new System.Drawing.Point(344, 221);
            this.trackBarMemoryLevel3.Maximum = 100;
            this.trackBarMemoryLevel3.Minimum = 1;
            this.trackBarMemoryLevel3.Name = "trackBarMemoryLevel3";
            this.trackBarMemoryLevel3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarMemoryLevel3.Size = new System.Drawing.Size(45, 104);
            this.trackBarMemoryLevel3.TabIndex = 5;
            this.trackBarMemoryLevel3.Value = 100;
            // 
            // textBoxMemoryLevel3
            // 
            this.textBoxMemoryLevel3.Location = new System.Drawing.Point(344, 331);
            this.textBoxMemoryLevel3.Name = "textBoxMemoryLevel3";
            this.textBoxMemoryLevel3.ReadOnly = true;
            this.textBoxMemoryLevel3.Size = new System.Drawing.Size(45, 20);
            this.textBoxMemoryLevel3.TabIndex = 6;
            this.textBoxMemoryLevel3.Text = "100";
            // 
            // checkBoxDisk
            // 
            this.checkBoxDisk.AutoSize = true;
            this.checkBoxDisk.Location = new System.Drawing.Point(12, 380);
            this.checkBoxDisk.Name = "checkBoxDisk";
            this.checkBoxDisk.Size = new System.Drawing.Size(206, 17);
            this.checkBoxDisk.TabIndex = 0;
            this.checkBoxDisk.Text = "Dysk fizyczny - Czas bezczynności (%)";
            this.checkBoxDisk.UseVisualStyleBackColor = true;
            this.checkBoxDisk.CheckedChanged += new System.EventHandler(this.checkBoxDisk_CheckedChanged);
            // 
            // trackBarDiskLevel1
            // 
            this.trackBarDiskLevel1.Enabled = false;
            this.trackBarDiskLevel1.Location = new System.Drawing.Point(242, 380);
            this.trackBarDiskLevel1.Maximum = 98;
            this.trackBarDiskLevel1.Minimum = 1;
            this.trackBarDiskLevel1.Name = "trackBarDiskLevel1";
            this.trackBarDiskLevel1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarDiskLevel1.Size = new System.Drawing.Size(45, 104);
            this.trackBarDiskLevel1.TabIndex = 1;
            this.trackBarDiskLevel1.Value = 1;
            this.trackBarDiskLevel1.ValueChanged += new System.EventHandler(this.trackBarDiskLevel1_ValueChanged);
            // 
            // textBoxDiskLevel1
            // 
            this.textBoxDiskLevel1.Location = new System.Drawing.Point(242, 490);
            this.textBoxDiskLevel1.Name = "textBoxDiskLevel1";
            this.textBoxDiskLevel1.ReadOnly = true;
            this.textBoxDiskLevel1.Size = new System.Drawing.Size(45, 20);
            this.textBoxDiskLevel1.TabIndex = 2;
            this.textBoxDiskLevel1.Text = "1";
            // 
            // trackBarDiskLevel2
            // 
            this.trackBarDiskLevel2.Enabled = false;
            this.trackBarDiskLevel2.Location = new System.Drawing.Point(293, 380);
            this.trackBarDiskLevel2.Maximum = 99;
            this.trackBarDiskLevel2.Minimum = 2;
            this.trackBarDiskLevel2.Name = "trackBarDiskLevel2";
            this.trackBarDiskLevel2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarDiskLevel2.Size = new System.Drawing.Size(45, 104);
            this.trackBarDiskLevel2.TabIndex = 3;
            this.trackBarDiskLevel2.Value = 2;
            this.trackBarDiskLevel2.ValueChanged += new System.EventHandler(this.trackBarDiskLevel2_ValueChanged);
            // 
            // textBoxDiskLevel2
            // 
            this.textBoxDiskLevel2.Location = new System.Drawing.Point(293, 490);
            this.textBoxDiskLevel2.Name = "textBoxDiskLevel2";
            this.textBoxDiskLevel2.ReadOnly = true;
            this.textBoxDiskLevel2.Size = new System.Drawing.Size(45, 20);
            this.textBoxDiskLevel2.TabIndex = 4;
            this.textBoxDiskLevel2.Text = "2";
            // 
            // trackBarDiskLevel3
            // 
            this.trackBarDiskLevel3.Enabled = false;
            this.trackBarDiskLevel3.Location = new System.Drawing.Point(344, 380);
            this.trackBarDiskLevel3.Maximum = 100;
            this.trackBarDiskLevel3.Minimum = 1;
            this.trackBarDiskLevel3.Name = "trackBarDiskLevel3";
            this.trackBarDiskLevel3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarDiskLevel3.Size = new System.Drawing.Size(45, 104);
            this.trackBarDiskLevel3.TabIndex = 5;
            this.trackBarDiskLevel3.Value = 100;
            // 
            // textBoxDiskLevel3
            // 
            this.textBoxDiskLevel3.Location = new System.Drawing.Point(344, 490);
            this.textBoxDiskLevel3.Name = "textBoxDiskLevel3";
            this.textBoxDiskLevel3.ReadOnly = true;
            this.textBoxDiskLevel3.Size = new System.Drawing.Size(45, 20);
            this.textBoxDiskLevel3.TabIndex = 6;
            this.textBoxDiskLevel3.Text = "100";
            // 
            // numericUpDownInterval
            // 
            this.numericUpDownInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownInterval.Location = new System.Drawing.Point(474, 67);
            this.numericUpDownInterval.Maximum = new decimal(new int[] {
            86400000,
            0,
            0,
            0});
            this.numericUpDownInterval.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownInterval.Name = "numericUpDownInterval";
            this.numericUpDownInterval.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownInterval.TabIndex = 7;
            this.numericUpDownInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(290, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Stany";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Niski";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(290, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Średni";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(341, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Wysoki";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(468, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Interwał (w milisekundach)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(468, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Lokalizacja statystyk dziennych";
            // 
            // textBoxPathData
            // 
            this.textBoxPathData.Location = new System.Drawing.Point(471, 151);
            this.textBoxPathData.Name = "textBoxPathData";
            this.textBoxPathData.ReadOnly = true;
            this.textBoxPathData.Size = new System.Drawing.Size(233, 20);
            this.textBoxPathData.TabIndex = 13;
            this.textBoxPathData.TextChanged += new System.EventHandler(this.textBoxPathData_TextChanged);
            // 
            // buttonPathData
            // 
            this.buttonPathData.Location = new System.Drawing.Point(710, 148);
            this.buttonPathData.Name = "buttonPathData";
            this.buttonPathData.Size = new System.Drawing.Size(75, 23);
            this.buttonPathData.TabIndex = 14;
            this.buttonPathData.Text = "Zmień";
            this.buttonPathData.UseVisualStyleBackColor = true;
            this.buttonPathData.Click += new System.EventHandler(this.buttonPathData_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(468, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(157, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Lokalizacja statystyk zbiorczych";
            // 
            // textBoxPathSummary
            // 
            this.textBoxPathSummary.Location = new System.Drawing.Point(471, 220);
            this.textBoxPathSummary.Name = "textBoxPathSummary";
            this.textBoxPathSummary.ReadOnly = true;
            this.textBoxPathSummary.Size = new System.Drawing.Size(233, 20);
            this.textBoxPathSummary.TabIndex = 13;
            this.textBoxPathSummary.TextChanged += new System.EventHandler(this.textBoxPathSummary_TextChanged);
            // 
            // buttonPathSummary
            // 
            this.buttonPathSummary.Location = new System.Drawing.Point(710, 217);
            this.buttonPathSummary.Name = "buttonPathSummary";
            this.buttonPathSummary.Size = new System.Drawing.Size(75, 23);
            this.buttonPathSummary.TabIndex = 14;
            this.buttonPathSummary.Text = "Zmień";
            this.buttonPathSummary.UseVisualStyleBackColor = true;
            this.buttonPathSummary.Click += new System.EventHandler(this.buttonPathSummary_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(526, 285);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Zapisz";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonModify
            // 
            this.buttonModify.Enabled = false;
            this.buttonModify.Location = new System.Drawing.Point(647, 285);
            this.buttonModify.Name = "buttonModify";
            this.buttonModify.Size = new System.Drawing.Size(75, 23);
            this.buttonModify.TabIndex = 16;
            this.buttonModify.Text = "Modyfikuj";
            this.buttonModify.UseVisualStyleBackColor = true;
            this.buttonModify.Click += new System.EventHandler(this.buttonModify_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(468, 380);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(147, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Mechanizm zbierania danych:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(622, 380);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "label9";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ClientApp";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ClientApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 527);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.buttonModify);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonPathSummary);
            this.Controls.Add(this.buttonPathData);
            this.Controls.Add(this.textBoxPathSummary);
            this.Controls.Add(this.textBoxPathData);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownInterval);
            this.Controls.Add(this.textBoxDiskLevel3);
            this.Controls.Add(this.textBoxMemoryLevel3);
            this.Controls.Add(this.textBoxProcessorLevel3);
            this.Controls.Add(this.trackBarDiskLevel3);
            this.Controls.Add(this.trackBarMemoryLevel3);
            this.Controls.Add(this.trackBarProcessorLevel3);
            this.Controls.Add(this.textBoxDiskLevel2);
            this.Controls.Add(this.textBoxMemoryLevel2);
            this.Controls.Add(this.textBoxProcessorLevel2);
            this.Controls.Add(this.trackBarDiskLevel2);
            this.Controls.Add(this.trackBarMemoryLevel2);
            this.Controls.Add(this.trackBarProcessorLevel2);
            this.Controls.Add(this.textBoxDiskLevel1);
            this.Controls.Add(this.textBoxMemoryLevel1);
            this.Controls.Add(this.textBoxProcessorLevel1);
            this.Controls.Add(this.trackBarDiskLevel1);
            this.Controls.Add(this.checkBoxDisk);
            this.Controls.Add(this.trackBarMemoryLevel1);
            this.Controls.Add(this.checkBoxMemory);
            this.Controls.Add(this.trackBarProcessorLevel1);
            this.Controls.Add(this.checkBoxProcessor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ClientApp";
            this.Text = "ClientApp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientApp_FormClosed);
            this.Shown += new System.EventHandler(this.ClientApp_Shown);
            this.Resize += new System.EventHandler(this.ClientApp_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProcessorLevel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProcessorLevel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProcessorLevel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemoryLevel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemoryLevel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMemoryLevel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDiskLevel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDiskLevel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDiskLevel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxProcessor;
        private System.Windows.Forms.TrackBar trackBarProcessorLevel1;
        private System.Windows.Forms.TextBox textBoxProcessorLevel1;
        private System.Windows.Forms.TextBox textBoxProcessorLevel2;
        private System.Windows.Forms.TrackBar trackBarProcessorLevel2;
        private System.Windows.Forms.TextBox textBoxProcessorLevel3;
        private System.Windows.Forms.TrackBar trackBarProcessorLevel3;
        private System.Windows.Forms.CheckBox checkBoxMemory;
        private System.Windows.Forms.TrackBar trackBarMemoryLevel1;
        private System.Windows.Forms.TextBox textBoxMemoryLevel1;
        private System.Windows.Forms.TrackBar trackBarMemoryLevel2;
        private System.Windows.Forms.TextBox textBoxMemoryLevel2;
        private System.Windows.Forms.TrackBar trackBarMemoryLevel3;
        private System.Windows.Forms.TextBox textBoxMemoryLevel3;
        private System.Windows.Forms.CheckBox checkBoxDisk;
        private System.Windows.Forms.TrackBar trackBarDiskLevel1;
        private System.Windows.Forms.TextBox textBoxDiskLevel1;
        private System.Windows.Forms.TrackBar trackBarDiskLevel2;
        private System.Windows.Forms.TextBox textBoxDiskLevel2;
        private System.Windows.Forms.TrackBar trackBarDiskLevel3;
        private System.Windows.Forms.TextBox textBoxDiskLevel3;
        private System.Windows.Forms.NumericUpDown numericUpDownInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPathData;
        private System.Windows.Forms.Button buttonPathData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPathSummary;
        private System.Windows.Forms.Button buttonPathSummary;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonModify;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NotifyIcon notifyIcon1;

    }
}

