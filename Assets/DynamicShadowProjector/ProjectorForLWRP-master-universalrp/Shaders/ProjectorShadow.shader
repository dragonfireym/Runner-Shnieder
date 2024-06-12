Shader "Projector For LWRP/Shadow" 
{
	Properties {
		[NoScaleOffset] _ShadowTex ("Cookie", 2D) = "gray" {}
		[NoScaleOffset] _FalloffTex ("FallOff", 2D) = "white" {}
		_Offset ("Offset", Range (-10, 0)) = -1.0
		_OffsetSlope ("Offset Slope Factor", Range (-1, 0)) = -1.0
	}
	SubShader
	{
		Tags {"Queue"="Transparent-1"}
        // Shader code
		Pass
        {
			ZWrite Off
			Fog { Color (1, 1, 1) }
			ColorMask RGB
			Blend DstColor Zero
			Offset [_OffsetSlope], [_Offset]

			HLSLPROGRAM
			#pragma vertex p4lwrp_vert_projector
			#pragma fragment p4lwrp_frag_projector_shadow
			#pragma shader_feature_local FSR_PROJECTOR_FOR_LWRP
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#include "P4LWRP.cginc"
			#include "Assets/DynamicShadowProjector/ProjectorForLWRP-master-universalrp/Shaders/P4LWRP.cginc"

			P4LWRP_V2F_PROJECTOR vert(float4 vertex : POSITION)
				{
					P4LWRP_V2F_PROJECTOR o;
					fsrTransformVertex(vertex, o.pos, o.uvShadow);
					UNITY_TRANSFER_FOG(o, o.pos);
					return o;
				}
 
				fixed4 frag(P4LWRP_V2F_PROJECTOR i) : SV_Target
				{
					fixed4 col;
 					fixed falloff = tex2D(_FalloffTex, i.uvShadow.zz).a;
					col.rgb = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow)).rgb;
					col.a = 1.0f;
					col.rgb = lerp(fixed3(1,1,1), col.rgb, falloff);
					UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(1,1,1,1));
					return col;
				}
			ENDHLSL
		}
	} 
}
