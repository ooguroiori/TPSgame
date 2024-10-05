using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMinoManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] EnemyStatusSO EnemyStatusSO;
    private NavMeshAgent agent;
    private float speed = 3f;
    private float distance;
    private Animator animator;

    private int hp;
    private bool isStopped = false;
    private float stopTime = 0.5f;
    private float stopTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        hp = EnemyStatusSO.enemyStatusList[1].HP;

        // targetが設定されていない場合は警告を表示
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            distance = Vector3.Distance(target.position, transform.position);
            if (!isStopped)
            {
                //一定距離に近づくと追尾
                if(distance < 17)
                {
                    if (distance < 10)
                    {
                        //至近距離で攻撃を行う
                        if (distance < 2)
                        {
                        }
                        else
                        {
                            agent.speed = 2f;
                            animator.SetBool("walk", true);
                            animator.SetBool("run", false);
                            agent.destination = target.position;
                        }
                    }else{
                        agent.speed = 5f;
                        agent.destination = target.position;
                        animator.SetBool("run", true);
                        animator.SetBool("walk", true);
                    }
                }else
                {
                    agent.destination = agent.transform.position;
                    animator.SetBool("walk", false);
                    animator.SetBool("run", false);
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
            animator.SetBool("run", false);
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