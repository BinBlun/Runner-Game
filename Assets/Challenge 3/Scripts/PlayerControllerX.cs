using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    public float floatForce;
    public float bounceForce = 2;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    private bool lowEnough;
    private float topBound = 14.5f;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        //get the posion
        if (transform.position.y < 14.5 && transform.position.y >= 0)
        {
            lowEnough = true;
        } 
        else if (transform.position.y < 0)
        {
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            lowEnough = true;
        }
        else
        {
            lowEnough = false;
            transform.position = new Vector3(transform.position.x, topBound, transform.position.z);
            playerRb.AddForce(Vector3.down * bounceForce, ForceMode.Impulse); 
        }

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && gameOver == false && lowEnough == true)
        {
            playerRb.AddForce(Vector3.up * floatForce * Time.deltaTime, ForceMode.Impulse);
        }

        
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
