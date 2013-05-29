using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public interface ContentLoader<T>
	{
		void LoadContent(ContentManager contentManager, string path);
		void UnloadContent();
		T Get(string name);
	}

    public class TextureLibrary : ContentLoader<Texture2D>
    {
		private Dictionary<string, Texture2D> m_textureLibrary = new Dictionary<string, Texture2D>();
		//private string m_basePath = "Content/";

		public void LoadContent(ContentManager Content, string path)
		{
			string fullpath;
			if (path == "")
			{
				fullpath = Content.RootDirectory;
			}
			else
			{
				fullpath = Content.RootDirectory + "/" + path;
				path += "/";
			}

			DirectoryInfo di = new DirectoryInfo(fullpath);
			FileInfo[] files = di.GetFiles();
			int length = files.Length;

			for (int i = 0; i < length; ++i)
			{
				string name = files[i].Name.Substring(0, files[i].Name.Length - 4);
				m_textureLibrary.Add(name, Content.Load<Texture2D>(path + name));
			}
		}

		public void UnloadContent()
		{
			m_textureLibrary.Clear();
		}

		public Texture2D Get(string name)
		{
			return m_textureLibrary[name];
		}
    }
}
