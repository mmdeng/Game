using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class Block
	{
		public VertexHexahedron Hexahedron
		{
			get
			{
				return _vertexHexahedron;
			}
			set
			{
				_vertexHexahedron = value;
			}
		}
		public ObjectxList Objectxs
		{
			get
			{
				return _objectxs;
			}
		}
		public BlockType Type
		{
			get
			{
				return _blockType;
			}
			set
			{
				_blockType = value;
			}
		}
		private ObjectxList _objectxs;
		private VertexHexahedron _vertexHexahedron;
		private BlockType _blockType;

		public Block()
		{
			_blockType = BlockType.Yellow;
			_objectxs = new ObjectxList();
		}
		public Block(BlockType type)
		{
			_blockType = type;
			_objectxs = new ObjectxList();
		}
		public void Rotate(XYZ axis)
		{
			MatrixSquare matrix = new MatrixSquare();
			matrix.SetRotation3D(axis, Math.PI / 2);

			Sn nCenter = new Sn(_objectxs[0].Position);
			_objectxs = _objectxs.GetRotated(matrix, nCenter);
		}
		public Block Clone()
		{
			Block clone = (Block)this.MemberwiseClone();
			clone._objectxs = _objectxs.Clone();
			return clone;
		}
		public void Add(ObjectxVertex objectx)
		{
			_objectxs.Add(objectx);
		}
		public void Add(Sn position)
		{
			ObjectxVertex objectx = new ObjectxVertex();
			objectx.SetAmbientDiffuse(Color.White);
			objectx.Position = new Sd(position);
			this.Add(objectx);
		}
		public void Move(XYZ dimension, int value)
		{
			foreach (Objectx item in _objectxs)
			{
				item.Position.AddValue(dimension, value);
			}
		}
		public void Move(Sn value)
		{
			foreach (Objectx item in _objectxs)
			{
				item.Position += value;
			}
		}
		public void Move(XYZ dimension)
		{
			Move(dimension, 1);
		}
		public Exception Draw(DirectxPallet pallet)
		{
			Exception error;
			foreach (ObjectxVertex objectx in _objectxs)
			{
				objectx.Buffer = _vertexHexahedron;
				Exception tmp = objectx.Draw(pallet);
				if (tmp != null) error = tmp;
			}
			return null;
		}
	}
}
