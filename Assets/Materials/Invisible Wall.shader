Shader "Custom/InvisibleAndHideBehind"
{
    SubShader
    {
        Tags { "Queue" = "Geometry+100" }
        
        Pass
        {
            // Don't render the object itself
            ColorMask 0

            // Render into the depth buffer to occlude objects behind it
            ZWrite On
            ZTest LEqual
        }
    }
    FallBack "Diffuse"
}
