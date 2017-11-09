using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class IntroMovie : MonoBehaviour
{
	private new Renderer renderer;

	private void Start()
	{
		renderer = GetComponent<Renderer>();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			MovieTexture movie = (MovieTexture)renderer.material.mainTexture;

			if (movie.isPlaying)
			{
				movie.Pause();
			}
			else
			{
				movie.Play();
			}
		}
	}
}
