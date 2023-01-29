using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{

	[SerializeField] private Transform target;

	private float smoothTime = 0.25f;
	private Vector3 offset = new Vector3(0f, 0f, -5f);
	private Vector3 velocity = Vector3.zero;

	void Update()
	{
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
		transform.position = smoothedPosition;
	}

}