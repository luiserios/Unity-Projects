using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if bottom of screen
        //respawn at top with a new random x position
        if(transform.position.y < -6f)
        {
            float randomX = Random.Range(-7.5f, 7.5f);
            transform.position = new Vector3(randomX, 8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //damage player
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            //Null check that player exists
            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
       
        //destroy enemy
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(this.gameObject);
        }
    }
}
