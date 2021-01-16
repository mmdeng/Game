using System;

namespace Tetris
{
	public class ProcBlock
	{
		private readonly Data data;

		/// <summary>
		/// 左移動スピード
		/// </summary>
		private int _moveLeftCount;
		/// <summary>
		/// 右移動スピード
		/// </summary>
		private int _moveRightCount;
		/// <summary>
		/// 強制落下スピード
		/// </summary>
		private int _forcedFallCount;
		/// <summary>
		/// 自然落下スピード
		/// </summary>
		private int _naturalFallCount;

		/// <summary>
		/// Constructor
		/// </summary>
		public ProcBlock(Data data)
		{
			this.data = data;

			_moveLeftCount = 0;
			_moveRightCount = 0;

			_forcedFallCount = 0;
			_naturalFallCount = 0;
		}
		/// <summary>
		/// 次の落下ブロックを作成
		/// </summary>
		public void CreateNextBlock()
		{
			// Nextブロックのインスタンスを現在のインスタンスとする。
			data.CurrentBlock = data.NextBlock;

			// 次のブロックタイプを乱数で決める
			var rand = new Random();
			var type = rand.Next(0, FallingBlock.BLOCKTYPES);

			//Nextブロックのインスタンス作成
			data.NextBlock = new FallingBlock(type);

			// ランダムな回数回転
			var rotate = rand.Next(4);
			for (var i = 0; i < rotate; i++)
			{
				data.NextBlock = data.NextBlock.Rotate();
			}

			if (data.CurrentBlock != null)
			{
				// 一番上の真中から落ちてくる。
				data.CurrentBlock.X = (data.X_MAX - data.CurrentBlock.Width) / 2;
				data.CurrentBlock.Y = 0;

				// もしフィールドのブロックに接触したらゲームオーバー
				if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X, data.CurrentBlock.Y))
				{
					data.stateApp = GameStatus.Over;
				}
			}
		}
		/// <summary>
		/// Block情報を更新
		/// </summary>
		public void UpdateBlock()
		{
			if (data.FlashingCount > 0)
			{
				FlashBlock();
			}
			else
			{
				MoveBlock();
			}
		}
		/// <summary>
		/// ブロックをフラッシュ
		/// </summary>
		private void FlashBlock()
		{
			data.FlashingCount--;
			if (data.FlashingCount > 0) return; // フラッシュ中。この間ブロックは動かない。

			// フラッシュ終了
			data.FlashingCount = 0;

			// 消した行数をｶｳﾝﾄしてスコアに反映させる。
			var erasedLines = 0;
			for (var y = data.Y_MAX - 1; y >= 0; y--)
			{
				if (data.FIELD_BLOCK[0, y] == FallingBlock.COLOR_INVALID)
				{
					// 行を消去して下に詰める
					data.FIELD_BLOCK.EraseLine(y);

					// 消去した行
					erasedLines++;

					// 消した行を再チェック
					y++;
				}
			}
			// スコア計算
			CalculateScore(erasedLines);

			// 次のブロックを作成し設定
			CreateNextBlock();
		}
		/// <summary>
		/// ブロックを移動
		/// </summary>
		private void MoveBlock()
		{
			const int FORCED_FALL_SPEED = 2;
			const int NATURAL_FALL_SPEED = 8;

			if (data.KeyLeftPressed)
			{
				_moveLeftCount++;
				if (FORCED_FALL_SPEED < _moveLeftCount)
				{
					_moveLeftCount = 0;
					MoveLeft();
				}
			}
			if (data.KeyRightPressed)
			{
				_moveRightCount++;
				if (FORCED_FALL_SPEED < _moveRightCount)
				{
					_moveRightCount = 0;
					MoveRight();
				}
			}
			//ボタンを押しているかどうかに関わらず、1回は下に落ちる。
			var isLanded = false;
			if (data.KeyDownPressed)
			{
				// 強制落下
				_forcedFallCount++;
				if (FORCED_FALL_SPEED < _forcedFallCount)
				{
					_forcedFallCount = 0;

					isLanded = !MoveDown();
					if (!isLanded)
					{
						data.score.TotalScore += 1;  // 下に進めると1点入る
					}
				}
			}
			else
			{
				// 自然落下
				_naturalFallCount++;
				if (NATURAL_FALL_SPEED < _naturalFallCount)
				{
					_naturalFallCount = 0;

					// 落下させる
					isLanded = !MoveDown();
				}
			}
			// 着地処理
			if (isLanded) Landed();
		}
		/// <summary>
		/// 着地処理
		/// </summary>
		private void Landed()
		{
			// フィールドのブロックとする
			data.FIELD_BLOCK.Paste(data.CurrentBlock, data.CurrentBlock.X, data.CurrentBlock.Y);

			var flashingLines = 0;
			for (var y = 0; y < data.Y_MAX; y++)
			{
				// 行全体がブロックで埋め尽くされている場合
				if (data.FIELD_BLOCK.IsLineFilled(y))
				{
					//一行でも消せる行があったらフラッシュする。
					data.FlashingCount = 40;

					// 該当行のブロックをCOLOR_INVALIDで塗りつぶす
					data.FIELD_BLOCK.FillLine(y, FallingBlock.COLOR_INVALID);

					flashingLines++;
				}
			}
			switch (flashingLines)
			{
				case 0: Music.StartWav("Land.wav"); break;
				case 1: Music.StartWav("Flash1.wav"); break;
				case 2: Music.StartWav("Flash2.wav"); break;
				case 3: Music.StartWav("Flash3.wav"); break;
				case 4: Music.StartWav("Flash4.wav"); break;
				default: break;
			}

			// 消える行がなかった時は次のブロックを作成する。
			if (data.FlashingCount == 0)
			{
				CreateNextBlock();
			}
		}
		/// <summary>
		/// ブロックを左へ移動
		/// </summary>
		/// <returns>True:移動成功, False:移動失敗</returns>
		private bool MoveLeft()
		{
			// 壁に接していたら移動できない。
			if (data.CurrentBlock.X + data.CurrentBlock.LeftEdge().X <= 0)
			{
				return false;
			}

			// Fieldブロックと接している場合は移動できない。
			if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X - 1, data.CurrentBlock.Y))
			{
				return false;
			}

			//移動実行
			data.CurrentBlock.X--;
			return true;
		}
		/// <summary>
		/// ブロックを右へ移動
		/// </summary>
		/// <returns>True:移動成功, False:移動失敗</returns>
		private bool MoveRight()
		{
			// 壁に接していたら移動できない。
			if (data.X_MAX - 1 <= data.CurrentBlock.X + data.CurrentBlock.RightEdge().X)
			{
				return false;
			}

			// Fieldブロックと接している場合は移動できない。
			if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X + 1, data.CurrentBlock.Y))
			{
				return false;
			}
			//移動実行
			data.CurrentBlock.X++;
			return true;
		}
		/// <summary>
		/// ブロックを下へ移動
		/// </summary>
		/// <returns>True:移動成功, False:移動失敗</returns>
		private bool MoveDown()
		{
			// 壁に接していたら移動できない。
			if (data.Y_MAX - 1 <= data.CurrentBlock.Y + data.CurrentBlock.BottomEdge().Y)
			{
				return false;
			}

			// Fieldブロックと接している場合は移動できない。
			if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X, data.CurrentBlock.Y + 1))
			{
				return false;
			}

			data.CurrentBlock.Y++;
			return true;
		}
		/// <summary>
		/// 落下ブロックを回転する
		/// </summary>
		public void Rotate()
		{
			if (data.FlashingCount > 0) return;

			// 回転後のブロックを取得
			var clone = data.CurrentBlock.Rotate();

			// 壁からはみ出る場合は回転できない
			if (clone.X + clone.LeftEdge().X < 0) return;
			if (data.X_MAX <= data.CurrentBlock.X + clone.RightEdge().X) return;
			if (data.Y_MAX <= data.CurrentBlock.Y + clone.BottomEdge().Y) return;

			// Fieldブロックと重なり合った場合は回転できない
			if (data.FIELD_BLOCK.IsPiledUp(clone, clone.X, clone.Y))
			{
				return;
			}
			// 回転成功。回転後のインスタンスを設定
			data.CurrentBlock = clone;
		}
		/// <summary>
		/// 消した行数からスコア計算
		/// </summary>
		/// <param name="erasedLines">消した行数</param>
		private void CalculateScore(int erasedLines)
		{
			switch (erasedLines)
			{
				case 1: data.score.Single++; data.score.TotalScore += 100; break;
				case 2: data.score.Double++; data.score.TotalScore += 200; break;
				case 3: data.score.Triple++; data.score.TotalScore += 500; break;
				case 4: data.score.Tetris++; data.score.TotalScore += 1000; break;
			}
			data.score.Lines += erasedLines;
		}
	}
}
