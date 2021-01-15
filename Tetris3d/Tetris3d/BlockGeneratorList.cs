using System;
using System.Collections.Generic;
using System.Text;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class BlockGeneratorList : List<BlockGenerator>
	{
		public int StockMax
		{
			get
			{
				return _stockMax;
			}
			set
			{
				_stockMax = value;
			}
		}
		public List<Block> Stocks
		{
			get
			{
				return _queueStock;
			}
		}

		private int _stockMax;
		private List<Block> _queueStock;

		public BlockGeneratorList()
		{
			_stockMax = 5;
			this.Add(new BlockGenerator1());
			this.Add(new BlockGenerator2());
			this.Add(new BlockGenerator3());
			this.Add(new BlockGenerator4());
			this.Add(new BlockGenerator5());

			_queueStock = new List<Block>();
			for (int i = 0; i < _stockMax; i++)
			{
				_queueStock.Add(this[0].Generate());
			}
		}
		public Block Generate(int nLevel)
		{
			_queueStock.Add(this[nLevel - 1].Generate());

			Block block = _queueStock[0];
			_queueStock.RemoveAt(0);
			return block.Clone();
		}
	}
}







