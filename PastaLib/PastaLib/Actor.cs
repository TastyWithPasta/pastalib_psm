using System;
using System.Collections.Generic;
using System.Linq;
using PastaGameLibrary.Components;
using PastaLib.Components;

namespace PastaLib
{
    public class Actor : IDisposable
    {
		static uint currentID = 0;
		uint _ID = 0;

		MyGame m_theGame;
		IPActor m_parent = null;
		
		//Mandatory components, there can be only one of them
		PTransformComponent m_transform = new PTransformComponent();
		PShaderComponent m_shader = null;
		
		//The actor can have multiple updatable components, but only one drawable.
		//The drawable can also be an updatable
		IPDrawableComponent m_currentDrawable;
		List<IPUpdatableComponent> m_currentUpdatables;
		List<IPActor> m_children = new List<IPActor>();
	
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
		public TransformComponent Transform
		{
			get{ return m_transform; }
		}
		public PShaderComponent Shader
		{
			get{ return m_shader; }
			set{ m_shader = value; }
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
			//Replaces current shader (not part of other updatables, needs to be updated at the end)
			if(component is PShaderComponent)
			{
				m_shader = (PShaderComponent)component;
				return;
			}
			
			//Adds component to current updatables
			if (component is IPUpdatableComponent)
				m_currentUpdatables.Add((IPUpdatableComponent)component);
			
			//Component replaces current drawable
			if (component is IPDrawableComponent)
			{
				if(m_drawable == null)
				{
					m_drawable = (IPDrawableComponent)component;	
				}
				else
				{
					if(m_drawable is IPUpdatableComponent)
						m_currentUpdatables.Remove(m_drawable);
					   
				}
			}
			m_allComponents.Add(component);
			component.Attach(this);
		}

		/// <summary>
		/// Returns all components matching the specified type.
		/// This is rather inefficient, accessing and caching components at initialisation.
		/// </summary>
		/// <typeparam name="ComponentType">Type of components to return.</typeparam>
		/// <returns>List of components matching the specified type.</returns>
		public List<ComponentType> GetComponents<ComponentType>() where ComponentType : PUpdatableComponent
		{
			List<ComponentType> result = new List<ComponentType>();
			for (int i = 0; i < m_allComponents.Count; ++i)
				if (m_allComponents[i].GetType().IsAssignableFrom(typeof(ComponentType)))
					result.Add((ComponentType)m_allComponents[i]);
			return result;
		}
		public ComponentType GetFirstComponent<ComponentType>() where ComponentType : PUpdatableComponent
		{
			List<ComponentType> result = GetComponents<ComponentType>();
			if (result.Count == 0)
				return null;
			return result[0];
		}

		//Two distinct types of methods since drawing and updating are handled separately by XNA.
		public void Update()
		{
			for (int i = 0; i < m_currentUpdatables.Count; ++i)
				m_currentUpdatables[i].Update();
			
			m_shader.Update();//Update shader values last
		}
		public void Draw()
		{
			if(m_currentDrawable == null)
				return;
			
			m_drawable.Draw();
		}
	}
}
