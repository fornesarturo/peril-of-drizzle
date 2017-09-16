using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteListener : MonoBehaviour {

	public Sprite right, left, run;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxisRaw ("Horizontal");

		if (Mathf.Abs (h) > 0) {
			
		}
	}
}
