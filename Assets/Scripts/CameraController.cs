using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	private Camera camera;

	public static CameraController cam;

	private void Start()
	{
		cam = this;
		camera = GetComponent<Camera>();
	}

	public Camera GetCamera()
	{
		return camera;
	}
}
