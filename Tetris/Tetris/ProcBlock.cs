using System;

namespace Tetris
{
	public class ProcBlock
	{
		private readonly Data data;

		/// <summary>
		/// ���ړ��X�s�[�h
		/// </summary>
		private int _moveLeftCount;
		/// <summary>
		/// �E�ړ��X�s�[�h
		/// </summary>
		private int _moveRightCount;
		/// <summary>
		/// ���������X�s�[�h
		/// </summary>
		private int _forcedFallCount;
		/// <summary>
		/// ���R�����X�s�[�h
		/// </summary>
		private int _naturalFallCount;

		/// <summary>
		/// Constructor
		/// </summary>
		public ProcBlock(Data data)
		{
			this.data = data;

			_moveLeftCount = 0;
			_moveRightCount = 0;

			_forcedFallCount = 0;
			_naturalFallCount = 0;
		}
		/// <summary>
		/// ���̗����u���b�N���쐬
		/// </summary>
		public void CreateNextBlock()
		{
			// Next�u���b�N�̃C���X�^���X�����݂̃C���X�^���X�Ƃ���B
			data.CurrentBlock = data.NextBlock;

			// ���̃u���b�N�^�C�v�𗐐��Ō��߂�
			var rand = new Random();
			var type = rand.Next(0, FallingBlock.BLOCKTYPES);

			//Next�u���b�N�̃C���X�^���X�쐬
			data.NextBlock = new FallingBlock(type);

			// �����_���ȉ񐔉�]
			var rotate = rand.Next(4);
			for (var i = 0; i < rotate; i++)
			{
				data.NextBlock = data.NextBlock.Rotate();
			}

			if (data.CurrentBlock != null)
			{
				// ��ԏ�̐^�����痎���Ă���B
				data.CurrentBlock.X = (data.X_MAX - data.CurrentBlock.Width) / 2;
				data.CurrentBlock.Y = 0;

				// �����t�B�[���h�̃u���b�N�ɐڐG������Q�[���I�[�o�[
				if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X, data.CurrentBlock.Y))
				{
					data.stateApp = GameStatus.Over;
				}
			}
		}
		/// <summary>
		/// Block�����X�V
		/// </summary>
		public void UpdateBlock()
		{
			if (data.FlashingCount > 0)
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
			data.FlashingCount--;
			if (data.FlashingCount > 0) return; // �t���b�V�����B���̊ԃu���b�N�͓����Ȃ��B

			// �t���b�V���I��
			data.FlashingCount = 0;

			// �������s�����Ă��ăX�R�A�ɔ��f������B
			var erasedLines = 0;
			for (var y = data.Y_MAX - 1; y >= 0; y--)
			{
				if (data.FIELD_BLOCK[0, y] == FallingBlock.COLOR_INVALID)
				{
					// �s���������ĉ��ɋl�߂�
					data.FIELD_BLOCK.EraseLine(y);

					// ���������s
					erasedLines++;

					// �������s���ă`�F�b�N
					y++;
				}
			}
			// �X�R�A�v�Z
			CalculateScore(erasedLines);

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

			if (data.KeyLeftPressed)
			{
				_moveLeftCount++;
				if (FORCED_FALL_SPEED < _moveLeftCount)
				{
					_moveLeftCount = 0;
					MoveLeft();
				}
			}
			if (data.KeyRightPressed)
			{
				_moveRightCount++;
				if (FORCED_FALL_SPEED < _moveRightCount)
				{
					_moveRightCount = 0;
					MoveRight();
				}
			}
			//�{�^���������Ă��邩�ǂ����Ɋւ�炸�A1��͉��ɗ�����B
			var isLanded = false;
			if (data.KeyDownPressed)
			{
				// ��������
				_forcedFallCount++;
				if (FORCED_FALL_SPEED < _forcedFallCount)
				{
					_forcedFallCount = 0;

					isLanded = !MoveDown();
					if (!isLanded)
					{
						data.score.TotalScore += 1;  // ���ɐi�߂��1�_����
					}
				}
			}
			else
			{
				// ���R����
				_naturalFallCount++;
				if (NATURAL_FALL_SPEED < _naturalFallCount)
				{
					_naturalFallCount = 0;

					// ����������
					isLanded = !MoveDown();
				}
			}
			// ���n����
			if (isLanded) Landed();
		}
		/// <summary>
		/// ���n����
		/// </summary>
		private void Landed()
		{
			// �t�B�[���h�̃u���b�N�Ƃ���
			data.FIELD_BLOCK.Paste(data.CurrentBlock, data.CurrentBlock.X, data.CurrentBlock.Y);

			var flashingLines = 0;
			for (var y = 0; y < data.Y_MAX; y++)
			{
				// �s�S�̂��u���b�N�Ŗ��ߐs������Ă���ꍇ
				if (data.FIELD_BLOCK.IsLineFilled(y))
				{
					//��s�ł�������s����������t���b�V������B
					data.FlashingCount = 40;

					// �Y���s�̃u���b�N��COLOR_INVALID�œh��Ԃ�
					data.FIELD_BLOCK.FillLine(y, FallingBlock.COLOR_INVALID);

					flashingLines++;
				}
			}
			switch (flashingLines)
			{
				case 0: Music.StartWav("Land.wav"); break;
				case 1: Music.StartWav("Flash1.wav"); break;
				case 2: Music.StartWav("Flash2.wav"); break;
				case 3: Music.StartWav("Flash3.wav"); break;
				case 4: Music.StartWav("Flash4.wav"); break;
				default: break;
			}

			// ������s���Ȃ��������͎��̃u���b�N���쐬����B
			if (data.FlashingCount == 0)
			{
				CreateNextBlock();
			}
		}
		/// <summary>
		/// �u���b�N�����ֈړ�
		/// </summary>
		/// <returns>True:�ړ�����, False:�ړ����s</returns>
		private bool MoveLeft()
		{
			// �ǂɐڂ��Ă�����ړ��ł��Ȃ��B
			if (data.CurrentBlock.X + data.CurrentBlock.LeftEdge().X <= 0)
			{
				return false;
			}

			// Field�u���b�N�Ɛڂ��Ă���ꍇ�͈ړ��ł��Ȃ��B
			if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X - 1, data.CurrentBlock.Y))
			{
				return false;
			}

			//�ړ����s
			data.CurrentBlock.X--;
			return true;
		}
		/// <summary>
		/// �u���b�N���E�ֈړ�
		/// </summary>
		/// <returns>True:�ړ�����, False:�ړ����s</returns>
		private bool MoveRight()
		{
			// �ǂɐڂ��Ă�����ړ��ł��Ȃ��B
			if (data.X_MAX - 1 <= data.CurrentBlock.X + data.CurrentBlock.RightEdge().X)
			{
				return false;
			}

			// Field�u���b�N�Ɛڂ��Ă���ꍇ�͈ړ��ł��Ȃ��B
			if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X + 1, data.CurrentBlock.Y))
			{
				return false;
			}
			//�ړ����s
			data.CurrentBlock.X++;
			return true;
		}
		/// <summary>
		/// �u���b�N�����ֈړ�
		/// </summary>
		/// <returns>True:�ړ�����, False:�ړ����s</returns>
		private bool MoveDown()
		{
			// �ǂɐڂ��Ă�����ړ��ł��Ȃ��B
			if (data.Y_MAX - 1 <= data.CurrentBlock.Y + data.CurrentBlock.BottomEdge().Y)
			{
				return false;
			}

			// Field�u���b�N�Ɛڂ��Ă���ꍇ�͈ړ��ł��Ȃ��B
			if (data.FIELD_BLOCK.IsPiledUp(data.CurrentBlock, data.CurrentBlock.X, data.CurrentBlock.Y + 1))
			{
				return false;
			}

			data.CurrentBlock.Y++;
			return true;
		}
		/// <summary>
		/// �����u���b�N����]����
		/// </summary>
		public void Rotate()
		{
			if (data.FlashingCount > 0) return;

			// ��]��̃u���b�N���擾
			var clone = data.CurrentBlock.Rotate();

			// �ǂ���͂ݏo��ꍇ�͉�]�ł��Ȃ�
			if (clone.X + clone.LeftEdge().X < 0) return;
			if (data.X_MAX <= data.CurrentBlock.X + clone.RightEdge().X) return;
			if (data.Y_MAX <= data.CurrentBlock.Y + clone.BottomEdge().Y) return;

			// Field�u���b�N�Əd�Ȃ荇�����ꍇ�͉�]�ł��Ȃ�
			if (data.FIELD_BLOCK.IsPiledUp(clone, clone.X, clone.Y))
			{
				return;
			}
			// ��]�����B��]��̃C���X�^���X��ݒ�
			data.CurrentBlock = clone;
		}
		/// <summary>
		/// �������s������X�R�A�v�Z
		/// </summary>
		/// <param name="erasedLines">�������s��</param>
		private void CalculateScore(int erasedLines)
		{
			switch (erasedLines)
			{
				case 1: data.score.Single++; data.score.TotalScore += 100; break;
				case 2: data.score.Double++; data.score.TotalScore += 200; break;
				case 3: data.score.Triple++; data.score.TotalScore += 500; break;
				case 4: data.score.Tetris++; data.score.TotalScore += 1000; break;
			}
			data.score.Lines += erasedLines;
		}
	}
}
