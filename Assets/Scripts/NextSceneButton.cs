using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
	[SerializeField]
	private string nextScene;

	public void NextLevel()
	{
		SceneManager.LoadScene(nextScene);
	}
}
