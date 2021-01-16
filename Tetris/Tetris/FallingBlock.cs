using System;
using System.Drawing;

namespace Tetris
{
	public class FallingBlock : Block, ICloneable
	{
		/// <summary>
		/// �u���b�N�^�C�v��
		/// </summary>
		public const int BLOCKTYPES = 7;
		/// <summary>
		/// �����u���b�N�̐F
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
			// ������
			X = 0;
			Y = 0;
		}
		/// <summary>
		/// �����u���b�N���쐬
		/// </summary>
		/// <param name="type">�u���b�N�^�C�v</param>
		/// <returns>�����u���b�N</returns>
		private static int[,] Create(int type)
		{
			// �����Ȓl
			if (type < 0 || BLOCKTYPES <= type)
			{
				throw new ArgumentOutOfRangeException(nameof(type));
			}

			// �u���b�N��������
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
		/// �N���[���쐬(�����^�w���)
		/// </summary>
		/// <returns>�N���[��</returns>
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
		/// �u���b�N����]������
		/// </summary>
		/// <param name="block">��]�O�̃u���b�N</param>
		/// <returns>��]��̃u���b�N(�N���[��)</returns>
		public static FallingBlock Rotate(FallingBlock block)
		{
			if (block == null) throw new ArgumentNullException(nameof(block));

			var clone = block.Clone();

			int x, y;
			var rotated = new int[clone.Width, clone.Height];

			// ��] (�E��])
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
		/// �� : �����Ƃ��[�ɂ�����̂̈ʒu���W���擾
		/// </summary>
		/// <returns>�ʒu���W</returns>
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
		/// �E : �����Ƃ��[�ɂ�����̂̈ʒu���W���擾
		/// </summary>
		/// <returns>�ʒu���W</returns>
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
		/// �� : �����Ƃ��[�ɂ�����̂̈ʒu���W���擾
		/// </summary>
		/// <returns>�ʒu���W</returns>
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
		/// �� : �����Ƃ��[�ɂ�����̂̈ʒu���W���擾
		/// </summary>
		/// <returns>�ʒu���W</returns>
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
