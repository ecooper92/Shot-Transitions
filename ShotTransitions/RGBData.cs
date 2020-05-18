
using System;

namespace ShotTransitions
{
	/// <summary>
	/// Description of RGBData.
	/// </summary>
	public class RGBData
	{
		private int stride;
		
		public RGBData(byte[] rgb, int stride)
		{
			RGB = rgb;
			this.stride = stride;
		}
		
		public byte[] RGB { get; private set;}
		
		public byte GetR(int index)
		{
			return RGB[index * 3 + 2];
		}
		
		public byte GetR(int col, int row)
		{
			return RGB[row*stride + col * 3 + 2];
		}
		
		public byte GetG(int index)
		{
			return RGB[index * 3 + 1];
		}
		
		public byte GetG(int col, int row)
		{
			return RGB[row*stride + col * 3 + 1];
		}
		
		public byte GetB(int index)
		{
			return RGB[index * 3];
		}
		
		public byte GetB(int col, int row)
		{
			return RGB[row*stride + col * 3];
		}
		
		public void SetR(int index, byte b)
		{
			RGB[index * 3 + 2] = b;
		}
		
		public void SetR(int col, int row, byte b)
		{
			RGB[row*stride + col * 3 + 2] = b;
		}
		
		public void SetG(int index, byte b)
		{
			RGB[index * 3 + 1] = b;
		}
		
		public void SetG(int col, int row, byte b)
		{
			RGB[row*stride + col * 3 + 1] = b;
		}
		
		public void SetB(int index, byte b)
		{
			RGB[index * 3] = b;
		}
		
		public void SetB(int col, int row, byte b)
		{
			RGB[row*stride + col * 3] = b;
		}
		
		public int Colors { get { return Columns * Rows; } }
		
		public int Columns { get { return stride / 3; } }
		
		public int Rows 
		{ 
			get 
			{ 
				if (stride <= 0)
					return 0;
				
				return RGB.Length / stride; 
			} 
		}
	}
}
