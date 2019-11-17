using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _firerate = 0.5f;
    private float _canfire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();//Find the gameObject. then get component

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manger is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        //fire laser if space key is pressed
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            FireLaser();
        }
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //-------------------Player controls/movement
        //My solution for player movement
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);   //Horizontal input
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);        //Vertical input

        //optimized player movement
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //if speedactive is false, use normal speed
        transform.Translate(direction * _speed * Time.deltaTime);

        //-------------------Player bounds
        //Y Bounds
        /*
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        */
        //Optimized y bounds of above if-else stamemnet using Clamp
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //X bounds
        if (transform.position.x > 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canfire = Time.time + _firerate;
        
        //if  tripleshotActive is true
        //fire 3 lasers (triple shot prefab)
        if (_isTripleShotActive == true)
        {
            //instantiate for the triple shot
            Instantiate(_tripleShotPrefab, (transform.position), Quaternion.identity);
        }
        //else fire 1 laser
        else
        {
            Instantiate(_laserPrefab, (transform.position + new Vector3(0, 1.05f, 0)), Quaternion.identity);
        }      
    }

    public void Damage()
    {
        _lives --;

        //check if dead
        //destroy player
        if (_lives < 1)
        {
            //communicate with spawn manager
            _spawnManager.onPlayerDeath();
            //let them know to stop spawning
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        //start the power down coroutine for triple shot
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //IEnumerator TripleShotPowerDownRoutine
    //wait for 5 seconds
    //set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
        _speed /= _speedMultiplier;
    }
}
