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

    // �⺻ �ӵ��� �ְ� �ӵ�
    public float speed = 5.0f;
    public float maxSpeed = 50.0f;
    // Ÿ�� �˻��� ���� ���̾� ����ũ
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
            // �̻��� �ӵ��� �ְ� �ӵ��� �ƴϸ� ����
            if (speed <= maxSpeed)
            {
                speed += speed * Time.deltaTime;
            }
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            //// �̻��� �̵�
            //transform.position += transform.up * speed * Time.deltaTime;

            //// �̻��� �� �κ��� ĳ������ �̵��� ���� �����
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
            // �÷��̾��� �±׸� ���� ������Ʈ ��ü �˻�
            GameObject[] searchTarget = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < searchTarget.Length; i++)
            {
                // �÷��̾� ĳ�������� �ƴ��� �˻�
                if (searchTarget[i].GetComponent<PlayerCtrl>().photonView.IsMine == false)
                {
                    target = searchTarget[i].transform.position;
                    break;
                }
            }
        }
        else
        {
            // �÷��̾��� �±׸� ���� ������Ʈ ��ü �˻�
            GameObject[] searchTarget = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < searchTarget.Length; i++)
            {
                // �÷��̾� ĳ�������� �ƴ��� �˻�
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
        // �÷��̾�� �浹�� ���
        if (coll.transform.CompareTag("Player"))
        {
            hitObjectDestroy();
        }
    }
    private void OnTriggerEnter(Collider coll)
    {
        // ����� �浹�� ���
        if (coll.transform.CompareTag("Shield"))
        {
            hitObjectDestroy();
        }
    }

    public void hitObjectDestroy()
    {
        // �̻��� �浹 �� ���� ó��        
        Destroy(gameObject);
        var hitInstance = Instantiate(hitEffect, transform.position, transform.rotation);
        var hitParticle = hitInstance.GetComponent<ParticleSystem>();
        Destroy(hitInstance, hitParticle.main.duration);
    }
}
