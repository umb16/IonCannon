using UnityEngine;
using System.Collections;


public class DontDublicateCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}

}
