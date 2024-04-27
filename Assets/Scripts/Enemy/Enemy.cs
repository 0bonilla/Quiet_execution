using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;

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
    public Transform currentWayPoint;
    public int currentWaypointIndex = 0; // Index of the current waypoint
    public int index;

    [HideInInspector]
    public bool isWaiting = false; // Flag to indicate if the enemy is currently waiting
    public float waitDuration = 3f; // Duration of wait time in seconds
    public float waiTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        currentWayPoint = waypoints[currentWaypointIndex];
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
        Dictionary<string, float> dict = new Dictionary<string, float>();
        currentWayPoint = waypoints[currentWaypointIndex];
    }
    private void IncreaseWaypontIndex()
    {
        currentWaypointIndex = MyRandoms.RangeRandom(0, waypoints.Length);
        if(index == currentWaypointIndex)
        {
            currentWaypointIndex++;
        }
        if (currentWaypointIndex == waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
        //ResetTargetPoint();
        currentWayPoint = waypoints[currentWaypointIndex];
    }
    //private void ResetTargetPoint()
    //{
    //    if (currentWaypointIndex >= waypoints.Length)
    //        currentWaypointIndex = 0;
    //}
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
            index = currentWaypointIndex;
            IncreaseWaypontIndex();
        }
    }
}
