using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core;


namespace PastaGameLibrary
{
    public static class TextureLibrary
    {
		private static Dictionary<string,Texture2D> _textures=new Dictionary<string,Texture2D>();
		private static Dictionary<string, Rgba[]> _colourData=new Dictionary<string, Rgba[]>();

		public static void Load(string dir){
			dir = "/Application/" + dir;
			foreach (string f in Directory.GetFiles(dir)) {
				string index=Path.GetFileNameWithoutExtension(f).ToLower();
				_textures.Add(index, new Texture2D(f, false));
				/*Image i=new Image(f);
				i.Decode();
				byte[] b=i.ToBuffer();
				_colourData.Add(index, new Rgba[b.Length/4]);
				for (int n=0; n<b.Length; n+=4)
					_colourData[index][n/4]=new Rgba(b[n], b[n+1], b[n+2], b[n+3]);*/
			}
		}
		
		public static void Add(string name, Texture2D texture){
			_textures[name.ToLower()]=texture;
		}
		
		public static Texture2D Get(string name){
			return _textures[name.ToLower()];
    	}
	}
}
