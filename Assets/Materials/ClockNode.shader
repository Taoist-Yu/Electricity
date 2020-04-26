Shader "Hidden/ClockNode"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Rate ("Rate", Range(0.0,1)) = 1
		_Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
		//开启alpha通道混合
		Blend SrcAlpha OneMinusSrcAlpha

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
            };
			
			//定义Uniform变量
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _Rate;
			uniform float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;

				v.vertex.y *= _Rate;
				v.uv.y *= _Rate;

                o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col * _Color;
            }
            ENDCG
        }
    }
}
