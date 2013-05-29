using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;

namespace PastaLib
{
	public class MyGame
	{
		VertexBuffer m_unitVB; //Might need to move this to a static context eventually
		Stopwatch m_stopWatch = null; //This too
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

		public MyGame()
		{
			m_graphics = new GraphicsContext();
			m_stopWatch = new Stopwatch();
 			m_timerManager = new PTimerManager(this);
		}

		public virtual void Initialise()
		{
			m_stopWatch.Start();
		}
		
		public virtual void Update()
		{
			m_elapsedTime = m_stopWatch.Elapsed;
			m_timerManager.Update();
		}
		public virtual void Draw()
		{}
	}
}
