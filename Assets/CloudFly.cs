using UnityEngine;
using System.Collections;

public class CloudFly : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.left * Time.deltaTime;
	}
}
