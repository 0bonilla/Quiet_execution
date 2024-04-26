using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] float speed;
    public float _chaseTime;
    public float _totalChaseTime;
    public bool ACooldown;
    Rigidbody _rb;
    [SerializeField] GameObject Daddy;

    public Transform[] waypoints; // Array to hold the patrol waypoints
    public Transform currentWaypoint;
    public int currentWaypointIndex = 0; // Index of the current waypoint

    [HideInInspector]
    public bool isWaiting = false; // Flag to indicate if the enemy is currently waiting
    public float waitDuration = 3f; // Duration of wait time in seconds
    public float waiTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        currentWaypoint = waypoints[currentWaypointIndex];
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
    public void CalculateDirection()
    {
        currentWaypoint = waypoints[currentWaypointIndex];
    }
    private void IncreaseWaypontIndex()
    {
        currentWaypointIndex++;
        ResetTargetPoint();
        currentWaypoint = waypoints[currentWaypointIndex];
    }
    private void ResetTargetPoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
            currentWaypointIndex = 0;
    }
    public void Attack()
    {
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        Daddy.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Daddy.SetActive(false);
    }
    public int Life => life;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            life--;
        }
        if (other.gameObject.CompareTag("PatrolPoint"))
        {
            waypoints[currentWaypointIndex].gameObject.SetActive(false);
            if (currentWaypointIndex - 1 < 0)
            {
                waypoints[waypoints.Length - 1].gameObject.SetActive(true);
            }
            else
            {
                waypoints[currentWaypointIndex - 1].gameObject.SetActive(true);
            }
            IncreaseWaypontIndex();
        }
    }
}
