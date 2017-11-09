using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
	private float moveSpeed = 2.5f;

	private new Animator animator;
	private new Rigidbody2D rigidbody;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
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
