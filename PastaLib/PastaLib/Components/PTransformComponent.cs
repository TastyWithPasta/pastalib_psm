using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sce.PlayStation.Core;

namespace PastaLib.Components
{
	public class PTransformComponent : IPComponent
	{
		Vector2 m_position = Vector2.Zero;
		Vector2 m_scale = Vector2.One;
		double m_direction = 0;
		protected Matrix4 m_transformMatrixCache;
		protected bool m_updateTransformMatrixFlag = true;
		
		PTransformComponent m_parentTransform = null;

		public PTransformComponent()
			: base()
		{ }

		protected override void OnParentChanged()
		{
			if (Container.Parent == null)
				m_parentTransform = null;
			else
				m_parentTransform = Container.Parent.GetComponents<PTransformComponent>()[0];
		}
		protected override void OnAttach(IPActor container)
		{}
		protected override void OnEnable()
		{}
		protected override void OnDisable()
		{}
		
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
				if(m_parentTransform == null)
					return TransformMatrixRelative;
				return TransformMatrixRelative.Multiply(m_parentTransform.TransformMatrixAbsolute);
			}
		}
		
		public Vector2 PositionAbsolute
		{
			get
			{
				if (m_parentTransform == null)
					return Position;
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
		
		public Vector2 Position
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
		public Vector2 Scale
		{
			get { return m_scale; }
			set { 
				m_scale = value; 
				m_updateTransformMatrixFlag = true;
			}
		}
		public Vector2 DirectionUnit
		{
			get
			{
				return new Vector2((float)Math.Cos(m_direction), (float)Math.Sin(m_direction));
			}
			set
			{
				throw new NotImplementedException();
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
		
		protected void CreateMatrix()
		{		
			var centerMatrix=Matrix4.Translation(new Vector3(-_origin.X, -_origin.Y, 0.0f));
			var transMatrix=Matrix4.Translation(new Vector3(ScreenX, ScreenY, 0.0f));
			var rotMatrix=Matrix4.RotationZ((float)Angle);
			var scaleMatrix=Matrix4.Scale(new Vector3(_scale.X * _baseDimensions.X, _scale.Y * _baseDimensions.Y, 1.0f));
			return transMatrix*rotMatrix*scaleMatrix*centerMatrix;
		}
	}
}
