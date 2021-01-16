using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	/// <summary>
	/// 得点
	/// </summary>
	public struct Score
	{
		public int TotalScore;
		public int Lines;
		public int Single;
		public int Double;
		public int Triple;
		public int Tetris;

		public void Reset()
		{
			TotalScore = 0;
			Lines = 0;
			Single = 0;
			Double = 0;
			Triple = 0;
			Tetris = 0;
		}
	}
}
