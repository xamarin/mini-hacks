using System;
using System.IO;

using Android.Opengl;
using Android.Util;
using Android.Content.Res;

namespace CardboardMonkey
{
	public static class DrawingUtils
	{
		const string Tag = "DrawingUtils";

		public static int LoadGlShader (int type, Resources res, int resId)
		{
			string code = ReadRawTextFile (res, resId);
			int shader = GLES20.GlCreateShader(type);
			GLES20.GlShaderSource(shader, code);
			GLES20.GlCompileShader(shader);

			// Get the compilation status.
			int[] compileStatus = new int[1];
			GLES20.GlGetShaderiv(shader, GLES20.GlCompileStatus, compileStatus, 0);

			// If the compilation failed, delete the shader.
			if (compileStatus[0] == 0) {
				Log.Error(Tag, "Error compiling shader: " + GLES20.GlGetShaderInfoLog(shader));
				GLES20.GlDeleteShader(shader);
				shader = 0;
			}

			if (shader == 0)
				throw new InvalidOperationException("Error creating shader.");

			return shader;
		}

		static string ReadRawTextFile (Resources res, int resId)
		{
			return new StreamReader (res.OpenRawResource (resId)).ReadToEnd ();
		}

		public static int LoadGlTexture (Resources res, int resId)
		{
			var texture = new int[1];
			GLES20.GlGenTextures (1, texture, 0);
			if (texture [0] == 0)
				throw new InvalidOperationException ("Can't create texture");
			var options = new Android.Graphics.BitmapFactory.Options {
				InScaled = false
			};
			var bmp = Android.Graphics.BitmapFactory.DecodeResource (res, resId, options);
			GLES20.GlBindTexture (GLES20.GlTexture2d, texture [0]);
			GLES20.GlTexParameteri (GLES20.GlTexture2d, GLES20.GlTextureMinFilter, GLES20.GlNearest);
			GLES20.GlTexParameteri (GLES20.GlTexture2d, GLES20.GlTextureMagFilter, GLES20.GlNearest);

			GLUtils.TexImage2D (GLES20.GlTexture2d, 0, bmp, 0);
			bmp.Recycle ();

			return texture [0];
		}
	}
}

