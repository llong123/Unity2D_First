using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


	public float xMin= -4.0f;
	public float xMax = 1.0f;
	public float yMin = 0.19f;
	public float yMax = 2.8f;

	private Transform target;

	void Start(){
	
		target = GameObject.Find("Player").transform;
	}

	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3 (Mathf.Clamp(target.position.x,xMin,xMax), 
			Mathf.Clamp(target.position.y,yMin,yMax),
			transform.position.z);

	}
}
