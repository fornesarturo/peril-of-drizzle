using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSpawnerScript : MonoBehaviour {

	public GameObject spawnOfEvil;
	private int count = 1;
	// Use this for initialization
	void Start () {
		StartCoroutine (spawn ());
	}

	// Update is called once per frame
	void Update () {
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 1f);
	}

	IEnumerator spawn() {
		while (count > 0) {
			yield return new WaitForSeconds (20f);
			Instantiate (spawnOfEvil, transform.position, transform.rotation);
			count--;
		}
		yield break;
	}
}
