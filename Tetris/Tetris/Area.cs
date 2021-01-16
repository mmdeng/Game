using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	/// <summary>
	/// 矩形
	/// </summary>
	public class Area
	{
		public int L;
		public int T;
		public int W;
		public int H;
		public Rectangle GetRectangle()
		{
			return new Rectangle(L, T, W, H);
		}
	}
}
