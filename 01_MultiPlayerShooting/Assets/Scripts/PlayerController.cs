﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        // move
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        // fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    void CmdFire()
    {
        // Bullet プレハブから Bullet を生成する
        var bullet = (GameObject)Instantiate (
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // 弾の速度を増加させる
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        
        NetworkServer.Spawn(bullet);

        // 2 秒後に弾を破壊する
        Destroy(bullet, 2.0f);
    }
}
