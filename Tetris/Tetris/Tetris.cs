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
		/// �t�H�[���쐬�����A����������
		/// </summary>
		public Tetris()
		{
			_data = new Data();

			// �f�B�X�v���C��������
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
				MessageBox.Show("��O�G���[���������܂���\r\n" + ex.Message, "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			_proc = new ProcBlock(_data);

			// �w�i�摜��`��
			Draw.MakeBackGround(_data);

			_proc.CreateNextBlock();        // Next�쐬�B
			_proc.CreateNextBlock();        // Next��Now�ɃR�s�[���āA�V����Next�쐬�B

			// �������͊������Ă��邩
			if (!_data.Initialized)
			{
				MessageBox.Show("�������������ł��܂���ł����B�@�A�v���P�[�V�������I�����܂��B", "Tetris", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				// �I������
				Dispose();
			}
			// �f�B�X�v���C��\��
			if (_FORM_MAIN != null) _FORM_MAIN.Show();
		}
		/// <summary>
		/// ���C�����[�v
		/// </summary>
		public void MainLoop()
		{
			var lastTime = DateTime.Now;
			while (_data.ContinueLoop)
			{
				// fps���Œ�
				while (true)
				{
					// 30ms�o�߂����烋�[�v�𔲂���B
					var time = DateTime.Now;
					var tspDiff = time - lastTime;
					if (30 < tspDiff.TotalMilliseconds)
					{
						lastTime = time;
						break;
					}
					System.Threading.Thread.Sleep(1);
				}

				// �u���b�N�f�[�^�X�V
				if (_data.stateApp == GameStatus.Playing)
				{
					_proc.UpdateBlock();
				}
				// �����I�ɍĕ`��
				var g = _FORM_MAIN.CreateGraphics();
				OnPaint(null, new PaintEventArgs(g, _FORM_MAIN.ClientRectangle));
				g.Dispose();

				Application.DoEvents();
			}
			// �I������
			Dispose();
		}
		/// <summary>
		/// ���\�[�X���
		/// </summary>
		private void Dispose()
		{
			// ����ȍ~�͍ĕ`��֎~
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
		/// ��ʏI��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClose(object sender, EventArgs e)
		{
			_data.ContinueLoop = false;
		}
		/// <summary>
		/// �ĕ`��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPaint(object sender, PaintEventArgs e)
		{
			Draw.DrawGame(_data, e);
		}

		/// <summary>
		/// �L�[������
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
						// �u���b�N����]
						_proc.Rotate();
						break;
					}
			}
		}
		/// <summary>
		/// �L�[���
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
						_data.KeyLeftPressed = false;   // ��
						break;
					}

				case Keys.Right: goto case Keys.NumPad6;
				case Keys.NumPad6:
					{
						_data.KeyRightPressed = false;  // ��
						break;
					}

				case Keys.Down: goto case Keys.NumPad2;
				case Keys.NumPad2:
					{
						_data.KeyDownPressed = false;   // ��
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
							// �Q�[�����~
							_data.stateApp = GameStatus.Over;
						}
						else
						{
							// �I��
							_data.ContinueLoop = false;
						}
						break;
					}
			}
		}
	}
}
