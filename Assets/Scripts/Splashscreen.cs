using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Splashscreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("joystick " + 1 + " button 7") || Input.GetKeyDown ("joystick " + 2 + " button 7") || Input.GetKeyDown ("joystick " + 3 + " button 7") || Input.GetKeyDown ("joystick " + 4 + " button 7")) {
			SceneManager.LoadScene ("CharacterSelect", LoadSceneMode.Single);
		}
	}
}
