using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce = 10;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver;
    private int jumpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2 && !gameOver) 
        {
            dirtParticle.Stop();
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            jumpCount++;
            if (jumpCount == 1)
            {
                playerAnim.SetTrigger("Jump_trig");
            }
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            Debug.Log(jumpCount);
        }

        if(gameOver == true)
        {
            dirtParticle.Stop();
        }   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            dirtParticle.Play();
            isOnGround = true;
            jumpCount = 0;
            
        } 
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            Debug.Log("GameOver");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            gameOver = true;
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
        
    }
}
