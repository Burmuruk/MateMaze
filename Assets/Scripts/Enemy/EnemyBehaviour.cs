using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float speed;
    public Transform[] wayPoints;
    private int wpIndex = 0;

    public Transform target;
    public GameObject bulletPrefab;
    
    public enum State {patrolling, seeking, attacking };
    public State enemyState;

    private float seekRange, attackRange;

    void Start()
    {
        enemyState = State.patrolling;
        seekRange = 0.5f * transform.Find("SeekRange").localScale.x;
        attackRange = 0.5f * transform.Find("AttackRange").localScale.x;
    }

    void Update()
    {

        //ChangeState();

        if (enemyState == State.patrolling)
            Patroll();

        if (enemyState == State.seeking)
            Seek();

        if (enemyState == State.attacking)
            Attack();
        
    }

    private void GoTo(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float dt = Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction.normalized, transform.up);
        transform.Translate(transform.forward * dt * speed, Space.World);
       
    }

    private void Patroll()
    {
        Vector3 targetPosition = wayPoints[wpIndex].position;
        GoTo(targetPosition);
        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance < 0.5f)
        {
            wpIndex++;
            if (wpIndex == wayPoints.Length - 1) { wpIndex = 0; }
        }
    }

    private void Attack()
    {
        if (GameObject.FindWithTag("EnemyAttack") == null)
        {
            Vector3 direction = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction.normalized, transform.up);
            GameObject gO = Instantiate(bulletPrefab, transform.position, transform.rotation);
            gO.GetComponent<Rigidbody>().velocity = 50f * transform.forward;
            Destroy(gO, 2f);
        }
    }

    private void Seek()
    {
        Vector3 targetPosition = target.position;
        GoTo(targetPosition);
    }

    
}

