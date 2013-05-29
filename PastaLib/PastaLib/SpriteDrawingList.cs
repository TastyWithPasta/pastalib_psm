using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary.Components;

namespace PastaGameLibrary
{
	public class SpriteDrawingList : ActiveList
	{
		public SpriteDrawingList(MyGame theGame, float updateTickInSeconds) : base(theGame, updateTickInSeconds)
		{
		}

		public void Add(PSpriteComponent sprite)
		{
			throw new NotImplementedException();
		}

		public void Remove(PSpriteComponent sprite)
		{
			throw new NotImplementedException();
		} 

		protected override void OnUpdate()
		{
			TheGame.SpriteBatch.Begin();
			base.OnUpdate();
			TheGame.SpriteBatch.End();
		}
	}
}
