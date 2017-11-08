using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerControl : MonoBehaviour
{
	[SerializeField]
	private Transform cameraPos;

	[SerializeField]
	private Ghost ghost;

	private Vector3 targetPos = Vector3.zero;

	private bool isDead = false;

	private Animator animator;
	private new Rigidbody2D rigidbody;
	private new SpriteRenderer renderer;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		cameraPos.localPosition = Vector3.Lerp(cameraPos.localPosition, 
			targetPos, Time.deltaTime * 5.0f);

		if(!isDead)
		{
			// Modify player velocity.
			float hor = Input.GetAxis("Horizontal");
			float ver = Input.GetAxis("Vertical");

			rigidbody.velocity = new Vector3(hor, ver, 0.0f) * 5.0f;

			animator.SetBool("IsWalking?", rigidbody.velocity.magnitude > 1.0f);

			// Modify player direction.
			Vector3 mousePos = Input.mousePosition;

			Vector3 playerPos = 
				CameraController.cam.GetCamera().WorldToScreenPoint(transform.position);

			Vector2 diff = mousePos - playerPos;

			hor = diff.x;
			ver = diff.y;

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

			// Modify camera position.
			targetPos = diff.normalized * 2.5f;
		}
	}

	private void Die()
	{
		isDead = true;

		Ghost ghostClone = Instantiate(ghost, transform.position, Quaternion.identity);
		ghostClone.SetSprite(renderer.sprite);

		cameraPos.SetParent(null);
		Destroy(gameObject);
	}
}
