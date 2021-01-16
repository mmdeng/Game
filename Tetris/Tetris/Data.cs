using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
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
		/// メイン画面エリア
		/// </summary>
		public readonly Area MAIN_AREA;
		/// <summary>
		/// フィールド領域エリア
		/// </summary>
		public readonly Area FIELD_AREA;
		/// <summary>
		/// Nextブロック表示領域エリア
		/// </summary>
		public readonly Area NEXT_AREA;
		/// <summary>
		/// スコア領域エリア
		/// </summary>
		public readonly Area SCORE_AREA;

		/// <summary>
		/// ブロック色付け用ビットマップ
		/// </summary>
		public readonly Bitmap BLOCK_BITMAP = null;
		/// <summary>
		/// 背景ビットマップ
		/// </summary>
		public readonly Bitmap BACKGROUND_BITMAP = null;

		/// <summary>
		/// フィールドブロックインスタンス
		/// </summary>
		public readonly FieldBlock FIELD_BLOCK = null;

		/// <summary>
		/// 現在のFallingBlock
		/// </summary>
		public FallingBlock CurrentBlock = null;
		/// <summary>
		/// 次のFallingBlock
		/// </summary>
		public FallingBlock NextBlock = null;

		/// <summary>
		/// ゲームステータス
		/// </summary>
		public GameStatus stateApp;
		/// <summary>
		/// スコア構造体
		/// </summary>
		public Score score;

		/// <summary>
		/// 初期化フラグ
		/// </summary>
		public bool Initialized;
		/// <summary>
		/// メインループ脱出用フラグ
		/// </summary>
		public bool ContinueLoop;
		/// <summary>
		/// Dispose済みフラグ
		/// </summary>
		public bool Disposed;

		/// <summary>
		/// ブロックを消した時のフラッシュ時間
		/// </summary>
		public int FlashingCount;

		/// <summary>
		/// 左キー押下フラグ
		/// </summary>
		public bool KeyLeftPressed;
		/// <summary>
		/// 右キー押下フラグ
		/// </summary>
		public bool KeyRightPressed;
		/// <summary>
		/// 下キー押下フラグ
		/// </summary>
		public bool KeyDownPressed;
		/// <summary>
		/// 
		/// </summary>
		public Data()
		{
			MAIN_AREA = new Area();
			FIELD_AREA = new Area();
			NEXT_AREA = new Area();
			SCORE_AREA = new Area();

			// 各座標値
			BLOCK_WIDTH = 24;
			BLOCK_HEIGHT = 24;

			X_MAX = 10;
			Y_MAX = 20;

			FIELD_AREA.L = BLOCK_WIDTH;
			FIELD_AREA.T = BLOCK_HEIGHT;
			FIELD_AREA.W = X_MAX * BLOCK_WIDTH;
			FIELD_AREA.H = Y_MAX * BLOCK_HEIGHT;

			NEXT_AREA.W = 4 * BLOCK_WIDTH;
			NEXT_AREA.H = 4 * BLOCK_HEIGHT;

			NEXT_AREA.L = FIELD_AREA.L + FIELD_AREA.W + BLOCK_WIDTH;
			NEXT_AREA.T = FIELD_AREA.T;

			SCORE_AREA.W = 125;
			SCORE_AREA.H = 150;

			SCORE_AREA.L = NEXT_AREA.L;
			SCORE_AREA.T = FIELD_AREA.T + FIELD_AREA.H - SCORE_AREA.H;

			MAIN_AREA.W = SCORE_AREA.L + SCORE_AREA.W + BLOCK_WIDTH;
			MAIN_AREA.H = FIELD_AREA.T + FIELD_AREA.H + BLOCK_HEIGHT;

			// フィールドブロック
			FIELD_BLOCK = new FieldBlock(X_MAX, Y_MAX, 0);

			// 背景用ビットマップ
			try
			{
				BACKGROUND_BITMAP = new Bitmap(MAIN_AREA.W, MAIN_AREA.H);
			}
			catch (Exception ex)
			{
				MessageBox.Show("例外エラーが発生しました\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// ブロック用ビットマップ
			try
			{
				var assembly = Assembly.GetExecutingAssembly();
				var stream = assembly.GetManifestResourceStream("Tetris.Resources.Block.bmp");
				BLOCK_BITMAP = (Bitmap)Image.FromStream(stream);
			}
			catch (FileNotFoundException ex)
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
			stateApp = GameStatus.Title;

			Disposed = false;

			FlashingCount = 0;

			// 初期化完了
			Initialized = true;
			ContinueLoop = true;
		}
	}
}
