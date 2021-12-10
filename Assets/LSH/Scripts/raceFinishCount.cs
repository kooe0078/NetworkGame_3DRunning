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
            // 카운트 다운 동안 들어오는 애들 순위 정리
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
                countText.text = "완주 실패!";
            timer--;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
