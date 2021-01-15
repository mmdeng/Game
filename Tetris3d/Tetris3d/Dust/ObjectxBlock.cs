//using System;
//using System.Collections.Generic;
//using System.Text;

//using System.Drawing;

//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;

//namespace Sigma.Mathematics.Graphic.Mdx.Tetris3d
//{
//    public class ObjectxBlock : ObjectxVertexBuffer
//    {
//        private Color _color;
//        private Block _block;
//        private List<ObjectxHexahedron> _list;

//        public ObjectxBlock()
//        {
//            _block = new Block();
//            _block.CreateSmaple();

//        }
//        public Exception CreateBuffer( Device device, Color color, double dBlockSize )
//        {
//            _list = new List<ObjectxHexahedron>();

//            for( int x = 0; x < _block.CountX; x++ )
//            {
//                for( int y = 0; y < _block.CountY; y++ )
//                {
//                    for( int z = 0; z < _block.CountZ; z++ )
//                    {
//                        if( _block[ x, y, z ] == false ) continue;

//                        //Sn nIndex=new Sn(x,y,z);
//                        //Sn nCenterIndex=_block.GetCenterIndex();
//                        //Sd sd = 
//                        //ObjectxHexahedron hexaheron = new ObjectxHexahedron();
//                        //hexaheron.SetPositionNormal(device, _sdPosition, 
//                    }
//                }
//            }
//            return null;
//        }
//    }
//}
