using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CRTScreen : MonoBehaviour
{
	[SerializeField]
	private Shader shader;

	private Material mat;

	private void Start()
	{
		mat = new Material(shader);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		mat.SetFloat("_Brightness", 27.0f);
		mat.SetFloat("_Contrast", 2.1f);
		Graphics.Blit(src, dst, mat);
	}
}
