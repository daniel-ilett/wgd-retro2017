using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRing : MonoBehaviour
{
	private float speed = 180.0f;

	private List<Bullet> bullets;

	public void Spawn(Bullet prefab, BulletType type, int count)
	{
		bullets = new List<Bullet>();

		for(int i = 0; i < count; ++i)
		{
			float angle = Mathf.PI * 2 / count * i;
			Bullet newBullet = Instantiate(prefab, transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f), Quaternion.identity, transform);
			bullets.Add(newBullet);
			
			newBullet.SetType(type);
		}

		speed /= count;
	}

	private void Update()
	{
		transform.Rotate(0.0f, 0.0f, Time.deltaTime * speed);
	}
}
