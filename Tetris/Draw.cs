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

		//========================================================================================
		// Name		: MakeBackGround
		// Function	: ”wŒiì¬BÅ‰‚Éˆê‰ñ‚¾‚¯ì¬‚µ‚Ä‚¨‚«Adat.BMP_BACK‚É•Û‚µ‚Ä‚¨‚­B
		//
		// Parameter	| Format			|i/o| Description
		//----------------------------------------------------------------------------------------
		// dat			| Data				| i | Data
		//----------------------------------------------------------------------------------------
		// Return		| void				| o | Nothing
		//========================================================================================
		public static void MakeBackGround( Data dat )
		{
			Rectangle rct;
			Brush brs;

			Graphics g = Graphics.FromImage( dat.BMP_BACK );
		
			// ”wŒi‚ğƒNƒŠƒA
			g.Clear( Color.Black );

			// ”wŒi“h‚è‚Â‚Ô‚µ
			rct = dat.RCT_DISP.GetRect();
			brs = new HatchBrush( HatchStyle.HorizontalBrick, Color.Black, Color.DarkGreen );
			g.FillRectangle( brs, rct );
			brs.Dispose();

			// Field—Ìˆæ“h‚è‚Â‚Ô‚µ
			rct = dat.RCT_FIELD.GetRect();
			g.FillRectangle( Brushes.Black, rct );

			// Field—Ìˆæ‹«ŠEü
			rct.X--;
			rct.Y--;
			rct.Width++;
			rct.Height++;
			g.DrawRectangle( Pens.Green, rct );

			// Next—Ìˆæ“h‚è‚Â‚Ô‚µ
			rct = dat.RCT_NEXT.GetRect();
			brs = new LinearGradientBrush( rct,	Color.Black, Color.DarkGreen, 45.0f );
			g.FillRectangle( brs, rct ); 
			brs.Dispose();

			// Next—Ìˆæ‹«ŠEü
			rct.X--;
			rct.Y--;
			rct.Width++;
			rct.Height++;
			g.DrawRectangle( Pens.Green, rct ); 

			// Score—Ìˆæ“h‚è‚Â‚Ô‚µ
			rct = dat.RCT_SCORE.GetRect();
			brs = new LinearGradientBrush( rct, Color.Black, Color.DarkGreen, 45.0f );
			g.FillRectangle( brs, rct ); 
			brs.Dispose();

			// Score—Ìˆæ‹«ŠEü
			rct.X--;
			rct.Y--;
			rct.Width++;
			rct.Height++;
			g.DrawRectangle( Pens.Green, rct );

			// •`‰æ‚ğŠ®—¹
			g.Dispose();
		}
		//========================================================================================
		// Name		: DrawGame
		// Function	: ¹Ş°ÑÃŞ°À‚ğÄ•`‰æ
		//
		// Parameter	| Format			|i/o| Description
		//----------------------------------------------------------------------------------------
		// dat			| Data				| i | Data
		// e			| PaintEventArgs	| i | OnPaintÊß×Ò°À
		//----------------------------------------------------------------------------------------
		// Return		| void				| o | Nothing
		//========================================================================================
		public static void DrawGame( Data dat, PaintEventArgs e )
		{
			string szDisp;

			if ( dat.bDisposed )			return;
			if ( dat.BMP_BACK == null )		return;
			if ( dat.BMP_BLOCK == null )	return;

			// ƒoƒbƒNƒoƒbƒtƒ@‚ğì¬
			Bitmap bmp = new Bitmap( dat.RCT_DISP.W, dat.RCT_DISP.H, e.Graphics );
			Graphics g = Graphics.FromImage( bmp );

			// ”wŒi‚ğ•`‰æ
			g.DrawImage( dat.BMP_BACK, 0, 0 );

			// •`‰æÂ°Ùì¬
			Font font = new Font( "Arial", 15.0f, FontStyle.Bold, GraphicsUnit.Pixel );
			Brush brs = new SolidBrush( Color.FromArgb( 0x60, 0xc0, 0xff ) );

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			Rectangle rctField = dat.RCT_FIELD.GetRect();

			DrawScore( dat, g, brs, font );	// Score•`‰æ

			switch( dat.stateApp )
			{
	//-------------------------------------------------------------------
	// Gameover
				case Status.GameOver:
					
					dat.nFlashingCount = 0;

					dat.FIELDBLOCK.Clear();

					Font fontTmp = new Font( "Arial", 24.0f, FontStyle.Bold, GraphicsUnit.Pixel );
					szDisp = "Game over!";
					g.DrawString( szDisp, fontTmp, Brushes.Red
						, rctField.Left + dat.RCT_FIELD.W / 2, dat.BLOCK_HEIGHT + 30, format );
					fontTmp.Dispose();

					// À²ÄÙ‚É–ß‚é
					goto case Status.Title;
				
	//-------------------------------------------------------------------
	// Title
				case Status.Title:
				{
					g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

					szDisp = "-- TETRIS --\r\n\r\n";
					szDisp += "[Enter] : start,\r\n";
					szDisp += "[Esc] : quit.\r\n";
					szDisp += "[©] : MoveLeft\r\n";
					szDisp += "[¨] : MoveRight\r\n";
					szDisp += "[«] : Down\r\n";
					szDisp += "[Space] : Rotate.\r\n";
					g.DrawString( szDisp, font, brs, dat.RCT_FIELD.GetRect(), format );
					break;
				}

	//-------------------------------------------------------------------
	// Playing
				case Status.GamePlaying:
				{
					DrawFieldBlock( dat, g );		// FieldBlock•`‰æ
					DrawFallingBlock( dat, g );		// FallingBlock•`‰æ
					DrawNextBlock( dat, g );		// NextBlock•`‰æ
					break;
				}
			}

			// ‰æ‘œ“]‘—
			e.Graphics.DrawImage( bmp, 0, 0 );

			// ÒÓØ‰ğ•ú
			font.Dispose();
			brs.Dispose();
			g.Dispose();
			bmp.Dispose();
		}
		//========================================================================================
		// Name		: DrawScore
		// Function	: ½º±•`‰æ
		//
		// Parameter	| Format			|i/o| Description
		//----------------------------------------------------------------------------------------
		// dat			| Data				| i | Data
		// g			| Graphics			| i | •`‰æ¸Ş×Ì¨¯¸
		// brs			| Brush				| i | Brush
		// font			| Font				| i | Font
		//----------------------------------------------------------------------------------------
		// Return		| void				| o | Nothing
		//========================================================================================
		private static void DrawScore( Data dat, Graphics g, Brush brs, Font font )
		{
			string szDisp;

			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			float fX,fY;
			fX = ( float )dat.RCT_SCORE.L;
			fY = ( float )dat.RCT_SCORE.T;

			szDisp = "Score: " + dat.score.TotalScore.ToString();
			g.DrawString( szDisp, font, brs, fX, fY );

			szDisp = "Erased lines: " + dat.score.Lines.ToString();
			g.DrawString( szDisp, font, brs, fX, fY + 20 );

			fX = ( float )dat.RCT_SCORE.L + 10;

			szDisp = "Single: " + dat.score.Single.ToString();
			g.DrawString( szDisp, font, brs, fX, fY + 40 );

			szDisp = "Double: " + dat.score.Double.ToString();
			g.DrawString( szDisp, font, brs, fX, fY + 60 );

			szDisp = "Triple: " + dat.score.Triple.ToString();
			g.DrawString( szDisp, font, brs, fX, fY + 80 );

			szDisp = "Tetris: " + dat.score.Tetris.ToString();
			g.DrawString( szDisp, font, brs, fX, fY + 100 );
		}
		//========================================================================================
		// Name		: DrawFieldBlock
		// Function	: Ì¨°ÙÄŞÌŞÛ¯¸•`‰æ
		//
		// Parameter	| Format			|i/o| Description
		//----------------------------------------------------------------------------------------
		// dat			| Data				| i | Data
		// g			| Graphics			| i | •`‰æ¸Ş×Ì¨¯¸
		//----------------------------------------------------------------------------------------
		// Return		| void				| o | Nothing
		//========================================================================================
		private static void DrawFieldBlock( Data dat, Graphics g )
		{
			int x,y;
			Rectangle rctDst;
			Rectangle rctSrc;
			for( x = 0; x < dat.X_MAX; x++ )
			{
				for( y = 0; y < dat.Y_MAX; y++ )
				{
					// ‰½‚à‚È‚¢êŠ‚Í•‚Ì‚Ü‚Ü
					if( dat.FIELDBLOCK[ x, y ] == 0 ) continue;

					// Á‚¦‚és‚ğƒtƒ‰ƒbƒVƒ…‚³‚¹‚é
					if( dat.FIELDBLOCK[ x, y ] == FallingBlock.COLOR_INVALID
						&& dat.nFlashingCount>0
						&& 5 <= dat.nFlashingCount % 10 )
					{
						//5Ò²İÙ°Ìß‚²‚Æ‚ÉCOLOR_INVALID‚Å•`‰æ‚·‚é‚©A‰½‚à‚µ‚È‚¢‚©“ü‚ê‘Ö‚í‚éB
						continue;
					}

					rctDst = new Rectangle( 
						dat.RCT_FIELD.L + x * dat.BLOCK_WIDTH,
						dat.RCT_FIELD.T + y * dat.BLOCK_HEIGHT,
						dat.BLOCK_WIDTH,
						dat.BLOCK_HEIGHT );

					rctSrc = GetBmpRect( dat, dat.FIELDBLOCK[x, y] );

					g.DrawImage( dat.BMP_BLOCK, rctDst, rctSrc, GraphicsUnit.Pixel );
				}
			}
		}
		//========================================================================================
		// Name		: GetBmpRect
		// Function	: FallingÌŞÛ¯¸•`‰æ
		//
		// Parameter	| Format			|i/o| Description
		//----------------------------------------------------------------------------------------
		// dat			| Data				| i | Data
		// g			| Graphics			| i | •`‰æ¸Ş×Ì¨¯¸
		//----------------------------------------------------------------------------------------
		// Return		| void				| o | Nothing
		//========================================================================================
		private static void DrawFallingBlock( Data dat, Graphics g )
		{
			if( dat.nFlashingCount > 0 )	return;

			int x,y;
			Rectangle rctDst;
			Rectangle rctSrc;

			for( x = 0; x < dat.fbNow.Width; x++ )
			{
				for( y = 0; y < dat.fbNow.Height; y++ )
				{
					if ( dat.fbNow[x, y] == 0 ) continue;

					rctDst = new Rectangle( 
						dat.RCT_FIELD.L + ( dat.fbNow.X + x ) * dat.BLOCK_WIDTH,
						dat.RCT_FIELD.T + ( dat.fbNow.Y + y ) * dat.BLOCK_HEIGHT,
						dat.BLOCK_WIDTH,
						dat.BLOCK_HEIGHT );

					rctSrc = GetBmpRect( dat, dat.fbNow[x, y] );
			
					g.DrawImage( dat.BMP_BLOCK, rctDst, rctSrc, GraphicsUnit.Pixel );
				}
			}
		}
		//========================================================================================
		// Name		: DrawNextBlock
		// Function	: NextÌŞÛ¯¸•`‰æ
		//
		// Parameter	| Format			|i/o| Description
		//----------------------------------------------------------------------------------------
		// dat			| Data				| i | Data
		// g			| Graphics			| i | •`‰æ¸Ş×Ì¨¯¸
		//----------------------------------------------------------------------------------------
		// Return		| void				| o | Nothing
		//========================================================================================
		private static void DrawNextBlock( Data dat, Graphics g )
		{
			int x,y;
			Rectangle rctDst;
			Rectangle rctSrc;
			for( x = 0; x < dat.fbNext.Width; x++ )
			{
				for( y = 0; y < dat.fbNext.Height; y++ )
				{
					if( dat.fbNext[x, y] == 0 ) continue;

					rctDst = new Rectangle( 
						dat.RCT_NEXT.L + x * dat.BLOCK_WIDTH,
						dat.RCT_NEXT.T + y * dat.BLOCK_HEIGHT,
						dat.BLOCK_WIDTH,
						dat.BLOCK_HEIGHT );

					rctSrc = GetBmpRect( dat, dat.fbNext[x, y] );

					g.DrawImage( dat.BMP_BLOCK, rctDst, rctSrc, GraphicsUnit.Pixel );
				}
			}
		}
		//========================================================================================
		// Name		: GetBmpRect
		// Function	: ÌŞÛ¯¸‚ÌBitmapÌ§²Ù‚Ìw’è—Ìˆæ‚ğ•Ô‚·
		//
		// Parameter	| Format			|i/o| Description
		//----------------------------------------------------------------------------------------
		// dat			| Data				| i | Data
		// nBlockType	| int				| i | ÌŞÛ¯¸À²Ìß²İÃŞ¯¸½
		//----------------------------------------------------------------------------------------
		// Return		| Rectangle			| o | Nothing
		//========================================================================================
		private static Rectangle GetBmpRect( Data dat, int nBlockType )
		{
			Rectangle rct;
			rct = new Rectangle( 
				nBlockType * dat.BLOCK_WIDTH,
				0,
				dat.BLOCK_WIDTH,
				dat.BLOCK_HEIGHT );
			return rct;
		}
	}
}





