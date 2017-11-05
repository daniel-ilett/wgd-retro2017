using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerControl : MonoBehaviour
{
	private Animator animator;
	private Rigidbody2D rigidbody;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

		float absHor = Mathf.Abs(hor);
		float absVer = Mathf.Abs(ver);

		if(absHor > absVer)
		{
			if (hor > 0.1f)
				animator.SetTrigger("Right");
			else if(hor < -0.1f)
				animator.SetTrigger("Left");
		}
		else
		{
			if (ver > 0.1f)
				animator.SetTrigger("Up");
			else if (ver < -0.1f)
				animator.SetTrigger("Down");
		}

		rigidbody.velocity = new Vector3(hor, ver, 0.0f) * 5.0f;

		animator.SetBool("IsWalking?", rigidbody.velocity.magnitude > 1.0f);
	}
}
