using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
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
		/// ���C����ʃG���A
		/// </summary>
		public readonly Area MAIN_AREA;
		/// <summary>
		/// �t�B�[���h�̈�G���A
		/// </summary>
		public readonly Area FIELD_AREA;
		/// <summary>
		/// Next�u���b�N�\���̈�G���A
		/// </summary>
		public readonly Area NEXT_AREA;
		/// <summary>
		/// �X�R�A�̈�G���A
		/// </summary>
		public readonly Area SCORE_AREA;

		/// <summary>
		/// �u���b�N�F�t���p�r�b�g�}�b�v
		/// </summary>
		public readonly Bitmap BLOCK_BITMAP = null;
		/// <summary>
		/// �w�i�r�b�g�}�b�v
		/// </summary>
		public readonly Bitmap BACKGROUND_BITMAP = null;

		/// <summary>
		/// �t�B�[���h�u���b�N�C���X�^���X
		/// </summary>
		public readonly FieldBlock FIELD_BLOCK = null;

		/// <summary>
		/// ���݂�FallingBlock
		/// </summary>
		public FallingBlock CurrentBlock = null;
		/// <summary>
		/// ����FallingBlock
		/// </summary>
		public FallingBlock NextBlock = null;

		/// <summary>
		/// �Q�[���X�e�[�^�X
		/// </summary>
		public GameStatus stateApp;
		/// <summary>
		/// �X�R�A�\����
		/// </summary>
		public Score score;

		/// <summary>
		/// �������t���O
		/// </summary>
		public bool Initialized;
		/// <summary>
		/// ���C�����[�v�E�o�p�t���O
		/// </summary>
		public bool ContinueLoop;
		/// <summary>
		/// Dispose�ς݃t���O
		/// </summary>
		public bool Disposed;

		/// <summary>
		/// �u���b�N�����������̃t���b�V������
		/// </summary>
		public int FlashingCount;

		/// <summary>
		/// ���L�[�����t���O
		/// </summary>
		public bool KeyLeftPressed;
		/// <summary>
		/// �E�L�[�����t���O
		/// </summary>
		public bool KeyRightPressed;
		/// <summary>
		/// ���L�[�����t���O
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

			// �e���W�l
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

			// �t�B�[���h�u���b�N
			FIELD_BLOCK = new FieldBlock(X_MAX, Y_MAX, 0);

			// �w�i�p�r�b�g�}�b�v
			try
			{
				BACKGROUND_BITMAP = new Bitmap(MAIN_AREA.W, MAIN_AREA.H);
			}
			catch (Exception ex)
			{
				MessageBox.Show("��O�G���[���������܂���\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// �u���b�N�p�r�b�g�}�b�v
			try
			{
				var assembly = Assembly.GetExecutingAssembly();
				var stream = assembly.GetManifestResourceStream("Tetris.Resources.Block.bmp");
				BLOCK_BITMAP = (Bitmap)Image.FromStream(stream);
			}
			catch (FileNotFoundException ex)
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
			stateApp = GameStatus.Title;

			Disposed = false;

			FlashingCount = 0;

			// ����������
			Initialized = true;
			ContinueLoop = true;
		}
	}
}
