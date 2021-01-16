using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
	class Tetris
	{
		private readonly Data _data = null;
		private readonly ProcBlock _proc = null;
		private readonly FormMain _FORM_MAIN = null;

		/// <summary>
		/// Constructor
		/// フォーム作成処理、初期化処理
		/// </summary>
		public Tetris()
		{
			_data = new Data();

			// ディスプレイを初期化
			try
			{
                _FORM_MAIN = new FormMain(new Size(_data.MAIN_AREA.W, _data.MAIN_AREA.H))
                {
                    Text = "Tetris"
                };

                _FORM_MAIN.Closed += new EventHandler(OnClose);
				_FORM_MAIN.Paint += new PaintEventHandler(OnPaint);
				_FORM_MAIN.KeyDown += new KeyEventHandler(OnKeyDown);
				_FORM_MAIN.KeyUp += new KeyEventHandler(OnKeyUp);
			}
			catch (Exception ex)
			{
				MessageBox.Show("例外エラーが発生しました\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			_proc = new ProcBlock(_data);

			// 背景画像を描画
			Draw.MakeBackGround(_data);

			_proc.CreateNextBlock();        // Next作成。
			_proc.CreateNextBlock();        // NextをNowにコピーして、新たにNext作成。

			// 初期化は完了しているか
			if (!_data.Initialized)
			{
				MessageBox.Show("初期化を完了できませんでした。　アプリケーションを終了します。", "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				// 終了処理
				Dispose();
			}
			// ディスプレイを表示
			if (_FORM_MAIN != null) _FORM_MAIN.Show();
		}
		/// <summary>
		/// メインループ
		/// </summary>
		public void MainLoop()
		{
			var lastTime = DateTime.Now;
			while (_data.ContinueLoop)
			{
				// fpsを固定
				while (true)
				{
					// 30ms経過したらループを抜ける。
					var time = DateTime.Now;
					var tspDiff = time - lastTime;
					if (30 < tspDiff.TotalMilliseconds)
					{
						lastTime = time;
						break;
					}
					System.Threading.Thread.Sleep(1);
				}

				// ブロックデータ更新
				if (_data.stateApp == GameStatus.Playing)
				{
					_proc.UpdateBlock();
				}
				// 強制的に再描画
				var g = _FORM_MAIN.CreateGraphics();
				OnPaint(null, new PaintEventArgs(g, _FORM_MAIN.ClientRectangle));
				g.Dispose();

				Application.DoEvents();
			}
			// 終了処理
			Dispose();
		}
		/// <summary>
		/// リソース解放
		/// </summary>
		private void Dispose()
		{
			// これ以降は再描画禁止
			_FORM_MAIN.Paint -= new PaintEventHandler(OnPaint);

			if (_data.BLOCK_BITMAP != null)
			{
				_data.BLOCK_BITMAP.Dispose();
			}
			if (_data.BACKGROUND_BITMAP != null)
			{
				_data.BACKGROUND_BITMAP.Dispose();
			}
            FormMain.EndMediaPlayer();

			if (_FORM_MAIN != null)
			{
				_FORM_MAIN.Close();
				_FORM_MAIN.Dispose();
			}
			_data.Disposed = true;
		}
		/// <summary>
		/// 画面終了
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClose(object sender, EventArgs e)
		{
			_data.ContinueLoop = false;
		}
		/// <summary>
		/// 再描画
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPaint(object sender, PaintEventArgs e)
		{
			Draw.DrawGame(_data, e);
		}

		/// <summary>
		/// キー押下時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Left: goto case Keys.NumPad4;
				case Keys.NumPad4:
					{
						_data.KeyLeftPressed = true;
						break;
					}

				case Keys.Right: goto case Keys.NumPad6;
				case Keys.NumPad6:
					{
						_data.KeyRightPressed = true;
						break;
					}

				case Keys.Down: goto case Keys.NumPad2;
				case Keys.NumPad2:
					{
						_data.KeyDownPressed = true;
						break;
					}
				case Keys.Space:
					{
						// ブロックを回転
						_proc.Rotate();
						break;
					}
			}
		}
		/// <summary>
		/// キー解放
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Left: goto case Keys.NumPad4;
				case Keys.NumPad4:
					{
						_data.KeyLeftPressed = false;   // ←
						break;
					}

				case Keys.Right: goto case Keys.NumPad6;
				case Keys.NumPad6:
					{
						_data.KeyRightPressed = false;  // →
						break;
					}

				case Keys.Down: goto case Keys.NumPad2;
				case Keys.NumPad2:
					{
						_data.KeyDownPressed = false;   // ↓
						break;
					}

				case Keys.Enter:
					{
						//Enter:GameStart
						if (_data.stateApp != GameStatus.Playing)
						{
                            FormMain.StartMediaPlayer();
							_data.score.Reset();
							_proc.CreateNextBlock();
							_data.stateApp = GameStatus.Playing;
						}
						break;
					}

				case Keys.Escape:
					{
                        FormMain.EndMediaPlayer();
						if (_data.stateApp == GameStatus.Playing)
						{
							// ゲーム中止
							_data.stateApp = GameStatus.Over;
						}
						else
						{
							// 終了
							_data.ContinueLoop = false;
						}
						break;
					}
			}
		}
	}
}
