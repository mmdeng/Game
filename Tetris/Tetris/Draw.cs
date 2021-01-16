using System;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text;

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
		/// <param name="data">Data</param>
		public static void MakeBackGround(Data data)
		{
			Rectangle rectangle;
			Brush brush;

			var g = Graphics.FromImage(data.BACKGROUND_BITMAP);

			// �w�i���N���A
			g.Clear(Color.Black);

			// �w�i�h��Ԃ�
			rectangle = data.MAIN_AREA.GetRectangle();
			brush = new HatchBrush(HatchStyle.HorizontalBrick, Color.Black, Color.DarkGreen);
			g.FillRectangle(brush, rectangle);
			brush.Dispose();

			// Field�̈�h��Ԃ�
			rectangle = data.FIELD_AREA.GetRectangle();
			g.FillRectangle(Brushes.Black, rectangle);

			// Field�̈拫�E��
			rectangle.X--;
			rectangle.Y--;
			rectangle.Width++;
			rectangle.Height++;
			g.DrawRectangle(Pens.Green, rectangle);

			// Next�̈�h��Ԃ�
			rectangle = data.NEXT_AREA.GetRectangle();
			brush = new LinearGradientBrush(rectangle, Color.Black, Color.DarkGreen, 45.0f);
			g.FillRectangle(brush, rectangle);
			brush.Dispose();

			// Next�̈拫�E��
			rectangle.X--;
			rectangle.Y--;
			rectangle.Width++;
			rectangle.Height++;
			g.DrawRectangle(Pens.Green, rectangle);

			// Score�̈�h��Ԃ�
			rectangle = data.SCORE_AREA.GetRectangle();
			brush = new LinearGradientBrush(rectangle, Color.Black, Color.DarkGreen, 45.0f);
			g.FillRectangle(brush, rectangle);
			brush.Dispose();

			// Score�̈拫�E��
			rectangle.X--;
			rectangle.Y--;
			rectangle.Width++;
			rectangle.Height++;
			g.DrawRectangle(Pens.Green, rectangle);

			// �`�������
			g.Dispose();
		}
		/// <summary>
		/// �Q�[���f�[�^���ĕ`��
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="e">OnPaint�p�����[�^</param>
		public static void DrawGame(Data data, PaintEventArgs e)
		{
			if (data.Disposed) return;
			if (data.BACKGROUND_BITMAP == null) return;
			if (data.BLOCK_BITMAP == null) return;

			// �o�b�N�o�b�t�@���쐬
			var bitmap = new Bitmap(data.MAIN_AREA.W, data.MAIN_AREA.H, e.Graphics);
			var g = Graphics.FromImage(bitmap);

			// �w�i��`��
			g.DrawImage(data.BACKGROUND_BITMAP, 0, 0);

			// �`��°ٍ쐬
			var font = new Font("Arial", 15.0f, FontStyle.Bold, GraphicsUnit.Pixel);
			var brush = new SolidBrush(Color.FromArgb(0x60, 0xc0, 0xff));

            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var fieldRectangle = data.FIELD_AREA.GetRectangle();

			DrawScore(data, g, brush, font);   // Score�`��

			switch (data.stateApp)
			{
				// Gameover
				case GameStatus.Over:

					data.FlashingCount = 0;

					data.FIELD_BLOCK.Clear();

					Font fontTmp = new Font("Arial", 24.0f, FontStyle.Bold, GraphicsUnit.Pixel);
					var message = "Game over!";
					g.DrawString(message, fontTmp, Brushes.Red
						, fieldRectangle.Left + data.FIELD_AREA.W / 2, data.BLOCK_HEIGHT + 30, format);
					fontTmp.Dispose();

					// ���قɖ߂�
					goto case GameStatus.Title;

				// Title
				case GameStatus.Title:
					{
						g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
						g.DrawString(GetTitle(), font, brush, data.FIELD_AREA.GetRectangle(), format); ;
						break;
					}

				// Playing
				case GameStatus.Playing:
					{
						DrawFieldBlock(data, g);
						DrawFallingBlock(data, g);
						DrawNextBlock(data, g);
						break;
					}
			}
			// �摜�]��
			e.Graphics.DrawImage(bitmap, 0, 0);

			// ���������
			font.Dispose();
			brush.Dispose();
			g.Dispose();
			bitmap.Dispose();
		}
		private static string GetTitle()
		{
			var title = new StringBuilder();
			title.AppendLine("-- TETRIS --");
			title.AppendLine();
			title.AppendLine("[Enter] : start");
			title.AppendLine("[Esc] : quit");
			title.AppendLine("[��] : MoveLeft");
			title.AppendLine("[��] : MoveRight");
			title.AppendLine("[��] : Down");
			title.AppendLine("[Space] : Rotate");
			return title.ToString();
		}
		/// <summary>
		/// �X�R�A�`��
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		/// <param name="brush">Brush</param>
		/// <param name="font">Font</param>
		private static void DrawScore(Data data, Graphics g, Brush brush, Font font)
		{
			string message;

			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			float x = data.SCORE_AREA.L;
			float y = data.SCORE_AREA.T;

			message = "Score: " + data.score.TotalScore.ToString();
			g.DrawString(message, font, brush, x, y);

			message = "Erased lines: " + data.score.Lines.ToString();
			g.DrawString(message, font, brush, x, y + 20);

			x = (float)data.SCORE_AREA.L + 10;

			message = "Single: " + data.score.Single.ToString();
			g.DrawString(message, font, brush, x, y + 40);

			message = "Double: " + data.score.Double.ToString();
			g.DrawString(message, font, brush, x, y + 60);

			message = "Triple: " + data.score.Triple.ToString();
			g.DrawString(message, font, brush, x, y + 80);

			message = "Tetris: " + data.score.Tetris.ToString();
			g.DrawString(message, font, brush, x, y + 100);
		}
		/// <summary>
		/// �t�B�[���h�u���b�N�`��
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		private static void DrawFieldBlock(Data data, Graphics g)
		{
			int x, y;
			Rectangle target;
			Rectangle source;
			for (x = 0; x < data.X_MAX; x++)
			{
				for (y = 0; y < data.Y_MAX; y++)
				{
					// �����Ȃ��ꏊ�͍��̂܂�
					if (data.FIELD_BLOCK[x, y] == 0) continue;

					// ������s���t���b�V��������
					if (data.FIELD_BLOCK[x, y] == FallingBlock.COLOR_INVALID
						&& data.FlashingCount > 0
						&& 5 <= data.FlashingCount % 10)
					{
						//5���C�����[�v���Ƃ�COLOR_INVALID�ŕ`�悷�邩�A�������Ȃ�������ւ��B
						continue;
					}

					target = new Rectangle(
						data.FIELD_AREA.L + x * data.BLOCK_WIDTH,
						data.FIELD_AREA.T + y * data.BLOCK_HEIGHT,
						data.BLOCK_WIDTH,
						data.BLOCK_HEIGHT);

					source = GetBmpRect(data, data.FIELD_BLOCK[x, y]);
					g.DrawImage(data.BLOCK_BITMAP, target, source, GraphicsUnit.Pixel);
				}
			}
		}
		/// <summary>
		/// Falling�u���b�N�`��
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		private static void DrawFallingBlock(Data data, Graphics g)
		{
			if (data.FlashingCount > 0) return;

			int x, y;
			Rectangle target;
			Rectangle source;
			for (x = 0; x < data.CurrentBlock.Width; x++)
			{
				for (y = 0; y < data.CurrentBlock.Height; y++)
				{
					if (data.CurrentBlock[x, y] == 0) continue;

					target = new Rectangle(
						data.FIELD_AREA.L + (data.CurrentBlock.X + x) * data.BLOCK_WIDTH,
						data.FIELD_AREA.T + (data.CurrentBlock.Y + y) * data.BLOCK_HEIGHT,
						data.BLOCK_WIDTH,
						data.BLOCK_HEIGHT);

					source = GetBmpRect(data, data.CurrentBlock[x, y]);
					g.DrawImage(data.BLOCK_BITMAP, target, source, GraphicsUnit.Pixel);
				}
			}
		}
		/// <summary>
		/// Next�u���b�N�`��
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="g">�`��O���t�B�b�N</param>
		private static void DrawNextBlock(Data data, Graphics g)
		{
			int x, y;
			Rectangle target;
			Rectangle source;
			for (x = 0; x < data.NextBlock.Width; x++)
			{
				for (y = 0; y < data.NextBlock.Height; y++)
				{
					if (data.NextBlock[x, y] == 0) continue;

					target = new Rectangle(
						data.NEXT_AREA.L + x * data.BLOCK_WIDTH,
						data.NEXT_AREA.T + y * data.BLOCK_HEIGHT,
						data.BLOCK_WIDTH,
						data.BLOCK_HEIGHT);

					source = GetBmpRect(data, data.NextBlock[x, y]);
					g.DrawImage(data.BLOCK_BITMAP, target, source, GraphicsUnit.Pixel);
				}
			}
		}
		/// <summary>
		/// �u���b�N��Bitmap�t�@�C���̎w��̈��Ԃ�
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="type">�u���b�N�^�C�v�C���f�b�N�X</param>
		/// <returns></returns>
		private static Rectangle GetBmpRect(Data data, int type)
		{
			return new Rectangle(
				type * data.BLOCK_WIDTH,
				0,
				data.BLOCK_WIDTH,
				data.BLOCK_HEIGHT);
		}
	}
}





