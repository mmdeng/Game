//using System;
//using System.Collections.Generic;
//using System.Text;

//using System.Drawing;

//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;

//namespace Sigma.Mathematics.Graphic.Mdx.Tetris3d
//{
//    public enum BlockType
//    {
//        Black=0,
//        Yellow = 1,
//        Cyan = 2,
//        Blue = 3,
//        Orange = 4,
//        Magenta = 5,
//        Lime = 6,
//        Red = 7,
//        Gray = 8,
//    }
//    public class Block : SpaceObject
//    {
//        public Sn Position
//        {
//            get
//            {
//                return _sdPosition;
//            }
//            set
//            {
//                _sdPosition = value;
//            }
//        }
//        public VertexHexahedron Hexahedron
//        {
//            get
//            {
//                return _vertexHexahedron;
//            }
//            set
//            {
//                _vertexHexahedron = value;
//            }
//        }
//        public ObjectxList Objectxs
//        {
//            get
//            {
//                return _listObjectxs;
//            }
//        }
//        public BlockType Type
//        {
//            get
//            {
//                return _blockType;
//            }
//            set
//            {
//                _blockType = value;
//            }
//        }
//        private Sn _sdPosition;
//        private Sd _sdRotation;
//        private ObjectxList _listObjectxs;
//        private VertexHexahedron _vertexHexahedron;
//        private BlockType _blockType;

//        public Block()
//        {
//            _blockType = BlockType.Yellow;
//            _sdRotation = new Sd();
//            _sdPosition = new Sn();
//            _listObjectxs = new ObjectxList();
//        }
//        public override SpaceObject Clone()
//        {
//            Block clone = ( Block )base.Clone();
//            clone.Position = _sdPosition.Clone();
//            return clone;
//        }
//        public void Move( XYZ dimension, int value )
//        {
//            _sdPosition.AddValue( dimension, value );
//        }
//        public void Move( Sn value )
//        {
//            _sdPosition += value;
//        }
//        public void Move( XYZ dimension )
//        {
//            Move( dimension, 1 );
//        }
//        //public void Rotate( XYZ dimension, double value )
//        //{
//        //    _sdRotation.AddDimensionValue( dimension, value );
//        //}
//        //public void Rotate( Sn value )
//        //{
//        //    _sdRotation += value;
//        //}
//        //public void Rotate90( XYZ dimension )
//        //{
//        //    _sdRotation.AddDimensionValue( dimension, Math.PI / 2 );
//        //}
//        public void CreateSmaple( )
//        {
//            SetSize( 3, 3, 3 );
//            this[ 0, 0, 0 ] = true;
//            this[ 1, 1, 1 ] = true;
//            this[ 2, 2, 2 ] = true;
//        }
//        //public List<int> GetZBottomList()
//        //{
//        //    List<int> list = new List<int>();
//        //    foreach( ObjectxVertex objectx in _listObjectxs )
//        //    {
//        //        list.Add( (int)objectx.Position.Z );
//        //    }
//        //    return list;
//        //}
//        public void UpdateObjectx( double dBlockSize )
//        {
//            _listObjectxs = new ObjectxList();
//            Sn nCenterIndex = this.GetCenterIndex();
//            for( int x = 0; x < this.CountX; x++ )
//            {
//                for( int y = 0; y < this.CountY; y++ )
//                {
//                    for( int z = 0; z < this.CountZ; z++ )
//                    {
//                        if( this[ x, y, z ] == null ) continue;
//                        if( ( bool )this[ x, y, z ] == false ) continue;

//                        Sn nIndex = new Sn( x, y, z );
//                        Sd dIndexFromCenter = new Sd( nIndex - nCenterIndex );
//                        Sd dFromCenter = dIndexFromCenter * dBlockSize;

//                        ObjectxVertex hexaheron = new ObjectxVertex();
//                        hexaheron.SetAmbientDiffuse( Color.White );
//                        hexaheron.Buffer = _vertexHexahedron;
//                        hexaheron.Position = dFromCenter.GetInverse( XYZ.Z );
//                        _listObjectxs.Add( hexaheron );
//                    }
//                }
//            }
//        }
//        public Exception Draw( DirectxPallet pallet )
//        {
//            Exception error;
//            pallet.Transform.PushMove( pallet.Device, _sdPosition.GetInverse( XYZ.Z ) );
//            foreach( ObjectxVertex objectx in _listObjectxs )
//            {
//                Exception tmp = objectx.Draw( pallet );
//                if( tmp != null ) error = tmp;
//            }
//            pallet.Transform.Pop( pallet.Device );
//            return null;
//        }
//    }
//}




