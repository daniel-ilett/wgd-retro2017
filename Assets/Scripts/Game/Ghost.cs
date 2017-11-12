using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	[SerializeField]
	private Shader shader;

	private float ghostTime = 0.0f;

	private Material material;
	private new SpriteRenderer renderer;

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		material = new Material(shader);
		renderer.material = material;

		Invoke("DestroyGhost", 1.51f);
	}

	private void Update()
	{
		transform.position += new Vector3(0.0f, Time.deltaTime * 2.5f, 0.0f);
		float ghostScale = 1.0f + ghostTime;
		transform.localScale = new Vector3(ghostScale, ghostScale, ghostScale);

		material.SetFloat("_GhostTime", ghostTime);
		material.SetFloat("_GhostFade", Mathf.Clamp(1.5f - ghostTime, 0.0f, 1.0f));
		ghostTime += Time.deltaTime;
	}

	private void DestroyGhost()
	{
		Destroy(gameObject);
	}

	public void SetSprite(Sprite sp)
	{
		renderer.sprite = sp;
	}
}
