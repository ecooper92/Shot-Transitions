using System;
using System.Drawing;
using Accord.Video.FFMPEG;

namespace ShotTransitions
{
	/// <summary>
	/// Represents the data of a video file at a given path. Provides methods 
	/// for processing the video frame by frame.
	/// </summary>
	public class FileVideoData : AbstractVideoData
	{
		private VideoFileReader reader = null;
		
		public FileVideoData(string path)
		{
			Open(path);
		}

		protected override void DoOpen()
		{
			reader = new VideoFileReader();
			reader.Open(path);
			
			FrameRate = reader.FrameRate.Value;
			
			// Hack because FrameCount doesn't always return correctly...
			int count = 0;
			while (GetNextFrame() != null)
				count++;
			FrameCount = count;
			
			reader = new VideoFileReader();
			reader.Open(path);
			currentFrame = 0;
		}

		protected override void DoSeek(int frame)
		{
			if (frame < CurrentFrame)
			{
				reader = new VideoFileReader();
				reader.Open(path);
				currentFrame = 0;
			}
			
			while (frame > CurrentFrame && GetNextFrame() != null) {}
		}
		
		protected override Bitmap DoGetNextFrame()
		{
			return reader.ReadVideoFrame();
		}

		protected override void DoDispose()
		{
			if (reader != null)
			{
				reader.Close();
				reader = null;
			}
		}
	}
}
