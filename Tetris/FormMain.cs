using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Tetris
{
	/// <summary>
	/// FormMain �̊T�v�̐����ł��B
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
		private AxWMPLib.AxWindowsMediaPlayer axMedia;

		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormMain()
		{
			InitializeComponent();

			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox     = false;
		}

		public FormMain( Size clientSize ) : this()
		{
			this.ClientSize = clientSize;
		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormMain));
			this.axMedia = new AxWMPLib.AxWindowsMediaPlayer();
			((System.ComponentModel.ISupportInitialize)(this.axMedia)).BeginInit();
			this.SuspendLayout();
			// 
			// axMedia
			// 
			this.axMedia.Enabled = true;
			this.axMedia.Location = new System.Drawing.Point(32, 40);
			this.axMedia.Name = "axMedia";
			this.axMedia.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMedia.OcxState")));
			this.axMedia.Size = new System.Drawing.Size(344, 240);
			this.axMedia.TabIndex = 0;
			this.axMedia.Visible = false;
			// 
			// FormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(440, 325);
			this.Controls.Add(this.axMedia);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.Text = "FormMain";
			this.Load += new System.EventHandler(this.FormMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.axMedia)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormMain_Load(object sender, System.EventArgs e)
		{
			axMedia.settings.setMode( "loop", true );
			StartMediaPlayer();
			EndMediaPlayer();
//			Music.StartWav( "./Land.wav" );
//			Music.EndWav();
//			Music.StartWav( "./Flash1.wav" );
//			Music.EndWav();
//			Music.StartWav( "./Flash2.wav" );
//			Music.EndWav();
//			Music.StartWav( "./Flash3.wav" );
//			Music.EndWav();
//			Music.StartWav( "./Flash4.wav" );
//			Music.EndWav();
		}
		//========================================================================================
		// Name		: StartMediaPlayer
		// Function	: 
		//========================================================================================
		public bool StartMediaPlayer()
		{
			try
			{
				axMedia.URL = "./Tetris.mp3";
			}
			catch( Exception ex )
			{
				string szError = ex.ToString();
				Console.WriteLine( szError );
				return false;
			}
			return true;
		}
		//========================================================================================
		// Name		: EndMediaPlayer
		// Function	: 
		//========================================================================================
		public bool EndMediaPlayer()
		{
			try
			{
				axMedia.Ctlcontrols.stop();
			}
			catch( Exception ex )
			{
				string szError = ex.ToString();
				Console.WriteLine( szError );
				return false;
			}
			return true;
		}

	}
}
