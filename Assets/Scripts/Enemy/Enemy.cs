using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] float speed;
    public bool ACooldown;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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

    public void Attack()
    {
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        print("Hola");
        ACooldown = false;
        yield return new WaitForSeconds(0.2f);
        ACooldown = true;
        print("Chau");
    }
    public int Life => life;
}
