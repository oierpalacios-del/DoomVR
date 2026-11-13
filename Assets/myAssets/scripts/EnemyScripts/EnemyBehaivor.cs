using UnityEngine;

public class EnemyBehaivor : MonoBehaviour
{
    public float speed = 3f;
    public float detectRange = 10f;
    public float stopRange = 1f;

    private Transform player;

    void Start()
    {

    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > stopRange && dist < detectRange)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, player.position, speed * Time.deltaTime);
                transform.LookAt(player);
            }
        }

    }
}