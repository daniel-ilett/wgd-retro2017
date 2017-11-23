using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PaletteSwap))]
public class Enemy : MonoBehaviour
{
	// Colour properties.
	[SerializeField]
	private Material rainbowBlendMaterial;
	[SerializeField]
	private Material hitFadeMaterial;
	private float hitFalloff = 0.0f;

	// Customisation properties.
	[SerializeField]
	private Color[] skinColors;
	[SerializeField]
	private Color[] hairColors;
	[SerializeField]
	private Color[] clothingColors;

	// Health and death.
	[SerializeField]
	private Ghost ghost;
	private int health = 1;

	//Combat.
	[SerializeField]
	private Bullet bullet;

	[SerializeField]
	private BulletRing ring;

	private BulletRing br;

	private float attackTime = 0.0f;
	private float rechargeTime = 2.5f;

	// Other properties.
	private float moveSpeed = 2.5f;
	private bool isSuper = false;

	private static int difficulty;

	private static int enemyCount;

	// Component references.
	private PaletteSwap swapper;
	private Animator animator;
	private new Rigidbody2D rigidbody;
	private new SpriteRenderer renderer;

	private void Start()
	{
		++enemyCount;

		swapper = GetComponent<PaletteSwap>();
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();

		swapper.SetColour(1, hairColors[Random.Range(0, hairColors.Length)]);
		swapper.SetColour(2, skinColors[Random.Range(0, skinColors.Length)]);
		swapper.SetColour(4, clothingColors[Random.Range(0, clothingColors.Length)]);
		swapper.SetColour(5, clothingColors[Random.Range(0, clothingColors.Length)]);

		hitFadeMaterial = new Material(hitFadeMaterial);
		rainbowBlendMaterial = new Material(rainbowBlendMaterial);

		// Add a red-fade shader to the materials list.
		Material[] materials = renderer.materials;
		Material[] moreMaterials = new Material[materials.Length + 1];

		for (int i = 0; i < materials.Length; ++i)
			moreMaterials[i] = materials[i];

		moreMaterials[materials.Length] = hitFadeMaterial;
		hitFadeMaterial.SetFloat("_BlendAmount", 0.0f);

		renderer.materials = moreMaterials;

		attackTime = Random.value * 5.0f;
	}

	public void Spawn(int difficulty)
	{
		Enemy.difficulty = difficulty;
		Invoke("CheckRainbow", Time.deltaTime * 2.0f);
	}

	private void CheckRainbow()
	{
		// Add a rainbow blend material to the materials list.
		if ((Random.value + Random.value) / 2.0f > 0.9f * (12.0f - difficulty / 3.0f) / 12.0f)
		{
			isSuper = true;
			health = 5;
			moveSpeed = 5.0f;

			renderer = GetComponent<SpriteRenderer>();

			Material[] materials = renderer.materials;
			Material[] moreMaterials = new Material[materials.Length + 1];

			materials = renderer.materials;
			moreMaterials = new Material[materials.Length + 1];

			for (int i = 0; i < materials.Length; ++i)
				moreMaterials[i] = materials[i];

			moreMaterials[materials.Length] = rainbowBlendMaterial;
			rainbowBlendMaterial.SetFloat("_BlendAmount", 0.5f);

			renderer.materials = moreMaterials;

			br = Instantiate(ring, transform);
			br.Spawn(bullet, BulletType.ENEMY, Random.Range(1, 7));
			br.gameObject.layer = 12;
		}
	}

	public void Update()
	{
		if (health > 0)
		{
			Vector2 diff = PlayerControl.player.transform.position - transform.position;

			float hor = diff.x;
			float ver = diff.y;

			float absHor = Mathf.Abs(hor);
			float absVer = Mathf.Abs(ver);

			if (absHor > absVer)
			{
				if (hor > 0.1f)
					animator.SetTrigger("Right");
				else if (hor < -0.1f)
					animator.SetTrigger("Left");
			}
			else
			{
				if (ver > 0.1f)
					animator.SetTrigger("Up");
				else if (ver < -0.1f)
					animator.SetTrigger("Down");
			}

			rigidbody.velocity = rigidbody.velocity / 2.0f + diff.normalized * moveSpeed / 2.0f;
			animator.SetBool("IsWalking?", rigidbody.velocity.magnitude > 1.0f);

			// Fire a bullet.
			if (Time.time > attackTime)
			{
				Bullet newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
				newBullet.Fire(diff.normalized, BulletType.ENEMY, 5.0f);

				attackTime = Time.time + rechargeTime * (1.0f + Random.value * 10.0f / difficulty);
			}
		}

		// Modify red amount.
		hitFalloff = Mathf.Lerp(hitFalloff, 0.0f, Time.deltaTime * 2.5f);

		hitFadeMaterial.SetFloat("_BlendAmount", hitFalloff);
	}

	public void GetHit(int damage, Vector2 direction)
	{
		rigidbody.AddForce(direction * 25.0f);
		health -= damage;
		hitFalloff = 1.0f;

		if (health <= 0)
			Die();
	}

	// Make the player fly off into the distance and spawn a ghost.
	private void Die()
	{
		Ghost gh = Instantiate(ghost, transform.position, ghost.transform.rotation);
		gh.SetSprite(renderer.sprite);

		Destroy(animator);
		Destroy(gameObject.GetComponent<Collider2D>());

		rigidbody.constraints = RigidbodyConstraints2D.None;
		rigidbody.AddTorque(Random.value > 0.5f ? 720.0f : -720.0f);

		GhostLabel.gh.AddGhost();
		ScoreLabel.sc.AddScore(isSuper ? 100 : 25);
		CameraController.cam.ScreenShake(0.15f, isSuper ? 3 : 2);

		--enemyCount;

		StartCoroutine(DestroyEnemy());
	}

	// After 1s, destroy the enemy object entirely.
	private IEnumerator DestroyEnemy()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		for (float i = 0; i < 1.0f; i += Time.deltaTime)
		{
			Color col = renderer.color;
			col.a = 1.0f - i;
			renderer.color = col;

			yield return wait;
		}
		Destroy(gameObject);
	}

	public static int GetEnemyCount()
	{
		return enemyCount;
	}

	public static int GetDifficulty()
	{
		return difficulty;
	}
}
