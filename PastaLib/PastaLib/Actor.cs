using System;
using System.Collections.Generic;
using System.Linq;
using PastaLib.Components;

namespace PastaLib
{
    public class Actor : IDisposable
    {
		static uint currentID = 0;
		uint _ID = 0;

		MyGame m_theGame;
		Actor m_parent = null;
		
		//Mandatory components, there can be only one of them
		PTransformComponent m_transform = new PTransformComponent();
		PShaderComponent m_shader = null;
		PComponent m_controller = null;
		
		//The actor can have multiple updatable components, but only one drawable.
		//The drawable can also be an updatable
		PDrawableComponent m_currentDrawable;
		List<PUpdatableComponent> m_currentUpdatables;
		List<Actor> m_children = new List<Actor>();
	
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
		public Actor Parent
		{
			get { return m_parent; }
		}
		public List<Actor> Children
		{
			get { return m_children; }
		}
		public MyGame TheGame
		{
			get { return m_theGame; }
		}
		public PTransformComponent Transform
		{
			get{ return m_transform; }
		}
		public PShaderComponent Shader
		{
			get{ return m_shader; }
			set{ m_shader = value; }
		}

		public bool BindParent(Actor parent)
		{
			if (m_parent == null)
				return false;
			if (m_parent == parent)
				return false;
			m_parent = parent;
			m_parent.BindChild(this);
			return true;
		}
		public bool BindChild(Actor child)
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
			Actor temp = m_parent;
			m_parent = null;
			temp.UnbindChild(this);
		}
		public void UnbindChild(Actor child)
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

		public void AddComponent(PComponent component)
		{
			//Replaces current shader (not part of other updatables, needs to be updated at the end)
			if(component is PShaderComponent)
			{
				m_shader = (PShaderComponent)component;
			}
			else
			{
				if (component is PDrawableComponent)
				m_currentDrawable = (PDrawableComponent)component;
			
				//Adds component to current updatables
				if (component is PUpdatableComponent)
					m_currentUpdatables.Add((PUpdatableComponent)component);
			}
			
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
			/*
			for (int i = 0; i < m_allComponents.Count; ++i)
				if (m_allComponents[i].GetType().IsAssignableFrom(typeof(ComponentType)))
					result.Add((ComponentType)m_allComponents[i]);
					*/
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
			
			m_currentDrawable.Update();
			m_shader.Update();//Update shader values last
		}
		public void Draw()
		{
			if(m_currentDrawable == null)
				return;
			
			m_currentDrawable.Draw();
		}
	}
}
