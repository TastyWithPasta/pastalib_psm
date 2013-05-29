using System;
using System.Collections.Generic;
using Sce.PlayStation.Core.Graphics;
using System.IO;

namespace NewGameLibrary {
	public static class Shaders {
		static Dictionary<string, ShaderProgram> _shaders;
		
		public static ShaderProgram TextureShader;
		public static ShaderProgram DefaultShader;
		
		public static void Init(){
			_shaders=new Dictionary<string, ShaderProgram>();
			
			// NOT REDUNDANT!
			/*ShaderProgram sp;      
            sp=new ShaderProgram("shaders/Texture.cgx");
            sp.SetUniformBinding(0, "WorldViewProj");
            sp.SetUniformBinding(1, "Colour");
            _shaders["default"]=sp;*/
			// NOT REDUNDANT!
			
			Add("default", new string[]{"WorldViewProj", "SourceFrame", "Colour"},new string[]{});
			Add("texture", new string[]{"WorldViewProj", "InColour"}, new string[]{});
			
			TextureShader = Get ("texture");
			DefaultShader = Get ("default");
		}

		public static void Add(string name, string[] uniformBindings, string[] attributeBindings){
			ShaderProgram sp=new ShaderProgram("/Application/shaders/"+name+".cgx");
			for (int i = 0; i < uniformBindings.Length; i++)
				sp.SetUniformBinding(i, uniformBindings[i]);
			for (int i = 0; i < attributeBindings.Length; i++)
				sp.SetAttributeBinding(i, uniformBindings[i]);
			_shaders[name]=sp;
		}
		
		public static ShaderProgram Get(string index){
			return _shaders[index];
		}
	}
}

