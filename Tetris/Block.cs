using System;

namespace Tetris
{
	/// <summary>
	/// Block �̊T�v�̐����ł��B
	/// </summary>
	public class Block
	{
		protected int [,] _Block;

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

		//--------------------------------------------------------------------------------
		// ���O: Block()
		// �T�v: �R���X�g���N�^
		//--------------------------------------------------------------------------------
		public Block( int width, int height ) : this( width, height, 0 )
		{
		}
		public Block( int width, int height, int nKind )
		{
			if ( width <= 0 ) throw new ArgumentOutOfRangeException( "width" );
			if ( height <= 0 ) throw new ArgumentOutOfRangeException( "height" );

			this._nWidth = width;
			this._nHeight = height;

			_Block = new int[ width, height ];

			// ������
			for( int y = 0; y < height; y++ )
			{
				for ( int x = 0; x < width; x++ )
				{
					_Block[x, y] = nKind;
				}
			}
		}

		public Block( int[,] nBlock )
		{
			if ( nBlock == null )
			{
				throw new ArgumentNullException( "block" );
			}
			this.SetBlockArray( nBlock );
		}

		//--------------------------------------------------------------------------------
		// ���O: this[,]
		// �T�v: �C���f�N�T�A�`����[x,y]
		//--------------------------------------------------------------------------------
		public int this[int x, int y]
		{
			get
			{
				if( IsLocationInRange( x, y ) )
				{
					return _Block[x, y];
				}
				return 0;
			}
			set
			{
				if( IsLocationInRange( x, y ) )
				{
					_Block[x, y] = value;
				}
			}
		}
		//--------------------------------------------------------------------------------
		// ���O: GetBlockArray
		// �T�v: �u���b�N�̓����\�L�̕�����z��`���ŕԂ�
		//--------------------------------------------------------------------------------
		protected int[,] GetBlockArray()
		{
			return (int[,])_Block.Clone();
		}

		//--------------------------------------------------------------------------------
		// ���O: GetBlockArray
		// �T�v: �u���b�N�̓����\�L��ݒ肷��
		//--------------------------------------------------------------------------------
		protected void SetBlockArray( int[,] nBlock )
		{
			if ( nBlock == null ) throw new ArgumentNullException( "block" );

			this._Block = (int[,])nBlock.Clone();
			_nWidth = nBlock.GetLength( 0 );
			_nHeight = nBlock.GetLength( 1 );
		}

		//--------------------------------------------------------------------------------
		// ���O: IsLocationInRange()
		// �T�v: �ʒu���W���͈͓��ɂ��邩���`�F�b�N����
		//--------------------------------------------------------------------------------
		private bool IsLocationInRange( int x, int y )
		{
			return ( 0 <= x && x < _nWidth ) && ( 0 <= y && y < _nHeight );
		}

		private bool IsLocationInRange( int x, int y, out bool xInRange, out bool yInRange )
		{
			xInRange = ( 0 <= x && x < _nWidth );
			yInRange = ( 0 <= y && y < _nHeight );

			return xInRange && yInRange;
		}

	}
}
