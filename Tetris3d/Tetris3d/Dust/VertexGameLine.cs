//using System;
//using System.Collections.Generic;
//using System.Text;

//using System.Drawing;

//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;

//namespace Sigma.Mathematics.Graphic.Mdx.Tetris3d
//{
//    public class VertexGameLine : VertexLines
//    {
//        public VertexGameLine()
//        {
//            _format = CustomVertex.PositionColored.Format;
//            _type = typeof( CustomVertex.PositionColored );
//            _premitiveType = PrimitiveType.LineList;
//        }
//        public Exception CreateVertex( Device device, Color color, Sn blockCount, float dBottom, float dBlockSize )
//        {
//            _nCount = ( blockCount.X + 1 ) + ( blockCount.Y + 1 ) + ( blockCount.X * blockCount.Y );

//            CustomVertex.PositionColored[] v = new CustomVertex.PositionColored[ _nCount * 2 ];

//            int n = 0;
//            Pn nMin = new Pn( -blockCount.X / 2 - dBlockSize / 2, -blockCount.Y / 2 - dBlockSize / 2 );
//            Pn nMax = new Pn( +blockCount.X / 2 + dBlockSize / 2, +blockCount.Y / 2 + dBlockSize / 2 );
//            for( float x = nMin.X; x <= nMax.X; x += dBlockSize )
//            {
//                v[ n ].Color = color.ToArgb();
//                v[ n ].X = x;
//                v[ n ].Y = nMin.Y;
//                v[ n ].Z = -dBottom;
//                n++;
//                v[ n ].Color = color.ToArgb();
//                v[ n ].X = x;
//                v[ n ].Y = nMax.Y;
//                v[ n ].Z = -dBottom;
//                n++;
//            }
//            for( float y = nMin.Y; y <= nMax.X; y += dBlockSize )
//            {
//                v[ n ].Color = color.ToArgb();
//                v[ n ].X = nMin.X;
//                v[ n ].Y = y;
//                v[ n ].Z = -dBottom;
//                n++;
//                v[ n ].Color = color.ToArgb();
//                v[ n ].X = nMax.X;
//                v[ n ].Y = y;
//                v[ n ].Z = -dBottom;
//                n++;
//            }
//            nMin = new Pn( -blockCount.X / 2, -blockCount.Y / 2 );
//            nMax = new Pn( +blockCount.X / 2, +blockCount.Y / 2 );
//            for( float x = nMin.X; x <= nMax.X; x += dBlockSize )
//            {
//                for( float y = nMin.Y; y <= nMax.X; y += dBlockSize )
//                {
//                    v[ n ].Color = color.ToArgb();
//                    v[ n ].X = x;
//                    v[ n ].Y = y;
//                    v[ n ].Z = -dBottom;
//                    n++;
//                    v[ n ].Color = color.ToArgb();
//                    v[ n ].X = x;
//                    v[ n ].Y = y;
//                    v[ n ].Z = +dBottom;
//                    n++;
//                }
//            }
//            return CreateVertex( device, v );
//        }
//    }
//}
