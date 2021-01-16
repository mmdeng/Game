using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
	public class Music
	{
		private enum FdwSound : uint
		{
			SND_SYNC = 0x0000,
			SND_ASYNC = 0x0001,
			SND_NODEFAULT = 0x0002,
			SND_MEMORY = 0x0004,
			SND_LOOP = 0x0008,
			SND_NOSTOP = 0x0010,
			SND_NOWAIT = 0x00002000,
			SND_ALIAS = 0x00010000,
			SND_ALIAS_ID = 0x00110000,
			SND_FILENAME = 0x00020000,
			SND_RESOURCE = 0x00040004
		}
		public static void StartWav(string szFileName)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var stream = assembly.GetManifestResourceStream("Tetris.Resources." + szFileName);
			var player = new SoundPlayer(stream);
			player.Play();
		}
		public static bool StartMm()
		{
			return true;
		}
		public static bool EndMm()
		{
			return true;
		}
	}
}
