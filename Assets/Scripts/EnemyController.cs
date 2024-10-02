using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform visionPoint;
    private PlayerController player;

    public Transform Player;

    public float visionAngle = 30f;
    public float visionDistance = 10f;
    public float moveSpeed = 2f;
    public float chaseDistance = 3f;
    

    private Vector3? lastKnownPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAt = Player.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        Vector3 displacement = Player.position - transform.position;
        displacement = displacement.normalized;
        if(Vector2.Distance(Player.position, transform.position) > 1.0f)
        {
            transform.position += (displacement * moveSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {

    }

    void Look()
    {
        Vector3 deltaToPlayer = player.transform.position - visionPoint.position;
        Vector3 directionToPlayer = deltaToPlayer.normalized;

        float dot = Vector3.Dot(transform.forward, directionToPlayer);
        if (dot < 0)
        {
            return;
        }

        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer > visionDistance)
        {
            return;
        }

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > visionAngle)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionDistance))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                lastKnownPlayerPosition = player.transform.position;
            }
        }
    }
}

//Vector3 lookAt = Player.position;
//lookAt.y = transform.position.y;
//transform.LookAt(lookAt);