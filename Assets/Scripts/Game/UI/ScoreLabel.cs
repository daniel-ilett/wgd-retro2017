using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ScoreLabel : MonoBehaviour
{
	[SerializeField]
	private Text scoreText;

	private int targetScore = 0;
	private int score = 0;

	public static ScoreLabel sc;

	private void Start()
	{
		sc = this;
	}

	public void AddScore(int amount)
	{
		targetScore += amount;
	}

	private void Update()
	{
		score += Mathf.RoundToInt((targetScore - score) * Time.deltaTime * 50.0f);
		scoreText.text = score.ToString();
	}
}
