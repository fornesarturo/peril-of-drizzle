using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour {

	public Sprite[] sprites;
	public Sprite[] spritesSelected;
	SpriteRenderer spriteRenderer;
	int characterSpriteNumber;
	public int playerNo;
	private bool selectCharacter;
	// Use this for initialization
	void Start () {
		this.spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
		this.spriteRenderer.sprite = this.sprites[0];
		this.selectCharacter = true;
		PlayerPrefs.SetInt ("PlayerSprite1", -1);
		PlayerPrefs.SetInt ("PlayerSprite2", -1);
		PlayerPrefs.SetInt ("PlayerSprite3", -1);
		PlayerPrefs.SetInt ("PlayerSprite4", -1);
		PlayerPrefs.SetInt ("PlayerLife1", -1);
		PlayerPrefs.SetInt ("PlayerLife2", -1);
		PlayerPrefs.SetInt ("PlayerLife3", -1);
		PlayerPrefs.SetInt ("PlayerLife4", -1);
        PlayerPrefs.SetInt ("Level1", 0);
        PlayerPrefs.SetInt ("Level2", 0);
        PlayerPrefs.SetInt ("Level3", 0);
        PlayerPrefs.SetInt ("LevelPlayed", 0);
        PlayerPrefs.SetInt ("Tries", 3);
    }
	
	// Update is called once per frame
	void Update () {
		if (this.selectCharacter) {
			if (Input.GetKeyDown ("joystick " + this.playerNo + " button 5") || Input.GetKeyDown (KeyCode.H)) {
				this.characterSpriteNumber = (this.characterSpriteNumber + 1) % 4;
				this.spriteRenderer.sprite = this.sprites [this.characterSpriteNumber];
			} else if (Input.GetKeyDown ("joystick " + this.playerNo + " button 4") || Input.GetKeyDown (KeyCode.G)) {
				this.characterSpriteNumber--;
				if (this.characterSpriteNumber == -1) {
					this.characterSpriteNumber = 3;
				}
				this.spriteRenderer.sprite = this.sprites [this.characterSpriteNumber];
			} else if (Input.GetKeyDown ("joystick " + this.playerNo + " button 0") || Input.GetKeyDown (KeyCode.Y)) {
				this.spriteRenderer.sprite = this.spritesSelected[this.characterSpriteNumber];
				this.selectCharacter = false;
				PlayerPrefs.SetInt ("PlayerSprite" + this.playerNo, this.characterSpriteNumber);
				PlayerPrefs.SetInt ("PlayerLife" + this.playerNo, 20);
			} else if (Input.GetKeyDown ("joystick " + this.playerNo + " button 1")) {
				SceneManager.LoadScene("Splashscreen", LoadSceneMode.Single);
			}
		} else {
			if (Input.GetKeyDown ("joystick " + this.playerNo + " button 1")) {
				this.spriteRenderer.sprite = this.sprites[this.characterSpriteNumber];
				PlayerPrefs.SetInt ("PlayerSprite" + this.playerNo, -1);
				PlayerPrefs.SetInt ("PlayerLife" + this.playerNo, -1);
				this.selectCharacter = true;
			}
			if (Input.GetKeyDown ("joystick " + this.playerNo + " button 7")) {
				SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, 1f);
	}
}
