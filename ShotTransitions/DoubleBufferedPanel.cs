using System;
using System.Windows.Forms;

namespace ShotTransitions
{
	/// <summary>
	/// Description of DoubleBufferedPanel.
	/// </summary>
	public class DoubleBufferedPanel : Panel
	{
		public DoubleBufferedPanel()
		{
			this.DoubleBuffered = true;
		}
	}
}
