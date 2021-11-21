using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public float upDis = 2.0f;
    private Vector3 openVec = Vector3.zero;
    private Vector3 closeVec = Vector3.zero;

    private float moveSpeed = 1.0f;
    public float minMoveSpeed = 1.0f;
    public float maxMoveSpeed = 3.0f;

    private float delayTime = 2.0f;
    public float minDelayTime = 1.0f;
    public float maxDelayTime = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        closeVec = transform.position;
        openVec = new Vector3(transform.position.x, transform.position.y + upDis, transform.position.z);
        StartCoroutine(openDoor());
    }
    IEnumerator openDoor()
    {
        SetRandomVal();
        while (Vector3.Distance(transform.position, openVec) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, openVec, moveSpeed * Time.deltaTime);
            yield return null;
        }      
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(closeDoor());
    }
    IEnumerator closeDoor()
    {
        SetRandomVal();
        while (Vector3.Distance(transform.position, closeVec) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, closeVec, moveSpeed * Time.deltaTime);
            yield return null;
        }     
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(openDoor());
    }
    private void SetRandomVal()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        delayTime = Random.Range(minDelayTime, maxDelayTime);
    }
}
