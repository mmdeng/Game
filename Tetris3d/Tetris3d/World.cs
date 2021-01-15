using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class World : SpaceObject
	{
		public VertexCubeMasterList Hexahedrons
		{
			get
			{
				return _vertexHexahedrons;
			}
			set
			{
				_vertexHexahedrons = value;
			}
		}
		private VertexCubeMasterList _vertexHexahedrons;

		public World()
		{
		}
		public void UpdateTargetBlock( Block block )
		{
			while( true )
			{
				bool bResult = CheckMove( block, XYZ.Z, -1 );
				if( bResult == false ) return;

				block.Move( XYZ.Z, -1 );
			}
		}
		public bool CheckBlock( Block block )
		{
			Sn nCenterIndex = this.GetCenterIndex();
			Sn snMin = new Sn();
			Sn snMax = this.Count - 1;
			foreach( ObjectxVertex objectx in block.Objectxs )
			{
				Sn nIndex = nCenterIndex + new Sn( objectx.Position );

				// 外枠チェック
				if( RangePoint.IsInRange( nIndex, snMin, snMax ) == false ) return false;

				// 既設ブロックチェック
				if( this[ nIndex ] != null ) return false;
			}
			return true;
		}
		public bool CheckMove( Block block, XYZ dimension, int nValue )
		{
			Block blockClone = block.Clone();
			blockClone.Move( dimension, nValue );
			return CheckBlock( blockClone );
		}
		public bool CheckRotate( Block block, XYZ dimension )
		{
			Block blockClone = block.Clone();
			blockClone.Rotate( dimension );
			return CheckBlock( blockClone );
		}
		public void BindBlock( Block block )
		{
			Sn nCenterIndex = this.GetCenterIndex();
			foreach( ObjectxVertex objectx in block.Objectxs )
			{
				Sn nIndex = nCenterIndex + new Sn( objectx.Position );

				VertexCube vertexCube = new VertexCube();
				vertexCube.Type = block.Type;
				vertexCube.Cube = ( ObjectxVertex )objectx.Clone();
				this[ nIndex ] = vertexCube;
			}
		}
		public List<int> GetCompletePlains()
		{
			List<int> list = new List<int>();
			for( int z = this.CountZ - 1; z >= 0; z-- )
			{
				bool bComplete = true;
				for( int x = 0; x < this.CountX; x++ )
				{
					for( int y = 0; y < this.CountY; y++ )
					{
						if( this[ x, y, z ] == null )
						{
							bComplete = false;
							break;
						}
					}
					if( bComplete == false ) break;
				}
				if( bComplete == true )
				{
					list.Add( z );
				}
			}
			if( list.Count <= 0 ) return null;

			return list;
		}
		public void FlashCompletePlains( List<int> list, int nStage )
		{
			foreach( int zComplete in list )
			{
				for( int x = 0; x < this.CountX; x++ )
				{
					for( int y = 0; y < this.CountY; y++ )
					{
						VertexCube vertexCube = this[ x, y, zComplete ] as VertexCube;
						if( vertexCube == null ) continue;

						if( nStage % 2 == 0 )
						{
							vertexCube.Cube.Buffer = _vertexHexahedrons[ ( int )BlockType.Gray ];
						}
						else
						{
							vertexCube.Cube.Buffer = _vertexHexahedrons[ ( int )vertexCube.Type ];
						}
					}
				}
			}
		}
		public void RemoveCompletePlains( List<int> list )
		{
			foreach( int zComplete in list )
			{
				for( int z = zComplete; z < this.CountZ - 1; z++ )
				{
					for( int x = 0; x < this.CountX; x++ )
					{
						for( int y = 0; y < this.CountY; y++ )
						{
							VertexCube vertexCube = this[ x, y, z + 1 ] as VertexCube;
							this[ x, y, z + 1 ] = null;

							if( vertexCube != null )
							{
								vertexCube.Cube.Position.Z--;
								vertexCube.Cube.Buffer = _vertexHexahedrons[ ( int )vertexCube.Type ];
							}
							this[ x, y, z ] = vertexCube;
						}
					}
				}
			}
		}
		public Exception Draw( DirectxPallet pallet )
		{
			for( int x = 0; x < this.CountX; x++ )
			{
				for( int y = 0; y < this.CountY; y++ )
				{
					for( int z = 0; z < this.CountZ; z++ )
					{
						VertexCube item = this[ x, y, z ] as VertexCube;
						if( item == null ) continue;

						item.Cube.Draw( pallet );
					}
				}
			}
			return null;
		}
	}
}
