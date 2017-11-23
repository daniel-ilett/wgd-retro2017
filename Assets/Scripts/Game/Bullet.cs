using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
	private int reflectionCount = 3;
	private BulletType type;

	private float startAngle;

	private Transform target;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	public void SetType(BulletType type)
	{
		gameObject.layer = (type == BulletType.PLAYER) ? 11 : 12;
	}

	public void Fire(Vector3 direction, BulletType type)
	{
		this.type = type;
		rigidbody.isKinematic = false;
		rigidbody.AddForce(direction * 10.0f, ForceMode2D.Impulse);

		SetType(type);
		Invoke("Die", 5.0f);
	}

	private void OnCollisionEnter2D(Collision2D hit)
	{
		Debug.Log(hit.collider.gameObject, hit.collider.gameObject);
		Vector2 diff = hit.collider.transform.position - hit.otherCollider.transform.position;

		switch (hit.collider.gameObject.tag)
		{
			case "Player":
				hit.collider.GetComponent<PlayerControl>().GetHit(1);
				Die();
				break;
			case "Enemy":
				hit.collider.GetComponent<Enemy>().GetHit(1, diff);
				Die();
				break;
			case "Destructible":
				DestructibleScenery sc = hit.collider.GetComponent<DestructibleScenery>();
				sc.GetHit(diff);
				Die();
				break;
			case "Bullet":
				Destroy(hit.collider.gameObject);
				Die();
				break;
			default:
				if (reflectionCount-- <= 0)
					Die();
				break;
		}
	}

	private void Die()
	{
		CameraController.cam.ScreenShake(0.1f, 1);
		Destroy(gameObject);
	}
}

public enum BulletType
{
	PLAYER, ENEMY, NEUTRAL
}
