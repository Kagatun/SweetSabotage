Shader "Custom/EnergyBarrier"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 1
        _DissolveHeight ("Dissolve Height", Range(0, 1)) = 1
        _DissolveSoftness ("Dissolve Softness", Range(0, 0.5)) = 0.1
        _Brightness ("Brightness", Range(0, 2)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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
                float3 localPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _EmissionColor;
            float _EmissionIntensity;
            float _DissolveHeight;
            float _DissolveSoftness;
            float _Brightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.localPos = v.vertex.xyz; // ���������� ��������� ����������
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ����������� ������ ������� �� 0 �� 1
                float height = (i.localPos.y + 0.5); // ��������� ���������� �� -0.5 �� 0.5, �������� � 0-1

                // ����������� ������, ����� ����������� ��� ����� �����
                float invertedHeight = 1 - height;

                // ��������� �����������
                float dissolve = smoothstep(_DissolveHeight - _DissolveSoftness, _DissolveHeight + _DissolveSoftness, invertedHeight);

                // �������� ���� ��������
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // ��������� ������� � ������������ � ����������� �� ������
                texColor.rgb *= _Brightness * dissolve; // ��������� �������
                texColor.a *= dissolve; // ��������� ������������

                // ��������� ���� � �������������
                fixed4 finalColor = texColor * _Color;

                // ��������� �������� (�������)
                fixed4 emission = _EmissionColor * _EmissionIntensity * dissolve;
                finalColor.rgb += emission.rgb;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}