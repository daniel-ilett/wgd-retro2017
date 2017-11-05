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
		//material = new Material(Shader.Find("Hidden/PaletteSwapSprite"));
		material = new Material(Shader.Find("Hidden/Ghost"));

		material.SetTexture("_MainTex", renderer.sprite.texture);
		renderer.material = material;

		//SwapColours(colours);
	}

	private void Update()
	{
		transform.position += new Vector3(0.0f, Time.deltaTime, 0.0f);
	}

	public void SwapColours(Color[] colours)
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
