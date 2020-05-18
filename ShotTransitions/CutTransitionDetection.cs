using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShotTransitions
{
	/// <summary>
	/// Description of HistogramTransitionDetection.
	/// </summary>
	public class CutTransitionDetection : AbstractTransitionDetection
	{
		private double[] result = null;
		private int buckets = 8;
		private int areas = 8;
		private double percentRemove = 0.25;
		
		private bool isFinalized = false;
		
		public CutTransitionDetection() {}
		
		public CutTransitionDetection(int buckets, int areas, double percentRemove)
		{
			this.buckets = buckets;
			this.areas = areas;
			this.percentRemove = percentRemove;
		}

		protected override void DoInitialize(int frameCount)
		{
			result = new double[frameCount];
			isFinalized = false;
		}
		
		protected override void DoCompareFrames(FrameTransition transition)
		{
			RGBData frame1 = transition.Frame1;
			RGBData frame2 = transition.Frame2;
			int index = transition.Index;
			
			int[,,,,] histogram1 = new int[areas, areas, buckets, buckets, buckets];
			int[,,,,] histogram2 = new int[areas, areas, buckets, buckets, buckets];
			int factor = 256 / buckets;
			double colSize = frame1.Columns / (double)areas;
			double rowSize = frame2.Rows / (double)areas;
			
			for (int row = 0; row < frame1.Rows; row++)
			{
				for (int col = 0; col < frame1.Columns; col++)
				{
					int r1 = frame1.GetR(col, row);
					int g1 = frame1.GetG(col, row);
					int b1 = frame1.GetB(col, row);
					
					histogram1[(int)(col / colSize), (int)(row / rowSize), r1 / factor, g1 / factor, b1 / factor]++;
					
					int r2 = frame2.GetR(col, row);
					int g2 = frame2.GetG(col, row);
					int b2 = frame2.GetB(col, row);
					
					histogram2[(int)(col / colSize), (int)(row / rowSize), r2 / factor, g2 / factor, b2 / factor]++;
				}
			}
			
			List<int> sums = new List<int>();
			for (int row = 0; row < areas; row++)
			{
				for (int col = 0; col < areas; col++)
				{
					int sum = 0;
					for (int k = 0; k < buckets; k++)
					{
						for (int j = 0; j < buckets; j++)
						{
							for (int i = 0; i < buckets; i++)
							{
								sum += Math.Abs(histogram1[col, row, i, j, k] - histogram2[col, row, i, j, k]);
							}
						}
					}
					sums.Add(sum);
				}
			}
			
			int removeCount = (int)(sums.Count * percentRemove);
			if (removeCount > 0)
			{
				sums = sums.OrderBy(s => s).ToList();
				for (int i = 0; i < removeCount; i++)
					sums.RemoveAt(sums.Count - 1);
			}
						
			double max = frame1.Columns * frame1.Rows * 2 - (colSize * rowSize * removeCount);
			result[index] = sums.Sum() / max;
		}

		protected override double[] DoComputeData()
		{
			if (!isFinalized)
			{
				for (int i = 0 ; i < result.Length - 1; i++)
				{
					result[i] = result[i + 1] - result[i];
				}
				
				isFinalized = true;
			}
			return result;
		}

		protected override TransitionData[] DoComputeTransitions()
		{
			List<TransitionData> transitions = new List<TransitionData>();
			double noiseThreshold = 0.1;
			double bounceThreshold = 0.25;
			int distanceThreshold = 10;
			
			int lastIndex = int.MinValue;
			for (int i = 0; i < result.Length - 1; i++)
			{
				if (result[i] >= noiseThreshold)
				{
					double nextMin = result[i + 1] <= result[i + 2] ? result[i + 1] : result[i + 2];
					if (nextMin <= result[i] * (bounceThreshold - 1) &&
					    nextMin >= result[i] * -(1 + bounceThreshold))
					{
						if (i > lastIndex + distanceThreshold)
							transitions.Add(new TransitionData(i, i));
						
						lastIndex = i;
					}
				}
			}
			
			return transitions.ToArray();
		}
	}
}
