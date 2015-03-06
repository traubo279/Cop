using UnityEngine;
using System.Collections;

public class GroundController : MonoBehaviour
{

	public float GetMaxNorthValue ()
	{
		return gameObject.transform.GetComponent<Renderer> ().bounds.max.z;
	}

	public float GetMaxSouthValue ()
	{
		return gameObject.transform.GetComponent<Renderer> ().bounds.min.z;
	}
}
