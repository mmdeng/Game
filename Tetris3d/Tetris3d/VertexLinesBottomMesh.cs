using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class VertexLinesBottomMesh : VertexLines
	{
		public VertexLinesBottomMesh()
		{
		}
		public Exception CreateVertex(Device device, Configuration configuration)
		{
			Sn blockCount = configuration.BlockCount;
			double blockSize = configuration.BlockSize;

			_count = (blockCount.X + 1) + (blockCount.Y + 1);
			double blockHalf = blockSize / 2;
			double bottom = blockCount.Z / 2 + blockHalf;

			Pd min = new Pd(-blockCount.X / 2 - blockHalf, -blockCount.Y / 2 - blockHalf);
			Pd max = new Pd(+blockCount.X / 2 + blockHalf, +blockCount.Y / 2 + blockHalf);

			List<Sd2> list = new List<Sd2>();
			for (double x = min.X; x <= max.X; x += blockSize)
			{
				Sd2 line = new Sd2();
				line.P1.SetValue(x, min.Y, bottom);
				line.P2.SetValue(x, max.Y, bottom);
				list.Add(line);
			}
			for (double y = min.Y; y <= max.X; y += blockSize)
			{
				Sd2 line = new Sd2();
				line.P1.SetValue(min.X, y, bottom);
				line.P2.SetValue(max.X, y, bottom);
				list.Add(line);
			}
			return CreatePositionColoredVertex(device, list, configuration.LineColor);
		}
	}
}

