using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	private Enemy prefab;

	[SerializeField]
	private PlayerControl player;

	private void Start()
	{
		InvokeRepeating("Spawn", 1.0f, 1.0f);
	}

	private void Spawn()
	{
		float dist = (transform.position - player.transform.position).magnitude;

		if (dist > 15.0f && dist < 30.0f)
		{
			Instantiate(prefab, transform.position, Quaternion.identity);
		}
	}
}
