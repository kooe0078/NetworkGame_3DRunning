using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpwaner : MonoBehaviour
{
    public GameObject wall;

    public bool bSpwanStop = false;

    private float wallSpawnPosY = 0.0f;

    private float minRandomSpwanPosZ = 0.0f;
    private float maxRandomSpwanPosZ = 0.0f;

    public float delayTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        wallSpawnPosY = transform.localScale.y / 2;
        minRandomSpwanPosZ = -1 * (transform.localScale.z / 2) + transform.position.z;
        maxRandomSpwanPosZ = (transform.localScale.z / 2) + transform.position.z;

        StartCoroutine(Spwan());
    }

    IEnumerator Spwan()
    {
        while (!bSpwanStop)
        {
            Vector3 spwanVec = new Vector3(transform.position.x, transform.position.y - wallSpawnPosY + 0.5f, SetRandomPos());
            Instantiate(wall, spwanVec, Quaternion.identity);
            yield return new WaitForSeconds(delayTime);
        }
    }
    private float SetRandomPos()
    {
        
        return  Random.Range(minRandomSpwanPosZ, maxRandomSpwanPosZ);
    }
}
