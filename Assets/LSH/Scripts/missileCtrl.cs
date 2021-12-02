using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class missileCtrl : MonoBehaviour
{
    private PhotonView pv;
    public bool bAttack = false;
    private Vector3 target = Vector3.zero;
    public GameObject hitEffect;
    //public string owner = "";

    // 기본 속도와 최고 속도
    public float speed = 5.0f;
    public float maxSpeed = 50.0f;
    // 타겟 검색을 위한 레이어 마스크
    //[SerializeField] LayerMask layerMask = 0;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        SearchTarget();
        pv.RPC("StartRPC", RpcTarget.Others);
    }
    void Update()
    {
        if (target != Vector3.zero)
        {
            // 미사일 속도가 최고 속도가 아니면 가속
            if (speed <= maxSpeed)
            {
                speed += speed * Time.deltaTime;
            }
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            //// 미사일 이동
            //transform.position += transform.up * speed * Time.deltaTime;

            //// 미사일 앞 부분을 캐릭터의 이동에 맞춰 기울임
            //Vector3 direction = (target.position - transform.position).normalized;
            //transform.up = Vector3.Lerp(transform.up, direction, 0.25f);
        }
    }
    [PunRPC]
    private void StartRPC()
    {
        SearchTarget();
    }
    public void SearchTarget()
    {
        if (bAttack)
        {
            // 플레이어의 태그를 가진 오브젝트 전체 검색
            GameObject[] searchTarget = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < searchTarget.Length; i++)
            {
                // 플레이어 캐릭터인지 아닌지 검사
                if (searchTarget[i].GetComponent<PlayerCtrl>().photonView.IsMine == false)
                {
                    target = searchTarget[i].transform.position;
                    break;
                }
            }
        }
        else
        {
            // 플레이어의 태그를 가진 오브젝트 전체 검색
            GameObject[] searchTarget = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < searchTarget.Length; i++)
            {
                // 플레이어 캐릭터인지 아닌지 검사
                if (searchTarget[i].GetComponent<PlayerCtrl>().photonView.IsMine)
                {
                    target = searchTarget[i].transform.position;
                    break;
                }
            }
        }
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
