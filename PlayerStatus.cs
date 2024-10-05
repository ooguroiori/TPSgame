using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI mpText;
    private CharacterController characterController;
    public int HP;
    public int MP;
    public int MaxMP;
    private Vector3 knockbackDirection;
    private float knockbackSpeed = 15f;
    [SerializeField] float knockbackForce; // ノックバックの力
    [SerializeField] float knockbackDuration; // ノックバックの継続時間
    [SerializeField] GameObject statusWindow;
    private float knockbackTimer;
    private Animator animator;
    public bool chant = false;
    private bool statusWindowFlag = false;

    // MP回復に関する設定
    public int mpRecoveryAmount = 1;        // 回復するMPの量
    public float mpRecoveryInterval = 5.0f; // 回復の間隔（秒）

    // 範囲チェック用
    public float detectionRange = 20f; // 一定範囲の設定
    public LayerMask enemyLayer;       // 敵を含むレイヤーマスク

    private GameObject nearestEnemy;   // 範囲内の最も近い敵

    // Start is called before the first frame update
    void Start()
    {
        HP = playerStatusSO.HP;
        MP = playerStatusSO.MP;
        MaxMP = playerStatusSO.MP;
        hpText.GetComponent<TextMeshProUGUI>().text = "HP:" + HP;
        mpText.GetComponent<TextMeshProUGUI>().text = "";   // MPをリセット

        for(int i = 0; i < MP; i++){
            // MPの数だけ♦を表示
            mpText.GetComponent<TextMeshProUGUI>().text += "♦";
        }
        
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        chant = false;

        // MP回復のコルーチンを開始
        StartCoroutine(RecoverMPOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        nearestEnemy = GetNearestEnemyInRange();    // 範囲内の最も近い敵を取得
        magicanimation();
        hpText.GetComponent<TextMeshProUGUI>().text = "HP:" + HP;

        // ノックバック処理
        if (knockbackTimer > 0)
        {
            characterController.Move(knockbackDirection * knockbackSpeed * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0 && animator)
            {
                animator.SetBool("Damage", false);
            }
        }

        // HPが0以下になったら死亡アニメーションを再生
        if(HP <= 0){
            HP = 0;
            animator.SetBool("Die", true);
        }

        // ポーズ処理
        if(Input.GetKeyDown(KeyCode.Tab)){
            switch(statusWindowFlag){
                case true:
                    statusWindow.SetActive(false);
                    Time.timeScale = 1; // ゲームの時間を再開
                    statusWindowFlag = false;
                    break;
                case false:
                    statusWindow.SetActive(true);
                    Time.timeScale = 0; // ゲームの時間を止める
                    statusWindowFlag = true;
                    break;
            }
        }
    }

        // 範囲内の最も近い敵を取得する関数
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 衝突したオブジェクトの情報を取得
        GameObject collidedObject = hit.gameObject;

        // 衝突したオブジェクトの名前をコンソールに表示
        Debug.Log("Collided with: " + collidedObject.name);

        // 衝突したオブジェクトが特定のタグを持っているかどうかをチェック
        if (collidedObject.CompareTag("Enemy"))
        {
            // 敵と衝突したときの処理
            Debug.Log("Hit an enemy!");
            HP = HP - 10;
            animator.SetBool("Damage", true);

            // ノックバックの計算
            knockbackDirection = (transform.position - collidedObject.transform.position).normalized;
            knockbackDirection.y = 0.1f; // 上方向の成分を追加して少し浮かせる
            knockbackDirection = knockbackDirection.normalized; // 正規化して全体の方向を維持
            knockbackSpeed = knockbackForce;
            knockbackTimer = knockbackDuration;
            // ノックバックの時間が終了したら、アニメーターの値を false に設定
            if (knockbackTimer <= 0 && animator)
            {
                animator.SetBool("Damage", false);
            }
        }
    }

    void magicanimation(){
        if (Input.GetKeyDown(KeyCode.Q) && nearestEnemy != null) {
            if(MP > 0 && chant == false){   // MPが0より大きく、チャント中でない場合
                MP -= 3;
                animator.SetInteger("chanting", 1);
                animator.SetBool("test", true);
                chant = true;
                StartCoroutine(ResetChanting());

                mpText.GetComponent<TextMeshProUGUI>().text = "";   // MPをリセット
                for(int i = 0; i < MP; i++){
                    // MPの数だけ♦を表示
                    mpText.GetComponent<TextMeshProUGUI>().text += "♦";
                }

            }else {
                Debug.Log("MPが足りません");
            }
        }else if( Input.GetKeyDown(KeyCode.Q) ){
            Debug.Log("距離が離れすぎています");
        }
    }
    
    private IEnumerator ResetChanting()
    {
        yield return new WaitForSeconds(3);
        animator.SetInteger("chanting", 0);
        chant = false;
    }

    // MPを一定時間ごとに回復させるコルーチン
    private IEnumerator RecoverMPOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(mpRecoveryInterval); // 指定した間隔で待つ

            if (MP < MaxMP)
            {
                MP += mpRecoveryAmount; // MPを回復
                if (MP > MaxMP)
                {
                    MP = MaxMP; // MPが最大値を超えないようにする
                }
                UpdateMPDisplay(); // MP表示の更新
            }
        }
    }

    // MPの表示を更新するメソッド
    private void UpdateMPDisplay()
    {
        mpText.GetComponent<TextMeshProUGUI>().text = ""; // MPをリセット
        for (int i = 0; i < MP; i++)
        {
            mpText.GetComponent<TextMeshProUGUI>().text += "♦"; // MPの数だけ♦を表示
        }
    }
}
