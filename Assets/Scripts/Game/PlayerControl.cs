using System.Collections;
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
	private Ghost ghost;

	[SerializeField]
	private Bullet bullet;

	private float attackTime = 0.0f;
	private float rechargeTime = 0.25f;

	[SerializeField]
	private Material hitFadeMaterial;
	private float hitFalloff = 0.0f;

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

		// Load colours.
		Load();

		hitFadeMaterial = new Material(hitFadeMaterial);

		// Add the red-fade material to the materials list.
		Material[] materials = renderer.materials;
		Material[] moreMaterials = new Material[materials.Length + 1];

		for (int i = 0; i < materials.Length; ++i)
			moreMaterials[i] = materials[i];

		moreMaterials[materials.Length] = hitFadeMaterial;

		renderer.materials = moreMaterials;

		LifeLabel.lf.SetLife(health);
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

			for(int i = 0; i < colours.Length; ++i)
				swapper.SetColour(i, colours[i]);
		}
	}

	private void Update()
	{
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

			// Fire a bullet.
			if (Input.GetButton("Fire1") && Time.time > attackTime)
			{
				Bullet newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
				newBullet.Fire(diff.normalized, BulletType.PLAYER, 10.0f);

				attackTime = Time.time + rechargeTime;
				ScoreLabel.sc.AddScore(-Enemy.GetDifficulty());
			}
		}

		// Modify red amount.
		hitFalloff = Mathf.Lerp(hitFalloff, 0.0f, Time.deltaTime * 2.5f);

		hitFadeMaterial.SetFloat("_BlendAmount", hitFalloff);
	}

	public void GetHit(int damage)
	{
		health -= damage;
		hitFalloff = 1.0f;
		LifeLabel.lf.SetLife(health);

		if (health <= 0)
			Die();
	}

	private void Die()
	{
		isDead = true;

		Ghost ghostClone = Instantiate(ghost, transform.position, Quaternion.identity);
		ghostClone.SetSprite(renderer.sprite);

		Destroy(gameObject);
	}
}
