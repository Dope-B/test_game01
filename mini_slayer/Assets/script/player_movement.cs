using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    static private player_movement player;
    private Animator animator;
    private Vector3 move_velecity;
    public Rigidbody2D rigid;
    private SpriteRenderer SpriteRenderer;
    public float speed = 8f;
    public float jump_power = 15f;
    public uint hp = 100; 
    public float hurt_delay;
    public string currentMap;
    public string previousMap;
    private RaycastHit2D ground;
    private RaycastHit2D ground2;
    private RaycastHit2D ground3;
    private RaycastHit2D ground4;
    private RaycastHit2D ground5;
    private RaycastHit2D ground6;
    private RaycastHit2D wall;
    private RaycastHit2D wall2;
    private bool on_ground;
    private bool Inc_on=false;
    private bool dead_on = false;
    private bool disable_attack;
    private bool disable_move;
    private bool disable_jump;
    private bool disable_crouch;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = this;
            DontDestroyOnLoad(this.gameObject);
            animator = gameObject.GetComponent<Animator>();
            rigid = gameObject.GetComponent<Rigidbody2D>();
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        else { Destroy(this.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        move();
        jump();
        crouch();
        attack();
    }

    private void FixedUpdate()
    {
        ground_check();
        fall_check();
        state_check();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (!Inc_on)
            {
                if (!animator.GetBool("hurt_on"))
                {
                    animator.SetBool("hurt_on", true);
                    Inc_on = true;
                    on_ground = false;
                    rigid.velocity = Vector2.zero;
                    if (collision.transform.position.x > transform.position.x)
                    {
                        rigid.AddForce(new Vector2(-5f, 8f), ForceMode2D.Impulse);
                    }
                    else
                    {
                        rigid.AddForce(new Vector2(5f, 8f), ForceMode2D.Impulse);
                    }
                    StartCoroutine(blink());
                }
            }
        }
    }

    void move()
    {
        if (!disable_move)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                wall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.right, 1.2f, 1 << 8);
                wall2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.right, 1f, 1 << 8);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.right * 1.2f, Color.green);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.right * 1f, Color.gray);
                move_velecity = Vector3.right;
                animator.SetBool("moving_on", true);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                wall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.left, 1.2f, 1 << 8);
                wall2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.left, 1f, 1 << 8);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.left * 1.2f, Color.green);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.left * 1f, Color.gray);
                move_velecity = Vector3.left;
                animator.SetBool("moving_on", true);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                move_velecity = Vector3.zero;
                animator.SetBool("moving_on", false);
            }
            if ((!wall || (wall && wall2)))
            {
                transform.position += move_velecity * speed * Time.deltaTime;
            }
        }
    }
    void jump()
    {
        if (!disable_jump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jump_on")&&on_ground)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
                animator.SetBool("jump_on", true);
                on_ground = false;
            }
        }
    }
    void attack()
    {
        if (!disable_attack)
        {
            if (Input.GetKeyDown(KeyCode.Z)&&!animator.GetBool("attack_on"))
            {
                animator.SetBool("attack_on", true);
            }
        }

    }
    void attack_off()
    {
        animator.SetBool("attack_on", false);
    }
    void crouch()
    {
        if (Input.GetKey(KeyCode.DownArrow)&&on_ground)
        {
            if (!animator.GetBool("crouch_on")) { animator.SetBool("crouch_on", true); }
        }
        else
        {
            if (animator.GetBool("crouch_on")) { animator.SetBool("crouch_on", false); }
        }
    }

    void fall_check()
    {
        if (rigid.velocity.y < -20f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -20f);
        }
        
    }

    void ground_check()
    {
        ground = Physics2D.Raycast(new Vector2(transform.position.x + 0.7f, transform.position.y - 0.7f), Vector2.down, 1.4f, 1 << 8);
        ground2 = Physics2D.Raycast(new Vector2(transform.position.x + 0.7f, transform.position.y - 0.7f), Vector2.down, 1f, 1 << 8);
        ground3 = Physics2D.Raycast(new Vector2(transform.position.x - 0.7f, transform.position.y - 0.7f), Vector2.down, 1.4f, 1 << 8);
        ground4 = Physics2D.Raycast(new Vector2(transform.position.x - 0.7f, transform.position.y - 0.7f), Vector2.down, 1f, 1 << 8);
        ground5 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.7f), Vector2.down, 1.5f, 1 << 8);
        ground6 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.7f), Vector2.down, 1f, 1 << 8);
        Debug.DrawRay(new Vector2(transform.position.x + 0.7f, transform.position.y - 0.7f), Vector2.down * 1.4f, Color.cyan);
        Debug.DrawRay(new Vector2(transform.position.x + 0.7f, transform.position.y - 0.7f), Vector2.down * 1.2f, Color.red);
        Debug.DrawRay(new Vector2(transform.position.x - 0.7f, transform.position.y - 0.7f), Vector2.down * 1.4f, Color.cyan);
        Debug.DrawRay(new Vector2(transform.position.x - 0.7f, transform.position.y - 0.7f), Vector2.down * 1.2f, Color.red);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.7f), Vector2.down * 1.5f, Color.blue);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.7f), Vector2.down * 1.2f, Color.red);
        if ((ground && !ground2) || (ground3 && !ground4)||(ground5&&!ground6))
        {
            if (rigid.velocity.y < 0f)
            {
                on_ground = true;
            }
        }
        else if (!ground && !ground3&&!ground5)
        {
            if (!animator.GetBool("jump_on")) { animator.SetBool("fall_on", true); }
            on_ground = false;
        }
        if (on_ground)
        {
            if (animator.GetBool("fall_on")) { animator.SetBool("fall_on", false); }
            else if (animator.GetBool("jump_on")) { animator.SetBool("jump_on", false); }
            if (rigid.velocity.x > 0)
            {
                rigid.velocity= new Vector2(rigid.velocity.x-0.3f, 0);
            }
            else if (rigid.velocity.x < 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x + 0.3f, 0);
            }
            if (rigid.velocity.x < 0.05 && rigid.velocity.x > -0.05f)
            {
                rigid.velocity = new Vector2(0, 0);
            }
            if (ground5)
            {
                transform.position = new Vector3(transform.position.x, ground5.point.y + 1.9f, 0);
            }
            else if (!ground5 && ground)
            {
                transform.position = new Vector3(transform.position.x, ground.point.y + 1.9f, 0);
            }
            else if(!ground5 && ground3){
                transform.position = new Vector3(transform.position.x, ground3.point.y + 1.9f, 0);
            }
            
        }
    }

    void state_check()
    {
        if (animator.GetBool("jump_on") || animator.GetBool("hurt_on"))
        {
            disable_attack = true;
        }
        else { disable_attack = false; }
        if (animator.GetBool("crouch_on") || animator.GetBool("attack_on")||animator.GetBool("hurt_on"))
        {
            disable_move = true;
        }
        else { disable_move = false; }
        if(animator.GetBool("attack_on") || animator.GetBool("hurt_on"))
        {
            disable_jump = true;
        }
        else { disable_jump = false; }
    }

    IEnumerator blink()
    {
        for(int i = 0; i < 3; i++)
        {
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 0.6f);
            yield return new WaitForSeconds(hurt_delay);
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 1f);
            if (i == 1) { animator.SetBool("hurt_on", false); }
            yield return new WaitForSeconds(hurt_delay);
        }
        Inc_on = false;
    }
}
   

