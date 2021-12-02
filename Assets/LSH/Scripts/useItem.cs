using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class useItem : MonoBehaviour
{
    public GameObject Missile;
    public GameObject Shield;
    private Vector3 missileSpawnPos = new Vector3(0, 20, 0);
    public GameObject boosterEffect;

    public bool isPlayerGetItem = false;
    public bool getMissile = false;
    public bool getShield = false;
    public bool getBooster = false;

    // 匂宕 坂 識情 貢 竺舛
    private PhotonView pv;

    private PlayerCtrl playerCtrl;
    private void Start()
    {
        playerCtrl = gameObject.GetComponent<PlayerCtrl>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        // if庚税 繕闇生稽 pv.IsMine聖 杏檎 硲什闘 巴傾戚嬢幻 焼戚奴聖 紫遂拝 呪 赤澗 薄雌 降持
        // 繕闇聖 杏走 省生檎 嬢恐 適虞拭辞 焼戚奴聖 紫遂背亀 硲什闘税 蝶遣斗拭幻 焼戚奴戚 紫遂喫
        // 訊 戚君澗走 乞牽畏陥 さげ
        // 3獣 11歳 蓄亜
        // 是拭 旋精 庚薦 訊昔走 乞牽為澗汽 背衣喫, 悦汽 1腰 政艦銅拭辞 蒐球床檎 2腰 政艦銅拭辞 採什斗稽 左績
        // 2腰 政艦銅拭辞 蒐球床檎 1腰拭辞 採什斗稽 左績 せせせせせせせせせせ 耕帖畏陥 遭促
        // 析舘 戚暗 旋壱 陥獣 政艦銅督析 差紫背辞 背瑳闇汽 照鞠檎 旋嬢砧壱 紬遇馬壱 菰堕背走檎 走随暗績
        // 3獣 37歳
        // 陥 菰堕背然革 せせせせせせせせせせせせせせせせせせせせせせせせせせせせせせせせせせせせ
        // 蟹 更 亨備 滴惟 闇窮暗 蒸製 煽凶採斗? 訊走?????????????
        // 宜畏革 遭促 せせせせせせせせせせせせ 
        // 走榎 坪球稽 企中 耕紫析 庶檎 巷繕闇 雌企廃砺 劾焼亜澗汽 唖切 鉢檎拭 魚稽 蟹身, 戚闇 鎧析 壱張暗績 切君姶 さぁ
        if (isPlayerGetItem)
        {
            // 原酔什 疎適遣生稽 耕紫析 降紫
            if (getMissile && Input.GetButtonDown("Fire1"))
            {
                useMssile();
                playerItemReset();
            }
            // 原酔什 酔適遣生稽 蒐球 持失
            if (getShield && Input.GetButtonDown("Fire2"))
            {
                Debug.Log("蒐球 紫遂1");
                useShield();
                playerItemReset();
            }
            // 原酔什 蕃 適遣獣 採什斗
            if (getBooster && Input.GetButtonDown("Fire3"))
            {
                Debug.Log("採什斗 紫遂1");
                useBooster();
                playerItemReset();
            }
        }
    }

    public void useMssile()
    {
        playerCtrl.bAttack = true;
        StartCoroutine(CreateMissile());
        // 耕紫析 紫遂 RPC 硲窒
        pv.RPC("useMissileRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useMissileRPC()
    {
        StartCoroutine(CreateMissile());
    }

    IEnumerator CreateMissile()
    {
        yield return new WaitForSeconds(0.5f);
        // missileSpawnPos = new Vector3(missileSpawnPos.position.x, missileSpawnPos.position.y, missileSpawnPos.position.z - 10);
        GameObject missile = Instantiate(Missile, missileSpawnPos, Quaternion.identity);
        missile.GetComponent<missileCtrl>().bAttack = playerCtrl.bAttack;
        yield return new WaitForSeconds(1.5f);
        playerCtrl.bAttack = false;
    }

    void useShield()
    {
        StartCoroutine(CreateShield());
        // 蒐球 紫遂 RPC 硲窒
        pv.RPC("useShieldRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useShieldRPC()
    {
        StartCoroutine(CreateShield());
    }

    IEnumerator CreateShield()
    {
        Debug.Log("蒐球 紫遂2");
        // 蒐球 持失 板 2段 及 薦暗
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
    }

    void useBooster()
    {
        StartCoroutine(CreateBooster());
        // 採什斗 紫遂 RPC 硲窒
        pv.RPC("useBoosterRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useBoosterRPC()
    {
        StartCoroutine(CreateBooster());
    }

    IEnumerator CreateBooster()
    {
        Debug.Log("採什斗 紫遂2");
        playerCtrl.maxSpeed *= 2.0f;
        //採什斗 紫遂 戚薙闘 持失
        var boosterInstance = Instantiate(boosterEffect, transform.position, transform.rotation);
        var boosterParticle = boosterInstance.GetComponent<ParticleSystem>();
        Destroy(boosterInstance, boosterParticle.main.duration);

        // 採什斗 紫遂 板 3段 及 戚疑紗亀 差姥
        yield return new WaitForSeconds(3.0f);
        playerCtrl.maxSpeed /= 2.0f;
    }

    void playerItemReset()
    {
        getMissile = false;
        getShield = false;
        getBooster = false;
        isPlayerGetItem = false;
    }
}
