namespace POWER_SUPPLY_CONTROL_SIMPLE
{
    partial class Form2
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.UDPCheckbox = new System.Windows.Forms.CheckBox();
            this.AllowCtrlCheckBox = new System.Windows.Forms.CheckBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.udpPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ApplyBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // UDPCheckbox
            // 
            this.UDPCheckbox.AutoSize = true;
            this.UDPCheckbox.Checked = true;
            this.UDPCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UDPCheckbox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UDPCheckbox.Location = new System.Drawing.Point(12, 221);
            this.UDPCheckbox.Name = "UDPCheckbox";
            this.UDPCheckbox.Size = new System.Drawing.Size(331, 28);
            this.UDPCheckbox.TabIndex = 0;
            this.UDPCheckbox.Text = "Establish UDP Broadcasting Server";
            this.UDPCheckbox.UseVisualStyleBackColor = true;
            // 
            // AllowCtrlCheckBox
            // 
            this.AllowCtrlCheckBox.AutoSize = true;
            this.AllowCtrlCheckBox.Checked = true;
            this.AllowCtrlCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllowCtrlCheckBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AllowCtrlCheckBox.Location = new System.Drawing.Point(12, 255);
            this.AllowCtrlCheckBox.Name = "AllowCtrlCheckBox";
            this.AllowCtrlCheckBox.Size = new System.Drawing.Size(335, 28);
            this.AllowCtrlCheckBox.TabIndex = 1;
            this.AllowCtrlCheckBox.Text = "Allow Controlled From Other Apps";
            this.AllowCtrlCheckBox.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea2.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea2.Name = "ChartArea";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartArea";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Red;
            series3.Legend = "Legend1";
            series3.Name = "currSeries";
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Navy;
            series4.IsVisibleInLegend = false;
            series4.Legend = "Legend1";
            series4.MarkerColor = System.Drawing.Color.Red;
            series4.Name = "VoltSeries";
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(778, 226);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            this.chart1.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.SystemDefault;
            title2.Name = "Title1";
            title2.Text = "Volt/Curr View";
            this.chart1.Titles.Add(title2);
            // 
            // udpPort
            // 
            this.udpPort.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.udpPort.Location = new System.Drawing.Point(479, 253);
            this.udpPort.Name = "udpPort";
            this.udpPort.Size = new System.Drawing.Size(100, 31);
            this.udpPort.TabIndex = 3;
            this.udpPort.Text = "6501";
            this.udpPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(426, 256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "port";
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyBtn.Location = new System.Drawing.Point(594, 248);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(149, 41);
            this.ApplyBtn.TabIndex = 5;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(778, 304);
            this.ControlBox = false;
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udpPort);
            this.Controls.Add(this.UDPCheckbox);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.AllowCtrlCheckBox);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "PSC Controller";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.CheckBox UDPCheckbox;
        public System.Windows.Forms.CheckBox AllowCtrlCheckBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public System.Windows.Forms.TextBox udpPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ApplyBtn;
    }
}