using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{

class Tetris
{

	private readonly Data		_dat = null;
	private readonly ProcBlock	_proc = null;
	private readonly FormMain	_FORM_MAIN = null;

	//========================================================================================
	// Name		: Tetris
	// Function	: ｺﾝｽﾄﾗｸﾀ。ﾌｫｰﾑ作成処理、初期化処理
	//========================================================================================
	public Tetris()
	{
		_dat			= new Data();

		// ディスプレイを初期化
		try
		{
			_FORM_MAIN = new FormMain( new Size( _dat.RCT_DISP.W, _dat.RCT_DISP.H ) );

			_FORM_MAIN.Text = "Tetris";

			_FORM_MAIN.Closed   += new EventHandler( OnClose );
			_FORM_MAIN.Paint    += new PaintEventHandler( OnPaint );
			_FORM_MAIN.KeyDown  += new KeyEventHandler( OnKeyDown );
			_FORM_MAIN.KeyUp    += new KeyEventHandler( OnKeyUp );
		}
		catch ( Exception ex )
		{
			MessageBox.Show( "例外エラーが発生しました\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			return;
		}

		_proc		= new ProcBlock( _dat, _FORM_MAIN );

		// 背景画像を描画
		Draw.MakeBackGround( _dat );

		_proc.CreateNextBlock();		// Next作成。
		_proc.CreateNextBlock();		// NextをNowにｺﾋﾟｰして、新たにNext作成。

		// 初期化は完了しているか
		if ( _dat.bInitialized == false )
		{
			MessageBox.Show( "初期化を完了できませんでした。　アプリケーションを終了します。", "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			
			// 終了処理
			Dispose();
		}

		// ディスプレイを表示
		if ( _FORM_MAIN != null ) _FORM_MAIN.Show();

	}

	//========================================================================================
	// Name		: MainLoop
	// Function	: ﾒｲﾝﾙｰﾌﾟ
	//========================================================================================
	public void MainLoop()
	{
		DateTime dtmLast = DateTime.Now;
		DateTime dtmCurr = DateTime.Now;
		TimeSpan tspDiff;

		while( _dat.bContinueLoop == true )
		{
			// fpsを固定
			while ( true )
			{
				// 30ms経過したらﾙｰﾌﾟを抜ける。
				dtmCurr = DateTime.Now;
				tspDiff = dtmCurr - dtmLast;
				if ( 30 < tspDiff.TotalMilliseconds )
				{
					dtmLast = dtmCurr;
					break;
				}
				System.Threading.Thread.Sleep( 1 );
			}

			// ﾌﾞﾛｯｸﾃﾞｰﾀ更新
			if ( _dat.stateApp == Status.GamePlaying )
			{
				_proc.UpdateBlock();
			}

			// 強制的に再描画
			Graphics g = _FORM_MAIN.CreateGraphics();
			OnPaint( null, new PaintEventArgs( g, _FORM_MAIN.ClientRectangle ) );
			g.Dispose();

			// 全てのｳｨﾝﾄﾞｳｽﾞﾒｯｾｰｼﾞを処理する。
			Application.DoEvents();
		}

		// 終了処理
		Dispose();
	}

	//========================================================================================
	// Name		: Dispose
	// Function	: ﾘｿｰｽ解放
	//========================================================================================
	private void Dispose()
	{
		// これ以降は再描画禁止
		_FORM_MAIN.Paint -= new PaintEventHandler( OnPaint );

		if ( _dat.BMP_BLOCK != null )
		{
			_dat.BMP_BLOCK.Dispose();
		}

		if ( _dat.BMP_BACK != null )
		{
			_dat.BMP_BACK.Dispose();
		}

		_FORM_MAIN.EndMediaPlayer();

		if ( _FORM_MAIN != null )
		{
			_FORM_MAIN.Close();
			_FORM_MAIN.Dispose();
		}
		_dat.bDisposed = true;
	}

	#region EventHandler

	//========================================================================================
	// Name		: OnClose
	// Function	: 画面終了
	//========================================================================================
	private void OnClose( object sender, EventArgs e )
	{
		_dat.bContinueLoop = false;
	}
	//========================================================================================
	// Name		: OnPaint
	// Function	: 再描画
	//========================================================================================
	private void OnPaint( object sender, PaintEventArgs e )
	{
		Draw.DrawGame( _dat, e );
	}

	//========================================================================================
	// Name		: OnKeyDown
	// Function	: ｷｰ押下時
	//========================================================================================
	private void OnKeyDown( object sender, KeyEventArgs e )
	{
		switch ( e.KeyCode )
		{
			case Keys.Left: goto case Keys.NumPad4;
			case Keys.NumPad4:
			{
				_dat.bKeyLeftPressed = true;
				break;
			}

			case Keys.Right: goto case Keys.NumPad6;
			case Keys.NumPad6:
			{
				_dat.bKeyRightPressed = true;
				break;
			}

			case Keys.Down: goto case Keys.NumPad2;
			case Keys.NumPad2:
			{
				_dat.bKeyDownPressed = true;
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

	//========================================================================================
	// Name		: OnKeyUp
	// Function	: ｷｰ解放
	//========================================================================================
	private void OnKeyUp( object sender, KeyEventArgs e )
	{
		switch ( e.KeyCode )
		{
			case Keys.Left: goto case Keys.NumPad4;
			case Keys.NumPad4:
			{
				_dat.bKeyLeftPressed = false;	// ←
				break;
			}

			case Keys.Right: goto case Keys.NumPad6;
			case Keys.NumPad6:
			{
				_dat.bKeyRightPressed = false;	// →
				break;
			}

			case Keys.Down: goto case Keys.NumPad2;
			case Keys.NumPad2:
			{
				_dat.bKeyDownPressed = false;	// ↓
				break;
			}

			case Keys.Enter:
			{
				//Enter:GameStart
				if ( _dat.stateApp != Status.GamePlaying )
				{
					_FORM_MAIN.StartMediaPlayer();
					_dat.score.Reset();
					_proc.CreateNextBlock();
					_dat.stateApp = Status.GamePlaying;
				}
				break;
			}

			case Keys.Escape:
			{
				_FORM_MAIN.EndMediaPlayer();
				if ( _dat.stateApp == Status.GamePlaying )
				{
					// ゲーム中止
					_dat.stateApp = Status.GameOver;
				}
				else
				{
					// 終了
					_dat.bContinueLoop = false;
				}
				break;
			}
		}
	}

	#endregion

}
}
