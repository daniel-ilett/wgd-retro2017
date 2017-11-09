using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WaveBackground : MonoBehaviour
{
	private Material mat;

	private float hue = 0.0f;
	private float sat = 0.5f;
	private float val = 0.75f;

	private void Start()
	{
		mat = GetComponent<Renderer>().material;

		if (mat == null)
			Debug.LogError("No material is attached to this Renderer!");
	}

	private void Update()
	{
		hue += Time.deltaTime * 0.025f;
		hue = hue % 1.0f;
		HSBColor col = new HSBColor(hue, sat, val, 1.0f);

		mat.SetColor("_ColorA", col.ToColor());
	}
}
