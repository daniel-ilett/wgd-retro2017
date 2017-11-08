using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	private float ghostTime = 0.0f;

	private Material material;
	private new SpriteRenderer renderer;

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		material = new Material(Shader.Find("Hidden/Ghost"));
		renderer.material = material;
	}

	private void Update()
	{
		transform.position += new Vector3(0.0f, Time.deltaTime, 0.0f);

		material.SetFloat("_GhostTime", ghostTime);
		material.SetFloat("_GhostFade", Mathf.Clamp(3.0f - ghostTime, 0.0f, 1.0f));
		ghostTime += Time.deltaTime;
	}

	public void SetSprite(Sprite sp)
	{
		renderer.sprite = sp;
		material.SetTexture("_MainTex", sp.texture);
	}
}
