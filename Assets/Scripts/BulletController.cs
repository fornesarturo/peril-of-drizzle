using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnBecameInvisible() {
		Destroy (gameObject);
	}

    void OnCollionEnter2D(Collision2D c) {
        switch (c.transform.tag) {
            case "Groud":
                Destroy(gameObject);
                break;
        }
    }
}
