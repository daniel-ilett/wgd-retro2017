using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PaletteSwap : MonoBehaviour
{
	[SerializeField]
	private Color[] colours;

	private Material material;
	private new SpriteRenderer renderer;

	private void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
		material = new Material(Shader.Find("Hidden/PaletteSwapSprite"));

		material.SetTexture("_MainTex", renderer.sprite.texture);
		renderer.material = material;

		SwapColours(colours);
	}

	public void SwapColours(Color[] colours)
	{
		if (colours.Length != 4)
			return;

		Matrix4x4 mat = new Matrix4x4();

		for (int i = 0; i < colours.Length; ++i)
			mat.SetRow(i, colours[i]);

		material.SetMatrix("_ColorMatrix", mat);
	}

	private static Vector4 ColorToVec(Color col)
	{
		return new Vector4(col.r, col.g, col.b, col.a);
	}
}
