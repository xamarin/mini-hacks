using System;

using Android.Opengl;
using Android.Util;
using Android.Content;
using Android.Content.Res;
using Java.Nio;

namespace CardboardMonkey
{
	public class Game
	{
		const string Tag = "Game";

		const float CameraZ = 0.01f;
		const float TimeDelta = 0.3f;

		const float YawLimit = 0.12f;
		const float PitchLimit = 0.12f;

		// We keep the light always position just above the user.
		readonly float[] lightPosInWorldSpace = new float[] {0.0f, 2.0f, 0.0f, 1.0f};
		readonly float[] lightPosInEyeSpace = new float[4];

		const int CoordsPerVertex = 3;

		Resources resources;

		FloatBuffer floorVertices;
		FloatBuffer floorColors;
		FloatBuffer floorNormals;

		FloatBuffer cubeVertices;
		FloatBuffer cubeTextureCoords;
		FloatBuffer cubeNormals;

		int glProgram;
		int positionParam;
		int normalParam;
		int colorParam;
		int modelViewProjectionParam;
		int lightPosParam;
		int modelViewParam;
		int modelParam;
		int isFloorParam;
		int texture;
		int texCoordParam;

		int monkeyNotFound;
		int monkeyFound;

		float[] headView;
		float[] modelCube;
		float[] camera;
		float[] view;
		float[] modelViewProjection;
		float[] modelView;

		float[] modelFloor;

		float mObjectDistance = 12f;
		float mFloorDepth = 20f;

		public Game (Context context, float[] headView)
		{
			this.resources = context.Resources;
			this.headView = headView;
			modelCube = new float[16];
			camera = new float[16];
			view = new float[16];
			modelViewProjection = new float[16];
			modelView = new float[16];
			modelFloor = new float[16];
		}

		/// <summary>
		/// Tells if the user is currently looking at the object.
		/// </summary>
		/// <value><c>true</c> if the camera is looking at object; otherwise, <c>false</c>.</value>
		public bool IsLookingAtObject {
			get {
				var objPositionVec = ComputeObjectPosition ();

				var pitch = (float)Math.Atan2 (objPositionVec [1], -objPositionVec [2]);
				var yaw   = (float)Math.Atan2 (objPositionVec [0], -objPositionVec [2]);

				return (Math.Abs (pitch) < PitchLimit) && (Math.Abs (yaw) < YawLimit);
			}
		}

		/// <summary>
		/// An approximation of how far the camera is from the object.
		/// </summary>
		/// <value>The distance between 0 (on the object) and 1 (farthest away)</value>
		public double DistanceRatioFromObject {
			get {
				var objPositionVec = ComputeObjectPosition ();
				var yaw = (float)Math.Atan2 (objPositionVec [0], -objPositionVec [2]);

				return Math.Abs (yaw) / Math.PI;
			}
		}

		// Returns the (x, y, z, 1) translation vector of the object wrt to the camera
		float[] ComputeObjectPosition ()
		{
			float[] initVec = { 0, 0, 0, 1.0f };
			float[] objPositionVec = new float[4];

			// Convert object space to camera space. Use the headView from onNewFrame.
			Matrix.MultiplyMM (modelView, 0, headView, 0, modelCube, 0);
			Matrix.MultiplyMV (objPositionVec, 0, modelView, 0, initVec, 0);

			return objPositionVec;
		}

		/// <summary>
		/// Hide the object in a different world position.
		/// </summary>
		/// <param name="angleXZ">Horizontal angle in degrees, should be between 90° and 270°</param>
		/// <param name="angleY">Vertical angle in degrees, should be between -40° and 40°</param>
		/// <param name="distance">Distance in GL coordinates, good results between 10 and 20</param>
		public void HideObject (float angleXZ, float angleY, float distance)
		{
			var rotationMatrix = new float[16];
			var posVec = new float[4];

			// First rotate in XZ plane and scale so that we vary the object's distance from the user.			
			Matrix.SetRotateM (rotationMatrix, 0, angleXZ, 0f, 1f, 0f);
			float oldObjectDistance = mObjectDistance;
			mObjectDistance = distance;
			float objectScalingFactor = mObjectDistance / oldObjectDistance;
			Matrix.ScaleM (rotationMatrix, 0, objectScalingFactor, objectScalingFactor, objectScalingFactor);
			Matrix.MultiplyMV (posVec, 0, rotationMatrix, 0, modelCube, 12);

			// Now get the up or down angle
			angleY = (float)(angleY * Math.PI) / 180;
			float newY = (float)Math.Tan (angleY) * mObjectDistance;

			Matrix.SetIdentityM (modelCube, 0);
			Matrix.TranslateM (modelCube, 0, posVec[0], newY, posVec[2]);
		}

