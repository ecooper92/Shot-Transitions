using System;

namespace ShotTransitions
{
	/// <summary>
	/// Description of VideoDataFactory.
	/// </summary>
	public class VideoDataFactory
	{
		public IVideoData GetVideoData(string path)
		{
			int index = path.LastIndexOf(".");
			if (index == -1)
				return null;
						
			string ext = path.Substring(index);
			if (ext == ".zip")
				return new ImagesVideoData(path);
			else
				return new FileVideoData(path);
		}
	}
}
