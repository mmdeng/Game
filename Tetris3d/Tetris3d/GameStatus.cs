using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class GameSystem
	{
		public int TotalScore
		{
			get
			{
				return _totalScore;
			}
			set
			{
				_totalScore = value;
			}
		}
		public int LineCount
		{
			get
			{
				return _lineCount;
			}
			set
			{
				_lineCount = value;
			}
		}
		public int Single
		{
			get
			{
				return _single;
			}
			set
			{
				_single = value;
			}
		}
		public int Double
		{
			get
			{
				return _double;
			}
			set
			{
				_double = value;
			}
		}
		public int Triple
		{
			get
			{
				return _triple;
			}
			set
			{
				_triple = value;
			}
		}
		public int Tetris
		{
			get
			{
				return _tetris;
			}
			set
			{
				_tetris = value;
			}
		}

		public StatusType Status
		{
			get
			{
				return _status;
			}
		}
		public int Level
		{
			get
			{
				return _level;
			}
			set
			{
				if (value < 1) return;
				if (5 < value) return;
				_level = value;
			}
		}

		private int _totalScore;
		private int _lineCount;
		private int _single;
		private int _double;
		private int _triple;
		private int _tetris;
		private int _level;
		private StatusType _status;

		public readonly Rectangle RECTANGLE_GAME;
		public readonly Rectangle RECTANGLE_SCORE;
		public readonly Rectangle RECTANGLE_MANUAL;

		public enum StatusType
		{
			Idling,
			Playing,
			Pausing,
			Over
		}
		public GameSystem()
		{
			_level = 1;
			RECTANGLE_GAME = new Rectangle(140, 5, 300, 750);
			RECTANGLE_SCORE = new Rectangle(5, 620, 126, 135);
			RECTANGLE_MANUAL = new Rectangle(5, 5, 126, 600);
			Reset();
		}
		public void Reset()
		{
			_totalScore = 0;
			_lineCount = 0;
			_single = 0;
			_double = 0;
			_triple = 0;
			_tetris = 0;
			_status = StatusType.Idling;
		}
		public void SetLineCount(int nLineCount)
		{
			switch (nLineCount)
			{
				case 1: _single++; _totalScore += 100; break;
				case 2: _double++; _totalScore += 200; break;
				case 3: _triple++; _totalScore += 500; break;
				case 4: _tetris++; _totalScore += 1000; break;
			}
			_lineCount += nLineCount;
		}
		public override string ToString()
		{
			return "Score : " + _totalScore + ", Lines : " + _lineCount;
		}
		public string GetStringStatus()
		{
			if (_status == StatusType.Idling) return "Press Enter key to start";
			else if (_status == StatusType.Over) return "Game over";
			else if (_status == StatusType.Pausing) return "Pausing";
			return null;
		}
		public string GetStringScore()
		{
			StringBuilder text = new StringBuilder();
			text.Append("Level  : ");
			text.AppendLine(_level.ToString());
			text.Append("Single : ");
			text.AppendLine(_single.ToString());
			text.Append("Double : ");
			text.AppendLine(_double.ToString());
			text.Append("Triple : ");
			text.AppendLine(_triple.ToString());
			text.Append("Tetris : ");
			text.AppendLine(_tetris.ToString());
			text.Append("Total  : ");
			text.AppendLine(_totalScore.ToString());
			return text.ToString();
		}
		public string GetStringManual()
		{
			StringBuilder text = new StringBuilder();
			text.AppendLine("¨ : ");
			text.AppendLine("Move on X axis");
			text.AppendLine();
			text.AppendLine("© : ");
			text.AppendLine("Move on X axis");
			text.AppendLine();
			text.AppendLine("ª : ");
			text.AppendLine("Move on Y axis");
			text.AppendLine();
			text.AppendLine("« : ");
			text.AppendLine("Move on Y axis");
			text.AppendLine();
			text.AppendLine("Space : ");
			text.AppendLine("Down");
			text.AppendLine();
			text.AppendLine();
			text.AppendLine("Z : ");
			text.AppendLine("Rotate Z axis");
			text.AppendLine();
			text.AppendLine("X : ");
			text.AppendLine("Rotate X axis");
			text.AppendLine();
			text.AppendLine("C : ");
			text.AppendLine("Rotate C axis");
			text.AppendLine();
			text.AppendLine();
			text.AppendLine("P : ");
			text.AppendLine("Pause / Resume");
			text.AppendLine();
			text.AppendLine("M : ");
			text.AppendLine("Change BGM");
			text.AppendLine();
			text.AppendLine("Mouse Drag : ");
			text.AppendLine("Move Camera");
			text.AppendLine();
			text.AppendLine("Num pad : ");
			text.AppendLine("1 : Level 1");
			text.AppendLine("2 : Level 2");
			text.AppendLine("3 : Level 3");
			text.AppendLine("4 : Level 4");
			text.AppendLine("5 : Level 5");
			text.AppendLine();
			return text.ToString();
		}
		public void TogglePause()
		{
			if (_status == StatusType.Playing)
			{
				_status = StatusType.Pausing;
			}
			else if (_status == StatusType.Pausing)
			{
				_status = StatusType.Playing;
			}
		}
		public void SetStatus(StatusType status)
		{
			if (_status == StatusType.Idling)
			{
				if (status == StatusType.Playing)
				{
					_status = StatusType.Playing;
				}
			}
			else if (_status == StatusType.Playing)
			{
				if (status == StatusType.Pausing)
				{
					_status = StatusType.Pausing;
				}
				else if (status == StatusType.Over)
				{
					_status = StatusType.Over;
				}
			}
			else if (_status == StatusType.Over)
			{
				if (status == StatusType.Playing)
				{
					_status = StatusType.Playing;
				}
			}
		}
	}
}
