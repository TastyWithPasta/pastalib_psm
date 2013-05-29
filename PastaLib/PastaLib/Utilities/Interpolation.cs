using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PastaLib.Utilities
{
	public interface IPInterpolation<T>
	{
		T GetInterpolation(T from, T to, float ratio);
	}

	public class PLerpInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * ratio;
		}
	}
	public class PSquareInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			ratio = ratio * 2 - 1;
			return from + (to - from) * ratio * ratio;
		}
	}
	public class PSineHalfInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * (float)(Math.Sin(ratio * Math.PI * 0.5f));
		}
	}
	public class PSineInterpolation : IPInterpolation<float>
	{
		public float GetInterpolation(float from, float to, float ratio)
		{
			return from + (to - from) * (float)(Math.Sin(ratio * Math.PI * 2.0));
		}
	}
}
