Shader "Custom/SwordShader" {
     Properties {
         _Color ("Color", Color) = (1,1,1,1)
         _Threshold ("Threshhold", Float) = 0.1
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
         _MetallicSmoothnessMap("Metallic Smoothness", 2D) = "white" {}
         _NormalMap("Normal Map", 2D) = "bump" {}
         _Emission("Emission (RGB)", 2D) = "black" {}
     }
     SubShader {
         Tags { "Queue"="Transparent" "RenderType"="Transparent" "ForceNoShadowCasting" = "True" }
         LOD 300
         Cull Off

         
         CGPROGRAM
         #pragma surface surf Standard
 
         sampler2D _MainTex;
         sampler2D _MetallicSmoothnessMap;
         sampler2D _NormalMap;
         sampler2D _Emission;
 
         struct Input {
             float2 uv_MainTex;
         };
 
         fixed4 _Color;
         half _Threshold;
 
         void surf (Input IN, inout SurfaceOutputStandard o) {

             half4 c = tex2D (_MainTex, IN.uv_MainTex);
             

             half4 output_col = c * _Color;
             

             half3 transparent_diff = c.xyz;
             half transparent_diff_squared = dot(transparent_diff,transparent_diff);
             

             if(transparent_diff_squared < _Threshold)
                 discard;

             half glow = transparent_diff_squared * _Threshold*10;
             o.Emission = output_col.rgb * (2-glow) * tex2D(_Emission, IN.uv_MainTex);
             o.Metallic = tex2D(_MetallicSmoothnessMap, IN.uv_MainTex).r;
             o.Smoothness = tex2D(_MetallicSmoothnessMap, IN.uv_MainTex).g;
             o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
             o.Albedo = output_col.rgb;
             o.Alpha = output_col.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }