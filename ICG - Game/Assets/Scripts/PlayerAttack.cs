using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{   [SerializeField] private float attackCooldown;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    public Transform attackPoint;
    [Header ("Attack Range")]
    public float attackRange;
    public LayerMask enemyLayers;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();
        
        cooldownTimer += Time.deltaTime;


    }
    private void Attack()
    {   
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(1);
        }
                
    }

    private void OnDrawGizmos()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
