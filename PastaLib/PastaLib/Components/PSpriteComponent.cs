using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using PastaLib.Utilities;

namespace PastaLib.Components
{
	public class PSpriteComponent : PDrawableComponent
	{
		private Rectangle m_srcRect = new Rectangle(0,0,0,0);
		private Rectangle m_destRect = new Rectangle(0,0,0,0);
		
		private Texture2D m_texture = null;
		private Vector2 m_origin = new Vector2(0.5f, 0.5f);
		private Vector2 m_renderOrigin = new Vector2(0.5f, 0.5f);
		private Vector4 m_colour = Vector4.One;
		private float m_depth = 1.0f;
		private float m_scaleX = 1, m_scaleY = 1; //Scaling of the destination rectangle
		private float m_width = 0, m_height = 0; //Base width of the destination rectangle

		private Animation m_animation = null;
		private int m_amountOfFrames = 0;
		private int m_frameRows = 0;
		private int m_frameColumns = 0;

		public PSpriteComponent(Texture2D texture, int sourceWidth, int sourceHeight)
		{
			m_srcRect.Width = sourceWidth;
			m_srcRect.Height = sourceHeight;
			Width = sourceWidth;
			Height = sourceHeight;
			Texture = texture;
			Origin = new Vector2(0.5f, 0.5f);
		}
		public PSpriteComponent(Texture2D texture)
		{
			m_srcRect.Width = texture.Width;
			m_srcRect.Height = texture.Height;
			Texture = texture;
			Origin = new Vector2(0.5f, 0.5f);
		}

		public Texture2D Texture
		{
			get { return m_texture; }
			set
			{
				m_texture = value;

				if (m_texture == null)
				{
					m_frameRows = 0;
					m_frameColumns = 0;
					m_amountOfFrames = 0;
				}
				else
				{
					m_frameRows = m_texture.Height / (int)m_srcRect.Height;
					m_frameColumns = m_texture.Width / (int)m_srcRect.Width;
					m_amountOfFrames = m_frameRows * m_frameColumns;
					if(m_amountOfFrames == 1)
					{
						Width = m_texture.Width;
						Height = m_texture.Height;
					}
				}
			}
		}
		public Animation Animation
		{
			get { return m_animation; }
			set { m_animation = value; }
		}
		public Vector4 Colour
		{
			get { return m_colour; }
			set { 
				m_colour = value;
			}
		}
		public float Depth
		{
			get { return m_depth; }
			set { m_depth = value; }
		}
		public Vector2 Origin
		{
			get { return m_origin; }
			set { 
				m_origin = value;
				m_renderOrigin.X = m_srcRect.Width * m_origin.X;
				m_renderOrigin.Y = m_srcRect.Height * m_origin.Y;
			}
		}
		public float Rotation
		{
			get { return (float)Container.Transform.Direction; }
		}

		public float Width
		{
			get { return m_width; }
			set
			{
				m_width = value;
				m_destRect.Width = (int)(m_scaleX * m_width);
			}
		}
		public float Height
		{
			get { return m_height; }
			set
			{
				m_height = value;
				m_destRect.Height = (int)(m_scaleY * m_height);
			}
		}
		public float ScaleX
		{
			get{ return m_scaleX; }
			set
			{ 
				m_scaleX = value;
				m_destRect.Width = (int)(m_scaleX * m_width);
			}
		}
		public float ScaleY
		{
			get { return m_scaleY; }
			set 
			{ 
				m_scaleY = value;
				m_destRect.Height = (int)(m_scaleY * m_height);
			}
		}
		public float Scale
		{
			set
			{ 
				m_scaleX = m_scaleY = value;
				m_destRect.Width = (int)(m_scaleX * m_width);
				m_destRect.Height = (int)(m_scaleY * m_height);
			}
		}
		public Rectangle DestinationRectangle
		{
			get
			{
				Vector3 pos = Container.Transform.Position;
				m_destRect.X = (int)pos.X;
				m_destRect.Y = (int)pos.Y;
				return m_destRect;
			}
		}
		public Rectangle SourceRectangle
		{
			get { return m_srcRect; }
			set 
			{ 
				m_srcRect = value;
				Origin = m_origin; //Update origin
			}
		}
		public void SetFrame(int frame)
		{
			//int frame = m_animation.GetCurrentFrame();
			frame = Math.Min(m_amountOfFrames, Math.Max(0, frame));
			m_srcRect.X = frame % m_frameColumns * m_srcRect.Width;
			m_srcRect.Y = frame / m_frameColumns * m_srcRect.Height;
		}
		
		protected override void OnUpdate()
		{
			if (m_animation == null)
				return;
			SetFrame((int)m_animation.GetCurrentFrame());
		}
		
		protected override void OnDraw()
		{
			if (Container.Transform == null || Container.Shader == null || m_texture == null)
				return;
			
			GraphicsContext context = Container.TheGame.Graphics;
			VertexBuffer vb = Container.TheGame.UnitVB; //Need to find another place to store that vb.
			
			context.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
			context.SetShaderProgram(Container.Shader.ShaderProgram);
			context.SetVertexBuffer(0, vb);
			context.SetTexture(0, m_texture);
			context.DrawArrays(DrawMode.TriangleFan, 0, 4);
		}
	}
}
