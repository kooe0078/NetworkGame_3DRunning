using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    private Quaternion startRot;
    private Quaternion endRot;

    public float xRotLimit = 20.0f;
    public float moveSpeed = 1.0f;

    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 3.0f;

    private float delayTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        startRot = Quaternion.Euler(-xRotLimit, 90, 0);
        endRot = Quaternion.Euler(xRotLimit, 90, 0);
        StartCoroutine(PendulumMovementStart());
    }

    IEnumerator PendulumMovementStart()
    {
        SetRandomVal();
        while (transform.rotation.x - 0.05f > startRot.x)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.deltaTime * moveSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(delayTime);
       StartCoroutine(PendulumMovementEnd());
    }
    IEnumerator PendulumMovementEnd()
    {
        SetRandomVal();
        while (transform.rotation.x + 0.05f < endRot.x)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, endRot, Time.deltaTime * moveSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(PendulumMovementStart());
    }
    private void SetRandomVal()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }
}
