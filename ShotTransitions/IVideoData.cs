using System;

namespace ShotTransitions
{
	/// <summary>
	/// Description of IVideoData.
	/// </summary>
	public interface IVideoData : IDisposable
	{		
		/// <summary>
		/// Cleans up and existing data, then opens a new video from the given
		/// path.
		/// </summary>
		/// <param name="path"></param>
		void Open(string path);
		
		/// <summary>
		/// Release all remaining resources.
		/// </summary>
		void Close();
		
		/// <summary>
		/// Gets the frame at the CurrentFrame index. Unlocks the bits and
		/// disposes of the last frame if it hasn't already been done.
		/// </summary>
		/// <returns></returns>
		ImageData GetNextFrame();
		
		/// <summary>
		/// Gets or Sets the next frame that will be received from the
		/// GetNextFrame() method.
		/// </summary>
		int CurrentFrame { get; set; }
		
		/// <summary>
		/// Gets the framerate in frames per second
		/// </summary>
		double FrameRate { get; set; }
		
		int FrameCount { get; }
	}
}
