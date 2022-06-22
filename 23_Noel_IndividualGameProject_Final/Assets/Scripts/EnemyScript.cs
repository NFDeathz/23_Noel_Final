using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float health;
    public static GameObject enemyRemains;
    public GameObject CoinPrefab;
    public GameObject CoinSpawn;
    public Transform target;
    public float speed;
    public AudioClip SkeletonDeath;

    public Animator animator;

    private bool CanMove = true;
    private AudioSource audioSource;

    Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //Enemy follows player
        if (CanMove == true)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            rig.MovePosition(pos);
            transform.LookAt(target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 1;
            if (health <= 0)
            {
                health = 0;
                Destroy(collision.gameObject);
                CanMove = false;
                animator.SetTrigger("DeadTrigger");
                audioSource.PlayOneShot(SkeletonDeath);
                StartCoroutine(WaitBeforeDeath());    
            }                 
        }
    }
    private IEnumerator WaitBeforeDeath()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Instantiate(CoinPrefab, CoinSpawn.transform.position, transform.rotation);
    }

    private void OnApplicationQuit()
    {
        Destroy(CoinPrefab);
    }
}
