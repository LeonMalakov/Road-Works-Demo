Shader "RenderTexture/RoadMaskShader"
{
    Properties
    {
        _Brush ("Texture", 2D) = "white" {}
    }
    SubShader
    {
       Lighting off
       Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0            

            sampler2D _Brush;
            float4 _Brush_ST;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float size = 0.0002f * _CustomRenderTextureWidth;

                // sample the texture
                float4 col = tex2D(_SelfTexture2D, IN.globalTexcoord.xy);

               // float4 draw = smoothstep(0, 0.2, distance(IN.localTexcoord.xy, _Brush_ST.xy));
                
                if (distance(IN.globalTexcoord.xy, _Brush_ST.wz) < size)
                    col = 0;


                return col;
            }
            ENDCG
        }
    }
}
