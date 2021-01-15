using System;
using System.Collections.Generic;
using System.Text;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public abstract class BlockGenerator
	{
		private RangedRandom _random;
		protected List<Block> _blocks;

		public BlockGenerator()
		{
			_blocks = new List<Block>();
		}
		public Block Generate()
		{
			if (_random == null)
			{
				_random = new RangedRandom(0, _blocks.Count - 1);
			}
			if (_random.ValueRange.Max != _blocks.Count - 1)
			{
				_random.ValueRange.Max = _blocks.Count - 1;
			}
			int nIndex = _random.NextInt();
			Block block = _blocks[nIndex];
			return (Block)block.Clone();
		}
	}
	public class BlockGenerator0 : BlockGenerator
	{
		public BlockGenerator0()
		{
			Block block;
			block = new Block(BlockType.Magenta);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(1, 1, 8));
			_blocks.Add(block);
		}

	}
	public class BlockGenerator1 : BlockGenerator
	{
		public BlockGenerator1()
		{
			Block block;
			block = new Block(BlockType.Red);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Cyan);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Magenta);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(1, 1, 8));
			_blocks.Add(block);

		}
	}
	public class BlockGenerator2 : BlockGenerator
	{
		public BlockGenerator2()
		{
			Block block;
			block = new Block(BlockType.Red);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(0, -1, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Cyan);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Magenta);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(1, 1, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Lime);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(-1, 0, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Yellow);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(-1, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 1, 8));
			_blocks.Add(block);


		}
	}
	public class BlockGenerator3 : BlockGenerator
	{
		public BlockGenerator3()
		{
			Block block;
			block = new Block(BlockType.Red);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(0, -1, 8));
			block.Add(new Sn(0, 2, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Magenta);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(1, 1, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Lime);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(-1, 0, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Yellow);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(-1, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 1, 8));
			_blocks.Add(block);

			block = new Block(BlockType.Cyan);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(0, -1, 8));
			block.Add(new Sn(1, 1, 8));
			_blocks.Add(block);
		}
	}
	public class BlockGenerator4 : BlockGenerator3
	{
		public BlockGenerator4()
		{
			Block block;
			block = new Block(BlockType.Orange);
			block.Add(new Sn(0, 0, 8));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(0, 0, 9));
			_blocks.Add(block);

		}
	}
	public class BlockGenerator5 : BlockGenerator4
	{
		public BlockGenerator5()
		{
			Block block;
			block = new Block(BlockType.Blue);
			block.Add(new Sn(0, 0, 9));
			block.Add(new Sn(0, 1, 8));
			block.Add(new Sn(1, 0, 8));
			block.Add(new Sn(0, -1, 8));
			_blocks.Add(block);

		}
	}
}



