using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class Configuration
	{
		public int BlockSize;
		public Sn BlockCount;
		public Color LineColor;

		public Configuration()
		{
			BlockSize = 1;
			BlockCount = new Sn(5, 5, 21);
			LineColor = Color.White;
		}
	}
}
