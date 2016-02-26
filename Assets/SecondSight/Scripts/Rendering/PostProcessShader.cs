using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class PostProcessShader : ImageEffectBase {

	public Color color;

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		material.SetColor ("_myColor", color);
		Graphics.Blit (source, destination, material);
	}

	void Update()
	{
		//TODO - Update shader parameters here!
		//e.g.
		//material.SetVector ("MyVectorName", Vector4.one);
	}
}