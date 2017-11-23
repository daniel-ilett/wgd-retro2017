using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class GhostLabel : MonoBehaviour
{
	[SerializeField]
	private Text ghostText;

	private int targetGhosts = 0;
	private int ghosts = 0;

	public static GhostLabel gh;

	private void Start()
	{
		gh = this;
		ghostText.text = ghosts.ToString();
	}

	public void AddGhost()
	{
		++ghosts;
		ghostText.text = ghosts.ToString();
	}
}
