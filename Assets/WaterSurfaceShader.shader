Shader "Custom/WaterSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" } 
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 5.0

        #pragma enable_d3d11_debug_symbols

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling 
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        #ifdef SHADER_API_D3D11
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
                return (x - float(x_grid)) * grid.x + (y - float(y_grid)) * grid.y + (z - float(z_grid)) * grid.z;
            }
            float getValueAtCoords(float x, float y, float z)
            {
                float scaledX = x * (xGridMax);
                float scaledY = y * (yGridMax);
                float scaledZ = z * (zGridMax);
                int x_less = int(floor(scaledX));
                int x_greater = x_less + 1;
                int y_less = int(floor(scaledY));
                int y_greater = y_less + 1;
                int z_less = int(floor(scaledZ));
                int z_greater = z_less + 1;
                float dx = scaledX - float(x_less);
                float dy = scaledY - float(y_less);
                float dz = scaledZ - float(z_less);
                float lll = getGrad(scaledX, scaledY, scaledZ, x_less, y_less, z_less);
                float llg = getGrad(scaledX, scaledY, scaledZ, x_less, y_less, z_greater);
                float lgl = getGrad(scaledX, scaledY, scaledZ, x_less, y_greater, z_less);
                float lgg = getGrad(scaledX, scaledY, scaledZ, x_less, y_greater, z_greater);
                float gll = getGrad(scaledX, scaledY, scaledZ, x_greater, y_less, z_less);
                float glg = getGrad(scaledX, scaledY, scaledZ, x_greater, y_less, z_greater);
                float ggl = getGrad(scaledX, scaledY, scaledZ, x_greater, y_greater, z_less);
                float ggg = getGrad(scaledX, scaledY, scaledZ, x_greater, y_greater, z_greater);
                float ll = interp(lll, llg, dz);
                float lg = interp(lgl, lgg, dz);
                float gl = interp(gll, glg, dz);
                float gg = interp(ggl, ggg, dz);
                float l = interp(ll, lg, dy);
                float g = interp(gl, gg, dy);
                return interp(l, g, dx); // man i hope this is right
            }
            float transformTime(float time) 
            {
                uint cycle = int(floor(time / zGridMax));
                if(cycle % 2 == 0) {
                    return time % zGridMax;
                } else {
                    return zGridMax - time % zGridMax;
                }
            }
            void surf (Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                float i = 0.5 + getValueAtCoords(IN.uv_MainTex.x, IN.uv_MainTex.y, transformTime(_Time[0]))/2; // 1/20 timescale works well
                o.Albedo.r = 1 - i + c.r * i;
                o.Albedo.b = 1 - i + c.b * i;
                o.Albedo.g = 1 - i + c.g * i;
                //o.Albedo.r = getValueAtCoords(IN.uv_MainTex.x, IN.uv_MainTex.y, transformTime(_Time[0]));
                //o.Albedo.g = getValueAtCoords(IN.uv_MainTex.x, IN.uv_MainTex.y, transformTime(_Time[0]));
                //o.Albedo.b = getValueAtCoords(IN.uv_MainTex.x, IN.uv_MainTex.y, transformTime(_Time[0]));
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                //o.Alpha = 1;
                o.Alpha = 0.75;
                //o.Alpha = c.a;
            }
        #endif

        #ifndef SHADER_API_D3D11
            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
            }
        #endif
        
        ENDCG
    }
    FallBack "Diffuse"
}
