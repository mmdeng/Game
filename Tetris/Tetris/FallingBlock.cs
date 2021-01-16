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

		protected int _nLocationX;
		/// <summary>
		/// �����u���b�N����̈ʒu
		/// </summary>
		public int X
		{
			get { return _nLocationX; }
			set { _nLocationX = value; }
		}

		protected int _nLocationY;
		/// <summary>
		/// �����u���b�N����̈ʒu
		/// </summary>
		public int Y
		{
			get { return _nLocationY; }
			set { _nLocationY = value; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		static FallingBlock()
		{
		}
		public FallingBlock(int type) : base(Create(type))
		{
			// ������
			_nLocationX = 0;
			_nLocationY = 0;
		}
		/// <summary>
		/// �����u���b�N���쐬
		/// </summary>
		/// <param name="nType">�u���b�N�^�C�v</param>
		/// <returns>�����u���b�N</returns>
		private static int[,] Create(int nType)
		{
			// �����Ȓl
			if (nType < 0 || BLOCKTYPES <= nType)
			{
				throw new ArgumentOutOfRangeException("type");
			}

			// �u���b�N��������
			int[,] block = null;

			switch (nType)
			{
				case 0:
					{
						block = new int[,] { { 1, 1 },
										 { 1, 1 } };
						break;
					}
				case 1:
					{
						block = new int[,] { { 0, 1, 0 },
										 { 1, 1, 0 },
										 { 0, 1, 0 } };
						break;
					}
				case 2:
					{
						block = new int[,] { { 0, 0, 0 },
										 { 1, 1, 1 },
										 { 0, 0, 1 } };
						break;
					}
				case 3:
					{
						block = new int[,] { { 0, 0, 1 },
										 { 1, 1, 1 },
										 { 0, 0, 0 } };
						break;
					}
				case 4:
					{
						block = new int[,] { { 0, 1, 1 },
										 { 1, 1, 0 },
										 { 0, 0, 0 } };
						break;
					}
				case 5:
					{
						block = new int[,] { { 0, 0, 0 },
										 { 1, 1, 0 },
										 { 0, 1, 1 } };
						break;
					}
				case 6:
					{
						block = new int[,] { { 0, 0, 1, 0 },
										 { 0, 0, 1, 0 },
										 { 0, 0, 1, 0 },
										 { 0, 0, 1, 0 } };
						break;
					}
			}

			if (block != null)
			{
				int x, y;
				for (x = 0; x < block.GetLength(0); x++)
				{
					for (y = 0; y < block.GetLength(1); y++)
					{
						if (block[x, y] != 0)
						{
							block[x, y] = nType + 1;
						}
					}
				}
			}
			return block;
		}
		/// <summary>
		/// �N���[���쐬(�����^�w���)
		/// </summary>
		/// <returns>�N���[��</returns>
		public FallingBlock Clone()
		{
			FallingBlock inst = (FallingBlock)this.MemberwiseClone();
			inst.SetBlockArray(inst.GetBlockArray());

			return inst;
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
			if (block == null) throw new ArgumentNullException("block");

			FallingBlock fbClone = block.Clone();

			int x, y;
			int[,] nRotated;

			nRotated = new int[fbClone.Width, fbClone.Height];

			// ��] (�E��])
			for (x = 0; x < fbClone.Width; x++)
			{
				for (y = 0; y < fbClone.Height; y++)
				{
					nRotated[x, y] = fbClone[fbClone.Width - 1 - y, x];

					// �E��]�̏ꍇ
					//	nRotated[x, y] = fbClone[ y, fbClone.Width - 1 - x ];
				}
			}

			fbClone.SetBlockArray(nRotated);

			return fbClone;
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
			Point pt = new Point(-1, -1);

			for (int x = 0; x < this.Width; x++)
			{
				for (int y = 0; y < this.Height; y++)
				{
					if (this[x, y] != 0)
					{
						pt.X = x;
						pt.Y = y;
						break;
					}
				}
				if (pt.X != -1) break;
			}
			return pt;
		}
		/// <summary>
		/// �E : �����Ƃ��[�ɂ�����̂̈ʒu���W���擾
		/// </summary>
		/// <returns>�ʒu���W</returns>
		public Point RightEdge()
		{
			Point pt = new Point(-1, -1);

			for (int x = this.Width - 1; x >= 0; x--)
			{
				for (int y = 0; y < this.Height; y++)
				{
					if (this[x, y] != 0)
					{
						pt.X = x;
						pt.Y = y;
					}
				}
				if (pt.X != -1) break;
			}

			return pt;
		}
		/// <summary>
		/// �� : �����Ƃ��[�ɂ�����̂̈ʒu���W���擾
		/// </summary>
		/// <returns>�ʒu���W</returns>
		public Point TopEdge()
		{
			Point pt = new Point(-1, -1);

			for (int y = 0; y < this.Height; y++)
			{
				for (int x = 0; x < this.Width; x++)
				{
					if (this[x, y] != 0)
					{
						pt.X = x;
						pt.Y = y;
						break;
					}
				}
				if (pt.Y != -1) break;
			}
			return pt;
		}
		/// <summary>
		/// �� : �����Ƃ��[�ɂ�����̂̈ʒu���W���擾
		/// </summary>
		/// <returns>�ʒu���W</returns>
		public Point BottomEdge()
		{
			Point pt = new Point(-1, -1);

			for (int y = this.Height - 1; y >= 0; y--)
			{
				for (int x = 0; x < this.Width; x++)
				{
					if (this[x, y] != 0)
					{
						pt.X = x;
						pt.Y = y;
					}
				}
				if (pt.Y != -1) break;
			}
			return pt;
		}
	}
}
