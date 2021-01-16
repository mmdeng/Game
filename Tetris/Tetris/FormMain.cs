using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class FormMain : Form
	{
		/// <summary>
		/// ダメ。2つ以上の音をMixして鳴らすことができない。
		/// MP3の再生もできない。BGMをWAVにすると２０Mぐらいになる。
		/// BGMはなしにする。
		/// </summary>
		//private SoundPlayer player;

		public FormMain()
		{
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;

			//var a = Assembly.GetExecutingAssembly();
			//var s = a.GetManifestResourceStream("Tetris.Resources.Bgm1.wav");
			//player = new SoundPlayer(s);
		}

		public FormMain(Size clientSize) : this()
		{
			this.ClientSize = clientSize;
		}

		public void StartMediaPlayer()
		{
		}
		public void EndMediaPlayer()
		{
		}

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
