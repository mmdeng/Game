using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public class Bgm : Soundx
	{
		public FormWmp Wmp
		{
			get
			{
				return _formWmp;
			}
		}
		protected FormWmp _formWmp;

		public Bgm(Control control)
			: base(control)
		{
			this.AddWav(@"Material\Land.wav");
			this.AddWav(@"Material\Flash1.wav");
			this.AddWav(@"Material\Flash2.wav");
			this.AddWav(@"Material\Flash3.wav");
			this.AddWav(@"Material\Flash4.wav");

			_formWmp = new FormWmp();
			_formWmp.Hide();

			_formWmp.Add(@"./Material/Bgm1.mp3");
			_formWmp.Add(@"./Material/Bgm2.mp3");
			_formWmp.Add(@"./Material/Bgm3.mp3");
			_formWmp.Add(@"./Material/Bgm4.mp3");
			_formWmp.Add(@"./Material/Bgm5.mp3");
			_formWmp.AddNone();
		}
	}
}
