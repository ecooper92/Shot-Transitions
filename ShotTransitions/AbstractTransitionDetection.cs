using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShotTransitions
{
	/// <summary>
	/// Description of AbstractTransitionDetection.
	/// </summary>
	public abstract class AbstractTransitionDetection : ITransitionDetection
	{
		private Semaphore completeLock = null;
		private Semaphore maxFrameLock = null;
		private int completed = 0;
		private int frameCount = 0;
		private int maxOpenFrames = 128;
		
		public Task<double[]> Calculate(IVideoData data)
		{
			Func<double[]> func = () =>
			{
				frameCount = data.FrameCount;
				DoInitialize(frameCount);
				
				completed = 0;
				maxFrameLock = new Semaphore(maxOpenFrames, maxOpenFrames);
				completeLock = new Semaphore(1, 1);
				completeLock.WaitOne();
				data.CurrentFrame = 0;
				
				RGBData frame1 = null;			
				ImageData data1 = data.GetNextFrame();
				if (data1 != null)
				{
					data1.Lock();
					frame1 = data1.Data;
				}
				
				RGBData frame2 = null;
				ImageData data2 = data.GetNextFrame();
				if (data2 != null)
				{
					data2.Lock();
					frame2 = data2.Data;
				}
				
				int counter = 0;
				while (data2 != null)
				{
					maxFrameLock.WaitOne();
					FrameTransition frameTransition = new FrameTransition(frame1, frame2, counter++);
					
					ThreadPool.QueueUserWorkItem(new WaitCallback((state) => 
	                {
						DoCompareFrames(frameTransition);
						
						maxFrameLock.Release();
												
						Interlocked.Increment(ref completed);
						if (completed >= frameCount - 1)
							completeLock.Release();
	                }));
					
					frame1 = frame2;
					
					data2 = data.GetNextFrame();
					if (data2 != null)
					{
						data2.Lock();
						frame2 = data2.Data;
					}
				}
				
				completeLock.WaitOne();
				double[] results = DoComputeData();
				completeLock.Release();
				
				return results;
			};
			
			return Task<double[]>.Run(func);
		}

		public double[] GetData()
		{
			return DoComputeData();
		}
		
		public TransitionData[] GetTransitions()
		{
			return DoComputeTransitions();
		}		
		
		protected abstract void DoInitialize(int frameCount);
		
		protected abstract void DoCompareFrames(FrameTransition frameTransition);
		
		protected abstract double[] DoComputeData();
		
		protected abstract TransitionData[] DoComputeTransitions();
		
		protected class FrameTransition
		{
			public FrameTransition(RGBData frame1, RGBData frame2, int index)
			{
				Frame1 = frame1;
				Frame2 = frame2;
				Index = index;
			}
			
			public RGBData Frame1 { get; private set; }
			
			public RGBData Frame2 { get; private set; }
			
			public int Index { get; private set; }
		}
	}
}