		public void Initialize ()
		{
			GLES20.GlClearColor(0.1f, 0.1f, 0.1f, 0.5f); // Dark background so text shows up well

			cubeVertices = PrepareBuffer (WorldLayoutData.CubeCoords);
			cubeNormals = PrepareBuffer (WorldLayoutData.CubeNormals);
			cubeTextureCoords = PrepareBuffer (WorldLayoutData.CubeTexCoords);

			floorVertices = PrepareBuffer (WorldLayoutData.FloorCoords);
			floorNormals = PrepareBuffer (WorldLayoutData.FloorNormals);
			floorColors = PrepareBuffer (WorldLayoutData.FloorColors);

			monkeyFound = DrawingUtils.LoadGlTexture (resources, Resource.Drawable.texture2);
			monkeyNotFound = DrawingUtils.LoadGlTexture (resources, Resource.Drawable.texture1);

			int vertexShader = DrawingUtils.LoadGlShader(GLES20.GlVertexShader, resources, Resource.Raw.vertex);
			int gridShader = DrawingUtils.LoadGlShader(GLES20.GlFragmentShader, resources, Resource.Raw.fragment);

			glProgram = GLES20.GlCreateProgram();
			GLES20.GlAttachShader(glProgram, vertexShader);
			GLES20.GlAttachShader(glProgram, gridShader);
			GLES20.GlLinkProgram(glProgram);

			GLES20.GlEnable(GLES20.GlDepthTest);

			// Object first appears directly in front of user
			Matrix.SetIdentityM(modelCube, 0);
			Matrix.TranslateM(modelCube, 0, 0, 0, -mObjectDistance);

			Matrix.SetIdentityM(modelFloor, 0);
			Matrix.TranslateM(modelFloor, 0, 0, -mFloorDepth, 0); // Floor appears below user

			CheckGlError("onSurfaceCreated");
		}

		public void PrepareFrame ()
		{
			GLES20.GlUseProgram(glProgram);

			modelViewProjectionParam = GLES20.GlGetUniformLocation(glProgram, "u_MVP");
			lightPosParam = GLES20.GlGetUniformLocation(glProgram, "u_LightPos");
			modelViewParam = GLES20.GlGetUniformLocation(glProgram, "u_MVMatrix");
			modelParam = GLES20.GlGetUniformLocation(glProgram, "u_Model");
			isFloorParam = GLES20.GlGetUniformLocation(glProgram, "u_IsFloor");
			texture = GLES20.GlGetUniformLocation (glProgram, "u_texture");

			// Build the Model part of the ModelView matrix.
			Matrix.RotateM(modelCube, 0, TimeDelta, 0.5f, 0.5f, 1.0f);

			// Build the camera matrix and apply it to the ModelView.
			Matrix.SetLookAtM(camera, 0, 0.0f, 0.0f, CameraZ, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);

			CheckGlError("onReadyToDraw");
		}

		public void FinishFrame ()
		{

		}

		public void Draw (float[] eyeView, float[] perspective)
		{
			GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit);

			positionParam = GLES20.GlGetAttribLocation(glProgram, "a_Position");
			normalParam = GLES20.GlGetAttribLocation(glProgram, "a_Normal");
			colorParam = GLES20.GlGetAttribLocation(glProgram, "a_Color");
			texCoordParam = GLES20.GlGetAttribLocation (glProgram, "a_texcoord");

			GLES20.GlEnableVertexAttribArray(positionParam);
			GLES20.GlEnableVertexAttribArray(normalParam);
			GLES20.GlEnableVertexAttribArray(texCoordParam);

			// Apply the eye transformation to the camera.
			Matrix.MultiplyMM(view, 0, eyeView, 0, camera, 0);

