using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileCtrl : MonoBehaviour
{
    // ������ٵ�, Ÿ�� ����
    private Rigidbody rigidbody;
    public Transform target;
    public GameObject hitEffect;
    //public string owner = "";

    // �⺻ �ӵ��� �ְ� �ӵ�
    public float speed = 5.0f;
    public float maxSpeed = 50.0f;


    // Ÿ�� �˻��� ���� ���̾� ����ũ
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
            // �̻��� �ӵ��� �ְ� �ӵ��� �ƴϸ� ����
            if (speed <= maxSpeed)
            {
                speed += speed * Time.deltaTime;
            }

            // �̻��� �̵�
            transform.position += transform.up * speed * Time.deltaTime;

            // �̻��� �� �κ��� ĳ������ �̵��� ���� �����
            Vector3 direction = (target.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, direction, 0.25f);
        }
    }

    public void SearchTarget()
    {
        // �÷��̾��� �±׸� ���� ������Ʈ ��ü �˻�
        GameObject[] searchTarget = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < searchTarget.Length; i++)
        {
            // �÷��̾� ĳ�������� �ƴ��� �˻�
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
        // �̻��� ���� 0.5�� �� �߰�
        yield return new WaitForSeconds(0.5f);
        SearchTarget();
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
