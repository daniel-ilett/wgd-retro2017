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

	[SerializeField]
	private Transform targetObj;

	[SerializeField]
	private Rigidbody2D moveObj;

	private new Camera camera;

	public static CameraController cam;

	private void Start()
	{
		cam = this;
		camera = GetComponent<Camera>();

		camera.orthographicSize = Screen.height / (pixelsPerUnit * 2 * pixelSize);
	}

	private void Update()
	{
		Vector3 desiredPos = targetObj.position;
		desiredPos.z = -10.0f;

		moveObj.velocity = (desiredPos - moveObj.transform.position) * 5.0f;
	}

	private void LateUpdate()
	{
		Vector3 pos = moveObj.transform.position;

		float snapThreshold = 1.0f / (pixelsPerUnit * pixelSize);
		float halfThreshold = snapThreshold / 2.0f;

		float x = pos.x % snapThreshold;
		float y = pos.y % snapThreshold;

		pos.x = pos.x - x + ((x > halfThreshold) ? snapThreshold : 0.0f);
		pos.y = pos.y - y + ((y > halfThreshold) ? snapThreshold : 0.0f);

		transform.position = pos;
	}

	public Camera GetCamera()
	{
		return camera;
	}
}
