using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHittingWall : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool hitWall;
    public AudioSource audiosource;
    public AudioClip bounce;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Hitting()) {
            hitWall = false;
        }
        if (Hitting() && !hitWall) {
            if (rb.velocity.magnitude >= 0.3f) {
                audiosource.PlayOneShot(bounce);
            }
            hitWall = true;
        }
    }

    private bool Hitting() {
        return transform.position.x >= 6.8 || transform.position.x <= -6.8 || transform.position.y >= 2.85 || transform.position.y <= -2.8;
    }
}
