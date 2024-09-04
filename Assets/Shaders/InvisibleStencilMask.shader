Shader "Custom/InvisibleStencilMask"
{
    Properties
    {
        _Stencil ("Stencil ID", Float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Geometry+1" "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Stencil
            {
                Ref [_Stencil]
                Comp Always
                Pass Replace
            }

            // Do not draw into the color buffer or the depth buffer
            ColorMask 0
            ZWrite Off
        }
    }

    Fallback Off
}
