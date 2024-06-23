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

    public Node[] waypoints; // Array to hold the patrol waypoints
    public Node currentWayPoint;
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
    }

    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
}
