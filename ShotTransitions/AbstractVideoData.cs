
using System;
using System.Drawing;

namespace ShotTransitions
{
	/// <summary>
	/// Description of AbstractVideoData.
	/// </summary>
	public abstract class AbstractVideoData : IVideoData
	{
		private ImageData lastPassedFrame = null;
		protected int currentFrame = 0;
		protected string path;
		
		/// <summary>
		/// Cleans up and existing data, then opens a new video from the given
		/// path.
		/// </summary>
		/// <param name="path"></param>
		public void Open(string path)
		{
			Dispose();
			FrameRate = -1;
			
			this.path = path;
			
			DoOpen();
			
			currentFrame = 0;
		}
		
		/// <summary>
		/// Gets the frame at the CurrentFrame index. Unlocks the bits and
		/// disposes of the last frame if it hasn't already been done.
		/// </summary>
		/// <returns></returns>
		public ImageData GetNextFrame()
		{
			// Cleanup previous frame
			CloseLastFrame();
			
			// Read the next frame and wrap it.
			Bitmap nextImage = DoGetNextFrame();

			if (nextImage == null)
				return null;
			
			lastPassedFrame = new ImageData(nextImage);
			
			// Increment frame counter
			currentFrame++;
			
			return lastPassedFrame;
		}
		
		/// <summary>
		/// Gets or Sets the next frame that will be received from the
		/// GetNextFrame() method.
		/// </summary>
		public int CurrentFrame 
		{ 
			get { return currentFrame; }
			set { DoSeek(value); }
		}

		public double FrameRate { get; set; }
		
		public int FrameCount { get; protected set; }
		
		/// <summary>
		/// Release all remaining resources.
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// Release all remaining resources.
		/// </summary>
		public void Dispose()
		{
			CloseLastFrame();
			
			DoDispose();
		}
		
		/// <summary>
		/// Child implementation of open.
		/// </summary>
		protected abstract void DoOpen();
		
		/// <summary>
		/// Child implementation of seeking.
		/// </summary>
		/// <param name="frame"></param>
		protected abstract void DoSeek(int frame);
		
		/// <summary>
		/// Child implementation of GetNextFrame.
		/// </summary>
		protected abstract Bitmap DoGetNextFrame();
		
		/// <summary>
		/// Child implementation of Dispose.
		/// </summary>
		protected abstract void DoDispose();
		
		/// <summary>
		/// Attempts to close the last frame that was passed out using 
		/// GetNextFrame().
		/// </summary>
		private void CloseLastFrame()
		{
			if (lastPassedFrame != null)
			{
				lastPassedFrame.Unlock();
				lastPassedFrame.Image.Dispose();
				lastPassedFrame = null;
			}
		}
	}
}
