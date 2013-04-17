namespace LampMonster
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, "2,5");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, "0,0");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_software_in = new System.Windows.Forms.CheckBox();
            this.checkBox_music_in = new System.Windows.Forms.CheckBox();
            this.checkBox_health_in = new System.Windows.Forms.CheckBox();
            this.checkBox_dvds_in = new System.Windows.Forms.CheckBox();
            this.checkBox_cameras_in = new System.Windows.Forms.CheckBox();
            this.checkBox_books_in = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_software_out = new System.Windows.Forms.CheckBox();
            this.checkBox_music_out = new System.Windows.Forms.CheckBox();
            this.checkBox_health_out = new System.Windows.Forms.CheckBox();
            this.checkBox_dvds_out = new System.Windows.Forms.CheckBox();
            this.checkBox_cameras_out = new System.Windows.Forms.CheckBox();
            this.checkBox_books_out = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(197, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 43);
            this.button1.TabIndex = 0;
            this.button1.Text = "GO";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_software_in);
            this.groupBox1.Controls.Add(this.checkBox_music_in);
            this.groupBox1.Controls.Add(this.checkBox_health_in);
            this.groupBox1.Controls.Add(this.checkBox_dvds_in);
            this.groupBox1.Controls.Add(this.checkBox_cameras_in);
            this.groupBox1.Controls.Add(this.checkBox_books_in);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 167);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Categories to train on";
            // 
            // checkBox_software_in
            // 
            this.checkBox_software_in.AutoSize = true;
            this.checkBox_software_in.Location = new System.Drawing.Point(7, 135);
            this.checkBox_software_in.Name = "checkBox_software_in";
            this.checkBox_software_in.Size = new System.Drawing.Size(68, 17);
            this.checkBox_software_in.TabIndex = 5;
            this.checkBox_software_in.Text = "Software";
            this.checkBox_software_in.UseVisualStyleBackColor = true;
            // 
            // checkBox_music_in
            // 
            this.checkBox_music_in.AutoSize = true;
            this.checkBox_music_in.Location = new System.Drawing.Point(7, 112);
            this.checkBox_music_in.Name = "checkBox_music_in";
            this.checkBox_music_in.Size = new System.Drawing.Size(54, 17);
            this.checkBox_music_in.TabIndex = 4;
            this.checkBox_music_in.Text = "Music";
            this.checkBox_music_in.UseVisualStyleBackColor = true;
            // 
            // checkBox_health_in
            // 
            this.checkBox_health_in.AutoSize = true;
            this.checkBox_health_in.Location = new System.Drawing.Point(7, 89);
            this.checkBox_health_in.Name = "checkBox_health_in";
            this.checkBox_health_in.Size = new System.Drawing.Size(57, 17);
            this.checkBox_health_in.TabIndex = 3;
            this.checkBox_health_in.Text = "Health";
            this.checkBox_health_in.UseVisualStyleBackColor = true;
            // 
            // checkBox_dvds_in
            // 
            this.checkBox_dvds_in.AutoSize = true;
            this.checkBox_dvds_in.Location = new System.Drawing.Point(7, 66);
            this.checkBox_dvds_in.Name = "checkBox_dvds_in";
            this.checkBox_dvds_in.Size = new System.Drawing.Size(56, 17);
            this.checkBox_dvds_in.TabIndex = 2;
            this.checkBox_dvds_in.Text = "DVD\'s";
            this.checkBox_dvds_in.UseVisualStyleBackColor = true;
            // 
            // checkBox_cameras_in
            // 
            this.checkBox_cameras_in.AutoSize = true;
            this.checkBox_cameras_in.Location = new System.Drawing.Point(7, 43);
            this.checkBox_cameras_in.Name = "checkBox_cameras_in";
            this.checkBox_cameras_in.Size = new System.Drawing.Size(67, 17);
            this.checkBox_cameras_in.TabIndex = 1;
            this.checkBox_cameras_in.Text = "Cameras";
            this.checkBox_cameras_in.UseVisualStyleBackColor = true;
            // 
            // checkBox_books_in
            // 
            this.checkBox_books_in.AutoSize = true;
            this.checkBox_books_in.Location = new System.Drawing.Point(7, 20);
            this.checkBox_books_in.Name = "checkBox_books_in";
            this.checkBox_books_in.Size = new System.Drawing.Size(56, 17);
            this.checkBox_books_in.TabIndex = 0;
            this.checkBox_books_in.Text = "Books";
            this.checkBox_books_in.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_software_out);
            this.groupBox2.Controls.Add(this.checkBox_music_out);
            this.groupBox2.Controls.Add(this.checkBox_health_out);
            this.groupBox2.Controls.Add(this.checkBox_dvds_out);
            this.groupBox2.Controls.Add(this.checkBox_cameras_out);
            this.groupBox2.Controls.Add(this.checkBox_books_out);
            this.groupBox2.Location = new System.Drawing.Point(197, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 167);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Categories to evaluate on";
            // 
            // checkBox_software_out
            // 
            this.checkBox_software_out.AutoSize = true;
            this.checkBox_software_out.Location = new System.Drawing.Point(6, 134);
            this.checkBox_software_out.Name = "checkBox_software_out";
            this.checkBox_software_out.Size = new System.Drawing.Size(68, 17);
            this.checkBox_software_out.TabIndex = 11;
            this.checkBox_software_out.Text = "Software";
            this.checkBox_software_out.UseVisualStyleBackColor = true;
            // 
            // checkBox_music_out
            // 
            this.checkBox_music_out.AutoSize = true;
            this.checkBox_music_out.Location = new System.Drawing.Point(6, 111);
            this.checkBox_music_out.Name = "checkBox_music_out";
            this.checkBox_music_out.Size = new System.Drawing.Size(54, 17);
            this.checkBox_music_out.TabIndex = 10;
            this.checkBox_music_out.Text = "Music";
            this.checkBox_music_out.UseVisualStyleBackColor = true;
            // 
            // checkBox_health_out
            // 
            this.checkBox_health_out.AutoSize = true;
            this.checkBox_health_out.Location = new System.Drawing.Point(6, 88);
            this.checkBox_health_out.Name = "checkBox_health_out";
            this.checkBox_health_out.Size = new System.Drawing.Size(57, 17);
            this.checkBox_health_out.TabIndex = 9;
            this.checkBox_health_out.Text = "Health";
            this.checkBox_health_out.UseVisualStyleBackColor = true;
            // 
            // checkBox_dvds_out
            // 
            this.checkBox_dvds_out.AutoSize = true;
            this.checkBox_dvds_out.Location = new System.Drawing.Point(6, 65);
            this.checkBox_dvds_out.Name = "checkBox_dvds_out";
            this.checkBox_dvds_out.Size = new System.Drawing.Size(56, 17);
            this.checkBox_dvds_out.TabIndex = 8;
            this.checkBox_dvds_out.Text = "DVD\'s";
            this.checkBox_dvds_out.UseVisualStyleBackColor = true;
            // 
            // checkBox_cameras_out
            // 
            this.checkBox_cameras_out.AutoSize = true;
            this.checkBox_cameras_out.Location = new System.Drawing.Point(6, 42);
            this.checkBox_cameras_out.Name = "checkBox_cameras_out";
            this.checkBox_cameras_out.Size = new System.Drawing.Size(67, 17);
            this.checkBox_cameras_out.TabIndex = 7;
            this.checkBox_cameras_out.Text = "Cameras";
            this.checkBox_cameras_out.UseVisualStyleBackColor = true;
            // 
            // checkBox_books_out
            // 
            this.checkBox_books_out.AutoSize = true;
            this.checkBox_books_out.Location = new System.Drawing.Point(6, 19);
            this.checkBox_books_out.Name = "checkBox_books_out";
            this.checkBox_books_out.Size = new System.Drawing.Size(56, 17);
            this.checkBox_books_out.TabIndex = 6;
            this.checkBox_books_out.Text = "Books";
            this.checkBox_books_out.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(197, 186);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(153, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Training size";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(92, 186);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(99, 20);
            this.numericUpDown1.TabIndex = 5;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(92, 212);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(99, 20);
            this.numericUpDown2.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Evaluation size";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Indomain",
            "OutofDomain",
            "Categorize"});
            this.comboBox1.Location = new System.Drawing.Point(70, 238);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "<Choose algorithm>";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(356, 13);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            series1.YValuesPerPoint = 2;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(344, 246);
            this.chart1.TabIndex = 8;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 276);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "LampMonster";
            this.TransparencyKey = System.Drawing.Color.Crimson;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_software_in;
        private System.Windows.Forms.CheckBox checkBox_music_in;
        private System.Windows.Forms.CheckBox checkBox_health_in;
        private System.Windows.Forms.CheckBox checkBox_dvds_in;
        private System.Windows.Forms.CheckBox checkBox_cameras_in;
        private System.Windows.Forms.CheckBox checkBox_books_in;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_software_out;
        private System.Windows.Forms.CheckBox checkBox_music_out;
        private System.Windows.Forms.CheckBox checkBox_health_out;
        private System.Windows.Forms.CheckBox checkBox_dvds_out;
        private System.Windows.Forms.CheckBox checkBox_cameras_out;
        private System.Windows.Forms.CheckBox checkBox_books_out;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

