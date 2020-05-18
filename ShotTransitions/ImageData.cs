using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ShotTransitions
{
	/// <summary>
	/// Represents locked bitmap data including the original image, the data,
	/// and the RGB Values.
	/// </summary>
	public class ImageData : IDisposable
	{		
		public ImageData(Bitmap image)
		{
			if (image == null)
				throw new ArgumentException("Image must not be null!");
			
			Image = image;
		}
		
		/// <summary>
		/// The currently locked image used to create this image data.
		/// </summary>
		public Bitmap Image { get; private set; }
		
		/// <summary>
		/// The data and properties of the locked image.
		/// </summary>
		public BitmapData BitmapData { get; private set; }
		
		/// <summary>
		/// The current RGB Data of the locked image.
		/// </summary>
		public RGBData Data { get; private set; }
		
		/// <summary>
		/// Gets a value indicating whether the image data is locked.
		/// </summary>
		public bool IsLocked { get; private set; }

		/// <summary>
		/// Unlocks the Bitmap, but does not dispose of it.  There may be cases
		/// where we need to reused the underlying image, but just need to free
		/// the locked resources.
		/// </summary>
		public void Dispose()
		{
			Unlock();
		}
		
		/// <summary>
		/// Locks the Bitmap for editing and copies the bytes into data allowing
		/// them to be modified.
		/// </summary>
		public void Lock() 
		{         
			if (!IsLocked)
			{
	            BitmapData = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
	
	            int numBytes = BitmapData.Stride * Image.Height;
	            Data = new RGBData(new byte[numBytes], BitmapData.Stride);
	
	            Marshal.Copy(BitmapData.Scan0, Data.RGB, 0, numBytes);
	            
	            IsLocked = true;
			}
		}
		
		/// <summary>
		/// Unlocks the Bitmap from editing, and copies any changes to Data back
		/// into the image.
		/// </summary>
		public void Unlock()
		{
			if (IsLocked)
			{
	            int numBytes = BitmapData.Stride * Image.Height;
	            Marshal.Copy(Data.RGB, 0, BitmapData.Scan0, numBytes);
	            Image.UnlockBits(BitmapData);
	            
	            BitmapData = null;
	            Data = null;
				IsLocked = false;
			}
		}
		
		/// <summary>
		/// Draw the given image on this image.
		/// </summary>
		/// <param name="srcData">The image to draw</param>
		/// <param name="destX">The X position on this image.</param>
		/// <param name="destY">The Y position on this image.</param>
		public void Draw(ImageData srcData, int destX, int destY)
		{
			Draw(srcData.Data, destX, destY);
		}
		
		/// <summary>
		/// Draw the given image on this image.
		/// </summary>
		/// <param name="srcData">The image to draw</param>
		/// <param name="destX">The X position on this image.</param>
		/// <param name="destY">The Y position on this image.</param>
		/// <param name="srcX">The X position on the source image.</param>
		/// <param name="srcY">The Y position on the source image.</param>
		/// <param name="srcWidth">The width of the source image to draw.</param>
		/// <param name="srcHeight">The height of the source image to draw.</param>
		public void Draw(ImageData srcData, int destX, int destY, int srcX, int srcY, int srcWidth, int srcHeight)
		{
			Draw(srcData.Data, destX, destY, srcX, srcY, srcWidth, srcHeight);
		}
		
		/// <summary>
		/// Draw the given image on this image.
		/// </summary>
		/// <param name="srcData">The image to draw</param>
		/// <param name="destX">The X position on this image.</param>
		/// <param name="destY">The Y position on this image.</param>
		public void Draw(RGBData srcData, int destX, int destY)
		{
			Draw(srcData, destX, destY, 0,0, srcData.Columns, srcData.Rows);
		}
		
		/// <summary>
		/// Draw the given image on this image.
		/// </summary>
		/// <param name="srcData">The image to draw</param>
		/// <param name="destX">The X position on this image.</param>
		/// <param name="destY">The Y position on this image.</param>
		/// <param name="srcX">The X position on the source image.</param>
		/// <param name="srcY">The Y position on the source image.</param>
		/// <param name="srcWidth">The width of the source image to draw.</param>
		/// <param name="srcHeight">The height of the source image to draw.</param>
		public void Draw(RGBData srcData, int destX, int destY, int srcX, int srcY, int srcWidth, int srcHeight)
		{
			for (int j = 0; j < srcHeight - srcY; j++)
			{
				for (int i = 0; i < srcWidth - srcX; i++)
				{
					Data.SetR(destX + i, destY + j, srcData.GetR(i + srcX, j + srcY));
					Data.SetG(destX + i, destY + j, srcData.GetG(i + srcX, j + srcY));
					Data.SetB(destX + i, destY + j, srcData.GetB(i + srcX, j + srcY));
				}
			}
		}
		
		/// <summary>
		/// Scales the pixel values of the image.
		/// </summary>
		/// <param name="xScale">The value by which to scale x.</param>
		/// <param name="yScale">The value by which to scale y.</param>
		/// <returns></returns>
		public RGBData Scale(double xScale, double yScale)
		{
			if (xScale < 0.0001)
				xScale = 0.0001;
			if (yScale < 0.0001)
				yScale = 0.0001;
			
			double xPixels = 1 / xScale;
			double yPixels = 1 / yScale;
			
			double xFraction = xPixels - (int)xPixels;
			double yFraction = yPixels - (int)yPixels;
			
			int scaledColumns = (int)(Data.Columns * xScale);
			int scaledRows = (int)(Data.Rows * yScale);
			int scaledStride = scaledColumns * 3;
			
			RGBData data = new RGBData(new byte[scaledStride * scaledRows], scaledStride);
			
			for (int row = 0; row < scaledRows; row++)
			{
				for (int col = 0; col < scaledColumns; col++)
				{
					double rSum = 0;
					double gSum = 0;
					double bSum = 0;
					
					int xPos = (int)(col * xPixels);
					int yPos = (int)(row * yPixels);
					
					for (int i = 0; i < (int)yPixels; i++)
					{
						for (int j = 0; j < (int)xPixels; j++)
						{
							rSum += Data.GetR(xPos + j, yPos + i);
							gSum += Data.GetG(xPos + j, yPos + i);
							bSum += Data.GetB(xPos + j, yPos + i);
						}
					}
					
					data.SetR(col, row, (byte)(rSum / (int)((int)xPixels * (int)yPixels)));
					data.SetG(col, row, (byte)(gSum / (int)((int)xPixels * (int)yPixels)));
					data.SetB(col, row, (byte)(bSum / (int)((int)xPixels * (int)yPixels)));
				}
			}
			
			return data;
		}
	}
}
