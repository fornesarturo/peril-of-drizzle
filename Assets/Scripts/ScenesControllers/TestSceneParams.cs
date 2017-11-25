using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneParams : MonoBehaviour {

	void Awake() {
		PlayerPrefs.SetInt ("PlayerLife1", 20);
		PlayerPrefs.SetInt ("PlayerLife2", 20);
		PlayerPrefs.SetInt ("PlayerSprite1", 3);
		PlayerPrefs.SetInt ("PlayerSprite2", 3);
	}
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("PlayerLife1", 20);
		PlayerPrefs.SetInt ("PlayerLife2", 20);
		PlayerPrefs.SetInt ("PlayerSprite1", 3);
		PlayerPrefs.SetInt ("PlayerSprite2", 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
