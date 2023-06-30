using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    BoxCollider col;
    bool isDead = false;
    [Header("Movement")]
    [SerializeField] float groundedCheckDist;
    [SerializeField] float jumpForce;
    [SerializeField] bool inAir = false;
    [SerializeField] float ignoreRaycastTime = 0.2f;
    [SerializeField] float rollCenterY;
    [SerializeField] float rollSizeY;
    float startingCenterY;
    float startingSizeY;
    float currentTime = 0f;
    bool ignoreRaycast = false;
    Rigidbody rb;
    GameManager gm;
    bool rolling = false;
    [SerializeField] float rollingTimer;
    float currentRollingTimer = 0;

    [Header("Audio")]
    [SerializeField] AudioClip jumpLandingSound;
    [SerializeField] AudioClip jumpingSound;
    [SerializeField] AudioClip deathSound;

    [Header("Input")]
    Vector2 swipeStartPos;
    bool canDetectSwipe;
    [SerializeField] float minimumSwipeDist;
    bool swipedUp;
    bool swipedDown;

    [Header("Powerup")]
    public bool invin = false;
    [SerializeField] float powerupTimer = 10f;
    Material mat;
    [SerializeField] Color colourChange;
    [SerializeField] GameObject explosionGO;
    float currentPowerupTimer = 0;

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
        col = GetComponent<BoxCollider>();
        startingCenterY = col.center.y;
        startingSizeY = col.size.y;
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    void Update()
    {
        if(!isDead)
        {
            CheckSwipe();
            if (ignoreRaycast)
            {
                currentTime += Time.deltaTime;
                if (currentTime > ignoreRaycastTime)
                {
                    ignoreRaycast = false;
                    currentTime = 0;
                }
            }
            if ((Input.GetKeyDown(KeyCode.Space) || swipedUp) && IsGrounded())
            {
                AudioManager.Instance.PlaySFX(jumpingSound);
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                animator.SetTrigger("Jump");
                inAir = true;
                ignoreRaycast = true;
            }
            else if((Input.GetKeyDown(KeyCode.S) || swipedDown) && IsGrounded())
            {
                animator.SetTrigger("Roll");
                col.center = new Vector3(col.center.x, rollCenterY, col.center.z);
                col.size = new Vector3(col.size.x, rollSizeY, col.size.z);
                rolling = true;
            }
            if (inAir && IsGrounded() && !ignoreRaycast)
            {
                AudioManager.Instance.PlaySFX(jumpLandingSound);
                animator.SetTrigger("FinishJump");
                inAir = false;
            }
            if(rolling)
            {
                currentRollingTimer += Time.deltaTime;
                if(currentRollingTimer > rollingTimer)
                {
                    currentRollingTimer = 0;
                    rolling = false;
                    col.center = new Vector3(col.center.x, startingCenterY, col.center.z);
                    col.size = new Vector3(col.size.x, startingSizeY, col.size.z);
                }
            }
            if(invin)
            {
                currentPowerupTimer += Time.deltaTime;
                if (currentPowerupTimer > powerupTimer)
                {
                    mat.color = Color.white;
                    invin = false;
                    currentPowerupTimer = 0;
                }
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
            if(!invin)
            {
                AudioManager.Instance.PlaySFX(deathSound);
                animator.SetTrigger("Death");
                Time.timeScale = 1;
                isDead = true;
                playerDeath?.Invoke();
            }
            else
            {
                if(collision.transform.GetComponent<MeshRenderer>())
                {
                    collision.transform.GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    collision.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
                Instantiate(explosionGO, collision.transform.position, Quaternion.identity);
                Destroy(collision.transform.GetComponent<Rigidbody>());
                foreach (Collider col in collision.transform.GetComponents<Collider>())
                {
                    Destroy(col);
                }
                addScore?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fence"))
        {
            addScore?.Invoke();
        }
        else if (other.CompareTag("Powerup"))
        {
            other.GetComponent<Powerup>().Effect();
            mat.color = colourChange;
            
        }
    }

    public void Restart()
    {
        animator.SetTrigger("Restart");
        isDead = false;
    }

    void CheckSwipe()
    {
        swipedUp = false;
        swipedDown = false;
        if (Input.touches.Length > 0)
        {
            Touch currentTouch = Input.GetTouch(0);

            switch (currentTouch.phase)
            {
                case TouchPhase.Began:
                    swipeStartPos = new Vector2(currentTouch.position.x / Screen.width, currentTouch.position.y / Screen.width);
                    break;

                case TouchPhase.Moved:

                    if (canDetectSwipe)
                    {
                        Vector2 endPos = new Vector2(currentTouch.position.x / Screen.width, currentTouch.position.y / Screen.width);
                        Vector2 swipeDirection = endPos - swipeStartPos;

                        if (swipeDirection.magnitude < minimumSwipeDist)
                        {
                            // Swipe was too short
                            return;
                        }

                        if (swipeDirection.y > 0)
                        {
                            swipedUp = true;
                        }
                        else
                        {
                            swipedDown = true;
                        }

                        canDetectSwipe = false;
                    }
                    break;

                case TouchPhase.Ended:

                    canDetectSwipe = true;
                    break;

                default:
                    break;
            }


        }
    }
}
