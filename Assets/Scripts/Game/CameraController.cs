using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	[SerializeField]
	private int pixelsPerUnit = 16;

	[SerializeField]
	private int pixelSize = 4;

	private new Camera camera;

	public static CameraController cam;

	private void Start()
	{
		cam = this;
		camera = GetComponent<Camera>();

		camera.orthographicSize = Screen.height / (pixelsPerUnit * 2 * pixelSize);
	}

	private void LateUpdate()
	{
		transform.localPosition = Vector3.zero;
		transform.Translate(new Vector3(0.0f, 0.0f, -10.0f));
		Vector3 pos = transform.position;

		float snapThreshold = 1.0f / (pixelsPerUnit);
		float halfThreshold = snapThreshold / 2.0f;

		float x = pos.x % snapThreshold;
		float y = pos.y % snapThreshold;
		//float z = pos.z % snapThreshold;

		Debug.Log(snapThreshold);

		pos.x = pos.x - x + ((x > halfThreshold) ? snapThreshold : 0.0f);
		pos.y = pos.y - y + ((y > halfThreshold) ? snapThreshold : 0.0f);
		//pos.z = pos.z - z + ((z > halfThreshold) ? snapThreshold : 0.0f);

		transform.position = pos;
	}

	public Camera GetCamera()
	{
		return camera;
	}
}
