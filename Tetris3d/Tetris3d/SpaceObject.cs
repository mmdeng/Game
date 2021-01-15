using System;
using System.Collections.Generic;
using System.Text;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class SpaceObject
	{
		private object[, ,] _values;

		//==========================================================================
		/// <summary>�R���X�g���N�^</summary>
		public SpaceObject()
		{
			SetSize(0, 0, 0);
		}
		//==========================================================================
		/// <summary>�R���X�g���N�^</summary>
		/// <param name="nSize">�T�C�Y</param>
		public SpaceObject(Sn nSize)
		{
			SetSize(nSize);
		}
		//==========================================================================
		/// <summary>�R���X�g���N�^</summary>
		/// <param name="x">�T�C�Y</param>
		/// <param name="y">�T�C�Y</param>
		/// <param name="z">�T�C�Y</param>
		public SpaceObject(int x, int y, int z)
		{
			SetSize(x, y, z);
		}
		//========================================================================================
		/// <summary>�̈�m��</summary>
		/// <param name="nSize">�T�C�Y</param>
		/// <returns>��O</returns>
		public Exception SetSize(Sn size)
		{
			return SetSize(size.X, size.Y, size.Z);
		}
		//========================================================================================
		/// <summary>�̈�m��</summary>
		/// <param name="x">�T�C�Y</param>
		/// <param name="y">�T�C�Y</param>
		/// <param name="z">�T�C�Y</param>
		/// <returns>��O</returns>
		public Exception SetSize(int x, int y, int z)
		{
			try
			{
				_values = new object[x, y, z];
				return null;
			}
			catch (Exception ex)
			{
				ex.Data.Add("x", x);
				ex.Data.Add("y", y);
				ex.Data.Add("z", z);
				return ex;
			}
		}
		//==========================================================================
		/// <summary>XYZ�����f�[�^���̎擾</summary>
		/// <remarks></remarks>
		/// <value></value>
		public Sn Count
		{
			get
			{
				return new Sn(this.CountX, this.CountY, this.CountZ);
			}
		}
		//==========================================================================
		/// <summary>X�����f�[�^���̎擾</summary>
		public int CountX
		{
			get
			{
				return _values.GetLength(0);
			}
		}
		//==========================================================================
		/// <summary>Y�����f�[�^���̎擾</summary>
		public int CountY
		{
			get
			{
				return _values.GetLength(1);
			}
		}
		//==========================================================================
		/// <summary>Z�����f�[�^���̎擾</summary>
		public int CountZ
		{
			get
			{
				return _values.GetLength(2);
			}
		}
		//==========================================================================
		/// <summary>�C���f�N�T</summary>
		/// <param name="x">�C���f�b�N�X</param>
		/// <param name="y">�C���f�b�N�X</param>
		/// <param name="z">�C���f�b�N�X</param>
		/// <returns>�l</returns>
		public object this[int x, int y, int z]
		{
			get
			{
				return _values[x, y, z];
			}
			set
			{
				_values[x, y, z] = value;
			}
		}
		//==========================================================================
		/// <summary>�C���f�N�T</summary>
		/// <remarks></remarks>
		/// <param name="index">�C���f�b�N�X</param>
		/// <returns>�l</returns>
		public object this[Sn index]
		{
			get
			{
				return this[index.X, index.Y, index.Z];
			}
			set
			{
				this[index.X, index.Y, index.Z] = value;
			}
		}
		//==========================================================================
		/// <summary>���S�C���f�b�N�X�擾</summary>
		/// <returns>���S�C���f�b�N�X</returns>
		public Sn GetCenterIndex()
		{
			return new Sn(this.CountX / 2, this.CountY / 2, this.CountZ / 2);
		}
		//==========================================================================
		/// <summary>�S�Ă̒l��ݒ�</summary>
		/// <remarks></remarks>
		/// <param name="value">�ݒ�l</param>
		public void SetAll(object value)
		{
			for (int x = 0; x < this.CountX; x++)
			{
				for (int y = 0; y < this.CountY; y++)
				{
					for (int z = 0; z < this.CountZ; z++)
					{
						this[x, y, z] = value;
					}
				}
			}
		}
		public virtual SpaceObject Clone()
		{
			SpaceObject clone = (SpaceObject)this.MemberwiseClone();
			return clone;
		}
	}
}
