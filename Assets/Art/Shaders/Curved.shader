Shader "Unlit/Curved"
{
   Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            fixed4 _MainColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Apply curved world effect
                float2 uv = i.vertex.xy / i.vertex.w;
                float r = length(uv);
                uv = normalize(uv);
                float angle = atan2(uv.y, uv.x) + sin(r * 6.283) * 0.1; // Adjust the curvature intensity here
                uv = float2(cos(angle), sin(angle)) * r;
                uv *= i.vertex.w; // Scale back to screen space
                uv = uv.xy;

                // Use the original color as-is
                fixed4 originalColor = i.color;

                // Combine the original color with the curved world effect
                fixed4 finalColor = originalColor * _MainColor;

                return finalColor;
            }
            ENDCG
        }
    }
}
