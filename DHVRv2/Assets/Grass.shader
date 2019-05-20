Shader "Custom/Grass"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _YOffset("U Offset", float) = 0.5
        _Rigidness("Rigidness", Range(1, 50)) = 25
        _Speed("Speed", Range(20,50)) = 25
        _SwayMax("Sway Max", Range(0, 0.1)) = 0.005
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "DisableBatching" = "True"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _YOffset;
        float _Rigidness;
        float _Speed;
        float _SwayMax;

        void vert(inout appdata_full v) {
            float3 wpos = mul(unity_ObjectToWorld, v.vertex);
            float x = sin(wpos.x / _Rigidness + (_Time.x * _Speed)) * (v.vertex.y - _YOffset) * 5;
            float y = sin(wpos.z / _Rigidness + (_Time.x * _Speed)) * (v.vertex.y - _YOffset) * 5;

            v.vertex.x += step(0, v.vertex.y - _YOffset) * x * _SwayMax;
            v.vertex.z += step(0, v.vertex.y - _YOffset) * y * _SwayMax;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
