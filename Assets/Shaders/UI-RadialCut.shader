Shader "Custom/UI-RadialCut"
{
    Properties
    {
        _MainTex   ("Sprite Texture", 2D) = "white" {}  
        _Color     ("Tint Color", Color) = (1,1,1,1)
        _CutValue  ("Cut Value", Range(0,1)) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanvasRenderer"="True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4   _MainTex_ST;
            fixed4   _Color;
            float    _CutValue;         // <-- здесь объ€вл€ем ваш параметр

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv    : TEXCOORD0;
            };

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.pos   = UnityObjectToClipPos(IN.vertex);
                OUT.uv    = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // считаем угол точки UV, нормируем в [0,1]
                float2 center = float2(0.5, 0.5);
                float2 dir    = IN.uv - center;
                float  angle  = atan2(dir.y, dir.x) / (2*UNITY_PI) + 0.5;

                // если угол больше CutValue Ч отбрасываем пиксель
                if (angle > _CutValue)
                    discard;

                // иначе рисуем текстуру как обычно
                fixed4 col = tex2D(_MainTex, IN.uv) * IN.color;
                return col;
            }
            ENDCG
        }
    }
}
