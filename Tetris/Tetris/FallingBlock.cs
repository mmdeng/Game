using System;
using System.Drawing;

namespace Tetris
{
	public class FallingBlock : Block, ICloneable
	{
		/// <summary>
		/// ブロックタイプ数
		/// </summary>
		public const int BLOCKTYPES = 7;
		/// <summary>
		/// 無効ブロックの色
		/// </summary>
		public const int COLOR_INVALID = 8; 

		public int X { get; set; }
		public int Y { get; set; }
		/// <summary>
		/// Constructor
		/// </summary>
		static FallingBlock()
		{
		}
		public FallingBlock(int type) : base(Create(type))
		{
			// 初期化
			X = 0;
			Y = 0;
		}
		/// <summary>
		/// 落下ブロックを作成
		/// </summary>
		/// <param name="type">ブロックタイプ</param>
		/// <returns>落下ブロック</returns>
		private static int[,] Create(int type)
		{
			// 無効な値
			if (type < 0 || BLOCKTYPES <= type)
			{
				throw new ArgumentOutOfRangeException(nameof(type));
			}

			// ブロックを初期化
			int[,] blocks = null;

			switch (type)
			{
				case 0:
					{
						blocks = new int[,] { { 1, 1 },
										 { 1, 1 } };
						break;
					}
				case 1:
					{
						blocks = new int[,] { { 0, 1, 0 },
										 { 1, 1, 0 },
										 { 0, 1, 0 } };
						break;
					}
				case 2:
					{
						blocks = new int[,] { { 0, 0, 0 },
										 { 1, 1, 1 },
										 { 0, 0, 1 } };
						break;
					}
				case 3:
					{
						blocks = new int[,] { { 0, 0, 1 },
										 { 1, 1, 1 },
										 { 0, 0, 0 } };
						break;
					}
				case 4:
					{
						blocks = new int[,] { { 0, 1, 1 },
										 { 1, 1, 0 },
										 { 0, 0, 0 } };
						break;
					}
				case 5:
					{
						blocks = new int[,] { { 0, 0, 0 },
										 { 1, 1, 0 },
										 { 0, 1, 1 } };
						break;
					}
				case 6:
					{
						blocks = new int[,] { { 0, 0, 1, 0 },
										 { 0, 0, 1, 0 },
										 { 0, 0, 1, 0 },
										 { 0, 0, 1, 0 } };
						break;
					}
			}

			if (blocks != null)
			{
				int x, y;
				for (x = 0; x < blocks.GetLength(0); x++)
				{
					for (y = 0; y < blocks.GetLength(1); y++)
					{
						if (blocks[x, y] != 0)
						{
							blocks[x, y] = type + 1;
						}
					}
				}
			}
			return blocks;
		}
		/// <summary>
		/// クローン作成(厳密型指定つき)
		/// </summary>
		/// <returns>クローン</returns>
		public FallingBlock Clone()
		{
			var clone = (FallingBlock)MemberwiseClone();
			clone.SetBlockArray(clone.GetBlockArray());

			return clone;
		}
		object ICloneable.Clone()
		{
			return this.Clone();
		}
		/// <summary>
		/// ブロックを回転させる
		/// </summary>
		/// <param name="block">回転前のブロック</param>
		/// <returns>回転後のブロック(クローン)</returns>
		public static FallingBlock Rotate(FallingBlock block)
		{
			if (block == null) throw new ArgumentNullException(nameof(block));

			var clone = block.Clone();

			int x, y;
			var rotated = new int[clone.Width, clone.Height];

			// 回転 (右回転)
			for (x = 0; x < clone.Width; x++)
			{
				for (y = 0; y < clone.Height; y++)
				{
					rotated[x, y] = clone[clone.Width - 1 - y, x];
				}
			}
			clone.SetBlockArray(rotated);
			return clone;
		}
		public FallingBlock Rotate()
		{
			return Rotate(this);
		}
		/// <summary>
		/// 左 : もっとも端にあるものの位置座標を取得
		/// </summary>
		/// <returns>位置座標</returns>
		public Point LeftEdge()
		{
			var point = new Point(-1, -1);
			for (var x = 0; x < Width; x++)
			{
				for (var y = 0; y < Height; y++)
				{
					if (this[x, y] != 0)
					{
						point.X = x;
						point.Y = y;
						break;
					}
				}
				if (point.X != -1) break;
			}
			return point;
		}
		/// <summary>
		/// 右 : もっとも端にあるものの位置座標を取得
		/// </summary>
		/// <returns>位置座標</returns>
		public Point RightEdge()
		{
			var point = new Point(-1, -1);
			for (var x = Width - 1; x >= 0; x--)
			{
				for (var y = 0; y < Height; y++)
				{
					if (this[x, y] != 0)
					{
						point.X = x;
						point.Y = y;
					}
				}
				if (point.X != -1) break;
			}
			return point;
		}
		/// <summary>
		/// 上 : もっとも端にあるものの位置座標を取得
		/// </summary>
		/// <returns>位置座標</returns>
		public Point TopEdge()
		{
			var point = new Point(-1, -1);
			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					if (this[x, y] != 0)
					{
						point.X = x;
						point.Y = y;
						break;
					}
				}
				if (point.Y != -1) break;
			}
			return point;
		}
		/// <summary>
		/// 下 : もっとも端にあるものの位置座標を取得
		/// </summary>
		/// <returns>位置座標</returns>
		public Point BottomEdge()
		{
			var point = new Point(-1, -1);
			for (var y = Height - 1; y >= 0; y--)
			{
				for (var x = 0; x < Width; x++)
				{
					if (this[x, y] != 0)
					{
						point.X = x;
						point.Y = y;
					}
				}
				if (point.Y != -1) break;
			}
			return point;
		}
	}
}
