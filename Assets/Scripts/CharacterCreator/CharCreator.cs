using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCreator : MonoBehaviour
{
	[SerializeField]
	private PaletteSwap swapper;

	[SerializeField]
	private CreatorPlayer player;

	private Dictionary<string, int> charMap;

	public static CharCreator cc;

	private void Start()
	{
		charMap = new Dictionary<string, int>();

		charMap.Add("Hair", 1);
		charMap.Add("Skin", 2);
		charMap.Add("Shirt", 4);
		charMap.Add("Trousers", 5);
		charMap.Add("Shoes", 6);

		cc = this;
	}

	public void SelectColour(string part, ColourButton button)
	{
		swapper.SetColour(charMap[part], button.colour);
		player.Hop();
	}
}
