// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit alpha-blended shader.
 // - no lighting
 // - no lightmap support
 // - no per-material color
 
 Shader "Unlit/Transparent2" {
 Properties {
     _DayTex ("Base (RGB) Trans (A)", 2D) = "white" {}
     _NightTex ("Base (RGB) Trans (A)", 2D) = "white" {}
     _Color ("Color", Color) = (1,1,1,1)
     _DayNightTime ("Time", Range(0,1)) = 0
 }
 
 SubShader {
     Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     LOD 100
     
     ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha 
     
     Pass {  
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma multi_compile_fog
             
             #include "UnityCG.cginc"
 
             struct appdata_t {
                 float4 vertex : POSITION;
                 float2 texcoord : TEXCOORD0;
             };
 
             struct v2f {
                 float4 vertex : SV_POSITION;
                 half2 texcoord : TEXCOORD0;
                 UNITY_FOG_COORDS(1)
             };
 
             sampler2D _DayTex;
             sampler2D _NightTex;
             float4 _DayTex_ST;
             float4 _NightTex_ST;
             float _DayNightTime;
             fixed4 _Color;
             
             v2f vert (appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.texcoord = TRANSFORM_TEX(v.texcoord, _DayTex);
                 UNITY_TRANSFER_FOG(o,o.vertex);
                 return o;
             }
             
             fixed4 frag (v2f i) : SV_Target
             {
                 fixed4 dayCol = tex2D(_DayTex, i.texcoord);
                 fixed4 nightCol = tex2D(_NightTex, i.texcoord);
                 fixed4 col = (_DayNightTime * dayCol + (1-_DayNightTime) * nightCol) * _Color;

                 UNITY_APPLY_FOG(i.fogCoord, col);
                 return col;
             }
         ENDCG
     }
 }
 
 }