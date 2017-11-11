using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PaletteSwap : MonoBehaviour
{
	[SerializeField]
	private Color[] colours;

	[SerializeField]
	private Shader shader;

	private Material material;
	private new SpriteRenderer renderer;

	private void Start()
	{
		// The Palette Swap shader always takes the first material slot.
		renderer = GetComponent<SpriteRenderer>();
		material = new Material(shader);

		//material.SetTexture("_MainTex", renderer.sprite.texture);
		renderer.material = material;

		SwapColours();
	}

	public void SetColour(int index, Color col)
	{
		colours[index] = col;

		SwapColours();
	}

	public Color[] GetColours()
	{
		return colours;
	}

	public void SetColours(Color[] colours)
	{
		this.colours = colours;

		SwapColours();
	}

	public void SwapColours()
	{
		if (colours.Length != 8)
			return;

		Matrix4x4[] mats = new Matrix4x4[2];

		mats[0] = new Matrix4x4();
		mats[1] = new Matrix4x4();

		for (int i = 0; i < colours.Length; ++i)
			mats[i / 4].SetRow(i % 4, ColorToVec(colours[i]));
		
		material.SetMatrixArray("_ColorMatrix", mats);
	}

	private static Vector4 ColorToVec(Color col)
	{
		return new Vector4(col.r, col.g, col.b, col.a);
	}
}
