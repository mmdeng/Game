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
		public FieldBlock( int width, int height ) : base( width, height, 0 )
		{
		}
			
		public FieldBlock( int width, int height, int nFill ) : base( width, height, nFill )
		{
		}

		//--------------------------------------------------------------------------------
		// ���O: Fill()
		// �T�v: �t�B�[���h�S�̂�C�ӂ̒l�Ŗ��߂�
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
		// ���O: Clear()
		// �T�v: �t�B�[���h�S�̂�0�Ŗ��߂�
		//--------------------------------------------------------------------------------
		public void Clear()
		{
			Initialize( 0 );
		}

		//--------------------------------------------------------------------------------
		// ���O: FillLine()
		// �T�v: �C�ӂ̍s��C�ӂ̒l�Ŗ��߂�
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
		// ���O: EraseLine()
		// �T�v: �C�ӂ̈�s���������� (�������s�������I�ɋl�߂�)
		//--------------------------------------------------------------------------------
		public void EraseLine( int y )
		{
			FillLine( y, 0 );

			// �������̍s�����ɋl�߂�
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
		// ���O: IsLineFilled()
		// �T�v: �C�ӂ̈�s���u���b�N�Ŗ��ߐs������Ă��邩��Ԃ�
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
		// ���O: IsLineEmpty()
		// �T�v: �w��s���󂩂ǂ�����Ԃ�
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
		// ���O: Paste()
		// �T�v: �C�ӂ̈ʒu�Ƀu���b�N�𒣂�t����
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
		// ���O: IsPiledUp()
		// �T�v: ��̃u���b�N���d�Ȃ邩
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
