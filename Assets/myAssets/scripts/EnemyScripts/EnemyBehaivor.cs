using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaivor : MonoBehaviour
{
    public EnemyData enemyData;
    private Transform player;

    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform salidaPistola;
    [SerializeField] private float bulletSpeed = 30f;

    [SerializeField] private NavMeshAgent agent;
    
    float attackTimer = 1f;
    float attackTimerCooldown;
    float dist;

    void Start()
    {
        attackTimerCooldown = attackTimer;
    }
    void Update()
    {
        if(attackTimerCooldown >= 0)
        {
            attackTimerCooldown -= Time.deltaTime;
        }
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        dist = Vector3.Distance(transform.position, player.position);
        if(player != null)
        {
            if(dist > enemyData.stopRange && dist < enemyData.detectRange)
            {
                if (enemyData.vuela)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position, player.position, enemyData.speed * Time.deltaTime);
                    transform.LookAt(player);
                }
                else
                {
                    agent.SetDestination(player.position);
                }
            }
            else if(dist < enemyData.stopRange)
            {
                agent.isStopped = true;
                if (enemyData.dispara)
                {
                    Disparar();
                }
            }
        }
    }
    public void TakeDamage(int amount)
    {
        enemyData.vidaActual += amount;
        if (enemyData.vidaActual <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void Disparar()
    {
        if(attackTimerCooldown <= 0)
        {
            GameObject balaInstance = Instantiate(balaPrefab, salidaPistola.position, salidaPistola.rotation);
            attackTimerCooldown = attackTimer;
            Rigidbody rb = balaInstance.GetComponent<Rigidbody>();

            if (rb != null)
                rb.linearVelocity = salidaPistola.forward * bulletSpeed;

            Destroy(balaInstance, 5f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            TakeDamage(-1);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (attackTimerCooldown <= 0)
        {
            attackTimerCooldown = attackTimer;
            if (other.gameObject.tag.Equals("Player"))
            {
                int damage = Random.Range(enemyData.damageMin, enemyData.damageMax);
                other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            }
        }
    }
}