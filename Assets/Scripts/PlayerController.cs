using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    bool isDead = false;
    [SerializeField] float groundedCheckDist;
    [SerializeField] float jumpForce;
    [SerializeField] bool inAir = false;
    [SerializeField] float ignoreRaycastTime = 0.2f;
    float currentTime = 0f;
    bool ignoreRaycast = false;
    Rigidbody rb;
    GameManager gm;
    [SerializeField] AudioClip jumpLandingSound;
    [SerializeField] AudioClip jumpingSound;
    [SerializeField] AudioClip deathSound;

    public delegate void PlayerDeath();
    public event PlayerDeath playerDeath;

    public delegate void AddScore();
    public event AddScore addScore;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true);
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(!isDead)
        {
            if (ignoreRaycast)
            {
                currentTime += Time.deltaTime;
                if (currentTime > ignoreRaycastTime)
                {
                    ignoreRaycast = false;
                    currentTime = 0;
                }
            }
            if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) && IsGrounded())
            {
                AudioManager.Instance.PlaySFX(jumpingSound);
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                animator.SetTrigger("Jump");
                inAir = true;
                ignoreRaycast = true;
            }
            if (inAir && IsGrounded() && !ignoreRaycast)
            {
                AudioManager.Instance.PlaySFX(jumpLandingSound);
                animator.SetTrigger("FinishJump");
                inAir = false;
            }
        }
    }

    bool IsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundedCheckDist) && !ignoreRaycast)
        {
            if(hit.transform.name == "Foreground")
            {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Fence") && !isDead)
        {
            AudioManager.Instance.PlaySFX(deathSound);
            animator.SetTrigger("Death");
            Time.timeScale = 1;
            isDead = true;
            playerDeath?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fence"))
        {
            addScore?.Invoke();
        }
    }

    public void Restart()
    {
        animator.SetTrigger("Restart");
        isDead = false;
        /*ignoreRaycast = false;
        currentTime = 0;
        inAir = false;*/
    }
}
