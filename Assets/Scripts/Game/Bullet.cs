using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
	private int reflectionCount = 3;
	private BulletType type;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	public void Fire(Vector3 direction, BulletType type)
	{
		this.type = type;
		rigidbody.AddForce(direction * 5.0f, ForceMode2D.Impulse);
		
		gameObject.layer = (type == BulletType.PLAYER) ? 11 : 12;
	}

	private void OnCollisionEnter2D(Collision2D hit)
	{
		if (hit.collider.gameObject.tag == "Player")
		{
			hit.collider.GetComponent<PlayerControl>().GetHit(1);
			Destroy(gameObject);
		}
		else if (hit.collider.gameObject.tag == "Enemy")
		{
			hit.collider.GetComponent<Enemy>().GetHit(1);
			Destroy(gameObject);
		}
		else if (reflectionCount-- <= 0)
		{
			Destroy(gameObject);
		}
	}
}

public enum BulletType
{
	PLAYER, ENEMY, NEUTRAL
}
