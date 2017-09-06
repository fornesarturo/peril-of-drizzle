using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb2;
    private static int speed = 10;
    private static float climbSpeed = 8f;
    private static Vector2 jumpVector = new Vector2(0, 15.0f);
    private static int jumps = 2;
    private static bool grounded;
    private static bool rope;
    private static long jumpTime = 10;
    private static float gravity = 4.0f;

    private void Awake() {
        rb2 = GetComponent<Rigidbody2D>();
        rb2.gravityScale = gravity;
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float jump = Input.GetAxisRaw("Jump");
        float fire = Input.GetAxisRaw("Fire1");

        print("Jump: " + jump + ", Fire: " + fire);
        rb2.velocity = new Vector2(h * speed, rb2.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space)) {
            if (jumps > 0) {
                print("Double Jump = " + jumps);
                jumps--;
                rb2.velocity = new Vector2(rb2.velocity.x, 0);
                rb2.AddForce(jumpVector, ForceMode2D.Impulse);
            }
        }

        if(rope && Mathf.Abs(v) > 0) {
            print("Going up the rope");
            rb2.gravityScale = 0;
            rb2.velocity = new Vector2();
            rb2.position = new Vector2( rb2.position.x, rb2.position.y + (v * climbSpeed * Time.deltaTime));
            Physics2D.IgnoreLayerCollision(gameObject.layer, 8, true);
            grounded = false;
            jumps = 0;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        switch (collision.transform.tag) {
            case "Ground":
                print("Grounded");
                jumps = 2;
                grounded = true;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        switch (collision.transform.tag) {
            case "Ground":
                print("Not Grounded");
                grounded = false;
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
                print("Let go of the Rope");
                Physics2D.IgnoreLayerCollision(gameObject.layer, 8, false);
                rb2.gravityScale = gravity;
                jumps = 1;
                break;
        }
    }

    IEnumerator jumpCorroutine() {

        yield return null;
    }
}
