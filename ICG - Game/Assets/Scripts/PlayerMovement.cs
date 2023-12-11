using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // FLIP DO PLAYER QUANDO MOVER ESQ/DIR
        if(horizontalInput > 0.01f)
            transform.localScale = new Vector3((float)2.5, (float)2.5, (float)2.5);
        else if(horizontalInput < -0.01f)
            transform.localScale = new Vector3((float)-2.5, (float)2.5, (float)2.5);

        //parêmetros de animação
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // print(onWall());
        // Lógica do pulo na parede
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2( horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else 
                body.gravityScale = 2;

            if(Input.GetKey(KeyCode.Space))
            {
                Jump();
                if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
                    SoundManager.instance.PlaySound(jumpSound);

            }
        }
        else
            wallJumpCooldown += Time.deltaTime; 
            

    }

    private void Jump()
    {   if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
            
        }
        else if (onWall() && !isGrounded())
        {   if(horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 6, 0); //(direção do player com a parede 
                //*força com o que o player se afasta da parede, foça com o que player vai para cima)
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 4); //(direção do player com a parede 
                //*força com o que o player se afasta da parede, foça com o que player vai para cima)
            
            wallJumpCooldown = 0;

        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
