using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DestructibleScenery : MonoBehaviour
{
	private new SpriteRenderer renderer;
	private new Rigidbody2D rigidbody;

	private void Start()
	{
		transform.tag = "Destructible";

		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
	}

	public void GetHit(Vector2 direction)
	{
		Destroy(gameObject.GetComponent<Collider2D>());
		rigidbody.isKinematic = false;

		rigidbody.AddForce(direction * 250.0f);
		rigidbody.AddTorque(Random.value > 0.5f ? 720.0f : -720.0f);

		StartCoroutine(DestroyScenery());
	}

	// After 1s, destroy the scenery object entirely.
	private IEnumerator DestroyScenery()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		ScoreLabel.sc.AddScore(5);

		for (float i = 0; i < 1.0f; i += Time.deltaTime)
		{
			Color col = renderer.color;
			col.a = 1.0f - i;
			renderer.color = col;

			yield return wait;
		}

		Destroy(gameObject);
	}
}
