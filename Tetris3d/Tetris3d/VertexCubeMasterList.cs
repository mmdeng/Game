using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class VertexCubeMasterList : List<VertexHexahedronTexture>
	{
		public Exception LoadFile(Device device, string path)
		{
			BitmapProvider bitmap = new BitmapProvider();
			Exception error = bitmap.LoadFile(path);
			if (error != null) return error;

			this.Clear();

			int size = bitmap.Image.Height;
			Rectangle rectangle;
			for (int i = 0; i < 9; i++)
			{
				BitmapTexture bitmapCube = new BitmapTexture();
				rectangle = new Rectangle(i * size, 0, size, size);
				error = bitmap.GetPart(rectangle, bitmapCube);
				if (error != null) return error;

				error = bitmapCube.CreateTexture(device);
				if (error != null) return error;

				VertexHexahedronTexture item = new VertexHexahedronTexture();
				item.Texture = bitmapCube.Texture;
				this.Add(item);
			}
			return null;
		}
		//==========================================================================
		/// <summary>頂点作成</summary>
		/// <remarks></remarks>
		/// <param name="device">デバイス</param>
		/// <param name="size">サイズ</param>
		/// <returns>例外</returns>
		public Exception SetPositionNormalTextured(Device device, double size)
		{
			Sd sizeData = new Sd(size, size, size);
			foreach (VertexHexahedronTexture item in this)
			{
				Exception error = item.SetPositionNormalTextured(device, sizeData);
				if (error != null) return error;
			}
			return null;
		}
	}
}
