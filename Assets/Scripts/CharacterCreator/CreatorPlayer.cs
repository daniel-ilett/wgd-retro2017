using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CreatorPlayer : MonoBehaviour
{
	[SerializeField]
	private PaletteSwap swapper;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void Hop()
	{
		animator.SetTrigger("Hop");
	}

	public void Save()
	{
		Color[] colours = swapper.GetColours();

		SColor[] sColours = new SColor[colours.Length];

		for (int i = 0; i < colours.Length; ++i)
			sColours[i] = new SColor(colours[i]);

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/charData.dat");
		bf.Serialize(file, sColours);
		file.Close();

		Debug.Log("Saved");
	}
}

[System.Serializable]
public struct SColor
{
	public float r, g, b, a;

	public SColor(Color col)
	{
		r = col.r;
		g = col.g;
		b = col.b;
		a = col.a;
	}
}
