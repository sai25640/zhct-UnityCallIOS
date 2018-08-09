Shader "WC/双纹理混合" {
	Properties {
		_MainTex ("主纹理 (RGB)", 2D) = "white" {}
		_BlendTex ("混合纹理 (RGB)", 2D) = "white"{}
	}
	SubShader {
		
		//通道1
		Pass{
			setTexture[_MainTex] {combine texture}				//主纹理
			setTexture[_BlendTex] {combine texture * previous}	//混合纹理 texture为_MainTex的颜色,previous为上一次SetTexture的结果 
		}
		//通道 N..
		//Pass
		//{
		//	ooooooxxxxxx
		//}
	} 
}
