using System;

namespace Tetris
{
	/// <summary>
	/// FieldBlock の概要の説明です。
	/// </summary>
	public class FieldBlock : Block
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FieldBlock(int width, int height) : base(width, height, 0)
		{
		}

		public FieldBlock(int width, int height, int fill) : base(width, height, fill)
		{
		}
		/// <summary>
		/// フィールド全体を任意の値で埋める
		/// </summary>
		public void Initialize()
		{
			Initialize(0);
		}

		public void Initialize(int fill)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					this[x, y] = fill;
				}
			}
		}
		/// <summary>
		/// フィールド全体を0で埋める
		/// </summary>
		public void Clear()
		{
			Initialize(0);
		}
		/// <summary>
		/// 任意の行を任意の値で埋める
		/// </summary>
		/// <param name="y"></param>
		/// <param name="value"></param>
		public void FillLine(int y, int value)
		{
			if (y < 0 || _height <= y)
			{
				throw new ArgumentOutOfRangeException(nameof(y));
			}
			for (int x = 0; x < _width; x++)
			{
				this[x, y] = value;
			}
		}
		/// <summary>
		/// 任意の一行を消去する (消えた行を自動的に詰める)
		/// </summary>
		/// <param name="y"></param>
		public void EraseLine(int y)
		{
			FillLine(y, 0);

			// それより上の行を下に詰める
			for (var i = y; i >= 1; i--)
			{
				for (var x = 0; x < _width; x++)
				{
					this[x, i] = this[x, i - 1];
				}
			}
		}
		/// <summary>
		/// 任意の一行がブロックで埋め尽くされているかを返す
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsLineFilled(int y)
		{
			if (y < 0 || _height <= y)
			{
				throw new ArgumentOutOfRangeException(nameof(y));
			}
			for (var x = 0; x < _width; x++)
			{
				if (this[x, y] == 0)
				{
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// 指定行が空かどうかを返す
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsLineEmpty(int y)
		{
			if (y < 0 || _height <= y)
			{
				throw new ArgumentOutOfRangeException(nameof(y));
			}
			for (var x = 0; x < _width; x++)
			{
				if (this[x, y] != 0)
				{
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// 任意の位置にブロックを張り付ける
		/// </summary>
		/// <param name="block"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Paste(Block block, int x, int y)
		{
			if (block == null) throw new ArgumentNullException(nameof(block));

			for (var nx = 0; nx < block.Width; nx++)
			{
				for (var ny = 0; ny < block.Height; ny++)
				{
					if (block[nx, ny] != 0)
					{
						this[x + nx, y + ny] = block[nx, ny];
					}
				}
			}
		}
		/// <summary>
		/// 二つのブロックが重なるか
		/// </summary>
		/// <param name="block"></param>
		/// <returns></returns>
		public bool IsPiledUp(Block block)
		{
			return IsPiledUp(this, block, 0, 0);
		}
		public bool IsPiledUp(Block block, int osX, int osY)
		{
			return IsPiledUp(this, block, osX, osY);
		}
		public static bool IsPiledUp(Block block1, Block block2)
		{
			return IsPiledUp(block1, block2, 0, 0);
		}
		public static bool IsPiledUp(Block block1, Block block2, int osX, int osY)
		{
			if (block1 == null) throw new ArgumentNullException(nameof(block1));
			if (block2 == null) throw new ArgumentNullException(nameof(block2));

			var w = block1.Width <= block2.Width ? block1.Width : block2.Width;
			var h = block1.Height <= block2.Height ? block1.Height : block2.Height;
			for (var x = 0; x < w; x++)
			{
				for (var y = 0; y < h; y++)
				{
					if (block1[x + osX, y + osY] != 0 && block2[x, y] != 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
