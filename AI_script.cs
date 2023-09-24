using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI_script : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] float chaseDist;
    [SerializeField] float atkDist;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletTransform;
    private bool shooting;

    public float enemyHp;
    [SerializeField] Slider enemyHPBar;

    private bool isAlive;
    [SerializeField] Transform enemyRespawn;

    [SerializeField] public Transform[] patrolPoints;
    private int currentPatrolPointIndex;
    private bool isFollowing;

    // Start is called before the first frame update
    void Start()
    {
        // Get the right components
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("FirstPerson").GetComponent<Transform>();
        enemyRespawn = GameObject.Find("EnemyRespawn").transform;

        // Get everything ready for the enemy to attack
        bullet = Resources.Load("Prefabs/enemyBullet") as GameObject;
        bulletTransform = transform.GetChild(0).GetChild(0).GetChild(0).transform;

        // Enemy movement start setup
        chaseDist = 20f;
        atkDist = 10f;
        agent.isStopped = false;

        enemyHp = 100;
        isAlive = true;
        shooting = false;

        patrolPoints = new Transform[] {GameObject.Find("patrolA").transform, GameObject.Find("patrolB").transform, enemyRespawn};
        currentPatrolPointIndex = 0;
        isFollowing = false;

        SetDestinationToPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        enemyHPBar.value = enemyHp;

        if (enemyHp <= 0 && isAlive)
        {
            isAlive = false;
            StartCoroutine(respawn());
        }

        if (isAlive)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            if (distToPlayer <= chaseDist && !isFollowing)
            {
                if (distToPlayer > atkDist)
                {
                    StartChasing();
                }
                else
                {
                    if (!shooting)
                    {
                        StartCoroutine(Attack());
                    }
                    else
                    {
                        FaceTarget(player.position);
                    }
                }
            }
            else
            {
                agent.isStopped = false;

                if (isFollowing)
                {
                    isFollowing = false;
                    SetDestinationToPatrolPoint();
                }
                else
                {
                    if (agent.remainingDistance <= 0.1f)
                    {
                        SetDestinationToPatrolPoint();
                    }
                }
            }
        }
    }

    public void StartChasing()
    {
        agent.SetDestination(player.position);
        agent.isStopped = false;
        isFollowing = true;
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
    }

    private void SetDestinationToPatrolPoint()
    {
        agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
    }

    IEnumerator Attack()
    {
        // attack the player
        agent.isStopped = true;
        shooting = true;
        Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);

        shooting = false;
        agent.isStopped = true;
    }

    IEnumerator respawn()
    {
        agent.transform.position = enemyRespawn.position;

        // Turn everything off
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        agent.isStopped = true;

        yield return new WaitForSeconds(10);


        // Turn everything on
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        agent.isStopped = false;

        enemyHp = 100;
        isAlive = true;
        yield return new WaitForSeconds(1);
    }
}
