using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileCtrl : MonoBehaviour
{
    // 리지드바디, 타겟 선언
    private Rigidbody rigidbody;
    public Transform target;

    // 기본 속도와 최고 속도
    public float speed = 5.0f;
    public float maxSpeed = 50.0f;

    // 타겟 검색을 위한 레이어 마스크
    //[SerializeField] LayerMask layerMask = 0;

    void Start()
    {
        StartCoroutine(ShootDelay());
    }

    void Update()
    {
        if (target != null)
        {
            // 미사일 속도가 최고 속도가 아니면
            if (speed <= maxSpeed)
            {
                speed += speed * Time.deltaTime;
            }

            // 미사일 이동
            transform.position += transform.up * speed * Time.deltaTime;

            // 미사일 앞 부분을 캐릭터의 이동에 맞춰 기울임
            Vector3 direction = (target.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, direction, 0.25f);
        }
    }

    public void SearchTarget()
    {
        // 일정 범위 내 해당 레이어 마스크의 타겟 선정
        //Collider[] coll = Physics.OverlapSphere(transform.position, 100.0f, layerMask);

        // 플레이어의 이름을 가진 오브젝트 추격
        target = GameObject.Find("Player").transform;

        //if(coll.Length > 0)
        //{
        //    target = coll[Random.Range(0, coll.Length)].transform;
        //}

        if (target == null)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ShootDelay()
    {
        // 미사일 생성 0.5초 후 추격
        yield return new WaitForSeconds(0.5f);
        SearchTarget();
    }

    private void OnCollisionEnter(Collision coll)
    {
        // 플레이어와 충돌할 경우
        if (coll.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("HitPlayer");
        }
    }
    private void OnTriggerEnter(Collider coll)
    {
        //쉴드와 충돌할 경우
        if (coll.transform.CompareTag("Shield"))
        {
            Destroy(gameObject);
            Debug.Log("HitShield");
        }
    }
}
