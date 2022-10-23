using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class normal_enemy : MonoBehaviour
{
    private RaycastHit2D ground;
    private RaycastHit2D wall;
    private RaycastHit2D fall_check;
    private Vector3 move_velecity;
    private Rigidbody2D rigid;
    public Animator animator;
    public GameObject hpBar;
    private SpriteRenderer SpriteRenderer;
    private Collider2D bound;
    private player_movement player;
    public Image hp_image;
    public Image de_hp_image;
    private bool right_on;
    private bool hurt_on;
    private bool recognize_on;
    private bool on_ground;
    public float ray_height;
    public float ray_width;
    public float hurt_delay;
    public float height;
    public float speed;
    public float hp;
    private float currentHp;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        bound=gameObject.GetComponent<Collider2D>();
        if (!bound) { Debug.Log("xx"); }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movement>();
        StartCoroutine(move_dir());
        hp_image.fillAmount = 1f;
        de_hp_image.fillAmount = 1f;
        currentHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("dead_on"))
        {
            move();
            check();
            recognize();
        }
        checkHp();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11&&isActiveAndEnabled)
        {
            if (!hurt_on)
            {
                hurt_on = true;
                recognize_on = true;
                rigid.velocity = Vector2.zero;
                currentHp -= 10;
                hp_image.fillAmount = currentHp / hp;
                if (currentHp <= 0)
                {
                    rigid.constraints = RigidbodyConstraints2D.FreezeAll;
                    bound.enabled = false;
                   animator.SetBool("dead_on", true);
                    return;
                }
                if (collision.transform.position.x > transform.position.x)
                {
                    rigid.AddForce(new Vector2(-5f, 0), ForceMode2D.Impulse); 
                }
                else
                {
                    rigid.AddForce(new Vector2(5f, 0), ForceMode2D.Impulse);
                }
                StartCoroutine(blink());
            }
        }
    }
    void destroy()
    {
        Destroy(this.gameObject);
    }
    void move()
    {
        if (right_on)
        {
            move_velecity = Vector2.right;
            wall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y-ray_height/3), Vector2.right, ray_width, 1 << 8);
            fall_check = Physics2D.Raycast(new Vector2(transform.position.x + ray_width, transform.position.y - ray_height / 3), Vector2.down, ray_height*2, 1 << 8);
            Debug.DrawRay(new Vector2(transform.position.x + ray_width, transform.position.y - ray_height / 3), Vector2.down * ray_height*2, Color.red);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - ray_height / 3), Vector2.right * ray_width, Color.green);
            transform.localScale = new Vector3(1, 1, 1);
            hpBar.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            move_velecity = Vector2.left;
            wall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - ray_height / 3), Vector2.left, ray_width, 1 << 8);
            fall_check = Physics2D.Raycast(new Vector2(transform.position.x - ray_width, transform.position.y - ray_height / 3), Vector2.down, ray_height*2, 1 << 8);
            Debug.DrawRay(new Vector2(transform.position.x - ray_width, transform.position.y - ray_height / 3), Vector2.down * ray_height*2, Color.red);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - ray_height / 3), Vector2.left * ray_width, Color.green);
            transform.localScale = new Vector3(-1, 1, 1);
            hpBar.transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position += move_velecity * speed * Time.deltaTime;
    }
    void check()
    {
        ground = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y-height/2), Vector2.down, ray_height, 1 << 8);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - height / 2), Vector2.down * ray_height, Color.blue);
       
        if (wall||!fall_check)
        {
            right_on = !right_on;
        }
        if (ground)
        {
            on_ground = true;
        }
        
        if (on_ground)
        {
            if (rigid.velocity.x > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x - 0.3f, 0);
            }
            else if (rigid.velocity.x < 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x + 0.3f, 0);
            }
            if (rigid.velocity.x < 0.05 && rigid.velocity.x > -0.05f)
            {
                rigid.velocity = new Vector2(0, 0);
            }
            transform.position = new Vector2(transform.position.x, ground.point.y + height);
        }

    }
    void recognize()
    {
        if (recognize_on)
        {
            StopCoroutine(move_dir());
            if (player.transform.position.x > transform.position.x)
            {
                right_on = true;
            }
            else
            {
                right_on = false;
            }
        }
    }
    void checkHp()
    {
        
        if (hp_image.fillAmount < de_hp_image.fillAmount)
        {
            de_hp_image.fillAmount -= 0.01f;
        }
    }
    IEnumerator move_dir()
    {
        int i = Random.Range(0, 2);
        if (i == 0) { right_on = true; }
        else { right_on = false; }
        yield return new WaitForSeconds(3);
        StartCoroutine(move_dir());
    }
    IEnumerator blink()
    {
        for(int i = 0; i < 2; i++)
        {
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 0.6f);
            yield return new WaitForSeconds(hurt_delay);
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(hurt_delay);
        }
        hurt_on = false;
    }
    
}
