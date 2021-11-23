using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpwaner : MonoBehaviour
{
    public GameObject wall;

    public bool bSpwanStop = false;

    private float wallSpawnPosY = 0.0f;

    private float minRandomSpwanPosX = 0.0f;
    private float maxRandomSpwanPosX = 0.0f;

    public float delayTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        wallSpawnPosY = transform.localScale.y / 2;
        minRandomSpwanPosX = -1 * (transform.localScale.z / 2) + transform.position.x;
        maxRandomSpwanPosX = (transform.localScale.z / 2) + transform.position.x;
        StartCoroutine(Spwan());
    }

    IEnumerator Spwan()
    {
        while (!bSpwanStop)
        {
            Vector3 spwanVec = new Vector3(SetRandomPos(), transform.position.y - wallSpawnPosY + 0.7f, transform.position.z);
            Instantiate(wall, spwanVec, Quaternion.identity);
            yield return new WaitForSeconds(delayTime);
        }
    }
    private float SetRandomPos()
    {      
        return  Random.Range(minRandomSpwanPosX, maxRandomSpwanPosX);
    }
}
