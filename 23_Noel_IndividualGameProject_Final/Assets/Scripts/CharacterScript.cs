using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterScript : MonoBehaviour
{
    public float maxCoins;
    public float walkSpeed;
    public float rotateSpeed;
    public float Score;
    public float Coins;
    public float bullets;
    public int regenRate = 1;
    
    public int maxHealth = 10;
    public int currentHealth;
    public HealthBar healthbar;


    private bool CanMove = true;

    public GameObject bulletPrefab;
    public GameObject bulletSpawn;
    public GameObject HealthText;
    public GameObject ScoreText;
    public GameObject CoinText;
    public GameObject ammotext;

    public AudioClip Coin;
    public AudioClip PistolShoot;
    public AudioClip PistolReload;
    public AudioClip Hurt;
    public AudioClip Death;
    public AudioClip Heal;

    public Animator animator;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {      
        healthbar.SetHealth(currentHealth);
        HealthText.GetComponent<Text>().text = currentHealth + "  / 10 ";
        ammotext.GetComponent<Text>().text = bullets + "/10 ";
        ScoreText.GetComponent<Text>().text = "Score: " + Score;
        CoinText.GetComponent<Text>().text = "X " + Coins;

        if (currentHealth >= 10)
        {
            currentHealth = 10;
        }

        if (!PauseMenu.isPaused)
        {
            if (CanMove == true)
            {
                //Movement (walk)
                if (Input.GetKey(KeyCode.W))
                {
                    transform.position += transform.forward * walkSpeed * Time.deltaTime;
                    animator.SetBool("IsWalkBool", true);
                    animator.SetBool("WalkForward", true);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    transform.position -= transform.forward * walkSpeed * Time.deltaTime;
                    animator.SetBool("IsWalkBool", true);
                    animator.SetBool("WalkBackward", true);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += transform.right * walkSpeed * Time.deltaTime;
                    animator.SetBool("IsWalkBool", true);
                    animator.SetBool("WalkRight", true);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    transform.position -= transform.right * walkSpeed * Time.deltaTime;
                    animator.SetBool("IsWalkBool", true);
                    animator.SetBool("WalkLeft", true);
                }
            

                //rotate
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * rotateSpeed, 0));
                    animator.SetBool("IsWalkBool", true);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * -rotateSpeed, 0));
                    animator.SetBool("IsWalkBool", true);
                }
            
                if (Input.anyKey == false)
                {
                    animator.SetBool("IsWalkBool", false);
                    animator.SetBool("WalkForward", false);
                    animator.SetBool("WalkBackward", false);
                    animator.SetBool("WalkLeft", false);
                    animator.SetBool("WalkRight", false);
                }

                //shoot
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetTrigger("AttackTrigger");
                    StartCoroutine(WaitBeforeBullet());                      
                }

                //Reload
                if (Input.GetKeyDown(KeyCode.R))
                {
                    animator.SetTrigger("ReloadingTrigger");
                    StartCoroutine(WaitForReload());
                }
            }
        }
    }

    private IEnumerator WaitBeforeBullet()
    {
        if(bullets >= 1)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(bulletPrefab, bulletSpawn.transform.position, transform.rotation);
            audioSource.PlayOneShot(PistolShoot);
            bullets -= 1;
        }
        else if (bullets <= 0)
        {
            bullets = 0;
        }
    }

    private IEnumerator WaitForReload()
    {
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(PistolReload);
        bullets = 10;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (currentHealth > 0)
            {
                currentHealth -= 1;
                audioSource.PlayOneShot(Hurt);
            }
            else if (currentHealth < 1)
            {
                currentHealth = 0;
                animator.SetBool("IsWalkBool", false);
                
                //animator.SetBool("IsDeadBool", true);

                if (CanMove == true)
                {
                    animator.SetTrigger("DeadTrigger");
                    CanMove = false;
                   StartCoroutine(waitForDead());       
                }        
            }       
        }

        if (collision.gameObject.tag == "Coin")
        {
            Score += 100;
            Coins += 10;
            maxCoins += 1;
            audioSource.PlayOneShot(Coin);
            Destroy(collision.gameObject);

            if (maxCoins == 20)
            {
                SceneManager.LoadScene("WinScene");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "HP Regen")
        {
            if (currentHealth < 10)
            {
                currentHealth += 1;
            }
            else if (currentHealth >= 10)
            {
                currentHealth = 10;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HP Regen")
        {
           audioSource.PlayOneShot(Heal);
        }
    }

    private IEnumerator waitForDead()
    {
        animator.SetTrigger("DeadTrigger");
        audioSource.PlayOneShot(Death);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("LoseScene");  
    }
}
