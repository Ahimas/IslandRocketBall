using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float powerupStrength = 10f;
    private float verticalInput;
    
    private Rigidbody playerRb;
    private GameObject focalPoint;
    
    private Vector3 powerupIndicatorOffset = new Vector3(0, -0.5f, 0);
    private Coroutine startedCoroutine;
    private GameObject rocketTmp;

    public bool isPowerup = false;
    public bool isSmash = false;

    public float speed = 10f;
    public float speedSmash = 20f;
    public GameObject powerupIndicator;
    public GameObject rocketPrefab;
    public PowerupType powerupType = PowerupType.None;

    public float smashHeight = 5f;
    public float smashRadius = 15f;
    public float smashForce = 5f;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");

        powerupIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * speed * verticalInput);

        powerupIndicator.transform.position = transform.position + powerupIndicatorOffset;

        if ( powerupType == PowerupType.Rockets && Input.GetKeyDown(KeyCode.F) )
        {
            foreach ( GameObject target in GameObject.FindGameObjectsWithTag("Enemy") ) {
                rocketTmp = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
                rocketTmp.GetComponent<Rocket>().Fire(target.transform);
            } 
        }

        if ( powerupType == PowerupType.Smash && Input.GetKeyDown(KeyCode.Space) && !isSmash )
        {
            StartCoroutine(Smash());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.CompareTag("Powerup") )
        {
            if (startedCoroutine != null )
            {
                StopCoroutine(startedCoroutine);
            }
            isPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            powerupType = other.gameObject.GetComponent<Powerup>().powerupType;
            Destroy(other.gameObject);

            startedCoroutine = StartCoroutine(PowerupCountdownRoutine());

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.CompareTag("Enemy") && isPowerup && powerupType == PowerupType.PushUp )
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.transform.position - this.transform.position;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        isPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
        powerupType = PowerupType.None;
    }

    IEnumerator Smash()
    {
        float floor = this.transform.position.y;

        isSmash = true;
        playerRb.velocity = Vector3.up * speedSmash;
        
        while ( transform.position.y < smashHeight )
        {   
            yield return null;
        }

        playerRb.velocity = Vector3.down * speedSmash * 2;

        while ( transform.position.y > floor  )
        { 
            yield return null; 
        }

        playerRb.velocity = Vector3.zero;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Rigidbody>().AddExplosionForce(smashForce, transform.position, smashRadius, 0, ForceMode.Impulse);
        }

        isSmash = false;
    }
}
