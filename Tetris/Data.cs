using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Tetris
{
	//�ް��׽
	public class Data
	{
		//------------------------------------------------------------
		// Readonly
		public readonly int		X_MAX;					// X����̨������ۯ��ő吔
		public readonly int		Y_MAX;					// X����̨������ۯ��ő吔

		public readonly int		BLOCK_WIDTH;			// ��ۯ�1���̕�
		public readonly int		BLOCK_HEIGHT;			// ��ۯ�1���̍���

		public readonly Rect	RCT_DISP;				// Ҳ݉�ʻ���
		public readonly Rect	RCT_FIELD;				// ̨���ޗ̈滲��
		public readonly Rect	RCT_NEXT;				// Next��ۯ��\���̈滲��
		public readonly Rect	RCT_SCORE;				// ����̈滲��

		public readonly Bitmap	BMP_BLOCK = null;		// ��ۯ��F�t���p�ޯ�ϯ��
		public readonly Bitmap	BMP_BACK = null;		// �w�i�ޯ�ϯ��

		public readonly FieldBlock FIELDBLOCK = null;	// ̨������ۯ��ݽ�ݽ

		//------------------------------------------------------------
		// ReadWrite
		public FallingBlock		fbNow = null;			// ���݂�FallingBlock
		public FallingBlock		fbNext = null;			// ����FallingBlock

		public Status	stateApp;						// �ްѽð��(enum)
		public Score	score;							// ����\����

		public bool		bInitialized;					// �������׸�
		public bool		bContinueLoop;					// Ҳ�ٰ�ߒE�o�p�׸�
		public bool		bDisposed;						// Dispose�ς��׸�


		public int		nFlashingCount;					// ��ۯ��������������ׯ������

		public bool		bKeyLeftPressed;				// ���������׸�
		public bool		bKeyRightPressed;				// �E�������׸�
		public bool		bKeyDownPressed;				// ���������׸�

		public Data()
		{
			RCT_DISP	= new Rect();
			RCT_FIELD	= new Rect();
			RCT_NEXT	= new Rect();
			RCT_SCORE	= new Rect();

			// �e���W�l
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

			// �t�B�[���h�u���b�N
			FIELDBLOCK = new FieldBlock( X_MAX, Y_MAX, 0 );

			// �w�i�p�r�b�g�}�b�v
			try
			{
				BMP_BACK = new Bitmap( RCT_DISP.W, RCT_DISP.H );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "��O�G���[���������܂���\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}

			// �u���b�N�p�r�b�g�}�b�v
			try
			{
				BMP_BLOCK = (Bitmap)Bitmap.FromFile( "Block.bmp" );
			}
			catch ( System.IO.FileNotFoundException ex )
			{
				MessageBox.Show( "�t�@�C����������܂���B\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "��O�G���[���������܂���\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return;
			}
			Initialize();
		}
		private void Initialize()
		{
			// ���
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
	//��`
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
	//���_
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

	//���ع���ݏ��
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
		[DllImport("winmm.dll")]
		extern static bool PlaySound( string pszSound, IntPtr hMod, FdwSound fdwSound );

		private static bool _bPlaying = false;

		//========================================================================================
		// Name		: StartWav
		// Function	: 
		//========================================================================================
		public static void StartWav( string szFileName )
		{
			PlaySound( szFileName, 
				IntPtr.Zero, FdwSound.SND_ASYNC | FdwSound.SND_FILENAME );
		}
		//========================================================================================
		// Name		: EndWav
		// Function	: 
		//========================================================================================
		public static bool EndWav()
		{
			if( _bPlaying == false )	return false;

			bool bRet = PlaySound( null, IntPtr.Zero, 0 );

			_bPlaying = !bRet;

			return bRet;
		}

		//========================================================================================
		// Name		: LoopWav
		// Function	: 
		//========================================================================================
		public static bool LoopWav( string szFileName )
		{
			if( _bPlaying == true )	return false;

			_bPlaying = PlaySound( szFileName, 
				IntPtr.Zero, FdwSound.SND_ASYNC | FdwSound.SND_FILENAME | FdwSound.SND_LOOP );

			return _bPlaying;
		}
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






