using System;
using PastaLib.Components;

namespace PastaLib
{
	public class PCollisionComponent : PUpdatableComponent
	{
		public PCollisionComponent ()
		{
		}

		#region implemented abstract members of PastaLib.Components.PUpdatableComponent
		protected override void OnUpdate ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

