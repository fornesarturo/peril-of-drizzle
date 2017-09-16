using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Animator anim;
	public GameObject bullet;
	private Rigidbody2D rb2;
	private static int speed = 10;
    private static float climbSpeed = 8f;
    private static Vector2 jumpVector = new Vector2(0, 15.0f);
    private static int jumps = 2;
    // private static bool grounded;
    private static bool rope;
    private static float gravity = 4.0f;
	public int life = 10;
	private int direction = 1;

    // Control of character
    public string horizontalControl = "Horizontal_P1";
    public string verticalControl = "Vertical_P1";
    public string jumpControl = "Jump_P1";
    public string fireControl = "Fire_P1";
    public int playerNo = 1;

    private void Awake() {
        rb2 = GetComponent<Rigidbody2D> ();
        rb2.gravityScale = gravity;
		anim = GetComponent<Animator> ();
    }

    void Start () {

	}

	void Update () {
        float h = Input.GetAxisRaw(horizontalControl);
        float v = Input.GetAxisRaw(verticalControl);
        float jump = Input.GetAxisRaw(jumpControl);
        float fire = Input.GetAxisRaw(fireControl);

		anim.SetFloat ("Speed", h);
        rb2.velocity = new Vector2(h * speed, rb2.velocity.y);

				if (h > 0) {
					direction = 1;
				} else if (h < 0) {
					direction = -1;
				}

				if(Input.GetKeyDown("joystick " + playerNo + " button 0") || Input.GetKeyDown(KeyCode.Space)) {
		            if (jumps > 0) {
		                jumps--;
		                rb2.velocity = new Vector2(rb2.velocity.x, 0);
		                rb2.AddForce(jumpVector, ForceMode2D.Impulse);
		            }
		        }

        if(rope && Mathf.Abs(v) > 0) {

            rb2.gravityScale = 0;
            rb2.velocity = new Vector2();
            rb2.position = new Vector2( rb2.position.x, rb2.position.y + (v * climbSpeed * Time.deltaTime));
            Physics2D.IgnoreLayerCollision(gameObject.layer, 8, true);
            // grounded = false;
            jumps = 0;
        }

		if (Input.GetKeyDown (KeyCode.Z)) {
			GameObject bulletClone  = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
			bulletClone.GetComponent<Rigidbody2D> ().AddForce(new Vector2(direction * 40, 0), ForceMode2D.Impulse);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        switch (collision.transform.tag) {
            case "Ground":
                jumps = 2;
                // grounded = true;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        switch (collision.transform.tag) {
            case "Ground":
                print("Not Grounded");
                // grounded = false;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch(collision.transform.tag) {
            case "Rope":
                print("Touching Rope");
                rope = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        switch (collision.transform.tag) {
            case "Rope":
                rope = false;
                Physics2D.IgnoreLayerCollision(gameObject.layer, 8, false);
                rb2.gravityScale = gravity;
                jumps = 1;
                break;
        }
    }
}
