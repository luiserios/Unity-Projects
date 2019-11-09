using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    
    void Update()
    {
        // translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // if laser position is greater than 8 on the y
        // destroy the object
        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }
}
