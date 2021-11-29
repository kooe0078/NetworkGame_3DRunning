using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBoxCtrl : MonoBehaviour
{
    public GameObject getEffect;

    private void OnCollisionEnter(Collision coll)
    {
        // �÷��̾�� �浹�� ���
        if (coll.transform.CompareTag("Player"))
        {
            // ���� ���� ����Ʈ ����
            var getInstance = Instantiate(getEffect, transform.position, transform.rotation);
            var getParticle = getInstance.GetComponent<ParticleSystem>();
            Destroy(getInstance, getParticle.main.duration);
            //���� ��Ȱ��ȭ �� 5�� �� �����
            gameObject.SetActive(false);

            Invoke("boxReset", 5.0f);
        }
    }    

    void boxReset()
    {
        // ������ �ڽ� �����
        gameObject.SetActive(true);
    }
}
