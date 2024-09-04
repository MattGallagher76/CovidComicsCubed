Shader "Custom/StencilMaskTransparent"
{
    SubShader
    {
        // First pass: Write to the stencil buffer
        Tags { "Queue" = "Geometry-1" }
        Stencil
        {
            Ref 1          // Reference value for the stencil buffer
            Comp Always    // Always set the stencil buffer to the reference value
            Pass Replace   // Replace the stencil buffer value with the reference value
        }
        ZWrite On
        ColorMask 0      // Do not write any color
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
                return float4(0, 0, 0, 0);
            }
            ENDCG
        }
    }

        SubShader
            {
                // Second pass: Render the transparent plane only where the stencil buffer is set
                Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
                Stencil
                {
                    Ref 1           // Reference value for the stencil buffer
                    Comp Equal      // Render only where the stencil buffer is equal to the reference value
                    Pass Keep
                }
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
                        // Return a fully transparent color
                        return float4(0, 0, 0, 0);
                    }
                    ENDCG
                }
            }

                FallBack "Diffuse"
}
