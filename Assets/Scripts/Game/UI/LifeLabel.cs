using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LifeLabel : MonoBehaviour
{
	[SerializeField]
	private Text lifeText;
	
	private int life = 0;

	public static LifeLabel lf;

	private void Start()
	{
		lf = this;
	}

	public void SetLife(int life)
	{
		this.life = life;
		lifeText.text = life.ToString();
	}
}
