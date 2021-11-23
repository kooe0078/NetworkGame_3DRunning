using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject rotateObj;
    public float rotSpeed = 100.0f;
    public float moveSpeed = 6.0f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyWall", 3f);
        rotateObj.transform.rotation = Quaternion.Euler(90, -90, 90);
    }

    // Update is called once per frame
    void Update()
    {
        rotateObj.transform.Rotate(new Vector3(rotSpeed * Time.deltaTime, 0, 0));
        rotateObj.transform.Translate(new Vector3(0, 0, -moveSpeed * Time.deltaTime),Space.World);
    }
    private void DestroyWall()
    {
        Destroy(rotateObj);
    }
}
