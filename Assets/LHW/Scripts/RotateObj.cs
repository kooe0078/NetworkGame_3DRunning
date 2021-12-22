using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public float rotSpeed = 100.0f;

    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
    }
    private void OnCollisionEnter(Collision collision)
    {      
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = transform;
        }     
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
