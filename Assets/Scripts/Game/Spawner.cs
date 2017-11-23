using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	private Enemy prefab;

	[SerializeField]
	private PlayerControl player;

	private int difficulty = 1;
	private const int difficultyCap = 10;

	private void Start()
	{
		Spawn();
		StartCoroutine(IncreaseDifficulty());
	}

	private void Spawn()
	{
		float dist = (transform.position - player.transform.position).magnitude;

		if (Enemy.GetEnemyCount() < 250 && dist > 15.0f)
		{
			Enemy en = Instantiate(prefab, transform.position, Quaternion.identity);
			en.Spawn(difficulty);
		}	

		Invoke("Spawn", 12.0f - difficulty);
	}

	private IEnumerator IncreaseDifficulty()
	{
		WaitForSeconds wait = new WaitForSeconds(15.0f);

		for(int i = 0; i < difficultyCap; ++i)
		{
			++difficulty;
			yield return wait;
		}
	}
}
