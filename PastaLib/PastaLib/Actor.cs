using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PastaGameLibrary
{
    public class Actor : IPActor, IDisposable
    {
		static uint currentID = 0;
		uint _ID = 0;

		MyGame m_theGame;
		IPActor m_parent = null;
		List<IPActor> m_children = new List<IPActor>();

		List<IPComponent> m_allComponents = new List<IPComponent>();
		List<IPUpdatable> m_updatableComponents = new List<IPUpdatable>();
		List<IPDrawable> m_drawableComponents = new List<IPDrawable>();

        public Actor(MyGame theGame) 
        {
			m_theGame = theGame;

			//Sets the entity's ID to a unique ID.
			_ID = currentID;
			++currentID;
			m_parent = null;
        }
        public void Dispose()
        {
			UnbindParent();
			UnbindChildren();
        }

		public int ID
		{
			get { return ID; }
		}
		public IPActor Parent
		{
			get { return m_parent; }
		}
		public List<IPActor> Children
		{
			get { return m_children; }
		}
		public MyGame TheGame
		{
			get { return m_theGame; }
		}

		public bool BindParent(IPActor parent)
		{
			if (m_parent == null)
				return false;
			if (m_parent == parent)
				return false;
			m_parent = parent;
			m_parent.BindChild(this);
			return true;
		}
		public bool BindChild(IPActor child)
		{
			if (child == null)
				return false;
			if (m_children.Contains(child))
				return false;
			m_children.Add(child);
			child.BindParent(this);
			return true;
		}

		public void UnbindParent()
		{
			if (m_parent == null)
				return;
			IPActor temp = m_parent;
			m_parent = null;
			temp.UnbindChild(this);
		}
		public void UnbindChild(IPActor child)
		{
			if (!m_children.Contains(child))
			{
				return;
			}
			m_children.Remove(child);
			child.UnbindParent();
		}
		public void UnbindChildren()
		{
			while(m_children.Count > 0)
			{
				UnbindChild(m_children[0]);
			}
		}

		public void AddComponent(IPComponent component)
		{
			if (component is IPUpdatable)
				m_updatableComponents.Add((IPUpdatable)component);
			if (component is IPDrawable)
				m_drawableComponents.Add((IPDrawable)component);
			m_allComponents.Add(component);
			component.Attach(this);
		}

		/// <summary>
		/// Returns all components matching the specified type.
		/// This is rather inefficient, accessing and caching components at initialisation.
		/// </summary>
		/// <typeparam name="ComponentType">Type of components to return.</typeparam>
		/// <returns>List of components matching the specified type.</returns>
		public List<ComponentType> GetComponents<ComponentType>() where ComponentType : PComponent
		{
			List<ComponentType> result = new List<ComponentType>();
			for (int i = 0; i < m_allComponents.Count; ++i)
				if (m_allComponents[i].GetType().IsAssignableFrom(typeof(ComponentType)))
					result.Add((ComponentType)m_allComponents[i]);
			return result;
		}
		public ComponentType GetFirstComponent<ComponentType>() where ComponentType : PComponent
		{
			List<ComponentType> result = GetComponents<ComponentType>();
			if (result.Count == 0)
				return null;
			return result[0];
		}

		//Two distinct types of methods since drawing and updating are handled separately by XNA.
		public void Update()
		{
			for (int i = 0; i < m_updatableComponents.Count; ++i)
				m_updatableComponents[i].Update();
		}
		public void Draw()
		{
			for (int i = 0; i < m_drawableComponents.Count; ++i)
				m_drawableComponents[i].Draw();
		}
	}
}
