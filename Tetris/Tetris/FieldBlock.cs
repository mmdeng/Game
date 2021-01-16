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
		public FieldBlock( int width, int height ) : base( width, height, 0 )
		{
		}
			
		public FieldBlock( int width, int height, int nFill ) : base( width, height, nFill )
		{
		}

		//--------------------------------------------------------------------------------
		// 名前: Fill()
		// 概要: フィールド全体を任意の値で埋める
		//--------------------------------------------------------------------------------
		public void Initialize()
		{
			Initialize( 0 );
		}

		public void Initialize( int nFill )
		{
			for ( int y = 0; y < this.Height; y++ )
			{
				for ( int x = 0; x < this.Width; x++ )
				{
					this[x, y] = nFill;
				}
			}
		}

		//--------------------------------------------------------------------------------
		// 名前: Clear()
		// 概要: フィールド全体を0で埋める
		//--------------------------------------------------------------------------------
		public void Clear()
		{
			Initialize( 0 );
		}

		//--------------------------------------------------------------------------------
		// 名前: FillLine()
		// 概要: 任意の行を任意の値で埋める
		//--------------------------------------------------------------------------------
		public void FillLine( int y, int nFillValue )
		{
			if ( y < 0 || _nHeight <= y )
			{
				throw new ArgumentOutOfRangeException( "y" );
			}
			for( int x = 0; x < _nWidth; x++ )
			{
				this[ x, y ] = nFillValue;
			}
		}

		//--------------------------------------------------------------------------------
		// 名前: EraseLine()
		// 概要: 任意の一行を消去する (消えた行を自動的に詰める)
		//--------------------------------------------------------------------------------
		public void EraseLine( int y )
		{
			FillLine( y, 0 );

			// それより上の行を下に詰める
			int i;
			for ( i = y; i >= 1; i-- )
			{
				for ( int x = 0; x < _nWidth; x++ )
				{
					this[ x, i ] = this[ x, i - 1 ];
				}
			}
		}

		//--------------------------------------------------------------------------------
		// 名前: IsLineFilled()
		// 概要: 任意の一行がブロックで埋め尽くされているかを返す
		//--------------------------------------------------------------------------------
		public bool IsLineFilled( int y )
		{
			if ( y < 0 || _nHeight <= y )
			{
				throw new ArgumentOutOfRangeException( "y" );
			}
			for ( int x = 0; x < _nWidth; x++ )
			{
				if ( this[ x, y ] == 0 )
				{
					return false;
				}
			}
			return true;
		}

		//--------------------------------------------------------------------------------
		// 名前: IsLineEmpty()
		// 概要: 指定行が空かどうかを返す
		//--------------------------------------------------------------------------------
		public bool IsLineEmpty( int y )
		{
			if ( y < 0 || _nHeight <= y )
			{
				throw new ArgumentOutOfRangeException( "y" );
			}
			for( int x = 0; x < _nWidth; x++ )
			{
				if ( this[ x, y ] != 0 )
				{
					return false;
				}
			}
			return true;
		}

		//--------------------------------------------------------------------------------
		// 名前: Paste()
		// 概要: 任意の位置にブロックを張り付ける
		//--------------------------------------------------------------------------------
		public void Paste( Block block, int nPosX, int nPosY )
		{
			if ( block == null ) throw new ArgumentNullException( "block" );

			for( int x = 0; x < block.Width; x++ )
			{
				for( int y = 0; y < block.Height; y++ )
				{
					if ( block[ x, y ] != 0 )
					{
						this[ nPosX + x, nPosY + y ] = block[ x, y ];
					}
				}
			}
		}

		//--------------------------------------------------------------------------------
		// 名前: IsPiledUp()
		// 概要: 二つのブロックが重なるか
		//--------------------------------------------------------------------------------
		public bool IsPiledUp( Block block )
		{
			return IsPiledUp( this, block, 0, 0 );
		}
		public bool IsPiledUp( Block block, int osX, int osY )
		{
			return IsPiledUp( this, block, osX, osY ); 
		}
		public static bool IsPiledUp( Block block1, Block block2 )
		{
			return IsPiledUp( block1, block2, 0, 0 );
		}
		public static bool IsPiledUp( Block block1, Block block2, int osX, int osY )
		{
			if ( block1 == null ) throw new ArgumentNullException( "block1" );
			if ( block2 == null ) throw new ArgumentNullException( "block2" );

			int w = ( block1.Width  <= block2.Width  ? block1.Width  : block2.Width );
			int h = ( block1.Height <= block2.Height ? block1.Height : block2.Height );
			int x, y;

			for ( x = 0; x < w; x++ )
			{
				for ( y = 0; y < h; y++ )
				{
					if ( block1[ x + osX, y + osY ] != 0 && block2[x, y] != 0 )
					{
						return true;
					}
				}
			}
			return false;
		}

	}

}