			// Set the position of the light
			Matrix.MultiplyMV(lightPosInEyeSpace, 0, view, 0, lightPosInWorldSpace, 0);
			GLES20.GlUniform3f(lightPosParam, lightPosInEyeSpace[0], lightPosInEyeSpace[1],
			                   lightPosInEyeSpace[2]);

			// Build the ModelView and ModelViewProjection matrices
			// for calculating cube position and light.
			Matrix.MultiplyMM(modelView, 0, view, 0, modelCube, 0);
			Matrix.MultiplyMM(modelViewProjection, 0, perspective, 0, modelView, 0);
			DrawCube ();

			// Set mModelView for the floor, so we draw floor in the correct location
			Matrix.MultiplyMM(modelView, 0, view, 0, modelFloor, 0);
			Matrix.MultiplyMM(modelViewProjection, 0, perspective, 0,
			                  modelView, 0);
			DrawFloor (perspective);
		}

		void DrawCube ()
		{
			// This is not the floor!
			GLES20.GlUniform1f(isFloorParam, 0f);

			// Set the Model in the shader, used to calculate lighting
			GLES20.GlUniformMatrix4fv(modelParam, 1, false, modelCube, 0);

			// Set the ModelView in the shader, used to calculate lighting
			GLES20.GlUniformMatrix4fv(modelViewParam, 1, false, modelView, 0);

			// Set the position of the cube
			GLES20.GlVertexAttribPointer(positionParam, CoordsPerVertex, GLES20.GlFloat, false, 0, cubeVertices);

			// Set the ModelViewProjection matrix in the shader.
			GLES20.GlUniformMatrix4fv(modelViewProjectionParam, 1, false, modelViewProjection, 0);

			// Set the normal positions of the cube, again for shading
			GLES20.GlVertexAttribPointer(normalParam, 3, GLES20.GlFloat, false, 0, cubeNormals);

			// Disable color for cube
			GLES20.GlDisableVertexAttribArray (colorParam);

			// Set the texture coordinates
			GLES20.GlVertexAttribPointer (texCoordParam, 2, GLES20.GlFloat, false, 0, cubeTextureCoords);

			GLES20.GlActiveTexture (GLES20.GlTexture0);

			if (IsLookingAtObject)
				GLES20.GlBindTexture (GLES20.GlTexture2d, monkeyFound);
			else
				GLES20.GlBindTexture (GLES20.GlTexture2d, monkeyNotFound);

			GLES20.GlUniform1i (texture, 0);

			GLES20.GlDrawArrays(GLES20.GlTriangles, 0, 36);
			CheckGlError("Drawing cube");
		}

		void DrawFloor(float[] perspective)
		{
			// This is the floor!
			GLES20.GlUniform1f(isFloorParam, 1f);

			// Set ModelView, MVP, position, normals, and color
			GLES20.GlUniformMatrix4fv(modelParam, 1, false, modelFloor, 0);
			GLES20.GlUniformMatrix4fv(modelViewParam, 1, false, modelView, 0);
			GLES20.GlUniformMatrix4fv(modelViewProjectionParam, 1, false, modelViewProjection, 0);
			GLES20.GlVertexAttribPointer(positionParam, CoordsPerVertex, GLES20.GlFloat,
			                             false, 0, floorVertices);
			GLES20.GlVertexAttribPointer(normalParam, 3, GLES20.GlFloat, false, 0, floorNormals);

			GLES20.GlEnableVertexAttribArray (colorParam);
			GLES20.GlVertexAttribPointer(colorParam, 4, GLES20.GlFloat, false, 0, floorColors);

			GLES20.GlDrawArrays(GLES20.GlTriangles, 0, 6);

			CheckGlError("Drawing floor");
		}

		FloatBuffer PrepareBuffer (float[] data)
		{
			ByteBuffer buffer = ByteBuffer.AllocateDirect (data.Length * 4);
			buffer.Order(ByteOrder.NativeOrder());
			var result = buffer.AsFloatBuffer();
			result.Put (data);
			result.Position(0);

			return result;
		}

		static void CheckGlError (string func)
		{
			int error;
			while ((error = GLES20.GlGetError()) != GLES20.GlNoError) {
				Log.Error(Tag, func + ": GlError " + error);
				throw new InvalidOperationException(func + ": GlError " + error);
			}
		}
	}
}

