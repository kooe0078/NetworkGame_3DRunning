using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class raceFinishCount : MonoBehaviour
{
    private PhotonView pv;
    public Text countText;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            pv.RPC("GameFinishRPC", RpcTarget.Others);
            // ī��Ʈ �ٿ� ���� ������ �ֵ� ���� ����
        }
    }

    [PunRPC]
    void GameFinishRPC()
    {
        StartCoroutine(FinishCountdown());
    }

    IEnumerator FinishCountdown()
    {
        int timer = 10;
        countText.gameObject.SetActive(true);
        while (timer != -1)
        {
            if (timer > 0)
                countText.text = timer.ToString();
            else
                countText.text = "���� ����!";
            timer--;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
