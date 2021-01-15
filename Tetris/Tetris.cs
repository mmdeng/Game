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
	// Function	: �ݽ�׸��B̫�э쐬�����A����������
	//========================================================================================
	public Tetris()
	{
		_dat			= new Data();

		// �f�B�X�v���C��������
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
			MessageBox.Show( "��O�G���[���������܂���\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			return;
		}

		_proc		= new ProcBlock( _dat, _FORM_MAIN );

		// �w�i�摜��`��
		Draw.MakeBackGround( _dat );

		_proc.CreateNextBlock();		// Next�쐬�B
		_proc.CreateNextBlock();		// Next��Now�ɺ�߰���āA�V����Next�쐬�B

		// �������͊������Ă��邩
		if ( _dat.bInitialized == false )
		{
			MessageBox.Show( "�������������ł��܂���ł����B�@�A�v���P�[�V�������I�����܂��B", "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			
			// �I������
			Dispose();
		}

		// �f�B�X�v���C��\��
		if ( _FORM_MAIN != null ) _FORM_MAIN.Show();

	}

	//========================================================================================
	// Name		: MainLoop
	// Function	: Ҳ�ٰ��
	//========================================================================================
	public void MainLoop()
	{
		DateTime dtmLast = DateTime.Now;
		DateTime dtmCurr = DateTime.Now;
		TimeSpan tspDiff;

		while( _dat.bContinueLoop == true )
		{
			// fps���Œ�
			while ( true )
			{
				// 30ms�o�߂�����ٰ�߂𔲂���B
				dtmCurr = DateTime.Now;
				tspDiff = dtmCurr - dtmLast;
				if ( 30 < tspDiff.TotalMilliseconds )
				{
					dtmLast = dtmCurr;
					break;
				}
				System.Threading.Thread.Sleep( 1 );
			}

			// ��ۯ��ް��X�V
			if ( _dat.stateApp == Status.GamePlaying )
			{
				_proc.UpdateBlock();
			}

			// �����I�ɍĕ`��
			Graphics g = _FORM_MAIN.CreateGraphics();
			OnPaint( null, new PaintEventArgs( g, _FORM_MAIN.ClientRectangle ) );
			g.Dispose();

			// �S�Ă̳���޳��ү���ނ���������B
			Application.DoEvents();
		}

		// �I������
		Dispose();
	}

	//========================================================================================
	// Name		: Dispose
	// Function	: ؿ�����
	//========================================================================================
	private void Dispose()
	{
		// ����ȍ~�͍ĕ`��֎~
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
	// Function	: ��ʏI��
	//========================================================================================
	private void OnClose( object sender, EventArgs e )
	{
		_dat.bContinueLoop = false;
	}
	//========================================================================================
	// Name		: OnPaint
	// Function	: �ĕ`��
	//========================================================================================
	private void OnPaint( object sender, PaintEventArgs e )
	{
		Draw.DrawGame( _dat, e );
	}

	//========================================================================================
	// Name		: OnKeyDown
	// Function	: ��������
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
				// �u���b�N����]
				_proc.Rotate();
				break;
			}
		}
	}

	//========================================================================================
	// Name		: OnKeyUp
	// Function	: �����
	//========================================================================================
	private void OnKeyUp( object sender, KeyEventArgs e )
	{
		switch ( e.KeyCode )
		{
			case Keys.Left: goto case Keys.NumPad4;
			case Keys.NumPad4:
			{
				_dat.bKeyLeftPressed = false;	// ��
				break;
			}

			case Keys.Right: goto case Keys.NumPad6;
			case Keys.NumPad6:
			{
				_dat.bKeyRightPressed = false;	// ��
				break;
			}

			case Keys.Down: goto case Keys.NumPad2;
			case Keys.NumPad2:
			{
				_dat.bKeyDownPressed = false;	// ��
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
					// �Q�[�����~
					_dat.stateApp = Status.GameOver;
				}
				else
				{
					// �I��
					_dat.bContinueLoop = false;
				}
				break;
			}
		}
	}

	#endregion

}
}
