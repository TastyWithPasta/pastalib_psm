using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sce.PlayStation.Core.Graphics;
namespace PastaLib.Components
{
	delegate void CResponseDelegate();

	//interface IPCollider
	//{
	//    void DoCollisions(GameTime gameTime, IPCollider actor);
	//    bool TestCollision(IPCollider collider);
	//    void CollisionResponse(GameTime gameTime, IPCollider actor);
	//}
	
	//Strong types instead of expensive hierarchy
	
	public abstract class PUpdatableComponent : PComponent, IPUpdatable
	{
		public void Update ()
		{
			if(!Enabled)
				return;
			OnUpdate();
		}
		
		protected abstract void OnUpdate();
	}
	
	public abstract class PDrawableComponent : IPUpdatable, IPDrawable
	{
		ShaderProgram m_shaderProgram;
		
		ShaderProgram ShaderProgram
		{
			get{return m_shaderProgram;}
			set{ m_shaderProgram = value;}
		}
		
		public void Draw ()
		{
			if(!Enabled)
				return;
			OnDraw();
		}
		protected abstract void OnDraw();
		
		//Worth the performance loss... Rare are the drawables which are not updated.
		//Besides, there is only one per actor
		public void Update ()
		{
			if(!Enabled)
				return;
			OnUpdate();
		}
		protected virtual void OnUpdate() {}
	}
}


