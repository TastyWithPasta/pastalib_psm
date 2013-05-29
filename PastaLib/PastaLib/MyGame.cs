using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;

namespace PastaLib
{
	public class MyGame
	{
		GraphicsContext graphics;
		VertexBuffer m_unitVB; //Might need to move this to a static context eventually
		StopWatch m_stopWatch = null; //This too
		TimeSpan m_elapsedTime;
		
		PTimerManager m_timerManager = null;
		GraphicsContext m_graphics;

		public TimeSpan ElapsedTime
		{
			get { return m_elapsedTime; }
		}
		public GraphicsContext Graphics
		{
			get { return m_graphics; }
		}
		public PTimerManager TimerManager
		{
			get { return m_timerManager; }
			set { m_timerManager = value; }
		}
		public ShaderProgram DefaultShader
		{
			get{ return m_defaultShader; }
		}

		public MyGame()
		{
			m_graphics = new GraphicsDeviceManager(this);
			m_stopWatch = new StopWatch();
 			m_timerManager = new PTimerManager(m_stopWatch);
		}

		protected override void Initialize()
		{
			base.Initialize();
			m_stopWatch.Start();
		}
		
		public void Run()
		{
			m_elapsedTime = m_stopWatch.Elapsed;
			Update();
			Draw();
		}

		protected override void Update()
		{
			m_gameTime = gameTime;
			m_timerManager.UpdateTimers(m_gameTime);
		}

		protected override void Draw()
		{
			m_gameTime = gameTime;
		}
	}
}
