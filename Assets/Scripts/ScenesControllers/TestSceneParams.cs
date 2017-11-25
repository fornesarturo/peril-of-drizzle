using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneParams : MonoBehaviour {

	void Awake() {
		PlayerPrefs.SetInt ("PlayerLife1", 20);
		PlayerPrefs.SetInt ("PlayerLife2", 20);
		PlayerPrefs.SetInt ("PlayerSprite1", 1);
		PlayerPrefs.SetInt ("PlayerSprite2", 0);
	}
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("PlayerLife1", 20);
		PlayerPrefs.SetInt ("PlayerLife2", 20);
		PlayerPrefs.SetInt ("PlayerSprite1", 1);
		PlayerPrefs.SetInt ("PlayerSprite2", 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
