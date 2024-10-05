using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] EnemyStatusSO EnemyStatusSO;
    private NavMeshAgent agent;
    private float speed = 3f;
    private float distance;
    private Animator animator;

    private Rigidbody rigidBody;
    private int hp;
    private bool isStopped = false;
    private float stopTime = 0.5f;
    private float stopTimer = 0f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        hp = EnemyStatusSO.enemyStatusList[0].HP;

        // targetが設定されていない場合は警告を表示
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (hp > 0)
        {
            if (hp % 100 == 0)
            {
                animator.SetBool("walk", false);
                animator.SetBool("atack", false);
                animator.SetBool("damage", true);
                agent.destination = agent.transform.position;
                // 動きを止める
                isStopped = true;
                stopTimer = 0f;
                Debug.Log("いたいよ><");
            }
            else if (target != null)
            {
                distance = Vector3.Distance(target.position, transform.position);

                if (!isStopped)
                {
                    //一定距離に近づくと追尾
                    if (distance < 15)
                    {
                        //至近距離で攻撃を行う
                        if (distance < 2)
                        {
                            animator.SetBool("damage", false);
                            animator.SetBool("walk", false);
                            animator.SetBool("atack", true);
                        }
                        else
                        {
                            agent.destination = target.position;
                            animator.SetBool("damage", false);
                            animator.SetBool("atack", false);
                            animator.SetBool("walk", true);
                        }
                    }
                    else
                    {
                        animator.SetBool("walk", false);
                        agent.destination = agent.transform.position;
                    }
                }
                else
                {
                    stopTimer += Time.deltaTime;
                    if (stopTimer >= stopTime)
                    {
                        isStopped = false;
                    }
                }
            }
            else
            {
                animator.SetBool("walk", false);
            }
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("atack", false);
            animator.SetBool("damage", false);
            animator.SetBool("die", true);
            Debug.Log("しんじゃった><");
            agent.destination = agent.transform.position;
        }

    }
    // 物理的な衝突
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("P_atk"))
        {
            hp -= 10;
            Debug.Log("HP:" + hp);
        }
    }

    // トリガーを使用した衝突
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P_atk"))
        {
            hp -= 10;
            Debug.Log("HP: " + hp);
        }
    }
}
