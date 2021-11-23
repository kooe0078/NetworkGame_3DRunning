using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public float rotSpeed = 100.0f;

    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
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
            Player.transform.parent = transform;
        }     
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.transform.parent = null;
        }
    }
}
