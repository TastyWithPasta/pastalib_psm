using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;

namespace PastaLib
{
	public class PTimerManager : IPUpdatable
	{
		MyGame m_game;
		List<PTimer> m_timers = new List<PTimer>();

		public PTimerManager(MyGame game)
		{
			m_game = game;
		}

		internal void AddTimer(PTimer timer)
		{
			m_timers.Add(timer);
		}
		internal void RemoveTimer(PTimer timer)
		{
			m_timers.Remove(timer);
		}
		public void Update()
		{
			float elapsedTime = (float)m_game.ElapsedTime.TotalSeconds;
			for (int i = 0; i < m_timers.Count; ++i)
				m_timers[i].Update(elapsedTime);  
		}
	}

	public class PTimer : IDisposable
	{
		public delegate void TimerTask();
		TimerTask m_task;
		bool m_paused = false;
		float m_intervalInSeconds = 1;
		float m_timeRemainingInSeconds = 0;
		PTimerManager m_manager = null;

		public bool Paused
		{
			get { return m_paused; }
			set { m_paused = value; }
		}
		public float ProgressRatio
		{
			get { return  1 - m_timeRemainingInSeconds / m_intervalInSeconds; }
			set { m_timeRemainingInSeconds = (1 - value) * m_intervalInSeconds; }
		}
		public float Interval 
		{
			get { return m_intervalInSeconds; }
			set { m_intervalInSeconds = value; }
		}

		public PTimer(PTimerManager manager, TimerTask task)
		{
			m_manager = manager;
			m_manager.AddTimer(this);
			m_task = task;
		}

		void Delay(float delayTimeInSeconds)
		{
			m_timeRemainingInSeconds -= delayTimeInSeconds; 
		}
		void Reset()
		{
			m_timeRemainingInSeconds = 0;
		}

		internal void Update(float elapsedTime)
		{
			m_timeRemainingInSeconds -= elapsedTime;
			while (m_timeRemainingInSeconds < 0)
			{
				if(m_task != null)
					m_task();
				m_timeRemainingInSeconds += m_intervalInSeconds;
			}
		}

		public void Dispose()
		{
			m_manager.RemoveTimer(this);
		}
	}
}
