using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ColourPicker : MonoBehaviour
{
	[SerializeField]
	private ColourButton[] colouredButtons;

	[SerializeField]
	private Text nameText;
	[SerializeField]
	private Text nameTextBG;

	[SerializeField]
	private Sprite activeSprite;

	[SerializeField]
	private Sprite inactiveSprite;

	[SerializeField]
	private string bodyPart;

	private ColourButton currentButton;

	private void Start()
	{
		foreach(ColourButton b in colouredButtons)
		{
			//ColorBlock cb = b.button.colors;
			//cb.normalColor = b.colour;
			//b.button.colors = cb;

			b.image.color = b.colour;
		}

		nameText.text = bodyPart;
		nameTextBG.text = bodyPart;

		currentButton = colouredButtons[0];

		Press(Random.Range(0, colouredButtons.Length));
	}

	public void Press(int index)
	{
		CharCreator.cc.SelectColour(bodyPart, colouredButtons[index]);

		currentButton.image.sprite = inactiveSprite;
		currentButton = colouredButtons[index];
		currentButton.image.sprite = activeSprite;
	}
}

[System.Serializable]
public struct ColourButton
{
	public Button button;
	public Image image;
	public Color colour;
}