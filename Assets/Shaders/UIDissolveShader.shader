Shader "Unlit/UIDissolveShader"
{
	Properties
	{
		[HDR]Color_AB9AD2CA("Color", Color) = (1, 1, 1, 1)
		[NoScaleOffset]_MainTex("MainTexture", 2D) = "white" {}
		_DissolveAmount("DissolveAmount", Range(0, 1)) = 0.0
		_DissolveScale("DissolveScale", Float) = 25


		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}
    SubShader
    {

		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Transparent"
			"Queue" = "Transparent+0"
		}

		Pass
		{
			Name "Sprite Lit"
			Tags
			{
				"LightMode" = "Universal2D"
			}

		// Render State
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		Cull Off
		ZTest[unity_GUIZTestMode]
		ZWrite Off
		// ColorMask

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		ColorMask[_ColorMask]


		HLSLPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		// Pragmas
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x
		#pragma target 2.0

		// Keywords
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
		#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
		#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
		#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define ATTRIBUTES_NEED_TEXCOORD0
		#define ATTRIBUTES_NEED_COLOR
		#define VARYINGS_NEED_TEXCOORD0
		#define VARYINGS_NEED_COLOR
		#define VARYINGS_NEED_SCREENPOSITION
		#define SHADERPASS_SPRITELIT

		// Includes
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

		// --------------------------------------------------
		// Graph

		// Graph Properties
		CBUFFER_START(UnityPerMaterial)
		float4 Color_AB9AD2CA;
		float _DissolveAmount;
		float _DissolveScale;
		CBUFFER_END
		TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
		SAMPLER(_SampleTexture2D_AC398F52_Sampler_3_Linear_Repeat);

		// Graph Functions

		void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
		{
			Out = A * B;
		}

		void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
		{
			Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
		}

		void Unity_OneMinus_float(float In, out float Out)
		{
			Out = 1 - In;
		}


		inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
		{
			return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
		}

		inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
		{
			return (1.0 - t)*a + (t*b);
		}


		inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
		{
			float2 i = floor(uv);
			float2 f = frac(uv);
			f = f * f * (3.0 - 2.0 * f);

			uv = abs(frac(uv) - 0.5);
			float2 c0 = i + float2(0.0, 0.0);
			float2 c1 = i + float2(1.0, 0.0);
			float2 c2 = i + float2(0.0, 1.0);
			float2 c3 = i + float2(1.0, 1.0);
			float r0 = Unity_SimpleNoise_RandomValue_float(c0);
			float r1 = Unity_SimpleNoise_RandomValue_float(c1);
			float r2 = Unity_SimpleNoise_RandomValue_float(c2);
			float r3 = Unity_SimpleNoise_RandomValue_float(c3);

			float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
			float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
			float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
			return t;
		}
		void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
		{
			float t = 0.0;

			float freq = pow(2.0, float(0));
			float amp = pow(0.5, float(3 - 0));
			t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

			freq = pow(2.0, float(1));
			amp = pow(0.5, float(3 - 1));
			t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

			freq = pow(2.0, float(2));
			amp = pow(0.5, float(3 - 2));
			t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

			Out = t;
		}

		void Unity_Add_float(float A, float B, out float Out)
		{
			Out = A + B;
		}

		void Unity_Clamp_float(float In, float Min, float Max, out float Out)
		{
			Out = clamp(In, Min, Max);
		}

		// Graph Vertex
		// GraphVertex: <None>

		// Graph Pixel
		struct SurfaceDescriptionInputs
		{
			float4 uv0;
		};

		struct SurfaceDescription
		{
			float4 Color;
			float4 Mask;
		};

		SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
		{
			SurfaceDescription surface = (SurfaceDescription)0;
			float4 _Property_D8F76F6_Out_0 = Color_AB9AD2CA;
			float4 _SampleTexture2D_AC398F52_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
			float _SampleTexture2D_AC398F52_R_4 = _SampleTexture2D_AC398F52_RGBA_0.r;
			float _SampleTexture2D_AC398F52_G_5 = _SampleTexture2D_AC398F52_RGBA_0.g;
			float _SampleTexture2D_AC398F52_B_6 = _SampleTexture2D_AC398F52_RGBA_0.b;
			float _SampleTexture2D_AC398F52_A_7 = _SampleTexture2D_AC398F52_RGBA_0.a;
			float4 _Multiply_2084DFB9_Out_2;
			Unity_Multiply_float(_Property_D8F76F6_Out_0, _SampleTexture2D_AC398F52_RGBA_0, _Multiply_2084DFB9_Out_2);
			float _Property_868B3EC1_Out_0 = _DissolveAmount;
			float _Remap_E6EF365C_Out_3;
			Unity_Remap_float(_Property_868B3EC1_Out_0, float2 (0, 1), float2 (0.5, 1.4), _Remap_E6EF365C_Out_3);
			float _OneMinus_E17EDFAE_Out_1;
			Unity_OneMinus_float(_Remap_E6EF365C_Out_3, _OneMinus_E17EDFAE_Out_1);
			float _Property_C98B9AB_Out_0 = _DissolveScale;
			float _SimpleNoise_A03B704F_Out_2;
			Unity_SimpleNoise_float(IN.uv0.xy, _Property_C98B9AB_Out_0, _SimpleNoise_A03B704F_Out_2);
			float _Add_A6E6417C_Out_2;
			Unity_Add_float(_OneMinus_E17EDFAE_Out_1, _SimpleNoise_A03B704F_Out_2, _Add_A6E6417C_Out_2);
			float _Remap_450C4373_Out_3;
			Unity_Remap_float(_Add_A6E6417C_Out_2, float2 (0, 1), float2 (-4, 4), _Remap_450C4373_Out_3);
			float _Clamp_7E50D601_Out_3;
			Unity_Clamp_float(_Remap_450C4373_Out_3, 0, 1, _Clamp_7E50D601_Out_3);
			float4 _Multiply_882D4908_Out_2;
			Unity_Multiply_float(_Multiply_2084DFB9_Out_2, (_Clamp_7E50D601_Out_3.xxxx), _Multiply_882D4908_Out_2);
			surface.Color = _Multiply_882D4908_Out_2;
			surface.Mask = (_Clamp_7E50D601_Out_3.xxxx);
			return surface;
		}

		// --------------------------------------------------
		// Structs and Packing

		// Generated Type: Attributes
		struct Attributes
		{
			float3 positionOS : POSITION;
			float3 normalOS : NORMAL;
			float4 tangentOS : TANGENT;
			float4 uv0 : TEXCOORD0;
			float4 color : COLOR;
			#if UNITY_ANY_INSTANCING_ENABLED
			uint instanceID : INSTANCEID_SEMANTIC;
			#endif
		};

		// Generated Type: Varyings
		struct Varyings
		{
			float4 positionCS : SV_POSITION;
			float4 texCoord0;
			float4 color;
			float4 screenPosition;
			#if UNITY_ANY_INSTANCING_ENABLED
			uint instanceID : CUSTOM_INSTANCE_ID;
			#endif
			#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
			#endif
			#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
			#endif
			#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
			#endif
		};

		// Generated Type: PackedVaryings
		struct PackedVaryings
		{
			float4 positionCS : SV_POSITION;
			#if UNITY_ANY_INSTANCING_ENABLED
			uint instanceID : CUSTOM_INSTANCE_ID;
			#endif
			float4 interp00 : TEXCOORD0;
			float4 interp01 : TEXCOORD1;
			float4 interp02 : TEXCOORD2;
			#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
			#endif
			#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
			#endif
			#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
			#endif
		};

		// Packed Type: Varyings
		PackedVaryings PackVaryings(Varyings input)
		{
			PackedVaryings output = (PackedVaryings)0;
			output.positionCS = input.positionCS;
			output.interp00.xyzw = input.texCoord0;
			output.interp01.xyzw = input.color;
			output.interp02.xyzw = input.screenPosition;
			#if UNITY_ANY_INSTANCING_ENABLED
			output.instanceID = input.instanceID;
			#endif
			#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
			#endif
			#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
			#endif
			#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			output.cullFace = input.cullFace;
			#endif
			return output;
		}

		// Unpacked Type: Varyings
		Varyings UnpackVaryings(PackedVaryings input)
		{
			Varyings output = (Varyings)0;
			output.positionCS = input.positionCS;
			output.texCoord0 = input.interp00.xyzw;
			output.color = input.interp01.xyzw;
			output.screenPosition = input.interp02.xyzw;
			#if UNITY_ANY_INSTANCING_ENABLED
			output.instanceID = input.instanceID;
			#endif
			#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
			output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
			#endif
			#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
			output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
			#endif
			#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			output.cullFace = input.cullFace;
			#endif
			return output;
		}

		// --------------------------------------------------
		// Build Graph Inputs

		SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
		{
			SurfaceDescriptionInputs output;
			ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





			output.uv0 = input.texCoord0;
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
		#else
		#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
		#endif
		#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

			return output;
		}


		// --------------------------------------------------
		// Main

		#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteLitPass.hlsl"

		ENDHLSL
	}

	Pass
	{
		Name "Sprite Normal"
		Tags
		{
			"LightMode" = "NormalsRendering"
		}

			// Render State
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			Cull Off
			ZTest LEqual
			ZWrite Off
			// ColorMask: <None>


			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// Debug
			// <None>

			// --------------------------------------------------
			// Pass

			// Pragmas
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// Keywords
			// PassKeywords: <None>
			// GraphKeywords: <None>

			// Defines
			#define _SURFACE_TYPE_TRANSPARENT 1
			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define ATTRIBUTES_NEED_TEXCOORD0
			#define VARYINGS_NEED_NORMAL_WS
			#define VARYINGS_NEED_TANGENT_WS
			#define VARYINGS_NEED_TEXCOORD0
			#define SHADERPASS_SPRITENORMAL

			// Includes
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

			// --------------------------------------------------
			// Graph

			// Graph Properties
			CBUFFER_START(UnityPerMaterial)
			float4 Color_AB9AD2CA;
			float _DissolveAmount;
			float _DissolveScale;
			CBUFFER_END
			TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
			SAMPLER(_SampleTexture2D_AC398F52_Sampler_3_Linear_Repeat);

			// Graph Functions

			void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
			{
				Out = A * B;
			}

			void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
			{
				Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
			}

			void Unity_OneMinus_float(float In, out float Out)
			{
				Out = 1 - In;
			}


			inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
			{
				return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
			}

			inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
			{
				return (1.0 - t)*a + (t*b);
			}


			inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac(uv);
				f = f * f * (3.0 - 2.0 * f);

				uv = abs(frac(uv) - 0.5);
				float2 c0 = i + float2(0.0, 0.0);
				float2 c1 = i + float2(1.0, 0.0);
				float2 c2 = i + float2(0.0, 1.0);
				float2 c3 = i + float2(1.0, 1.0);
				float r0 = Unity_SimpleNoise_RandomValue_float(c0);
				float r1 = Unity_SimpleNoise_RandomValue_float(c1);
				float r2 = Unity_SimpleNoise_RandomValue_float(c2);
				float r3 = Unity_SimpleNoise_RandomValue_float(c3);

				float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
				float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
				float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
				return t;
			}
			void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
			{
				float t = 0.0;

				float freq = pow(2.0, float(0));
				float amp = pow(0.5, float(3 - 0));
				t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3 - 1));
				t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3 - 2));
				t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

				Out = t;
			}

			void Unity_Add_float(float A, float B, out float Out)
			{
				Out = A + B;
			}

			void Unity_Clamp_float(float In, float Min, float Max, out float Out)
			{
				Out = clamp(In, Min, Max);
			}

			// Graph Vertex
			// GraphVertex: <None>

			// Graph Pixel
			struct SurfaceDescriptionInputs
			{
				float4 uv0;
			};

			struct SurfaceDescription
			{
				float4 Color;
				float3 Normal;
			};

			SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
			{
				SurfaceDescription surface = (SurfaceDescription)0;
				float4 _Property_D8F76F6_Out_0 = Color_AB9AD2CA;
				float4 _SampleTexture2D_AC398F52_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
				float _SampleTexture2D_AC398F52_R_4 = _SampleTexture2D_AC398F52_RGBA_0.r;
				float _SampleTexture2D_AC398F52_G_5 = _SampleTexture2D_AC398F52_RGBA_0.g;
				float _SampleTexture2D_AC398F52_B_6 = _SampleTexture2D_AC398F52_RGBA_0.b;
				float _SampleTexture2D_AC398F52_A_7 = _SampleTexture2D_AC398F52_RGBA_0.a;
				float4 _Multiply_2084DFB9_Out_2;
				Unity_Multiply_float(_Property_D8F76F6_Out_0, _SampleTexture2D_AC398F52_RGBA_0, _Multiply_2084DFB9_Out_2);
				float _Property_868B3EC1_Out_0 = _DissolveAmount;
				float _Remap_E6EF365C_Out_3;
				Unity_Remap_float(_Property_868B3EC1_Out_0, float2 (0, 1), float2 (0.5, 1.4), _Remap_E6EF365C_Out_3);
				float _OneMinus_E17EDFAE_Out_1;
				Unity_OneMinus_float(_Remap_E6EF365C_Out_3, _OneMinus_E17EDFAE_Out_1);
				float _Property_C98B9AB_Out_0 = _DissolveScale;
				float _SimpleNoise_A03B704F_Out_2;
				Unity_SimpleNoise_float(IN.uv0.xy, _Property_C98B9AB_Out_0, _SimpleNoise_A03B704F_Out_2);
				float _Add_A6E6417C_Out_2;
				Unity_Add_float(_OneMinus_E17EDFAE_Out_1, _SimpleNoise_A03B704F_Out_2, _Add_A6E6417C_Out_2);
				float _Remap_450C4373_Out_3;
				Unity_Remap_float(_Add_A6E6417C_Out_2, float2 (0, 1), float2 (-4, 4), _Remap_450C4373_Out_3);
				float _Clamp_7E50D601_Out_3;
				Unity_Clamp_float(_Remap_450C4373_Out_3, 0, 1, _Clamp_7E50D601_Out_3);
				float4 _Multiply_882D4908_Out_2;
				Unity_Multiply_float(_Multiply_2084DFB9_Out_2, (_Clamp_7E50D601_Out_3.xxxx), _Multiply_882D4908_Out_2);
				surface.Color = _Multiply_882D4908_Out_2;
				surface.Normal = float3 (0, 0, 1);
				return surface;
			}

			// --------------------------------------------------
			// Structs and Packing

			// Generated Type: Attributes
			struct Attributes
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};

			// Generated Type: Varyings
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS;
				float4 tangentWS;
				float4 texCoord0;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			// Generated Type: PackedVaryings
			struct PackedVaryings
			{
				float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
				float3 interp00 : TEXCOORD0;
				float4 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			// Packed Type: Varyings
			PackedVaryings PackVaryings(Varyings input)
			{
				PackedVaryings output = (PackedVaryings)0;
				output.positionCS = input.positionCS;
				output.interp00.xyz = input.normalWS;
				output.interp01.xyzw = input.tangentWS;
				output.interp02.xyzw = input.texCoord0;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				output.cullFace = input.cullFace;
				#endif
				return output;
			}

			// Unpacked Type: Varyings
			Varyings UnpackVaryings(PackedVaryings input)
			{
				Varyings output = (Varyings)0;
				output.positionCS = input.positionCS;
				output.normalWS = input.interp00.xyz;
				output.tangentWS = input.interp01.xyzw;
				output.texCoord0 = input.interp02.xyzw;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				output.cullFace = input.cullFace;
				#endif
				return output;
			}

			// --------------------------------------------------
			// Build Graph Inputs

			SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
			{
				SurfaceDescriptionInputs output;
				ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





				output.uv0 = input.texCoord0;
			#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
			#else
			#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
			#endif
			#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

				return output;
			}


			// --------------------------------------------------
			// Main

			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteNormalPass.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "Sprite Forward"
			Tags
			{
				"LightMode" = "UniversalForward"
			}

				// Render State
				Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
				Cull Off
				ZTest LEqual
				ZWrite Off
				// ColorMask: <None>


				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				// Debug
				// <None>

				// --------------------------------------------------
				// Pass

				// Pragmas
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0

				// Keywords
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				// GraphKeywords: <None>

				// Defines
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define ATTRIBUTES_NEED_NORMAL
				#define ATTRIBUTES_NEED_TANGENT
				#define ATTRIBUTES_NEED_TEXCOORD0
				#define ATTRIBUTES_NEED_COLOR
				#define VARYINGS_NEED_TEXCOORD0
				#define VARYINGS_NEED_COLOR
				#define SHADERPASS_SPRITEFORWARD

				// Includes
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

				// --------------------------------------------------
				// Graph

				// Graph Properties
				CBUFFER_START(UnityPerMaterial)
				float4 Color_AB9AD2CA;
				float _DissolveAmount;
				float _DissolveScale;
				CBUFFER_END
				TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
				SAMPLER(_SampleTexture2D_AC398F52_Sampler_3_Linear_Repeat);

				// Graph Functions

				void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
				{
					Out = A * B;
				}

				void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
				{
					Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
				}

				void Unity_OneMinus_float(float In, out float Out)
				{
					Out = 1 - In;
				}


				inline float Unity_SimpleNoise_RandomValue_float(float2 uv)
				{
					return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
				}

				inline float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
				{
					return (1.0 - t)*a + (t*b);
				}


				inline float Unity_SimpleNoise_ValueNoise_float(float2 uv)
				{
					float2 i = floor(uv);
					float2 f = frac(uv);
					f = f * f * (3.0 - 2.0 * f);

					uv = abs(frac(uv) - 0.5);
					float2 c0 = i + float2(0.0, 0.0);
					float2 c1 = i + float2(1.0, 0.0);
					float2 c2 = i + float2(0.0, 1.0);
					float2 c3 = i + float2(1.0, 1.0);
					float r0 = Unity_SimpleNoise_RandomValue_float(c0);
					float r1 = Unity_SimpleNoise_RandomValue_float(c1);
					float r2 = Unity_SimpleNoise_RandomValue_float(c2);
					float r3 = Unity_SimpleNoise_RandomValue_float(c3);

					float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
					float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
					float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
					return t;
				}
				void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
				{
					float t = 0.0;

					float freq = pow(2.0, float(0));
					float amp = pow(0.5, float(3 - 0));
					t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

					freq = pow(2.0, float(1));
					amp = pow(0.5, float(3 - 1));
					t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

					freq = pow(2.0, float(2));
					amp = pow(0.5, float(3 - 2));
					t += Unity_SimpleNoise_ValueNoise_float(float2(UV.x*Scale / freq, UV.y*Scale / freq))*amp;

					Out = t;
				}

				void Unity_Add_float(float A, float B, out float Out)
				{
					Out = A + B;
				}

				void Unity_Clamp_float(float In, float Min, float Max, out float Out)
				{
					Out = clamp(In, Min, Max);
				}

				// Graph Vertex
				// GraphVertex: <None>

				// Graph Pixel
				struct SurfaceDescriptionInputs
				{
					float4 uv0;
				};

				struct SurfaceDescription
				{
					float4 Color;
					float3 Normal;
				};

				SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
				{
					SurfaceDescription surface = (SurfaceDescription)0;
					float4 _Property_D8F76F6_Out_0 = Color_AB9AD2CA;
					float4 _SampleTexture2D_AC398F52_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
					float _SampleTexture2D_AC398F52_R_4 = _SampleTexture2D_AC398F52_RGBA_0.r;
					float _SampleTexture2D_AC398F52_G_5 = _SampleTexture2D_AC398F52_RGBA_0.g;
					float _SampleTexture2D_AC398F52_B_6 = _SampleTexture2D_AC398F52_RGBA_0.b;
					float _SampleTexture2D_AC398F52_A_7 = _SampleTexture2D_AC398F52_RGBA_0.a;
					float4 _Multiply_2084DFB9_Out_2;
					Unity_Multiply_float(_Property_D8F76F6_Out_0, _SampleTexture2D_AC398F52_RGBA_0, _Multiply_2084DFB9_Out_2);
					float _Property_868B3EC1_Out_0 = _DissolveAmount;
					float _Remap_E6EF365C_Out_3;
					Unity_Remap_float(_Property_868B3EC1_Out_0, float2 (0, 1), float2 (0.5, 1.4), _Remap_E6EF365C_Out_3);
					float _OneMinus_E17EDFAE_Out_1;
					Unity_OneMinus_float(_Remap_E6EF365C_Out_3, _OneMinus_E17EDFAE_Out_1);
					float _Property_C98B9AB_Out_0 = _DissolveScale;
					float _SimpleNoise_A03B704F_Out_2;
					Unity_SimpleNoise_float(IN.uv0.xy, _Property_C98B9AB_Out_0, _SimpleNoise_A03B704F_Out_2);
					float _Add_A6E6417C_Out_2;
					Unity_Add_float(_OneMinus_E17EDFAE_Out_1, _SimpleNoise_A03B704F_Out_2, _Add_A6E6417C_Out_2);
					float _Remap_450C4373_Out_3;
					Unity_Remap_float(_Add_A6E6417C_Out_2, float2 (0, 1), float2 (-4, 4), _Remap_450C4373_Out_3);
					float _Clamp_7E50D601_Out_3;
					Unity_Clamp_float(_Remap_450C4373_Out_3, 0, 1, _Clamp_7E50D601_Out_3);
					float4 _Multiply_882D4908_Out_2;
					Unity_Multiply_float(_Multiply_2084DFB9_Out_2, (_Clamp_7E50D601_Out_3.xxxx), _Multiply_882D4908_Out_2);
					surface.Color = _Multiply_882D4908_Out_2;
					surface.Normal = float3 (0, 0, 1);
					return surface;
				}

				// --------------------------------------------------
				// Structs and Packing

				// Generated Type: Attributes
				struct Attributes
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					float4 tangentOS : TANGENT;
					float4 uv0 : TEXCOORD0;
					float4 color : COLOR;
					#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};

				// Generated Type: Varyings
				struct Varyings
				{
					float4 positionCS : SV_POSITION;
					float4 texCoord0;
					float4 color;
					#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : CUSTOM_INSTANCE_ID;
					#endif
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
					#endif
				};

				// Generated Type: PackedVaryings
				struct PackedVaryings
				{
					float4 positionCS : SV_POSITION;
					#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : CUSTOM_INSTANCE_ID;
					#endif
					float4 interp00 : TEXCOORD0;
					float4 interp01 : TEXCOORD1;
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
					#endif
				};

				// Packed Type: Varyings
				PackedVaryings PackVaryings(Varyings input)
				{
					PackedVaryings output = (PackedVaryings)0;
					output.positionCS = input.positionCS;
					output.interp00.xyzw = input.texCoord0;
					output.interp01.xyzw = input.color;
					#if UNITY_ANY_INSTANCING_ENABLED
					output.instanceID = input.instanceID;
					#endif
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					output.cullFace = input.cullFace;
					#endif
					return output;
				}

				// Unpacked Type: Varyings
				Varyings UnpackVaryings(PackedVaryings input)
				{
					Varyings output = (Varyings)0;
					output.positionCS = input.positionCS;
					output.texCoord0 = input.interp00.xyzw;
					output.color = input.interp01.xyzw;
					#if UNITY_ANY_INSTANCING_ENABLED
					output.instanceID = input.instanceID;
					#endif
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					output.cullFace = input.cullFace;
					#endif
					return output;
				}

				// --------------------------------------------------
				// Build Graph Inputs

				SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
				{
					SurfaceDescriptionInputs output;
					ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





					output.uv0 = input.texCoord0;
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
				#else
				#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
				#endif
				#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

					return output;
				}


				// --------------------------------------------------
				// Main

				#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteForwardPass.hlsl"

				ENDHLSL
			}

	}
		FallBack "Hidden/Shader Graph/FallbackError"
}