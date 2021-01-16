using System;

namespace Tetris
{
	/// <summary>
	/// Block の概要の説明です。
	/// </summary>
	public class Block
	{
		protected int[,] _Block;

		protected int _nWidth;
		protected int _nHeight;

		public int Width
		{
			get { return _nWidth; }
		}

		public int Height
		{
			get { return _nHeight; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		public Block(int width, int height) : this(width, height, 0)
		{
		}
		public Block(int width, int height, int nKind)
		{
			if (width <= 0) throw new ArgumentOutOfRangeException("width");
			if (height <= 0) throw new ArgumentOutOfRangeException("height");

			this._nWidth = width;
			this._nHeight = height;

			_Block = new int[width, height];

			// 初期化
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					_Block[x, y] = nKind;
				}
			}
		}

		public Block(int[,] nBlock)
		{
			if (nBlock == null)
			{
				throw new ArgumentNullException("block");
			}
			this.SetBlockArray(nBlock);
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
					return _Block[x, y];
				}
				return 0;
			}
			set
			{
				if (IsLocationInRange(x, y))
				{
					_Block[x, y] = value;
				}
			}
		}
		/// <summary>
		/// ブロックの内部表記の複製を配列形式で返す
		/// </summary>
		/// <returns></returns>
		protected int[,] GetBlockArray()
		{
			return (int[,])_Block.Clone();
		}
		/// <summary>
		/// ブロックの内部表記を設定する
		/// </summary>
		/// <param name="nBlock"></param>
		protected void SetBlockArray(int[,] nBlock)
		{
			if (nBlock == null) throw new ArgumentNullException("block");

			this._Block = (int[,])nBlock.Clone();
			_nWidth = nBlock.GetLength(0);
			_nHeight = nBlock.GetLength(1);
		}

		/// <summary>
		/// 位置座標が範囲内にあるかをチェックする
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private bool IsLocationInRange(int x, int y)
		{
			return (0 <= x && x < _nWidth) && (0 <= y && y < _nHeight);
		}
	}
}
