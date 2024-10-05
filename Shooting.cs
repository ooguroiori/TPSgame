using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Shooting : MonoBehaviour {
 
    public GameObject bulletPrefab;
    public float shotSpeed;
    public int shotCount = 30;
    private float shotInterval;
    [SerializeField] private AudioSource audio;//AudioSource型の変数を宣言 使用するAudioSourceコンポーネントをアタッチ必要
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip reload;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = true;  // 音楽をループさせる
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
 
            shotInterval += 1;
 
            if (shotInterval % 5 == 0 && shotCount > 0)
            {
                shotCount -= 1;
 
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                bullet.tag = "P_atk"; // 弾丸にタグを設定
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed);
                audio.PlayOneShot(shot);

 
                //射撃されてから3秒後に銃弾のオブジェクトを破壊する.
 
                Destroy(bullet, 3.0f);
            }
 
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            shotCount = 30;
            audio.PlayOneShot(reload);
           
        }else{
            audio.Stop();  // 音楽を停止する
        }
 
    }
}