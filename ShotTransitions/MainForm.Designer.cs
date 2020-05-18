
namespace ShotTransitions
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel outerLayoutPanel;
		private ShotTransitions.DoubleBufferedPanel imagePanel;
		private System.Windows.Forms.Timer videoTimer;
		private System.Windows.Forms.TrackBar positionTrackBar;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart;
		private System.Windows.Forms.TableLayoutPanel controlLayoutPanel;
		private System.Windows.Forms.Button buttonOpen;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Label labelFrame;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.outerLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.imagePanel = new ShotTransitions.DoubleBufferedPanel();
            this.positionTrackBar = new System.Windows.Forms.TrackBar();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.controlLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelFrame = new System.Windows.Forms.Label();
            this.videoTimer = new System.Windows.Forms.Timer(this.components);
            this.checkBoxPlay = new System.Windows.Forms.CheckBox();
            this.outerLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.controlLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // outerLayoutPanel
            // 
            this.outerLayoutPanel.ColumnCount = 1;
            this.outerLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.outerLayoutPanel.Controls.Add(this.imagePanel, 0, 0);
            this.outerLayoutPanel.Controls.Add(this.positionTrackBar, 0, 1);
            this.outerLayoutPanel.Controls.Add(this.chart, 0, 2);
            this.outerLayoutPanel.Controls.Add(this.controlLayoutPanel, 0, 3);
            this.outerLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.outerLayoutPanel.Name = "outerLayoutPanel";
            this.outerLayoutPanel.RowCount = 4;
            this.outerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.outerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.outerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 251F));
            this.outerLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.outerLayoutPanel.Size = new System.Drawing.Size(753, 555);
            this.outerLayoutPanel.TabIndex = 0;
            // 
            // imagePanel
            // 
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(0, 0);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(753, 240);
            this.imagePanel.TabIndex = 0;
            // 
            // positionTrackBar
            // 
            this.positionTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionTrackBar.Location = new System.Drawing.Point(0, 240);
            this.positionTrackBar.Margin = new System.Windows.Forms.Padding(0);
            this.positionTrackBar.Name = "positionTrackBar";
            this.positionTrackBar.Size = new System.Drawing.Size(753, 29);
            this.positionTrackBar.TabIndex = 1;
            this.positionTrackBar.Scroll += new System.EventHandler(this.PositionTrackBarScroll);
            // 
            // chart
            // 
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(15, 269);
            this.chart.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(723, 251);
            this.chart.TabIndex = 2;
            // 
            // controlLayoutPanel
            // 
            this.controlLayoutPanel.ColumnCount = 5;
            this.controlLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.controlLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.controlLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.controlLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.controlLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.controlLayoutPanel.Controls.Add(this.buttonOpen, 3, 0);
            this.controlLayoutPanel.Controls.Add(this.buttonSave, 4, 0);
            this.controlLayoutPanel.Controls.Add(this.labelFrame, 0, 0);
            this.controlLayoutPanel.Controls.Add(this.checkBoxPlay, 1, 0);
            this.controlLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlLayoutPanel.Location = new System.Drawing.Point(0, 520);
            this.controlLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.controlLayoutPanel.Name = "controlLayoutPanel";
            this.controlLayoutPanel.RowCount = 1;
            this.controlLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.controlLayoutPanel.Size = new System.Drawing.Size(753, 35);
            this.controlLayoutPanel.TabIndex = 3;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpen.Location = new System.Drawing.Point(556, 3);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(94, 29);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.ButtonOpenClick);
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(656, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(94, 29);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // labelFrame
            // 
            this.labelFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFrame.Location = new System.Drawing.Point(0, 0);
            this.labelFrame.Margin = new System.Windows.Forms.Padding(0);
            this.labelFrame.Name = "labelFrame";
            this.labelFrame.Size = new System.Drawing.Size(100, 35);
            this.labelFrame.TabIndex = 2;
            this.labelFrame.Text = "Frame - ";
            this.labelFrame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // videoTimer
            // 
            this.videoTimer.Tick += new System.EventHandler(this.VideoTimerTick);
            // 
            // checkBoxPlay
            // 
            this.checkBoxPlay.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPlay.AutoSize = true;
            this.checkBoxPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPlay.Location = new System.Drawing.Point(103, 3);
            this.checkBoxPlay.Name = "checkBoxPlay";
            this.checkBoxPlay.Size = new System.Drawing.Size(94, 29);
            this.checkBoxPlay.TabIndex = 3;
            this.checkBoxPlay.Text = "Play";
            this.checkBoxPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxPlay.UseVisualStyleBackColor = true;
            this.checkBoxPlay.CheckedChanged += new System.EventHandler(this.checkBoxPlay_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 555);
            this.Controls.Add(this.outerLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "ShotTransitions";
            this.outerLayoutPanel.ResumeLayout(false);
            this.outerLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.controlLayoutPanel.ResumeLayout(false);
            this.controlLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}

        private System.Windows.Forms.CheckBox checkBoxPlay;
	}
}
