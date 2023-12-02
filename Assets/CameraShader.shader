Shader "Hidden/CameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 viewVector : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                float3 view = mul(unity_CameraInvProjection, float4(v.uv, 0,-1));
                o.viewVector = mul(unity_CameraToWorld, float4(view, 0));
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv)) * length(i.viewVector);
                //if(depth < 1000) {
                //    int increaseRate = 3;
                //    col.r = max(col.r - depth * increaseRate / 251,0.251);
                //    col.g = max(col.g - depth * increaseRate / 541,0.541);
                //    col.b = max(col.b - depth * increaseRate / 616,0.616);
                //}
                return col;
            }
            ENDCG
        }
    }
}
