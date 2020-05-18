using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace ShotTransitions
{
	/// <summary>
	/// Represents the data of a collection of images that are the frames of a 
	/// video. Provides methods for processing the video frame by frame.
	/// </summary>
	public class ImagesVideoData : AbstractVideoData
	{
		private FileStream fileStream = null;
		private ZipFile zipFile = null;
		
		public ImagesVideoData(string path)
		{
			Open(path);
		}

		protected override void DoOpen()
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			zipFile = new ZipFile(fileStream);
			
			FrameCount = (int)zipFile.Count;
		}

		protected override void DoSeek(int frame)
		{
			if (frame < 0)
				currentFrame = 0;
			else if (frame >= FrameCount)
				currentFrame = FrameCount - 1;
			
			currentFrame = frame;
		}

		protected override Bitmap DoGetNextFrame()
		{
			if (CurrentFrame >= zipFile.Count)
				return null;
			
			using (Stream inStream = zipFile.GetInputStream(CurrentFrame))
				return new Bitmap(inStream);
		}

		protected override void DoDispose()
		{
			if (zipFile != null)
			{
				zipFile.Close();
				zipFile = null;
			}
			
			if (fileStream != null)
			{
				fileStream.Dispose();
				fileStream = null;
			}
		}
	}
}
