using System;

namespace Tetris
{
	public class ProcBlock
	{
		private Data dat;

		private int _nMoveLeftCount;        // 左移動スピード
		private int _nMoveRightCount;       // 右移動スピード
		private int _nForcedFallCount;      // 強制落下スピード
		private int _nNaturalFallCount;     // 自然落下スピード

		private FormMain FORM_MAIN;

		/// <summary>
		/// Constructor
		/// </summary>
		public ProcBlock(Data tmp, FormMain frm)
		{
			FORM_MAIN = frm;
			dat = tmp;

			_nMoveLeftCount = 0;
			_nMoveRightCount = 0;

			_nForcedFallCount = 0;
			_nNaturalFallCount = 0;
		}
		/// <summary>
		/// 次の落下ブロックを作成
		/// </summary>
		public void CreateNextBlock()
		{
			// Nextブロックのインスタンスを現在のインスタンスとする。
			dat.fbNow = dat.fbNext;

			// 次のブロックタイプを乱数で決める
			Random rand = new Random();
			int nType = rand.Next(0, FallingBlock.BLOCKTYPES);

			//Nextブロックのインスタンス作成
			dat.fbNext = new FallingBlock(nType);

			// ランダムな回数回転
			int nRotate = rand.Next(4);
			for (int i = 0; i < nRotate; i++)
			{
				dat.fbNext = dat.fbNext.Rotate();
			}

			if (dat.fbNow != null)
			{
				// 一番上の真中から落ちてくる。
				dat.fbNow.X = (dat.X_MAX - dat.fbNow.Width) / 2;
				dat.fbNow.Y = 0;

				// もしフィールドのブロックに接触したらゲームオーバー
				if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X, dat.fbNow.Y))
				{
					dat.stateApp = Status.GameOver;
				}
			}
		}
		/// <summary>
		/// Block情報を更新
		/// </summary>
		public void UpdateBlock()
		{
			if (dat.nFlashingCount > 0)
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
			dat.nFlashingCount--;
			if (dat.nFlashingCount > 0) return; // フラッシュ中。この間ブロックは動かない。

			// フラッシュ終了
			dat.nFlashingCount = 0;

			// 消した行数をｶｳﾝﾄしてスコアに反映させる。
			int nErasedLines = 0;
			for (int y = dat.Y_MAX - 1; y >= 0; y--)
			{
				if (dat.FIELDBLOCK[0, y] == FallingBlock.COLOR_INVALID)
				{
					// 行を消去して下に詰める
					dat.FIELDBLOCK.EraseLine(y);

					// 消去した行
					nErasedLines++;

					// 消した行を再チェック
					y++;
				}
			}
			// スコア計算
			CalculateScore(nErasedLines);

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

			if (dat.bKeyLeftPressed == true)
			{
				_nMoveLeftCount++;
				if (FORCED_FALL_SPEED < _nMoveLeftCount)
				{
					_nMoveLeftCount = 0;
					MoveLeft();
				}
			}
			if (dat.bKeyRightPressed == true)
			{
				_nMoveRightCount++;
				if (FORCED_FALL_SPEED < _nMoveRightCount)
				{
					_nMoveRightCount = 0;
					MoveRight();
				}
			}
			//ボタンを押しているかどうかに関わらず、1回は下に落ちる。
			bool bLanded = false;
			if (dat.bKeyDownPressed == true)
			{
				// 強制落下
				_nForcedFallCount++;
				if (FORCED_FALL_SPEED < _nForcedFallCount)
				{
					_nForcedFallCount = 0;

					bLanded = !MoveDown();
					if (bLanded == false)
					{
						dat.score.TotalScore += 1;  // 下に進めると1点入る
					}
				}
			}
			else
			{
				// 自然落下
				_nNaturalFallCount++;
				if (NATURAL_FALL_SPEED < _nNaturalFallCount)
				{
					_nNaturalFallCount = 0;

					// 落下させる
					bLanded = !MoveDown();
				}
			}

			// 着地処理
			if (bLanded == true) Landed();
		}
		/// <summary>
		/// 着地処理
		/// </summary>
		private void Landed()
		{
			// フィールドのブロックとする
			dat.FIELDBLOCK.Paste(dat.fbNow, dat.fbNow.X, dat.fbNow.Y);

			int nFlachLines = 0;
			for (int y = 0; y < dat.Y_MAX; y++)
			{
				// 行全体がブロックで埋め尽くされている場合
				if (dat.FIELDBLOCK.IsLineFilled(y) == true)
				{
					//一行でも消せる行があったらフラッシュする。
					dat.nFlashingCount = 40;

					// 該当行のブロックをCOLOR_INVALIDで塗りつぶす
					dat.FIELDBLOCK.FillLine(y, FallingBlock.COLOR_INVALID);

					nFlachLines++;
				}
			}
			switch (nFlachLines)
			{
				case 0: Music.StartWav("Land.wav"); break;
				case 1: Music.StartWav("Flash1.wav"); break;
				case 2: Music.StartWav("Flash2.wav"); break;
				case 3: Music.StartWav("Flash3.wav"); break;
				case 4: Music.StartWav("Flash4.wav"); break;
				default: break;
			}

			// 消える行がなかった時は次のブロックを作成する。
			if (dat.nFlashingCount == 0)
			{
				CreateNextBlock();
			}
		}
		/// <summary>
		/// ブロックを左へ移動
		/// </summary>
		/// <returns>True:移動成功　　False:移動失敗</returns>
		private bool MoveLeft()
		{
			// 壁に接していたら移動できない。
			if (dat.fbNow.X + dat.fbNow.LeftEdge().X <= 0)
			{
				return false;
			}

			// Fieldブロックと接している場合は移動できない。
			if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X - 1, dat.fbNow.Y))
			{
				return false;
			}

			//移動実行
			dat.fbNow.X--;
			return true;
		}
		/// <summary>
		/// ブロックを右へ移動
		/// </summary>
		/// <returns>True:移動成功　　False:移動失敗</returns>
		private bool MoveRight()
		{
			// 壁に接していたら移動できない。
			if (dat.X_MAX - 1 <= dat.fbNow.X + dat.fbNow.RightEdge().X)
			{
				return false;
			}

			// Fieldブロックと接している場合は移動できない。
			if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X + 1, dat.fbNow.Y))
			{
				return false;
			}
			//移動実行
			dat.fbNow.X++;
			return true;
		}
		/// <summary>
		/// ブロックを下へ移動
		/// </summary>
		/// <returns>True:移動成功　　False:移動失敗(着地した)</returns>
		private bool MoveDown()
		{
			// 壁に接していたら移動できない。
			if (dat.Y_MAX - 1 <= dat.fbNow.Y + dat.fbNow.BottomEdge().Y)
			{
				return false;
			}

			// Fieldブロックと接している場合は移動できない。
			if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X, dat.fbNow.Y + 1))
			{
				return false;
			}

			dat.fbNow.Y++;
			return true;
		}
		/// <summary>
		/// 落下ブロックを回転する
		/// </summary>
		public void Rotate()
		{
			if (dat.nFlashingCount > 0) return;

			// 回転後のブロックを取得
			FallingBlock fbClone = dat.fbNow.Rotate();

			// 壁からはみ出る場合は回転できない
			if (fbClone.X + fbClone.LeftEdge().X < 0) return;
			if (dat.X_MAX <= dat.fbNow.X + fbClone.RightEdge().X) return;
			if (dat.Y_MAX <= dat.fbNow.Y + fbClone.BottomEdge().Y) return;

			// Fieldブロックと重なり合った場合は回転できない
			if (dat.FIELDBLOCK.IsPiledUp(fbClone, fbClone.X, fbClone.Y))
			{
				return;
			}

			// 回転成功。回転後のインスタンスを設定
			dat.fbNow = fbClone;
		}
		/// <summary>
		/// 消した行数からスコア計算
		/// </summary>
		/// <param name="nErasedLines">消した行数</param>
		private void CalculateScore(int nErasedLines)
		{
			switch (nErasedLines)
			{
				case 1: dat.score.Single++; dat.score.TotalScore += 100; break;
				case 2: dat.score.Double++; dat.score.TotalScore += 200; break;
				case 3: dat.score.Triple++; dat.score.TotalScore += 500; break;
				case 4: dat.score.Tetris++; dat.score.TotalScore += 1000; break;
			}
			dat.score.Lines += nErasedLines;
		}
	}
}
