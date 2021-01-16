using System;

namespace Tetris
{
	/// <summary>
	/// FieldBlock �̊T�v�̐����ł��B
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
		/// �t�B�[���h�S�̂�C�ӂ̒l�Ŗ��߂�
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
		/// �t�B�[���h�S�̂�0�Ŗ��߂�
		/// </summary>
		public void Clear()
		{
			Initialize(0);
		}
		/// <summary>
		/// �C�ӂ̍s��C�ӂ̒l�Ŗ��߂�
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
		/// �C�ӂ̈�s���������� (�������s�������I�ɋl�߂�)
		/// </summary>
		/// <param name="y"></param>
		public void EraseLine(int y)
		{
			FillLine(y, 0);

			// �������̍s�����ɋl�߂�
			for (var i = y; i >= 1; i--)
			{
				for (var x = 0; x < _width; x++)
				{
					this[x, i] = this[x, i - 1];
				}
			}
		}
		/// <summary>
		/// �C�ӂ̈�s���u���b�N�Ŗ��ߐs������Ă��邩��Ԃ�
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
		/// �w��s���󂩂ǂ�����Ԃ�
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
		/// �C�ӂ̈ʒu�Ƀu���b�N�𒣂�t����
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
		/// ��̃u���b�N���d�Ȃ邩
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
