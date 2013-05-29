using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sce.PlayStation.Core;

namespace PastaLib.Components
{
	public class PTransformComponent : PComponent
	{
		private Vector3 m_origin = new Vector3(0,0,0);
		private Vector3 m_actualOrigin = new Vector3(0,0,0);
		
		Vector3 m_position = Vector3.Zero;
		Vector3 m_scale = Vector3.One;
		double m_direction = 0;
		protected Matrix4 m_transformMatrixCache;
		protected bool m_updateTransformMatrixFlag = true;
		
		public PTransformComponent()
			: base()
		{ }

		
		public Matrix4 TransformMatrixRelative
		{
			get{
				if(m_updateTransformMatrixFlag)
				{
					m_updateTransformMatrixFlag = false;
					m_transformMatrixCache = CreateMatrix();
					
				}
				return m_transformMatrixCache;
			}
		}
		public Matrix4 TransformMatrixAbsolute
		{
			get{
				if(Container.Parent.Transform == null)
					return TransformMatrixRelative;
				return TransformMatrixRelative.Multiply(Container.Parent.Transform.TransformMatrixAbsolute);
			}
		}
		
		public Vector3 PositionAbsolute
		{
			get
			{
				return TransformMatrixAbsolute.TransformPoint(Vector3.Zero);
			}
		}
		public double DirectionAbsolute
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public Vector2 ScaleAbsolute
		{
			get{
				throw new NotImplementedException();
			}
		}
		
		public Vector3 Origin
		{
			get { return m_origin; }
			set { 
				m_origin = value;
				m_actualOrigin = m_origin * -1;
			}
		}
		public Vector3 Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_updateTransformMatrixFlag = true;
				m_position = value;
			}
		}
		public Vector3 Scale
		{
			get { return m_scale; }
			set { 
				m_scale = value; 
				m_updateTransformMatrixFlag = true;
			}
		}
		public double Direction
		{
			get
			{
				return m_direction;
			}
			set
			{
				m_direction = value;
				m_updateTransformMatrixFlag = true;
			}
		}
		
		protected Matrix4 CreateMatrix()
		{		
			Matrix4 centerMatrix=Matrix4.Translation(m_actualOrigin);
			Matrix4 transMatrix=Matrix4.Translation(Position);
			Matrix4 rotMatrix=Matrix4.RotationZ((float)Direction);
			Matrix4 scaleMatrix=Matrix4.Scale(Scale);
			return transMatrix*rotMatrix*scaleMatrix*centerMatrix;
		}
	}
}
