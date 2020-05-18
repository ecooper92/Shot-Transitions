using System;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ShotTransitions
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private VideoDataFactory videoDataFactory = new VideoDataFactory();
		private IVideoData data;
		private Bitmap currentImage;
		private ITransitionDetection cutDetection = new CutTransitionDetection(4,20,0.3); // Best 4, 20, 0.3
		private ITransitionDetection fadeDetection = new FadeTransitionDetection();
		
		public MainForm()
		{
			InitializeComponent();
			
			imagePanel.Paint += (object sender, PaintEventArgs e) => 
			{
				if (currentImage != null)
				{
					try
					{
						e.Graphics.DrawImage(currentImage, 0, 0, imagePanel.Width, imagePanel.Height);
					}
					catch (Exception ex)
					{
						
					}
				}
			};
			
			SelectVideo();
		}
		
		private async void SelectVideo()
		{			
			OpenFileDialog ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				data = videoDataFactory.GetVideoData(ofd.FileName);
				if (data.FrameRate == -1)
					data.FrameRate = 30;
				
				double[] histResults = await cutDetection.Calculate(data);
				double[] fadeResults = await fadeDetection.Calculate(data);
				
				SetChartData("test1", histResults, cutDetection.GetTransitions(), -1);
				SetChartData("test2", fadeResults, fadeDetection.GetTransitions(), 0);
				
				data.CurrentFrame = 0;
				positionTrackBar.Maximum = data.FrameCount;
				positionTrackBar.Value = 0;
                videoTimer.Interval = (int)(1000 / data.FrameRate);
                checkBoxPlay.Checked = true;
				videoTimer.Start();
			}
		}
		
		private void SaveResults()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "PNG file (*.png)|*.png";
			
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				int counter = 0;
				int maxSize = 100;
				int widthMargin = 30;
				int heightMargin = 30;
				int rowWidth = 5;
				
				Bitmap resultImage = null;
				Graphics g = null;
				
				int lastFrame = data.CurrentFrame;
				bool isPlaying = videoTimer.Enabled;
				data.CurrentFrame = 0;
				
				TransitionData[] cuts = cutDetection.GetTransitions();
				TransitionData[] fades = fadeDetection.GetTransitions();
				
				var all = cuts.Union(fades).OrderBy(t => t.StartFrame);
				foreach (TransitionData transition in all)
				{
					bool isCut = true;
					int frameIndex = transition.StartFrame + 2;
					if (frameIndex >= data.FrameCount)
						frameIndex = data.FrameCount - 2;
						
					if (transition.StartFrame != transition.EndFrame)
					{
						frameIndex = (transition.StartFrame + transition.EndFrame) / 2;
						isCut = false;
					}
					
					data.CurrentFrame = frameIndex;
					ImageData frame = data.GetNextFrame();
					
					int column = counter % rowWidth;
					int row = counter / rowWidth;
					
					int width = frame.Image.Width > maxSize ? maxSize : frame.Image.Width;
					int height = frame.Image.Height > maxSize ? maxSize : frame.Image.Height;
					
					widthMargin = (int)(width * 0.3);
					heightMargin = (int)(height * 0.3);
					
					int rowHeight = (all.Count() / rowWidth) + 1;
					
					if (resultImage == null)
					{
						resultImage = new Bitmap(width * rowWidth + (rowWidth + 1) * widthMargin, rowHeight * height + (rowHeight + 1) * heightMargin);
						
						g = Graphics.FromImage(resultImage);
						g.TextRenderingHint = TextRenderingHint.AntiAlias;
					}
					
					int left = widthMargin + column * (width + widthMargin);
					int top = heightMargin + row * (height + heightMargin);
					
					g.DrawImage(frame.Image, left, top, width, height);
					
					string transitionType = "Cut";
					if (!isCut)
						transitionType = "Fade";
					
					transitionType += " - " + frameIndex.ToString();
					
					Font font = new Font("Segoe UI", 11, FontStyle.Regular, GraphicsUnit.Pixel);
					
					SizeF sizeType = g.MeasureString(transitionType, font);
					g.DrawString(transitionType, font,Brushes.Black, left + width / 2f - sizeType.Width / 2f, top + height + (heightMargin / 2f) - (sizeType.Height / 2));
									
					counter++;
				}
				
				g.Dispose();
				resultImage.Save(sfd.FileName, ImageFormat.Png);
				resultImage.Dispose();
				
				if (isPlaying)
					data.CurrentFrame = lastFrame;
			}
		}
		
		void VideoTimerTick(object sender, EventArgs e)
		{
            UpdateFrame();
		}

        private void UpdateFrame()
        {
            UpdateFrameLabel(data.CurrentFrame);
            positionTrackBar.Value = data.CurrentFrame;
            ImageData nextFrame = data.GetNextFrame();
            if (nextFrame != null)
            {
                currentImage = nextFrame.Image;
            }
            imagePanel.Invalidate();
        }
		
		void PositionTrackBarScroll(object sender, EventArgs e)
		{
            data.CurrentFrame = positionTrackBar.Value;
            UpdateFrame();
		}
		
		private void SetChartData(string name, double[] result, TransitionData[] transitions, double yMin)
		{
			chart.ChartAreas.Add(name);
			chart.ChartAreas[name].AxisX.Minimum = 0;
			chart.ChartAreas[name].AxisX.Maximum = result.Length;
			chart.ChartAreas[name].AxisY.Minimum = yMin;
			chart.ChartAreas[name].AxisY.Maximum = 1;
			chart.ChartAreas[name].AxisX.Enabled = AxisEnabled.False;
			chart.ChartAreas[name].AxisX2.Enabled = AxisEnabled.False;
			chart.ChartAreas[name].AxisY.Enabled = AxisEnabled.False;
			chart.ChartAreas[name].AxisY2.Enabled = AxisEnabled.False;
			chart.ChartAreas[name].Position.Auto = false;
			
			for (int i = 0; i < chart.ChartAreas.Count; i++)
			{
				ChartArea ca = chart.ChartAreas[i];
				ca.Position.X = 0;
				ca.Position.Y = (100 / chart.ChartAreas.Count) * i;
				ca.Position.Width = 100;
				ca.Position.Height = 100 / chart.ChartAreas.Count;
			}
			
			string seriesName1 = name + "1";
			chart.Series.Add(seriesName1);
			chart.Series[seriesName1].ChartType = SeriesChartType.Line;
			chart.Series[seriesName1].ChartArea = name;
			chart.Series[seriesName1].Color = Color.FromArgb(255, 90, 155, 212);
			chart.Series[seriesName1].Points.Clear();
			
			for (int i = 0; i < result.Length; i++)
			{
				chart.Series[seriesName1].Points.AddXY(i, result[i]);
			}
			
			if (transitions != null && transitions.Length > 0)
			{
				for (int i = 0; i < transitions.Length; i++)
				{
					string seriesName3 = name + "20" + i;
					chart.Series.Add(seriesName3);
					chart.Series[seriesName3].ChartType = SeriesChartType.Line;
					chart.Series[seriesName3].ChartArea = name;
					chart.Series[seriesName3].BorderWidth = 2;
					chart.Series[seriesName3].Color = Color.FromArgb(255, 241, 90, 96);
					chart.Series[seriesName3].Points.Clear();
					chart.Series[seriesName3].Points.AddXY(transitions[i].StartFrame, 1);
					chart.Series[seriesName3].Points.AddXY(transitions[i].StartFrame, 0.8);
					
					if (transitions[i].StartFrame != transitions[i].EndFrame)
					{
						seriesName3 = name + "21" + i;
						chart.Series.Add(seriesName3);
						chart.Series[seriesName3].ChartType = SeriesChartType.Line;
						chart.Series[seriesName3].ChartArea = name;
						chart.Series[seriesName3].Color = Color.FromArgb(255, 241, 90, 96);
						chart.Series[seriesName3].Points.Clear();
						chart.Series[seriesName3].Points.AddXY(transitions[i].StartFrame, 0.9);
						chart.Series[seriesName3].Points.AddXY(transitions[i].EndFrame, 0.9);
						
						seriesName3 = name + "22" + i;
						chart.Series.Add(seriesName3);
						chart.Series[seriesName3].ChartType = SeriesChartType.Line;
						chart.Series[seriesName3].ChartArea = name;
						chart.Series[seriesName3].BorderWidth = 2;
						chart.Series[seriesName3].Color = Color.FromArgb(255, 241, 90, 96);
						chart.Series[seriesName3].Points.Clear();
						chart.Series[seriesName3].Points.AddXY(transitions[i].EndFrame, 1);
						chart.Series[seriesName3].Points.AddXY(transitions[i].EndFrame, 0.8);
					}
					
				}
			}
		}
		
		void ButtonOpenClick(object sender, EventArgs e)
		{
			SelectVideo();
		}
		
		void ButtonSaveClick(object sender, EventArgs e)
		{
			SaveResults();
		}
		
		private void UpdateFrameLabel(int frame)
		{
			labelFrame.Text = "Frame - " + frame;
		}

        private void checkBoxPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlay.Checked)
            {
                checkBoxPlay.Text = "Pause";
                if (data != null)
                    videoTimer.Start();
            }
            else
            {
                checkBoxPlay.Text = "Play";
                videoTimer.Stop();
            }
        }
	}
}
