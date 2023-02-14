using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	[SerializeField] private Transform target;

	[SerializeField] private float smoothTime = 0.25f;
	[SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
	private Vector3 velocity = Vector3.zero;

	void Update()
	{
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
		transform.position = smoothedPosition;
	}

	private void OnEnable() {
		GameManager.OnReset += ResetCamera;
	}
	private void OnDisable() {
		GameManager.OnReset -= ResetCamera;
	}

	void ResetCamera() {
		transform.position = target.position;
	}
}