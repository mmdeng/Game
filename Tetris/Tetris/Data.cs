using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Media;
using System.IO;

namespace Tetris
{
	/// <summary>
	/// データクラス
	/// </summary>
	public class Data
	{
		/// <summary>
		/// X方向フィールドブロック最大数
		/// </summary>
		public readonly int X_MAX;
		/// <summary>
		/// X方向フィールドブロック最大数
		/// </summary>
		public readonly int Y_MAX;

		/// <summary>
		/// ブロック1個分の幅
		/// </summary>
		public readonly int BLOCK_WIDTH;
		/// <summary>
		/// ブロック1個分の高さ
		/// </summary>
		public readonly int BLOCK_HEIGHT;
		/// <summary>
		/// メイン画面サイズ
		/// </summary>
		public readonly Rect RCT_DISP;
		/// <summary>
		/// フィールド領域サイズ
		/// </summary>
		public readonly Rect RCT_FIELD;
		/// <summary>
		/// Nextブロック表示領域サイズ
		/// </summary>
		public readonly Rect RCT_NEXT;
		/// <summary>
		/// スコア領域サイズ
		/// </summary>
		public readonly Rect RCT_SCORE;

		/// <summary>
		/// ブロック色付け用ビットマップ
		/// </summary>
		public readonly Bitmap BMP_BLOCK = null;
		/// <summary>
		/// 背景ビットマップ
		/// </summary>
		public readonly Bitmap BMP_BACK = null;

		/// <summary>
		/// フィールドブロックインスタンス
		/// </summary>
		public readonly FieldBlock FIELDBLOCK = null;

		/// <summary>
		/// 現在のFallingBlock
		/// </summary>
		public FallingBlock fbNow = null;
		/// <summary>
		/// 次のFallingBlock
		/// </summary>
		public FallingBlock fbNext = null;

		/// <summary>
		/// ゲームステータス
		/// </summary>
		public Status stateApp;
		/// <summary>
		/// スコア構造体
		/// </summary>
		public Score score;

		/// <summary>
		/// 初期化フラグ
		/// </summary>
		public bool bInitialized;
		/// <summary>
		/// メインループ脱出用フラグ
		/// </summary>
		public bool bContinueLoop;
		/// <summary>
		/// Dispose済みフラグ
		/// </summary>
		public bool bDisposed;


		/// <summary>
		/// ブロックを消した時のフラッシュ時間
		/// </summary>
		public int nFlashingCount;

		/// <summary>
		/// 左キー押下フラグ
		/// </summary>
		public bool bKeyLeftPressed;
		/// <summary>
		/// 右キー押下フラグ
		/// </summary>
		public bool bKeyRightPressed;
		/// <summary>
		/// 下キー押下フラグ
		/// </summary>
		public bool bKeyDownPressed;
		/// <summary>
		/// 
		/// </summary>
		public Data()
		{
			RCT_DISP = new Rect();
			RCT_FIELD = new Rect();
			RCT_NEXT = new Rect();
			RCT_SCORE = new Rect();

			// 各座標値
			BLOCK_WIDTH = 24;
			BLOCK_HEIGHT = 24;

			X_MAX = 10;
			Y_MAX = 20;

			RCT_FIELD.L = BLOCK_WIDTH;
			RCT_FIELD.T = BLOCK_HEIGHT;
			RCT_FIELD.W = X_MAX * BLOCK_WIDTH;
			RCT_FIELD.H = Y_MAX * BLOCK_HEIGHT;

			RCT_NEXT.W = 4 * BLOCK_WIDTH;
			RCT_NEXT.H = 4 * BLOCK_HEIGHT;

			RCT_NEXT.L = RCT_FIELD.L + RCT_FIELD.W + BLOCK_WIDTH;
			RCT_NEXT.T = RCT_FIELD.T;

			RCT_SCORE.W = 125;
			RCT_SCORE.H = 150;

			RCT_SCORE.L = RCT_NEXT.L;
			RCT_SCORE.T = RCT_FIELD.T + RCT_FIELD.H - RCT_SCORE.H;

			RCT_DISP.W = RCT_SCORE.L + RCT_SCORE.W + BLOCK_WIDTH;
			RCT_DISP.H = RCT_FIELD.T + RCT_FIELD.H + BLOCK_HEIGHT;

			// フィールドブロック
			FIELDBLOCK = new FieldBlock(X_MAX, Y_MAX, 0);

			// 背景用ビットマップ
			try
			{
				BMP_BACK = new Bitmap(RCT_DISP.W, RCT_DISP.H);
			}
			catch (Exception ex)
			{
				MessageBox.Show("例外エラーが発生しました\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// ブロック用ビットマップ
			try
			{
				var assm = Assembly.GetExecutingAssembly();
				var stream = assm.GetManifestResourceStream("Tetris.Resources.Block.bmp");
				BMP_BLOCK = (Bitmap)Image.FromStream(stream);
			}
			catch (System.IO.FileNotFoundException ex)
			{
				MessageBox.Show("ファイルが見つかりません。\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show("例外エラーが発生しました\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Initialize();
		}
		/// <summary>
		/// 
		/// </summary>
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
	/// <summary>
	/// 矩形
	/// </summary>
	public class Rect
	{
		public int L;
		public int T;
		public int W;
		public int H;
		public Rectangle GetRect()
		{
			return new Rectangle(L, T, W, H);
		}
	}
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
	/// <summary>
	/// アプリケーション状態
	/// </summary>
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
			SND_SYNC = 0x0000,
			SND_ASYNC = 0x0001,
			SND_NODEFAULT = 0x0002,
			SND_MEMORY = 0x0004,
			SND_LOOP = 0x0008,
			SND_NOSTOP = 0x0010,
			SND_NOWAIT = 0x00002000,
			SND_ALIAS = 0x00010000,
			SND_ALIAS_ID = 0x00110000,
			SND_FILENAME = 0x00020000,
			SND_RESOURCE = 0x00040004
		}
		public static void StartWav(string szFileName)
		{
			var a = Assembly.GetExecutingAssembly();
			var s = a.GetManifestResourceStream("Tetris.Resources." + szFileName);
			var player = new SoundPlayer(s);
			player.Play();
		}
		public static bool StartMm()
		{
			return true;
		}
		public static bool EndMm()
		{
			return true;
		}
	}

}
