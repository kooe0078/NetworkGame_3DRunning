using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileCtrl : MonoBehaviour
{
    // 리지드바디, 타겟 선언
    private Rigidbody rigidbody;
    public Transform target;
    public GameObject hitEffect;
    //public string owner = "";

    // 기본 속도와 최고 속도
    public float speed = 5.0f;
    public float maxSpeed = 50.0f;


    // 타겟 검색을 위한 레이어 마스크
    //[SerializeField] LayerMask layerMask = 0;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(ShootDelay());
    }

    void Update()
    {
        if (target != null)
        {
            // 미사일 속도가 최고 속도가 아니면 가속
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
        // 플레이어의 태그를 가진 오브젝트 전체 검색
        GameObject[] searchTarget = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < searchTarget.Length; i++)
        {
            // 플레이어 캐릭터인지 아닌지 검사
            if (searchTarget[i].GetComponent<PlayerCtrl>().photonView.IsMine == false)
            {
                target = searchTarget[i].transform;
                Debug.Log(target);
                break;
            }
        }

        if (target == null)
        {
            Destroy(gameObject);
            Debug.Log("target null");
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
            hitObjectDestroy();
        }
    }
    private void OnTriggerEnter(Collider coll)
    {
        // 쉴드와 충돌할 경우
        if (coll.transform.CompareTag("Shield"))
        {
            hitObjectDestroy();
        }
    }

    public void hitObjectDestroy()
    {
        // 미사일 충돌 후 삭제 처리        
        Destroy(gameObject);
        var hitInstance = Instantiate(hitEffect, transform.position, transform.rotation);
        var hitParticle = hitInstance.GetComponent<ParticleSystem>();
        Destroy(hitInstance, hitParticle.main.duration);
    }
}
