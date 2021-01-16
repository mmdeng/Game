using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tetris
{
	public class Draw
	{
		public Draw()
		{
		}

		/// <summary>
		/// �w�i�쐬�B�ŏ��Ɉ�񂾂��쐬���Ă����Adat.BMP_BACK�ɕێ����Ă����B
		/// </summary>
		/// <param name="dat">Data</param>
		public static void MakeBackGround(Data dat)
		{
			Rectangle rct;
			Brush brs;

			Graphics g = Graphics.FromImage(dat.BMP_BACK);

			// �w�i���N���A
			g.Clear(Color.Black);

			// �w�i�h��Ԃ�
			rct = dat.RCT_DISP.GetRect();
			brs = new HatchBrush(HatchStyle.HorizontalBrick, Color.Black, Color.DarkGreen);
			g.FillRectangle(brs, rct);
			brs.Dispose();

			// Field�̈�h��Ԃ�
			rct = dat.RCT_FIELD.GetRect();
			g.FillRectangle(Brushes.Black, rct);

			// Field�̈拫�E��
			rct.X--;
			rct.Y--;
			rct.Width++;
			rct.Height++;
			g.DrawRectangle(Pens.Green, rct);

			// Next�̈�h��Ԃ�
			rct = dat.RCT_NEXT.GetRect();
			brs = new LinearGradientBrush(rct, Color.Black, Color.DarkGreen, 45.0f);
			g.FillRectangle(brs, rct);
			brs.Dispose();

			// Next�̈拫�E��
			rct.X--;
			rct.Y--;
			rct.Width++;
			rct.Height++;
			g.DrawRectangle(Pens.Green, rct);

			// Score�̈�h��Ԃ�
			rct = dat.RCT_SCORE.GetRect();
			brs = new LinearGradientBrush(rct, Color.Black, Color.DarkGreen, 45.0f);
			g.FillRectangle(brs, rct);
			brs.Dispose();

			// Score�̈拫�E��
			rct.X--;
			rct.Y--;
			rct.Width++;
			rct.Height++;
			g.DrawRectangle(Pens.Green, rct);

			// �`�������
			g.Dispose();
		}
		/// <summary>
		/// �Q�[���f�[�^���ĕ`��
		/// </summary>
		/// <param name="dat">Data</param>
		/// <param name="e">OnPaint�p�����[�^</param>
		public static void DrawGame(Data dat, PaintEventArgs e)
		{
			string szDisp;

			if (dat.bDisposed) return;
			if (dat.BMP_BACK == null) return;
			if (dat.BMP_BLOCK == null) return;

			// �o�b�N�o�b�t�@���쐬
			Bitmap bmp = new Bitmap(dat.RCT_DISP.W, dat.RCT_DISP.H, e.Graphics);
			Graphics g = Graphics.FromImage(bmp);

			// �w�i��`��
			g.DrawImage(dat.BMP_BACK, 0, 0);

			// �`��°ٍ쐬
			Font font = new Font("Arial", 15.0f, FontStyle.Bold, GraphicsUnit.Pixel);
			Brush brs = new SolidBrush(Color.FromArgb(0x60, 0xc0, 0xff));

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			Rectangle rctField = dat.RCT_FIELD.GetRect();

			DrawScore(dat, g, brs, font);   // Score�`��

			switch (dat.stateApp)
			{
				// Gameover
				case Status.GameOver:

					dat.nFlashingCount = 0;

					dat.FIELDBLOCK.Clear();

					Font fontTmp = new Font("Arial", 24.0f, FontStyle.Bold, GraphicsUnit.Pixel);
					szDisp = "Game over!";
					g.DrawString(szDisp, fontTmp, Brushes.Red
						, rctField.Left + dat.RCT_FIELD.W / 2, dat.BLOCK_HEIGHT + 30, format);
					fontTmp.Dispose();

					// ���قɖ߂�
					goto case Status.Title;

				// Title
				case Status.Title:
					{
						g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

						szDisp = "-- TETRIS --\r\n\r\n";
						szDisp += "[Enter] : start,\r\n";
						szDisp += "[Esc] : quit.\r\n";
						szDisp += "[��] : MoveLeft\r\n";
						szDisp += "[��] : MoveRight\r\n";
						szDisp += "[��] : Down\r\n";
						szDisp += "[Space] : Rotate.\r\n";
						g.DrawString(szDisp, font, brs, dat.RCT_FIELD.GetRect(), format);
						break;
					}

				// Playing
				case Status.GamePlaying:
					{
						DrawFieldBlock(dat, g);     // FieldBlock�`��
						DrawFallingBlock(dat, g);       // FallingBlock�`��
						DrawNextBlock(dat, g);      // NextBlock�`��
						break;
					}
			}

			// �摜�]��
			e.Graphics.DrawImage(bmp, 0, 0);

			// ��؉��
			font.Dispose();
			brs.Dispose();
			g.Dispose();
			bmp.Dispose();
		}
		/// <summary>
		/// �X�R�A�`��
		/// </summary>
		/// <param name="dat">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		/// <param name="brs">Brush</param>
		/// <param name="font">Font</param>
		private static void DrawScore(Data dat, Graphics g, Brush brs, Font font)
		{
			string szDisp;

			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			float fX, fY;
			fX = (float)dat.RCT_SCORE.L;
			fY = (float)dat.RCT_SCORE.T;

			szDisp = "Score: " + dat.score.TotalScore.ToString();
			g.DrawString(szDisp, font, brs, fX, fY);

			szDisp = "Erased lines: " + dat.score.Lines.ToString();
			g.DrawString(szDisp, font, brs, fX, fY + 20);

			fX = (float)dat.RCT_SCORE.L + 10;

			szDisp = "Single: " + dat.score.Single.ToString();
			g.DrawString(szDisp, font, brs, fX, fY + 40);

			szDisp = "Double: " + dat.score.Double.ToString();
			g.DrawString(szDisp, font, brs, fX, fY + 60);

			szDisp = "Triple: " + dat.score.Triple.ToString();
			g.DrawString(szDisp, font, brs, fX, fY + 80);

			szDisp = "Tetris: " + dat.score.Tetris.ToString();
			g.DrawString(szDisp, font, brs, fX, fY + 100);
		}
		/// <summary>
		/// �t�B�[���h�u���b�N�`��
		/// </summary>
		/// <param name="dat">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		private static void DrawFieldBlock(Data dat, Graphics g)
		{
			int x, y;
			Rectangle rctDst;
			Rectangle rctSrc;
			for (x = 0; x < dat.X_MAX; x++)
			{
				for (y = 0; y < dat.Y_MAX; y++)
				{
					// �����Ȃ��ꏊ�͍��̂܂�
					if (dat.FIELDBLOCK[x, y] == 0) continue;

					// ������s���t���b�V��������
					if (dat.FIELDBLOCK[x, y] == FallingBlock.COLOR_INVALID
						&& dat.nFlashingCount > 0
						&& 5 <= dat.nFlashingCount % 10)
					{
						//5���C�����[�v���Ƃ�COLOR_INVALID�ŕ`�悷�邩�A�������Ȃ�������ւ��B
						continue;
					}

					rctDst = new Rectangle(
						dat.RCT_FIELD.L + x * dat.BLOCK_WIDTH,
						dat.RCT_FIELD.T + y * dat.BLOCK_HEIGHT,
						dat.BLOCK_WIDTH,
						dat.BLOCK_HEIGHT);

					rctSrc = GetBmpRect(dat, dat.FIELDBLOCK[x, y]);

					g.DrawImage(dat.BMP_BLOCK, rctDst, rctSrc, GraphicsUnit.Pixel);
				}
			}
		}
		/// <summary>
		/// Falling�u���b�N�`��
		/// </summary>
		/// <param name="dat">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		private static void DrawFallingBlock(Data dat, Graphics g)
		{
			if (dat.nFlashingCount > 0) return;

			int x, y;
			Rectangle rctDst;
			Rectangle rctSrc;

			for (x = 0; x < dat.fbNow.Width; x++)
			{
				for (y = 0; y < dat.fbNow.Height; y++)
				{
					if (dat.fbNow[x, y] == 0) continue;

					rctDst = new Rectangle(
						dat.RCT_FIELD.L + (dat.fbNow.X + x) * dat.BLOCK_WIDTH,
						dat.RCT_FIELD.T + (dat.fbNow.Y + y) * dat.BLOCK_HEIGHT,
						dat.BLOCK_WIDTH,
						dat.BLOCK_HEIGHT);

					rctSrc = GetBmpRect(dat, dat.fbNow[x, y]);

					g.DrawImage(dat.BMP_BLOCK, rctDst, rctSrc, GraphicsUnit.Pixel);
				}
			}
		}
		/// <summary>
		/// Next�u���b�N�`��
		/// </summary>
		/// <param name="dat">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		private static void DrawNextBlock(Data dat, Graphics g)
		{
			int x, y;
			Rectangle rctDst;
			Rectangle rctSrc;
			for (x = 0; x < dat.fbNext.Width; x++)
			{
				for (y = 0; y < dat.fbNext.Height; y++)
				{
					if (dat.fbNext[x, y] == 0) continue;

					rctDst = new Rectangle(
						dat.RCT_NEXT.L + x * dat.BLOCK_WIDTH,
						dat.RCT_NEXT.T + y * dat.BLOCK_HEIGHT,
						dat.BLOCK_WIDTH,
						dat.BLOCK_HEIGHT);

					rctSrc = GetBmpRect(dat, dat.fbNext[x, y]);

					g.DrawImage(dat.BMP_BLOCK, rctDst, rctSrc, GraphicsUnit.Pixel);
				}
			}
		}
		/// <summary>
		/// �u���b�N��Bitmap�t�@�C���̎w��̈��Ԃ�
		/// </summary>
		/// <param name="dat">Data</param>
		/// <param name="nBlockType">�u���b�N�^�C�v�C���f�b�N�X</param>
		/// <returns></returns>
		private static Rectangle GetBmpRect(Data dat, int nBlockType)
		{
			Rectangle rct;
			rct = new Rectangle(
				nBlockType * dat.BLOCK_WIDTH,
				0,
				dat.BLOCK_WIDTH,
				dat.BLOCK_HEIGHT);
			return rct;
		}
	}
}





