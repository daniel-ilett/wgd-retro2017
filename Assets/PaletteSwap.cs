using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSwap : MonoBehaviour
{
	[SerializeField]
	private Color col0, col1, col2, col3;

	private Material mat;

	private void OnEnable()
	{
		if (mat == null)
			mat = new Material(Shader.Find("Hidden/PaletteSwap"));
	}

	private void OnDisable()
	{
		if (mat != null)
			DestroyImmediate(mat);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Matrix4x4 colMatrix = new Matrix4x4();

		colMatrix.SetRow(0, ColorToVec4(col0));
		colMatrix.SetRow(1, ColorToVec4(col1));
		colMatrix.SetRow(2, ColorToVec4(col2));
		colMatrix.SetRow(3, ColorToVec4(col3));

		mat.SetMatrix("_ColMatrix", colMatrix);
		Graphics.Blit(src, dst, mat);
	}

	static Vector4 ColorToVec4(Color col)
	{
		return new Vector4(col.r, col.g, col.b, col.a);
	}
}
