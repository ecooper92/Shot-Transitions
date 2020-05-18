using System;

namespace ShotTransitions
{
	/// <summary>
	/// Description of TransitionData.
	/// </summary>
	public class TransitionData
	{
		public TransitionData(int startFrame, int endFrame)
		{
			StartFrame = startFrame;
			EndFrame = endFrame;
		}
		
		public int StartFrame { get; private set; }
		
		public int EndFrame { get; private set; }
	}
}
