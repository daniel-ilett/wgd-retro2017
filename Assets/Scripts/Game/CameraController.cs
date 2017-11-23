using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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

	private Vector3 shakeOffset;
	private Coroutine shake;
	private int shakeStrength;

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
		if(targetObj != null)
		{
			Vector3 desiredPos = targetObj.position;
			desiredPos.z = -10.0f;

			moveObj.velocity = (desiredPos - moveObj.transform.position) * 5.0f;
		}
		else if(Input.GetButtonDown("Cancel"))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void LateUpdate()
	{
		Vector3 pos = moveObj.transform.position;

		float snapThreshold = 1.0f / (pixelsPerUnit);
		float halfThreshold = snapThreshold / 2.0f;

		float x = pos.x % snapThreshold;
		float y = pos.y % snapThreshold;

		pos.x = pos.x - x + ((x > halfThreshold) ? snapThreshold : 0.0f);
		pos.y = pos.y - y + ((y > halfThreshold) ? snapThreshold : 0.0f);

		float offset = 0.5f / pixelsPerUnit;
		transform.position = pos + shakeOffset + new Vector3(offset, offset, 0.0f);
	}

	public Camera GetCamera()
	{
		return camera;
	}

	public void ScreenShake(float duration, int strength)
	{
		if(shake != null)
		{
			if (strength < shakeStrength)
				return;

			StopCoroutine(shake);
		}

		StartCoroutine(Shake(duration, strength));
	}

	private IEnumerator Shake(float duration, int strength)
	{ 
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float snapThreshold = 1.0f / (pixelsPerUnit);

		for (float i = 0; i < duration; i += Time.deltaTime)
		{
			float xAdd = Random.Range(-strength, strength) * snapThreshold;
			float yAdd = Random.Range(-strength, strength) * snapThreshold;

			shakeOffset = new Vector3(xAdd, yAdd, 0.0f);
			yield return wait;
		}

		shakeOffset = Vector3.zero;
	}
}
