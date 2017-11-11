using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Material rainbowBlendMaterial;

	private float moveSpeed = 2.5f;

	private bool isSuper = false;

	private Animator animator;
	private new Rigidbody2D rigidbody;
	private new SpriteRenderer renderer;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();

		if((Random.value + Random.value) / 2.0f > 0.9f)
		{
			isSuper = true;

			// Add a rainbow blend material to the materials list.
			Material[] materials = renderer.materials;
			Material[] moreMaterials = new Material[materials.Length + 1];

			for (int i = 0; i < materials.Length; ++i)
				moreMaterials[i] = materials[i];

			moreMaterials[materials.Length] = rainbowBlendMaterial;
			rainbowBlendMaterial.SetFloat("_BlendAmount", 0.5f);

			renderer.materials = moreMaterials;
		}
	}

	public void Update()
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
}
