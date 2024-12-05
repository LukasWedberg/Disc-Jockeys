Shader "Unlit/HueShifter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorToShift("Hue to shift" , Color) = (1,0,1,0)
        _ColorToShiftTo("New hue" , Color) = (1,0,0,0)
        _ColorShiftTolerance("Old Color Tolerance" , float) = .2
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorToShift;
            float4 _ColorToShiftTo;
            float _ColorShiftTolerance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            float3 ConvertToHSV(float3 In ){
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                float D = Q.x - min(Q.w, Q.y);
                float E = 1e-10;
                float3 HSV = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);
            
                return HSV;
            }


            //This function is courtesy of unity shader graphs. Yay!
            //I also modified it quite a bit. 
            //Normally it shifts the hue by a degrees parameter, but we need it to go to a specific hue.
            float3 Hue_Targeted_Shift(float3 In)
            {
                //First we need to convert the target color to a hue. 
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 P = lerp(float4(_ColorToShiftTo.bg, K.wz), float4(_ColorToShiftTo.gb, K.xy), step(_ColorToShiftTo.b, _ColorToShiftTo.g));
                float4 Q = lerp(float4(P.xyw, _ColorToShiftTo.r), float4(_ColorToShiftTo.r, P.yzx), step(P.x, _ColorToShiftTo.r));
                float D = Q.x - min(Q.w, Q.y);
                float E = 1e-10;
                float3 targetHSV = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);


                //Then we need to convert the base color to a hue. 
                 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                P = lerp(float4(_ColorToShift.bg, K.wz), float4(_ColorToShift.gb, K.xy), step(_ColorToShift.b, _ColorToShift.g));
                Q = lerp(float4(P.xyw, _ColorToShift.r), float4(_ColorToShift.r, P.yzx), step(P.x, _ColorToShift.r));
                D = Q.x - min(Q.w, Q.y);
                E = 1e-10;
                float3 baseHSV = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);


                //We subratct the base from the target to get the offset.
                float offset = frac(targetHSV.x - baseHSV.x);



                //Then we actually offset it!
                K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                D = Q.x - min(Q.w, Q.y);
                E = 1e-10;
                float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);

                float hue = hsv.x + offset.x;
                hsv.x = (hue < 0)
                        ? hue + 1
                        : (hue > 1)
                            ? hue - 1
                            : hue;

                float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
                return hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                //We also want to mask out any colors that aren't the ones we want to shift. 
                //We can do this by getting the distance. Haha!

                //float3 targetedColorMask = 1-step( _ColorShiftTolerance ,distance(ConvertToHSV(col.rgb).x, ConvertToHSV(_ColorToShift).x) );
                //float3 targetedColorMask = distance(ConvertToHSV(col.rgb).x, ConvertToHSV(_ColorToShift).x);
                float3 targetedColorMask = smoothstep(0, ConvertToHSV(_ColorToShift.x), ConvertToHSV(col.rgb).x );

                
                //Now we're going to be pretty clever about shifting the hue of certain parts of the texture!
                

                col.rgb = targetedColorMask;

                //col.rgb = Hue_Targeted_Shift(col.rgb)


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
