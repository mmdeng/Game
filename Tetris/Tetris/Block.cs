using System;

namespace Tetris
{
	/// <summary>
	/// Block の概要の説明です。
	/// </summary>
	public class Block
	{
		protected int[,] Blocks;

		protected int _width;
		protected int _height;

		public int Width
		{
			get { return _width; }
		}

		public int Height
		{
			get { return _height; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		public Block(int width, int height) : this(width, height, 0)
		{
		}
		public Block(int width, int height, int type)
		{
			if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
			if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

			this._width = width;
			this._height = height;

			Blocks = new int[width, height];

			// 初期化
			for (var y = 0; y < height; y++)
			{
				for (var x = 0; x < width; x++)
				{
					Blocks[x, y] = type;
				}
			}
		}

		public Block(int[,] blocks)
		{
			if (blocks == null) throw new ArgumentNullException(nameof(blocks));
			SetBlockArray(blocks);
		}
		/// <summary>
		/// インデクサ
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int this[int x, int y]
		{
			get
			{
				if (IsLocationInRange(x, y))
				{
					return Blocks[x, y];
				}
				return 0;
			}
			set
			{
				if (IsLocationInRange(x, y))
				{
					Blocks[x, y] = value;
				}
			}
		}
		/// <summary>
		/// ブロックの内部表記の複製を配列形式で返す
		/// </summary>
		/// <returns></returns>
		protected int[,] GetBlockArray()
		{
			return (int[,])Blocks.Clone();
		}
		/// <summary>
		/// ブロックの内部表記を設定する
		/// </summary>
		/// <param name="blocks"></param>
		protected void SetBlockArray(int[,] blocks)
		{
			if (blocks == null) throw new ArgumentNullException(nameof(blocks));

			Blocks = (int[,])blocks.Clone();
			_width = blocks.GetLength(0);
			_height = blocks.GetLength(1);
		}

		/// <summary>
		/// 位置座標が範囲内にあるかをチェックする
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private bool IsLocationInRange(int x, int y)
		{
			return (0 <= x && x < _width) && (0 <= y && y < _height);
		}
	}
}
