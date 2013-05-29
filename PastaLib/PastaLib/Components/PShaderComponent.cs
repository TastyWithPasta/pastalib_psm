using System;
using Sce.PlayStation.Core.Graphics;

namespace PastaLib.Components
{
	public abstract class PShaderComponent : PUpdatableComponent
	{
		ShaderProgram m_shaderProgram;
		
		public PShaderComponent (ShaderProgram shaderProgram) : base()
		{
			m_shaderProgram = shaderProgram;
		}
		
		public ShaderProgram ShaderProgram
		{
			get{ return m_shaderProgram; }
		}

		protected override void OnUpdate ()
		{
			if(!Enabled)
				return;
			
			//Update shader values
			SetShaderValues();
		}
		
		protected abstract void SetShaderValues();
		
		/*
					var worldViewProj = camera.CameraMatrix * CreateModelMatrix();	
			_shader.SetUniformValue(0, ref worldViewProj);
			//_shader.SetUniformValue(0, ref worldViewProj);
			_shader.SetUniformValue(1, ref _srcFrame);
			_shader.SetUniformValue(2, ref _colour);	
		*/
	}
}

