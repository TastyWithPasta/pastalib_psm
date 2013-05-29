using System;

namespace PastaLib.Components
{
	public class PShaderComponent : IPComponent
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

		public void Update ()
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

