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
                float3 view = mul(unity_CameraInvProjection, float4(v.uv * 2 - 1, 0, -1));
                o.viewVector = mul(unity_CameraToWorld, float4(view, 0));
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            StructuredBuffer<float3> perlin;
            float lowerBoundX;
            float lowerBoundY;
            float lowerBoundZ;
            float boundX;
            float boundY;
            float boundZ;
            int xGridMax;
            int yGridMax;
            int zGridMax;
            int sampleResolution = 10;

            float interp(float start, float end, float along)
            {
                if (along <= 0)
                {
                    along = 0;
                }
                else if (along >= 1)
                {
                    along = 1;
                }
                return start + (3 * along * along - 2 * along * along * along) * (end - start); // Using smoothstep here https://en.wikipedia.org/wiki/Smoothstep
            }
            float3 getFromPerlin(int x, int y, int z) {
                return perlin[z + y * yGridMax + x * xGridMax * xGridMax];
            }
            float getGrad(float x, float y, float z, int x_grid, int y_grid, int z_grid)
            {
                float3 grid = getFromPerlin(x_grid, y_grid, z_grid);
                return (x - x_grid) * grid.x + (y - y_grid) * grid.y + (z - z_grid) * grid.z;
            }
            float getValueAtCoords(float x, float y, float z)
            {
                float scaledX = (x - lowerBoundX) / (boundX - lowerBoundX + 1) * xGridMax;
                float scaledY = (y - lowerBoundY) / (boundY - lowerBoundY + 1) * yGridMax;
                float scaledZ = (z - lowerBoundZ) / (boundZ - lowerBoundZ + 1) * zGridMax;
                int x_less = int(floor(scaledX));
                int x_greater = x_less + 1;
                int y_less = int(floor(scaledY));
                int y_greater = y_less + 1;
                int z_less = int(floor(scaledZ));
                int z_greater = z_less + 1;
                float dx = scaledX - x_less;
                float dy = scaledY - y_less;
                float dz = scaledZ - z_less;
                float lll = getGrad(x, y, z, x_less, y_less, z_less);
                float llg = getGrad(x, y, z, x_less, y_less, z_greater);
                float lgl = getGrad(x, y, z, x_less, y_greater, z_less);
                float lgg = getGrad(x, y, z, x_less, y_greater, z_greater);
                float gll = getGrad(x, y, z, x_greater, y_less, z_less);
                float glg = getGrad(x, y, z, x_greater, y_less, z_greater);
                float ggl = getGrad(x, y, z, x_greater, y_greater, z_less);
                float ggg = getGrad(x, y, z, x_greater, y_greater, z_greater);
                return max(-0.5, min(0.5, interp(interp(interp(lll, gll, dx), interp(lgl, ggl, dx), dy), interp(interp(llg, glg, dx), interp(lgg, ggg, dx), dy), dz))); // man i hope this is right
            }
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float depth = min(50, LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv)) * length(i.viewVector));
                int steps = int(floor(depth / sampleResolution));
                float3 viewVecNormalized = i.viewVector / length(i.viewVector);
                float3 position = _WorldSpaceCameraPos;
                float totalMurk = 5;
                for(int i = 0; i < 10; i++) {
                    totalMurk += getValueAtCoords(position.x, position.y, position.z);
                    position += viewVecNormalized * 5;
                } 
                col.r = max(col - depth / 25,0);
                col.b = col.b + 0.3;
                col.g = col.g + 0.3;
                col = max(col - totalMurk/10, 0);
                //col.r = (totalMurk/10);
                //col.b = totalMurk/10;
                //col.g = totalMurk/10;
                return col;
            }
            ENDCG
        }
    }
}
