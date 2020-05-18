using System;
using System.Collections.Generic;
using System.Threading;

namespace ShotTransitions
{
	/// <summary>
	/// Description of FadeTransitionDetection.
	/// </summary>
	public class FadeTransitionDetection : AbstractTransitionDetection
	{
		private double[] result = null;
		
		private const double rFactor = 0.229;
		private const double gFactor = 0.587;
		private const double bFactor = 0.114;
		
		private const double blackThreshold = 0.05;
		private const double error = 0.00;
		private const int fadeMinRange = 5;
		private const double fadeMinMagnitude = 0.15;
		private const int canFailSlope = 2;
		private const double maxSlopeError = 0.075;
		
		private bool isScaled = false;
		
		public FadeTransitionDetection() { }

		protected override void DoInitialize(int frameCount)
		{
			result = new double[frameCount];
			isScaled = false;
		}

		protected override void DoCompareFrames(FrameTransition frameTransition)
		{
			RGBData frame1 = frameTransition.Frame1;
			RGBData frame2 = frameTransition.Frame2;
			int index = frameTransition.Index;
			
			double sum = 0;
			
			for (int row = 0; row < frame1.Rows; row++)
			{
				for (int col = 0; col < frame1.Columns; col++)
				{
					double i1 = rFactor * frame1.GetR(col, row) +
								gFactor * frame1.GetG(col, row) +
								bFactor * frame1.GetB(col, row);
					
					sum += i1;
				}
			}
			
			result[index] = sum / (frame1.Columns * frame1.Rows);
		}

		protected override double[] DoComputeData()
		{
			if (!isScaled)
			{
				double max = double.MinValue;
				
				for (int i = 0; i < result.Length; i++)
				{
					if (result[i] > max)
						max = result[i];
				}
				
				for (int i = 0; i < result.Length; i++)
				{
					result[i] /= max;
				}
				
				isScaled = true;
			}
			
			return result;
		}

		protected override TransitionData[] DoComputeTransitions()
		{
			List<TransitionData> transitions = new List<TransitionData>();
			
			List<int> increasingPoints = new List<int>();
			List<int> decreasingPoints = new List<int>();
			
			for (int i = 0; i < result.Length - 1; i++)
			{
				if (result[i] <= (blackThreshold * (1 - error)) && result[i + 1] > (blackThreshold * (1 + error)))
				{
					increasingPoints.Add(i);
				}
				else if (result[i] > (blackThreshold * (1 - error)) && result[i + 1] <= (blackThreshold * (1 + error)))
				{
					decreasingPoints.Add(i);
				}
			}
			
			foreach (int start in increasingPoints)
			{
				int range = GetIncreaseEnd(result, start);
				int end = GetIncreasingSlopeEnd(result, start, start + range);
				
				if (end - start >= fadeMinRange && (result[end] - result[start]) > fadeMinMagnitude)
					transitions.Add(new TransitionData(start, end));
			}
			
			foreach (int end in decreasingPoints)
			{
				int range = GetDecreaseEnd(result, end);
				int start = GetDecreasingSlopeEnd(result, end - range, end);
				
				if (end - start >= fadeMinRange && (result[start] - result[end]) > fadeMinMagnitude)
					transitions.Add(new TransitionData(end, start));
			}
			
			return transitions.ToArray();
		}
		
		private int GetIncreaseEnd(double[] results, int start)
		{
			// Check if the values are increasing
			int failedIncrease = 0;
			int counter = 0;
			while (start + counter < results.Length - 1 && failedIncrease < canFailSlope)
			{
				if (results[start + counter + 1] > results[start + counter])
					failedIncrease = 0;
				else
					failedIncrease++;
				counter++;
			}
			
			return counter - failedIncrease;
		}
		
		private int GetDecreaseEnd(double[] results, int end)
		{
			// Check if the values are increasing
			int failedIncrease = 0;
			int counter = 0;
			while (end - counter > 0 && failedIncrease < canFailSlope)
			{
				if (results[end - counter - 1] > results[end - counter])
					failedIncrease = 0;
				else
					failedIncrease++;
				counter++;
			}
			
			return counter - failedIncrease;
		}
		
		private int GetIncreasingSlopeEnd(double[] results, int start, int end)
		{
			int newEnd = end;
			if (newEnd - start < fadeMinRange)
				return start;
			
			double slope = (results[start + fadeMinRange] - results[start]) / fadeMinRange;
			int failedLinear = 0;
			
			// Check if they follow a linear slope
			for (int i = start; i < newEnd; i++)
			{
				double error = Math.Abs(results[i] - (results[start] + (slope * (i - start))));
				
				if (error > maxSlopeError)
				{
					failedLinear++;
					if (failedLinear >= canFailSlope)
					{
						newEnd = i - failedLinear;
					}
				}
				else
				{
					failedLinear = 0;
				}
			}
			
			return newEnd;
		}
		
		private int GetDecreasingSlopeEnd(double[] results, int start, int end)
		{
			int newStart = start;
			if (end - newStart < fadeMinRange)
				return end;
			
			double slope = (results[newStart] - results[newStart + fadeMinRange]) / fadeMinRange;
			int failedLinear = 0;
			
			// Check if they follow a linear slope
			for (int i = end; i > newStart; i--)
			{
				double error = Math.Abs(results[i] - (results[end] + (slope * Math.Abs(i - end))));
				
				if (error > maxSlopeError)
				{
					failedLinear++;
					if (failedLinear >= canFailSlope)
					{
						end = i - failedLinear;
					}
				}
				else
				{
					failedLinear = 0;
				}
			}
			
			return newStart;
		}
	}
}
