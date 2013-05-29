using System;
using Sce.PlayStation.Core.Audio;
using System.Collections.Generic;
using System.IO;

namespace NewGameLibrary {
	
	public static class SoundLibrary {
		private static Dictionary<string,Sound> _sounds=new Dictionary<string,Sound>();
		private static Dictionary<string,SoundPlayer> _players=new Dictionary<string,SoundPlayer>();
		
		public static void Load(string dir){
			dir = "/Application/" + dir;
			string[] sounds = Directory.GetFiles(dir);
			foreach (string f in sounds)
				Add(Path.GetFileNameWithoutExtension(f).ToLower(), new Sound(f));
		}
		
		public static void Add(string name, Sound sound){
			_sounds[name.ToLower()]=sound;
			_players[name.ToLower()]=sound.CreatePlayer();
		}
		
		public static Sound Get(string name){
			return _sounds[name.ToLower()];
		}
		
		public static SoundPlayer GetPlayer(string name){
			return _players[name.ToLower()];
		}
	}
}

