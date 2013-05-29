using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary.Utilities;

namespace PastaGameLibrary.Components
{
	public class PSpriteComponent : PComponent, IPDrawable, IPUpdatable
	{
		private Rectangle m_srcRect = Rectangle.Empty;
		private Rectangle m_destRect = Rectangle.Empty;
		
		private Texture2D m_texture = null;
		private Vector2 m_origin = new Vector2(0.5f, 0.5f);
		private Vector2 m_renderOrigin = new Vector2(0.5f, 0.5f);
		private Color m_colour = Color.White;
		private Color m_renderColour = Color.White;
		private float m_depth = 1.0f;
		private float m_scaleX = 1, m_scaleY = 1; //Scaling of the destination rectangle
		private float m_width = 0, m_height = 0; //Base width of the destination rectangle
		private PTransformComponent m_transformComponent = null;

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
					m_frameRows = m_texture.Height / m_srcRect.Height;
					m_frameColumns = m_texture.Width / m_srcRect.Width;
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
		public Color Colour
		{
			get { return m_colour; }
			set { 
				m_colour = value;
				m_renderColour.R = (byte)(m_colour.R * m_colour.A);
				m_renderColour.G = (byte)(m_colour.G * m_colour.A);
				m_renderColour.B = (byte)(m_colour.B * m_colour.A);
				m_renderColour.A = m_colour.A;
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
			get { return (float)m_transformComponent.Direction; }
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
				Vector2 pos = m_transformComponent.Position;
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

		protected override void OnParentChanged()
		{
		}
		protected override void OnEnable()
		{
		}
		protected override void OnDisable()
		{
		}
		protected override void OnAttach(IPActor container)
		{
			m_transformComponent = Container.GetFirstComponent<PTransformComponent>();
		}
		public void Update()
		{
			if (m_animation == null)
				return;
			SetFrame((int)m_animation.GetCurrentFrame());
		}
		public void Draw()
		{
			if (m_transformComponent == null || m_texture == null)
				return;
			
			GraphicsContext context = Container.TheGame.Graphics;
			VertexBuffer vb = Container.TheGame.UnitVB;
			
			context.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
			context.SetShaderProgram(shader);
			context.SetVertexBuffer(0, vb);
			context.SetTexture(0, texture);
			context.DrawArrays(DrawMode.TriangleFan, 0, 4);
			
			Container.TheGame.SpriteBatch.Draw(m_texture, DestinationRectangle, SourceRectangle, Colour, Rotation, m_renderOrigin, SpriteEffects.None, 0);
		}
	}
}
