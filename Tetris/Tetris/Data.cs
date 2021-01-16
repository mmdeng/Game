using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Media;
using System.IO;

namespace Tetris
{
	//データクラス
	public class Data
	{
		//------------------------------------------------------------
		// Readonly
		public readonly int		X_MAX;					// X方向フィールドブロック最大数
		public readonly int		Y_MAX;					// X方向フィールドブロック最大数

		public readonly int		BLOCK_WIDTH;			// ブロック1個分の幅
		public readonly int		BLOCK_HEIGHT;			// ブロック1個分の高さ

		public readonly Rect	RCT_DISP;				// メイン画面サイズ
		public readonly Rect	RCT_FIELD;				// フィールド領域サイズ
		public readonly Rect	RCT_NEXT;				// Nextブロック表示領域サイズ
		public readonly Rect	RCT_SCORE;				// スコア領域サイズ

		public readonly Bitmap	BMP_BLOCK = null;		// ブロック色付け用ビットマップ
		public readonly Bitmap	BMP_BACK = null;		// 背景ビットマップ

		public readonly FieldBlock FIELDBLOCK = null;	// フィールドブロックインスタンス

		//------------------------------------------------------------
		// ReadWrite
		public FallingBlock		fbNow = null;			// 現在のFallingBlock
		public FallingBlock		fbNext = null;			// 次のFallingBlock

		public Status	stateApp;						// ゲームステータス(enum)
		public Score	score;							// スコア構造体

		public bool		bInitialized;					// 初期化フラグ
		public bool		bContinueLoop;					// メインループ脱出用フラグ
		public bool		bDisposed;						// Dispose済みフラグ


		public int		nFlashingCount;					// ブロックを消した時のフラッシュ時間

		public bool		bKeyLeftPressed;				// 左キー押下フラグ
		public bool		bKeyRightPressed;				// 右キー押下フラグ
		public bool		bKeyDownPressed;				// 下キー押下フラグ

		public Data()
		{
			RCT_DISP	= new Rect();
			RCT_FIELD	= new Rect();
			RCT_NEXT	= new Rect();
			RCT_SCORE	= new Rect();

			// 各座標値
			BLOCK_WIDTH  = 24;
			BLOCK_HEIGHT = 24;
		
			X_MAX = 10;
			Y_MAX = 20;

			RCT_FIELD.L	= BLOCK_WIDTH;
			RCT_FIELD.T	= BLOCK_HEIGHT;
			RCT_FIELD.W	= X_MAX * BLOCK_WIDTH;
			RCT_FIELD.H	= Y_MAX * BLOCK_HEIGHT;

			RCT_NEXT.W	= 4 * BLOCK_WIDTH;
			RCT_NEXT.H	= 4 * BLOCK_HEIGHT;

			RCT_NEXT.L	= RCT_FIELD.L + RCT_FIELD.W + BLOCK_WIDTH;
			RCT_NEXT.T	= RCT_FIELD.T;

			RCT_SCORE.W	= 125;
			RCT_SCORE.H	= 150;

			RCT_SCORE.L	= RCT_NEXT.L;
			RCT_SCORE.T	= RCT_FIELD.T + RCT_FIELD.H - RCT_SCORE.H;

			RCT_DISP.W	= RCT_SCORE.L + RCT_SCORE.W + BLOCK_WIDTH;
			RCT_DISP.H	= RCT_FIELD.T + RCT_FIELD.H + BLOCK_HEIGHT;

			// フィールドブロック
			FIELDBLOCK = new FieldBlock( X_MAX, Y_MAX, 0 );

			// 背景用ビットマップ
			try
			{
				BMP_BACK = new Bitmap( RCT_DISP.W, RCT_DISP.H );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "例外エラーが発生しました\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}

			// ブロック用ビットマップ
			try
			{
				var assm = Assembly.GetExecutingAssembly();
				var stream = assm.GetManifestResourceStream("Tetris.Resources.Block.bmp");
				BMP_BLOCK= (Bitmap)Image.FromStream(stream);
			}
			catch ( System.IO.FileNotFoundException ex )
			{
				MessageBox.Show( "ファイルが見つかりません。\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "例外エラーが発生しました\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}
			Initialize();
		}
		private void Initialize()
		{
			// スコア
			score = new Score();
			score.Reset();

			// フラグを初期化
			stateApp = Status.Title;

			bDisposed = false;

			nFlashingCount = 0;

			// 初期化完了
			bInitialized = true;
			bContinueLoop = true;

		}
	}
	//矩形
	public class Rect
	{
		public int L;
		public int T;
		public int W;
		public int H;
		public Rectangle GetRect()
		{
			return new Rectangle( L, T, W, H );
		}
	}
	//得点
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

	//ｱﾌﾟﾘｹｰｼｮﾝ状態
	public enum Status
	{
		Title,
		GamePlaying,
		GameOver,
	}

	public class Music
	{
		private enum FdwSound : uint
		{
			SND_SYNC		= 0x0000,
			SND_ASYNC		= 0x0001,
			SND_NODEFAULT	= 0x0002,
			SND_MEMORY		= 0x0004,
			SND_LOOP		= 0x0008,
			SND_NOSTOP		= 0x0010,
			SND_NOWAIT		= 0x00002000,
			SND_ALIAS		= 0x00010000,
			SND_ALIAS_ID	= 0x00110000,
			SND_FILENAME	= 0x00020000,
			SND_RESOURCE	= 0x00040004
		}
		//[DllImport("winmm.dll")]
		//extern static bool PlaySound( string pszSound, IntPtr hMod, FdwSound fdwSound );

		//private static bool _bPlaying = false;

		//========================================================================================
		// Name		: StartWav
		// Function	: 
		//========================================================================================
		public static void StartWav( string szFileName )
		{
            var a = Assembly.GetExecutingAssembly();
            var s = a.GetManifestResourceStream("Tetris.Resources." + szFileName);
            var player = new SoundPlayer(s);
            player.Play();

            //MediaPlayer myPlayer = new MediaPlayer();
            //myPlayer.Open(new System.Uri(audioPath));
            //myPlayer.Play();

   //         var path = Path.Combine("Resources", szFileName);
			//PlaySound(path,
			//	IntPtr.Zero, FdwSound.SND_ASYNC | FdwSound.SND_FILENAME );
		}
		////========================================================================================
		//// Name		: EndWav
		//// Function	: 
		////========================================================================================
		//public static bool EndWav()
		//{
		//	if( _bPlaying == false )	return false;

		//	bool bRet = PlaySound( null, IntPtr.Zero, 0 );

		//	_bPlaying = !bRet;

		//	return bRet;
		//}

		////========================================================================================
		//// Name		: LoopWav
		//// Function	: 
		////========================================================================================
		//public static bool LoopWav( string szFileName )
		//{
		//	if( _bPlaying == true )	return false;

		//	_bPlaying = PlaySound( szFileName, 
		//		IntPtr.Zero, FdwSound.SND_ASYNC | FdwSound.SND_FILENAME | FdwSound.SND_LOOP );

		//	return _bPlaying;
		//}
		//========================================================================================
		// Name		: StartMm
		// Function	: 
		//========================================================================================
		public static bool StartMm()
		{
//			axMMControl1.FileName = "./Tetris.WAV";
//			axMMControl1.Command = "Open" ;
//			axMMControl1.Command = "Play" ;

			return true;
		}
		//========================================================================================
		// Name		: EndMm
		// Function	: 
		//========================================================================================
		public static bool EndMm()
		{
			return true;
		}
	}

}






