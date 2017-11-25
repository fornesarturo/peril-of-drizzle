using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsCamera : MonoBehaviour {

    public Texture creditsImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI() {

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), creditsImage, ScaleMode.StretchToFill);

    }
}
