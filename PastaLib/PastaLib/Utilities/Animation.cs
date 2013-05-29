using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaGameLibrary.Utilities
{
	public class Animation
	{
		int m_startFrame, m_endFrame;
		IPInterpolation<float> m_interpolator = new PLerpInterpolation();
		PTimer m_timer = null;

		public Animation(MyGame theGame, int startFrame, int endFrame, float intervalInSeconds)
		{
			m_timer = new PTimer(theGame.TimerManager, null);
			m_startFrame = startFrame;
			m_endFrame = endFrame;
			m_timer.Interval = intervalInSeconds;
		}

		public IPInterpolation<float> Interpolator
		{
			get { return m_interpolator; }
			set { m_interpolator = value; }
		}

		public int GetCurrentFrame()
		{
			float interpolation = m_interpolator.GetInterpolation(m_startFrame, m_endFrame + 1, m_timer.ProgressRatio);
			return (int)interpolation;
		}
	}
}
