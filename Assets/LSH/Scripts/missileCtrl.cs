using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileCtrl : MonoBehaviour
{
    // ������ٵ�, Ÿ�� ����
    private Rigidbody rigidbody;
    public Transform target;

    // �⺻ �ӵ��� �ְ� �ӵ�
    public float speed = 5.0f;
    public float maxSpeed = 50.0f;

    // Ÿ�� �˻��� ���� ���̾� ����ũ
    //[SerializeField] LayerMask layerMask = 0;

    void Start()
    {
        StartCoroutine(ShootDelay());
    }

    void Update()
    {
        if (target != null)
        {
            // �̻��� �ӵ��� �ְ� �ӵ��� �ƴϸ�
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
        // ���� ���� �� �ش� ���̾� ����ũ�� Ÿ�� ����
        //Collider[] coll = Physics.OverlapSphere(transform.position, 100.0f, layerMask);

        // �÷��̾��� �̸��� ���� ������Ʈ �߰�
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
        // �̻��� ���� 0.5�� �� �߰�
        yield return new WaitForSeconds(0.5f);
        SearchTarget();
    }

    private void OnCollisionEnter(Collision coll)
    {
        // �÷��̾�� �浹�� ���
        if (coll.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("HitPlayer");
        }
    }
    private void OnTriggerEnter(Collider coll)
    {
        //����� �浹�� ���
        if (coll.transform.CompareTag("Shield"))
        {
            Destroy(gameObject);
            Debug.Log("HitShield");
        }
    }
}
