Shader "Custom/BlackHole" 
{
	Properties
	{
		// マテリアル色
		_MainColor("MainColor",Color) = (1,1,1,1)
		// テクスチャー色
		_MainTex("MainTex",2D) = "white"{}
		// 法線テクスチャー
		_BumpTex("BumpTex",2D) = "bump"{}
		// 法線スケール
		_BumpScale("BumpScale",Float) = 1.0
		// スペキュラー
		_Specular("Specular",Color) = (1,1,1,1)
		// 光沢
		_Gloss("Gloss",Range(8.0,255)) = 20
		// 設定されたブラックホール中心点からの影響範囲
		_Range("Range",Float) = 5
		// 影響値
		_HoleAmount("HoleAmount",Range(1.0,2.0)) = 1.5
	}
		SubShader
		{
			Pass
			{
				// 平行光源
				Tags
				{
					"LightMode" = "ForwardBase"
				}

				CGPROGRAM
				// 頂点シェーダー
				#pragma vertex vert	
				// ピクセルシェーダー
				#pragma fragment frag
				// サーフェスシェーダー ライティングモデル
				#include "Lighting.cginc"
				#include "UnityCG.cginc"

				// マテリアル色
				fixed4 _MainColor;
				// テクスチャー(色)
				sampler2D _MainTex;
				
				float4 _MainTex_ST;
				// 法線テクスチャー
				sampler2D _BumpTex;
				float4 _BumpTex_ST;
				// 法線スケール
				float _BumpScale;
				// スペキュラー
				fixed4 _Specular;
				// 光沢
				float _Gloss;
				// 設定されたブラックホール中心点からの影響範囲
				float _Range;
				// 影響値
				half _HoleAmount;
				// 設定されたブラックホール中心点
				float3 _BlackHolePos;

				struct a2v 
				{
					float4 vertex:POSITION;
					float3 normal:NORMAL;
					float4 tangent:TANGENT;
					float4 texcoord:TEXCOORD0;
				};

				struct v2f 
				{
					float4 pos:SV_POSITION;
					float4 uv:TEXCOORD0;
					float3 lightDir:TEXCOORD1;
					float3 viewDir:TEXCOORD2;
				};

				v2f vert(a2v v) 
				{
					// 出力内容
					v2f o;
					// このモデルのワールド座標
					float4 oriWorldPos = mul(unity_ObjectToWorld,v.vertex);
					// 変化後のワールド座標、始めは元の座標
					float4 worldPos = oriWorldPos;
					// ブラックホールとの距離
					float dis = distance(oriWorldPos,_BlackHolePos);

					// lerp(x, y, s) yをxにミックスする　clamp(x, min, max) xをminとmaxの範囲に計算
					// ワールド座標 = lerp(変化後のワールド座標、　clamp(ブラックホール中心点の影響範囲 - オブジェクトとブラックホール中心点との距離  / 範囲))
					worldPos.xyz = lerp(oriWorldPos,_BlackHolePos,clamp((_Range - dis)*_HoleAmount / _Range,0,1));
					// mul(ビュー行列×射影行列 , 計算されらワールド座標)
					o.pos = mul(UNITY_MATRIX_VP,worldPos);
					// uvの計算
					o.uv.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
					o.uv.zw = TRANSFORM_TEX(v.texcoord,_BumpTex);
					// 法線計算
					float3 biNormal = cross(normalize(v.normal),normalize(v.tangent.xyz))*v.tangent.w;
					// 回転計算
					float3x3 rotation = float3x3(v.tangent.xyz,biNormal,v.normal);
					// ライト計算
					o.lightDir = mul(rotation,ObjSpaceLightDir(v.vertex).xyz);
					o.viewDir = mul(rotation,ObjSpaceViewDir(v.vertex).xyz);

					return o;
				}

				fixed4 frag(v2f i) :SV_Target
				{
					// ライト正規化
					fixed3 tangentLightDir = normalize(i.lightDir);
					// ビュー正規化
					fixed3 tangentViewDir = normalize(i.viewDir);
					// テクスチャーのuvを正規化
					fixed3 tangentNormal = UnpackNormal(tex2D(_BumpTex,i.uv.zw));
					// 法線テクスチャースケール計算
					tangentNormal.xy *= _BumpScale;
					tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy,tangentNormal.xy)));
					// オブジェクトの自体色
					fixed3 albedo = tex2D(_MainTex,i.uv.xy)*_MainColor.rgb;
					// 環境光
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;
					// 拡散光
					fixed3 diffuse = _LightColor0.rgb*albedo*max(0,dot(tangentNormal,tangentLightDir));
					// 半分の正規化されたライト+ビュー
					fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);
					// 鏡面光
					fixed3 specular = _LightColor0.rgb*_Specular.rgb*pow(saturate(dot(halfDir,tangentNormal)),_Gloss);

					return fixed4(ambient + diffuse + specular,1.0);
				}
				ENDCG
			}
		}
}