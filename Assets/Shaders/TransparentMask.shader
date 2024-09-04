Shader "Custom/TransparentDepthMask"
{
    SubShader
    {
        // Render the depth of the plane to mask objects behind it
        Tags {"Queue" = "Overlay" }
        ZWrite On
        ColorMask 0
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // This fragment shader does nothing, but it's needed to complete the pass
                return float4(0,0,0,0);
            }
            ENDCG
        }
    }

        SubShader
            {
                // Render the transparent plane
                Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha

                Pass
                {
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #include "UnityCG.cginc"

                    struct appdata
                    {
                        float4 vertex : POSITION;
                    };

                    struct v2f
                    {
                        float4 pos : SV_POSITION;
                    };

                    v2f vert(appdata v)
                    {
                        v2f o;
                        o.pos = UnityObjectToClipPos(v.vertex);
                        return o;
                    }

                    float4 frag(v2f i) : SV_Target
                    {
                        // Return a transparent color
                        return float4(0, 0, 0, 0);
                    }
                    ENDCG
                }
            }

                FallBack "Diffuse"
}
