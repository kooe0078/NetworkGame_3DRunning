using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float moveSpeed = 6.0f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyWall", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
    }
    private void DestroyWall()
    {
        Destroy(gameObject);
    }
}
