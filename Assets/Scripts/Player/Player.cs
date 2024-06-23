using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerModel
{
    public float speed;
    [SerializeField] int life;
    [SerializeField] float TotalCooldown;
    public float AttackCooldown;
    Rigidbody _rb;
    [SerializeField] GameObject Daddy;
    public bool dead;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }
    public void LookDir()
    {     
        //// Utilizar la posicion del mouse como rotación
        //Vector3 mousePos = Input.mousePosition;
        //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y - transform.position.y));
        //Vector3 lookDirection = mousePos - transform.position;
        //lookDirection.y = 0; 
        //lookDirection.Normalize();
        //Quaternion rotation = Quaternion.LookRotation(lookDirection);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }
    public void Attack()
    {
        //Metodo de ataque
        if(AttackCooldown > TotalCooldown)
            StartCoroutine(PUM());
    }
    private IEnumerator PUM()
    {
        Daddy.SetActive(true);
        AttackCooldown = 0;
        yield return new WaitForSeconds(0.3f);
        Daddy.SetActive(false);
    }
    public int Life => life;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            dead = true;
            life--;
        }
    }
}
