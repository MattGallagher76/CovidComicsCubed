Shader "Custom/InvisibleAndHideShader"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        // Disable color writing
        ColorMask 0 

        Pass
        {
            // Apply a depth bias to ensure consistency
            Offset 1, 1

            // Render into depth buffer only
            ZWrite On
        }
    }

    // Disable shadow casting and receiving
    Fallback Off
}
