﻿using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
	[SerializeField]
	private PaletteSwap swapper;

	[SerializeField]
	private Transform cameraPos;

	[SerializeField]
	private Ghost ghost;

	[SerializeField]
	private Shader redFadeShader;

	private Material redMat;
	private float redBlend = 0.0f;

	private Vector3 targetPos = Vector3.zero;

	private int health = 100;
	private const int maxHealth = 100;

	private bool isDead = false;

	private Animator animator;
	private new Rigidbody2D rigidbody;
	private new SpriteRenderer renderer;

	public static PlayerControl player;

	private void Start()
	{
		player = this;

		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();

		/*
		// Add the red-fade material to the materials list.
		redMat = new Material(redFadeShader);
		Material[] materials = renderer.materials;
		Material[] moreMaterials = new Material[materials.Length + 1];

		for (int i = 0; i < materials.Length; ++i)
			moreMaterials[i] = materials[i];

		moreMaterials[materials.Length] = redMat;

		renderer.materials = moreMaterials;

		for (int i = 0; i < moreMaterials.Length; ++i)
			Debug.Log(moreMaterials[i]);
		*/

		// Load colours.
		Load();
	}

	private void Load()
	{
		SColor[] sColours;
		if (File.Exists(Application.persistentDataPath + "/charData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/charData.dat", FileMode.Open);
			sColours = (SColor[])bf.Deserialize(file);
			file.Close();

			Color[] colours = new Color[sColours.Length];

			for (int i = 0; i < sColours.Length; ++i)
				colours[i] = new Color(sColours[i].r, sColours[i].g, sColours[i].b, sColours[i].a);

			swapper.SetColours(colours);
		}
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

			/*
			// Modify red amount.
			redBlend = Mathf.Lerp(redBlend, 0.0f, Time.deltaTime * 2.5f);

			//redMat.SetTexture("_MainTex", renderer.material.mainTexture);
			redMat.SetFloat("_BlendAmount", redBlend);
			*/
		}

		if (Input.GetButtonDown("Fire1"))
			GetHit(1);
	}

	public void GetHit(int damage)
	{
		health -= damage;
		redBlend = 1.0f;
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
