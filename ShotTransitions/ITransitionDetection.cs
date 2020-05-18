using System;
using System.Threading.Tasks;

namespace ShotTransitions
{
	/// <summary>
	/// Description of ITransitionDetection.
	/// </summary>
	public interface ITransitionDetection
	{
		Task<double[]> Calculate(IVideoData data);
		
		double[] GetData();
		
		TransitionData[] GetTransitions();
	}
}
