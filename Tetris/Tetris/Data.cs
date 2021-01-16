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
	/// �f�[�^�N���X
	/// </summary>
	public class Data
	{
		/// <summary>
		/// X�����t�B�[���h�u���b�N�ő吔
		/// </summary>
		public readonly int X_MAX;
		/// <summary>
		/// X�����t�B�[���h�u���b�N�ő吔
		/// </summary>
		public readonly int Y_MAX;

		/// <summary>
		/// �u���b�N1���̕�
		/// </summary>
		public readonly int BLOCK_WIDTH;
		/// <summary>
		/// �u���b�N1���̍���
		/// </summary>
		public readonly int BLOCK_HEIGHT;
		/// <summary>
		/// ���C����ʃT�C�Y
		/// </summary>
		public readonly Rect RCT_DISP;
		/// <summary>
		/// �t�B�[���h�̈�T�C�Y
		/// </summary>
		public readonly Rect RCT_FIELD;
		/// <summary>
		/// Next�u���b�N�\���̈�T�C�Y
		/// </summary>
		public readonly Rect RCT_NEXT;
		/// <summary>
		/// �X�R�A�̈�T�C�Y
		/// </summary>
		public readonly Rect RCT_SCORE;

		/// <summary>
		/// �u���b�N�F�t���p�r�b�g�}�b�v
		/// </summary>
		public readonly Bitmap BMP_BLOCK = null;
		/// <summary>
		/// �w�i�r�b�g�}�b�v
		/// </summary>
		public readonly Bitmap BMP_BACK = null;

		/// <summary>
		/// �t�B�[���h�u���b�N�C���X�^���X
		/// </summary>
		public readonly FieldBlock FIELDBLOCK = null;

		/// <summary>
		/// ���݂�FallingBlock
		/// </summary>
		public FallingBlock fbNow = null;
		/// <summary>
		/// ����FallingBlock
		/// </summary>
		public FallingBlock fbNext = null;

		/// <summary>
		/// �Q�[���X�e�[�^�X
		/// </summary>
		public Status stateApp;
		/// <summary>
		/// �X�R�A�\����
		/// </summary>
		public Score score;

		/// <summary>
		/// �������t���O
		/// </summary>
		public bool bInitialized;
		/// <summary>
		/// ���C�����[�v�E�o�p�t���O
		/// </summary>
		public bool bContinueLoop;
		/// <summary>
		/// Dispose�ς݃t���O
		/// </summary>
		public bool bDisposed;


		/// <summary>
		/// �u���b�N�����������̃t���b�V������
		/// </summary>
		public int nFlashingCount;

		/// <summary>
		/// ���L�[�����t���O
		/// </summary>
		public bool bKeyLeftPressed;
		/// <summary>
		/// �E�L�[�����t���O
		/// </summary>
		public bool bKeyRightPressed;
		/// <summary>
		/// ���L�[�����t���O
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

			// �e���W�l
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

			// �t�B�[���h�u���b�N
			FIELDBLOCK = new FieldBlock(X_MAX, Y_MAX, 0);

			// �w�i�p�r�b�g�}�b�v
			try
			{
				BMP_BACK = new Bitmap(RCT_DISP.W, RCT_DISP.H);
			}
			catch (Exception ex)
			{
				MessageBox.Show("��O�G���[���������܂���\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// �u���b�N�p�r�b�g�}�b�v
			try
			{
				var assm = Assembly.GetExecutingAssembly();
				var stream = assm.GetManifestResourceStream("Tetris.Resources.Block.bmp");
				BMP_BLOCK = (Bitmap)Image.FromStream(stream);
			}
			catch (System.IO.FileNotFoundException ex)
			{
				MessageBox.Show("�t�@�C����������܂���B\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show("��O�G���[���������܂���\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Initialize();
		}
		/// <summary>
		/// 
		/// </summary>
		private void Initialize()
		{
			// �X�R�A
			score = new Score();
			score.Reset();

			// �t���O��������
			stateApp = Status.Title;

			bDisposed = false;

			nFlashingCount = 0;

			// ����������
			bInitialized = true;
			bContinueLoop = true;

		}
	}
	/// <summary>
	/// ��`
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
	/// ���_
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
	/// �A�v���P�[�V�������
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
