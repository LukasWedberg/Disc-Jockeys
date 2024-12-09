Shader "Unlit/HighwayBender"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BendingStartPos("Start Pos", float) = 0
        _BendingEndPos("End Pos", float) = 0
        _BendingAmount("Bending Amount", float) = 1
        _BendingContrast("Bending Contrast", float) = 1
        _PivotPoint("Pivot Point", Vector) = (0,0,0,0)

        _rotX ("x rotation", Range(-2,2)) = 0
        _rotY ("y rotation", Range(-2,2)) = 0
        _rotZ ("z rotation", Range(-2,2)) = 0

        


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

            #define TAU 6.28318530718


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
                float bendDist: TEXCOORD1;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _BendingStartPos;
            float _BendingEndPos;
            float _BendingAmount;
            float _BendingContrast;

            float4 _PivotPoint;

            float _rotX;
            float _rotY;
            float _rotZ;




            float4x4 rotation_matrix (float3 axis, float angle) {
                axis = normalize(axis);
                float s = sin(angle);
                float c = cos(angle);
                float oc = 1.0 - c;
                
                return float4x4(
                    oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,  0.0,
                    oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,  0.0,
                    oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c,           0.0,
                    0.0,                                0.0,                                0.0,                                1.0);
            }



            v2f vert (appdata v)
            {
                v2f o;
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float3 worldPos = mul( unity_ObjectToWorld, v.vertex);

                //float pivotPointToWorldPos = 


                float distanceScaling = pow( smoothstep(  _BendingStartPos, _BendingEndPos, worldPos.y), _BendingContrast ) *.5 ;

                o.bendDist = distanceScaling;

                // set vertex color
                //o.color = v.color;
                
                float4x4 x = rotation_matrix(float3(1, 0 ,0), _rotX  * TAU * distanceScaling );
                float4x4 y = rotation_matrix(float3 (0, 1 ,0), _rotY * TAU * distanceScaling );
                float4x4 z = rotation_matrix(float3(0, 0 ,1), _rotZ * TAU * distanceScaling );

                float4x4 rotation = mul( mul(x , y), z );

                v.vertex = mul( rotation, v.vertex );


                o.vertex = UnityObjectToClipPos(v.vertex);
                

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                
                
                //col.rgb = i.bendDist;
                
                
                
                return col;
            }
            ENDCG
        }
    }
}
