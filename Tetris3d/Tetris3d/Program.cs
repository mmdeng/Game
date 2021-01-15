using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault( false );
			//Application.Run( new FormMain() );

			FormDirectx form = new FormMain();
			DirectxMainLoop loop = new DirectxMainLoop(form);
		}
	}
}