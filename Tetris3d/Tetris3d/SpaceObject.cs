using System;
using System.Collections.Generic;
using System.Text;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class SpaceObject
	{
		private object[, ,] _values;

		//==========================================================================
		/// <summary>コンストラクタ</summary>
		public SpaceObject()
		{
			SetSize(0, 0, 0);
		}
		//==========================================================================
		/// <summary>コンストラクタ</summary>
		/// <param name="nSize">サイズ</param>
		public SpaceObject(Sn nSize)
		{
			SetSize(nSize);
		}
		//==========================================================================
		/// <summary>コンストラクタ</summary>
		/// <param name="x">サイズ</param>
		/// <param name="y">サイズ</param>
		/// <param name="z">サイズ</param>
		public SpaceObject(int x, int y, int z)
		{
			SetSize(x, y, z);
		}
		//========================================================================================
		/// <summary>領域確保</summary>
		/// <param name="nSize">サイズ</param>
		/// <returns>例外</returns>
		public Exception SetSize(Sn size)
		{
			return SetSize(size.X, size.Y, size.Z);
		}
		//========================================================================================
		/// <summary>領域確保</summary>
		/// <param name="x">サイズ</param>
		/// <param name="y">サイズ</param>
		/// <param name="z">サイズ</param>
		/// <returns>例外</returns>
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
		/// <summary>XYZ方向データ数の取得</summary>
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
		/// <summary>X方向データ数の取得</summary>
		public int CountX
		{
			get
			{
				return _values.GetLength(0);
			}
		}
		//==========================================================================
		/// <summary>Y方向データ数の取得</summary>
		public int CountY
		{
			get
			{
				return _values.GetLength(1);
			}
		}
		//==========================================================================
		/// <summary>Z方向データ数の取得</summary>
		public int CountZ
		{
			get
			{
				return _values.GetLength(2);
			}
		}
		//==========================================================================
		/// <summary>インデクサ</summary>
		/// <param name="x">インデックス</param>
		/// <param name="y">インデックス</param>
		/// <param name="z">インデックス</param>
		/// <returns>値</returns>
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
		/// <summary>インデクサ</summary>
		/// <remarks></remarks>
		/// <param name="index">インデックス</param>
		/// <returns>値</returns>
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
		/// <summary>中心インデックス取得</summary>
		/// <returns>中心インデックス</returns>
		public Sn GetCenterIndex()
		{
			return new Sn(this.CountX / 2, this.CountY / 2, this.CountZ / 2);
		}
		//==========================================================================
		/// <summary>全ての値を設定</summary>
		/// <remarks></remarks>
		/// <param name="value">設定値</param>
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
