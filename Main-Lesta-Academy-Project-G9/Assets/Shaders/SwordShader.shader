Shader "Custom/transparent_col" {
     Properties {
         _Color ("Color", Color) = (1,1,1,1)
         _Threshold ("Threshhold", Float) = 0.1
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
     }
     SubShader {
         Tags { "Queue"="Transparent" "RenderType"="Transparent" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert alpha
 
         sampler2D _MainTex;
 
         struct Input {
             float2 uv_MainTex;
         };
 
         fixed4 _Color;
         half _Threshold;
 
         void surf (Input IN, inout SurfaceOutput o) {

             half4 c = tex2D (_MainTex, IN.uv_MainTex);
             

             half4 output_col = c * _Color;
             

             half3 transparent_diff = c.xyz;
             half transparent_diff_squared = dot(transparent_diff,transparent_diff);
             

             if(transparent_diff_squared < _Threshold)
                 discard;

             half glow = transparent_diff_squared * _Threshold*10;
             o.Emission = output_col.rgb * glow;

             o.Albedo = output_col.rgb;
             o.Alpha = output_col.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }