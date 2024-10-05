using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour {
    private ParticleSystem particle;
    public GameObject bulletPrefab;
    public float shotInterval;
    public float shotSpeed;
    private PlayerStatus playerStatus;
    public float detectionRange; // 一定範囲の設定
    public LayerMask enemyLayer;       // 敵を含むレイヤーマスク

    private bool hasShot = false;      // 弾丸を一度撃ったかどうかのフラグ

    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        particle = GetComponent<ParticleSystem>();
        detectionRange = playerStatus.detectionRange;

        if (particle == null) {
            Debug.LogError("ParticleSystemコンポーネントが見つかりません。GameObjectにParticleSystemがアタッチされていることを確認してください。");
        }
    }

    void Update()
    {
        bool isChanting = playerStatus.chant;
        GameObject nearestEnemy = GetNearestEnemyInRange();

        // 敵が範囲内にいてチャントが有効な場合
        if (isChanting && nearestEnemy != null && !hasShot) {
            if (!particle.isPlaying) {
                particle.Play();
                StartCoroutine(SpawnBulletAfterDelay(3.0f, nearestEnemy)); // 3秒後に弾丸を生成
                hasShot = true;  // 一度撃ったことを記録
            }
        }

        // チャントが終わったらフラグをリセット
        if (!isChanting) {
            hasShot = false;
        }
    }

    GameObject GetNearestEnemyInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = collider.gameObject;
            }
        }

        return nearestEnemy;
    }

    IEnumerator SpawnBulletAfterDelay(float delay, GameObject target)
    {
        yield return new WaitForSeconds(delay);

        // 弾丸をターゲットの近くに生成する
        GameObject bullet = Instantiate(bulletPrefab, target.transform.position, Quaternion.identity);
        bullet.tag = "P_atk";

        CapsuleCollider bulletcc = bullet.GetComponent<CapsuleCollider>();
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletcc.isTrigger = true;

        particle.Stop();

        // 3秒後に弾丸を破壊
        Destroy(bullet, 3.0f);
    }

    // 範囲を可視化するためのデバッグ描画
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
