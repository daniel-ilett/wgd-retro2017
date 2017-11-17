using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PaletteSwap))]
public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Material rainbowBlendMaterial;

	[SerializeField]
	private Material hitFadeMaterial;
	private float hitFalloff = 0.0f;

	[SerializeField]
	private Ghost ghost;

	[SerializeField]
	private Color[] skinColors;
	[SerializeField]
	private Color[] hairColors;
	[SerializeField]
	private Color[] clothingColors;

	private bool isDead = false;

	private float moveSpeed = 2.5f;

	private bool isSuper = false;

	private int health = 2;

	private PaletteSwap swapper;
	private Animator animator;
	private new Rigidbody2D rigidbody;
	private new SpriteRenderer renderer;

	private void Start()
	{
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

		// Add a rainbow blend material to the materials list.
		//if ((Random.value + Random.value) / 2.0f > 0.9f)
		//{
			isSuper = true;
			health = 5;

			materials = renderer.materials;
			moreMaterials = new Material[materials.Length + 1];

			for (int i = 0; i < materials.Length; ++i)
				moreMaterials[i] = materials[i];

			moreMaterials[materials.Length] = rainbowBlendMaterial;
			rainbowBlendMaterial.SetFloat("_BlendAmount", 0.5f);

			renderer.materials = moreMaterials;
		//}
	}

	public void Update()
	{
		if(!isDead)
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

			rigidbody.velocity = diff.normalized * moveSpeed;
			animator.SetBool("IsWalking?", rigidbody.velocity.magnitude > 1.0f);
		}

		// Modify red amount.
		//hitFalloff = Mathf.Lerp(hitFalloff, 0.0f, Time.deltaTime * 2.5f);
		
		hitFadeMaterial.SetFloat("_BlendAmount", hitFalloff);
	}

	public void GetHit(int damage)
	{
		health -= damage;
		hitFalloff = 1.0f;

		if (health <= 0)
			Die();
	}

	private void Die()
	{
		isDead = true;

		Ghost gh = Instantiate(ghost, transform.position, ghost.transform.rotation);
		gh.SetSprite(renderer.sprite);

		Destroy(animator);
		Destroy(gameObject.GetComponent<Collider2D>());

		rigidbody.constraints = RigidbodyConstraints2D.None;
		rigidbody.AddTorque(720.0f);

		StartCoroutine(DestroyEnemy());
	}

	private IEnumerator DestroyEnemy()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		for(float i = 0; i < 1.0f; i += Time.deltaTime)
		{
			Color col = renderer.color;
			col.a = 1.0f - i;
			renderer.color = col;

			yield return wait;
		}

		Destroy(gameObject);
	}
}
