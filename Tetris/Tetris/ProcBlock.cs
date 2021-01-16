using System;

namespace Tetris
{
	public class ProcBlock
	{
		private Data dat;

		private int _nMoveLeftCount;        // ���ړ��X�s�[�h
		private int _nMoveRightCount;       // �E�ړ��X�s�[�h
		private int _nForcedFallCount;      // ���������X�s�[�h
		private int _nNaturalFallCount;     // ���R�����X�s�[�h

		private FormMain FORM_MAIN;

		/// <summary>
		/// Constructor
		/// </summary>
		public ProcBlock(Data tmp, FormMain frm)
		{
			FORM_MAIN = frm;
			dat = tmp;

			_nMoveLeftCount = 0;
			_nMoveRightCount = 0;

			_nForcedFallCount = 0;
			_nNaturalFallCount = 0;
		}
		/// <summary>
		/// ���̗����u���b�N���쐬
		/// </summary>
		public void CreateNextBlock()
		{
			// Next�u���b�N�̃C���X�^���X�����݂̃C���X�^���X�Ƃ���B
			dat.fbNow = dat.fbNext;

			// ���̃u���b�N�^�C�v�𗐐��Ō��߂�
			Random rand = new Random();
			int nType = rand.Next(0, FallingBlock.BLOCKTYPES);

			//Next�u���b�N�̃C���X�^���X�쐬
			dat.fbNext = new FallingBlock(nType);

			// �����_���ȉ񐔉�]
			int nRotate = rand.Next(4);
			for (int i = 0; i < nRotate; i++)
			{
				dat.fbNext = dat.fbNext.Rotate();
			}

			if (dat.fbNow != null)
			{
				// ��ԏ�̐^�����痎���Ă���B
				dat.fbNow.X = (dat.X_MAX - dat.fbNow.Width) / 2;
				dat.fbNow.Y = 0;

				// �����t�B�[���h�̃u���b�N�ɐڐG������Q�[���I�[�o�[
				if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X, dat.fbNow.Y))
				{
					dat.stateApp = Status.GameOver;
				}
			}
		}
		/// <summary>
		/// Block�����X�V
		/// </summary>
		public void UpdateBlock()
		{
			if (dat.nFlashingCount > 0)
			{
				FlashBlock();
			}
			else
			{
				MoveBlock();
			}
		}
		/// <summary>
		/// �u���b�N���t���b�V��
		/// </summary>
		private void FlashBlock()
		{
			dat.nFlashingCount--;
			if (dat.nFlashingCount > 0) return; // �t���b�V�����B���̊ԃu���b�N�͓����Ȃ��B

			// �t���b�V���I��
			dat.nFlashingCount = 0;

			// �������s�����Ă��ăX�R�A�ɔ��f������B
			int nErasedLines = 0;
			for (int y = dat.Y_MAX - 1; y >= 0; y--)
			{
				if (dat.FIELDBLOCK[0, y] == FallingBlock.COLOR_INVALID)
				{
					// �s���������ĉ��ɋl�߂�
					dat.FIELDBLOCK.EraseLine(y);

					// ���������s
					nErasedLines++;

					// �������s���ă`�F�b�N
					y++;
				}
			}
			// �X�R�A�v�Z
			CalculateScore(nErasedLines);

			// ���̃u���b�N���쐬���ݒ�
			CreateNextBlock();
		}
		/// <summary>
		/// �u���b�N���ړ�
		/// </summary>
		private void MoveBlock()
		{
			const int FORCED_FALL_SPEED = 2;
			const int NATURAL_FALL_SPEED = 8;

			if (dat.bKeyLeftPressed == true)
			{
				_nMoveLeftCount++;
				if (FORCED_FALL_SPEED < _nMoveLeftCount)
				{
					_nMoveLeftCount = 0;
					MoveLeft();
				}
			}
			if (dat.bKeyRightPressed == true)
			{
				_nMoveRightCount++;
				if (FORCED_FALL_SPEED < _nMoveRightCount)
				{
					_nMoveRightCount = 0;
					MoveRight();
				}
			}
			//�{�^���������Ă��邩�ǂ����Ɋւ�炸�A1��͉��ɗ�����B
			bool bLanded = false;
			if (dat.bKeyDownPressed == true)
			{
				// ��������
				_nForcedFallCount++;
				if (FORCED_FALL_SPEED < _nForcedFallCount)
				{
					_nForcedFallCount = 0;

					bLanded = !MoveDown();
					if (bLanded == false)
					{
						dat.score.TotalScore += 1;  // ���ɐi�߂��1�_����
					}
				}
			}
			else
			{
				// ���R����
				_nNaturalFallCount++;
				if (NATURAL_FALL_SPEED < _nNaturalFallCount)
				{
					_nNaturalFallCount = 0;

					// ����������
					bLanded = !MoveDown();
				}
			}

			// ���n����
			if (bLanded == true) Landed();
		}
		/// <summary>
		/// ���n����
		/// </summary>
		private void Landed()
		{
			// �t�B�[���h�̃u���b�N�Ƃ���
			dat.FIELDBLOCK.Paste(dat.fbNow, dat.fbNow.X, dat.fbNow.Y);

			int nFlachLines = 0;
			for (int y = 0; y < dat.Y_MAX; y++)
			{
				// �s�S�̂��u���b�N�Ŗ��ߐs������Ă���ꍇ
				if (dat.FIELDBLOCK.IsLineFilled(y) == true)
				{
					//��s�ł�������s����������t���b�V������B
					dat.nFlashingCount = 40;

					// �Y���s�̃u���b�N��COLOR_INVALID�œh��Ԃ�
					dat.FIELDBLOCK.FillLine(y, FallingBlock.COLOR_INVALID);

					nFlachLines++;
				}
			}
			switch (nFlachLines)
			{
				case 0: Music.StartWav("Land.wav"); break;
				case 1: Music.StartWav("Flash1.wav"); break;
				case 2: Music.StartWav("Flash2.wav"); break;
				case 3: Music.StartWav("Flash3.wav"); break;
				case 4: Music.StartWav("Flash4.wav"); break;
				default: break;
			}

			// ������s���Ȃ��������͎��̃u���b�N���쐬����B
			if (dat.nFlashingCount == 0)
			{
				CreateNextBlock();
			}
		}
		/// <summary>
		/// �u���b�N�����ֈړ�
		/// </summary>
		/// <returns>True:�ړ������@�@False:�ړ����s</returns>
		private bool MoveLeft()
		{
			// �ǂɐڂ��Ă�����ړ��ł��Ȃ��B
			if (dat.fbNow.X + dat.fbNow.LeftEdge().X <= 0)
			{
				return false;
			}

			// Field�u���b�N�Ɛڂ��Ă���ꍇ�͈ړ��ł��Ȃ��B
			if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X - 1, dat.fbNow.Y))
			{
				return false;
			}

			//�ړ����s
			dat.fbNow.X--;
			return true;
		}
		/// <summary>
		/// �u���b�N���E�ֈړ�
		/// </summary>
		/// <returns>True:�ړ������@�@False:�ړ����s</returns>
		private bool MoveRight()
		{
			// �ǂɐڂ��Ă�����ړ��ł��Ȃ��B
			if (dat.X_MAX - 1 <= dat.fbNow.X + dat.fbNow.RightEdge().X)
			{
				return false;
			}

			// Field�u���b�N�Ɛڂ��Ă���ꍇ�͈ړ��ł��Ȃ��B
			if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X + 1, dat.fbNow.Y))
			{
				return false;
			}
			//�ړ����s
			dat.fbNow.X++;
			return true;
		}
		/// <summary>
		/// �u���b�N�����ֈړ�
		/// </summary>
		/// <returns>True:�ړ������@�@False:�ړ����s(���n����)</returns>
		private bool MoveDown()
		{
			// �ǂɐڂ��Ă�����ړ��ł��Ȃ��B
			if (dat.Y_MAX - 1 <= dat.fbNow.Y + dat.fbNow.BottomEdge().Y)
			{
				return false;
			}

			// Field�u���b�N�Ɛڂ��Ă���ꍇ�͈ړ��ł��Ȃ��B
			if (dat.FIELDBLOCK.IsPiledUp(dat.fbNow, dat.fbNow.X, dat.fbNow.Y + 1))
			{
				return false;
			}

			dat.fbNow.Y++;
			return true;
		}
		/// <summary>
		/// �����u���b�N����]����
		/// </summary>
		public void Rotate()
		{
			if (dat.nFlashingCount > 0) return;

			// ��]��̃u���b�N���擾
			FallingBlock fbClone = dat.fbNow.Rotate();

			// �ǂ���͂ݏo��ꍇ�͉�]�ł��Ȃ�
			if (fbClone.X + fbClone.LeftEdge().X < 0) return;
			if (dat.X_MAX <= dat.fbNow.X + fbClone.RightEdge().X) return;
			if (dat.Y_MAX <= dat.fbNow.Y + fbClone.BottomEdge().Y) return;

			// Field�u���b�N�Əd�Ȃ荇�����ꍇ�͉�]�ł��Ȃ�
			if (dat.FIELDBLOCK.IsPiledUp(fbClone, fbClone.X, fbClone.Y))
			{
				return;
			}

			// ��]�����B��]��̃C���X�^���X��ݒ�
			dat.fbNow = fbClone;
		}
		/// <summary>
		/// �������s������X�R�A�v�Z
		/// </summary>
		/// <param name="nErasedLines">�������s��</param>
		private void CalculateScore(int nErasedLines)
		{
			switch (nErasedLines)
			{
				case 1: dat.score.Single++; dat.score.TotalScore += 100; break;
				case 2: dat.score.Double++; dat.score.TotalScore += 200; break;
				case 3: dat.score.Triple++; dat.score.TotalScore += 500; break;
				case 4: dat.score.Tetris++; dat.score.TotalScore += 1000; break;
			}
			dat.score.Lines += nErasedLines;
		}
	}
}
