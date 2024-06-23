using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour, IBoid
{
    [SerializeField] int life;
    [SerializeField] float speed;
    public float _chaseTime;
    public float _totalChaseTime;
    public bool ACooldown;
    Rigidbody _rb;
    [SerializeField] GameObject _punch;

    public Transform[] waypoints; // Array to hold the patrol waypoints
    public Transform currentWayPoint;
    public int currentWaypointIndex = 0; // Index of the current waypoint
    public int index;

    [HideInInspector]
    public bool isWaiting = false; // Flag to indicate if the enemy is currently waiting
    public float waitDuration = 3f; // Duration of wait time in seconds
    public float waiTimer;

    LeaderBehaviour _leaderTarget;

    public List<RarityInfo> rarityInfo;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _leaderTarget = GetComponent<LeaderBehaviour>();
    }
    public void Dead()
    {
        Destroy(gameObject);
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }
    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;
            transform.forward = dir;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void CalculateDirection() // todo lo que tiene que ver con los waypoints va a controller
    {
        currentWayPoint = waypoints[currentWaypointIndex];
    }
    private void IncreaseWaypontIndex()
    {
        //Se randomiza el currentWaypointIndex de manera controlada
        currentWaypointIndex = MyRandoms.RangeRandom(0, waypoints.Length);
        if (index == currentWaypointIndex)
        {
            currentWaypointIndex++;
        }
        if (currentWaypointIndex == waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
        currentWayPoint = waypoints[currentWaypointIndex];

    }
    public void Attack()
    {
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        _punch.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _punch.SetActive(false);
    }
    public int Life => life;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            life--;
        }
        // Una vez que nuestro enemigo colisiona con un punto de paatrullaje este pasa al siguiente
        if (other.gameObject.CompareTag("PatrolPoint"))
        {
            index = currentWaypointIndex;
            IncreaseWaypontIndex();
        }
    }

    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
}
