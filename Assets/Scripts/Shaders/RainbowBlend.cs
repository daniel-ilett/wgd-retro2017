using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RainbowBlend : MonoBehaviour
{
	[SerializeField]
	private Shader shader;

	private Material mat;
	private new Renderer renderer;

	private void Awake()
	{
		mat = new Material(shader);
		renderer = GetComponent<Renderer>();

		mat.SetFloat("_BlendAmount", 0.25f);

		Material[] materials = renderer.materials;

		Material[] moreMaterials = new Material[materials.Length + 1];

		for(int i = 0; i < materials.Length; ++i)
			moreMaterials[i] = materials[i];

		moreMaterials[materials.Length] = mat;

		renderer.materials = moreMaterials;
	}
}
