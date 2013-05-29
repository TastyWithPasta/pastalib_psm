using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaLib
{
	/*
	public interface IPComponent
	{
		bool Enabled { get; set; }
		IPActor Container { get; }
		void Attach(IPActor container);
	}
	
	
	public interface IPActor
	{
		int ID { get; }
		IPActor Parent { get; }
		List<IPActor> Children { get; }
		MyGame TheGame { get; }

		bool BindParent(IPActor parent);
		bool BindChild(IPActor child);
		void UnbindParent();
		void UnbindChild(IPActor child);
		void UnbindChildren();

		void AddComponent(IPComponent component);
		List<ComponentType> GetComponents<ComponentType>() where ComponentType : PComponent;
		ComponentType GetFirstComponent<ComponentType>() where ComponentType : PComponent;
	}
	*/
	
	public abstract class PComponent
	{
		bool m_enabled;
		Actor m_container;

		public Actor Container
		{
			get { return m_container; }
		}
		public bool Enabled
		{
			get { return m_enabled;	}
			set
			{
				m_enabled = value;
				if (m_enabled)
					OnEnable();
				else
					OnDisable();
			}
		}

		protected virtual void OnParentChanged(){}
		protected virtual void OnEnable(){}
		protected virtual void OnDisable(){}

		public void Attach(Actor container)
		{
			m_container = container;
			OnAttach(container);
		}
		protected virtual void OnAttach(Actor container){}

	}

	public interface IPUpdatable
	{
		void Update();
	}
	public interface IPDrawable
	{
		void Draw();
	}


}
