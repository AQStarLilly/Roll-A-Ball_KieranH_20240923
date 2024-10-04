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
        Vector3 lookAt = new Vector3(Player.position.x, Player.position.y, Player.position.z);
        transform.LookAt(lookAt);

        Vector3 displacement = Player.position - transform.position;
        displacement.y = 0;

        if (Vector3.Distance(new Vector3(Player.position.x, 0, Player.position.z), new Vector3(transform.position.x, 0, transform.position.z)) > 0.5f)
        {
            transform.position += displacement.normalized * moveSpeed * Time.deltaTime;
        }
        CheckForPlayerContact();
    }

    void CheckForPlayerContact()
    {
        if (Vector3.Distance(Player.position, transform.position) < 1f)
        {
            PlayerController playerController = Player.GetComponent<PlayerController>();
            if(playerController != null)
            {
                playerController.ResetPosition();
            }
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

