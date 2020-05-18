using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ShotTransitions
{
	/// <summary>
	/// Description of OtherTransitionDetection.
	/// </summary>
	public class OtherTransitionDetection : AbstractTransitionDetection
	{
		int frames = 0;
		int counter = 0;
		ImageData outputImageData;

		protected override void DoInitialize(int frameCount)
		{
			counter = 0;
			frames = frameCount;
		}

		protected override void DoCompareFrames(FrameTransition transition)
		{
			RGBData frame1 = transition.Frame1;
			RGBData frame2 = transition.Frame2;
			int index = transition.Index;
			
			if (outputImageData == null) {
				outputImageData = new ImageData(new Bitmap(frames, frame1.Rows * 3));
				outputImageData.Lock();
			}
				
			for (int i = 0; i < frame1.Rows; i++)
			{
				outputImageData.Data.SetR(counter, i, frame1.GetR(frame1.Columns / 4, i));
				outputImageData.Data.SetG(counter, i, frame1.GetG(frame1.Columns / 4, i));
				outputImageData.Data.SetB(counter, i, frame1.GetB(frame1.Columns / 4, i));
				
				outputImageData.Data.SetR(counter, i + frame1.Rows, frame1.GetR(frame1.Columns / 2, i));
				outputImageData.Data.SetG(counter, i + frame1.Rows, frame1.GetG(frame1.Columns / 2, i));
				outputImageData.Data.SetB(counter, i + frame1.Rows, frame1.GetB(frame1.Columns / 2, i));
				
				outputImageData.Data.SetR(counter, i + frame1.Rows * 2, frame1.GetR(frame1.Columns * 3 / 4, i));
				outputImageData.Data.SetG(counter, i + frame1.Rows * 2, frame1.GetG(frame1.Columns * 3 / 4, i));
				outputImageData.Data.SetB(counter, i + frame1.Rows * 2, frame1.GetB(frame1.Columns * 3 / 4, i));
			}
		}

		protected override double[] DoComputeData()
		{
			outputImageData.Unlock();
			outputImageData.Image.Save("Test.png", ImageFormat.Png);
			return new double[0];
		}

		protected override TransitionData[] DoComputeTransitions()
		{
			throw new NotImplementedException();
		}
	}
}
