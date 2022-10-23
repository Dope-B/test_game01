using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeleton : MonoBehaviour

{
    Animator animator;
    normal_enemy enemy;
    Rigidbody2D rigid;
    public GameObject canvas;
    // Start is called before the first frame update
    private void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();
        enemy = gameObject.GetComponentInParent<normal_enemy>();
        rigid = gameObject.transform.parent.GetComponentInParent<Rigidbody2D>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10&&!animator.GetBool("alive_on"))
        {
            animator.enabled = true;
            StartCoroutine(alive());
        }
    }
    IEnumerator alive()
    {
        animator.SetBool("alive_on", true);
        yield return new WaitForSeconds(0.7f);
        rigid.bodyType = RigidbodyType2D.Dynamic;
        enemy.enabled = true;
        canvas.gameObject.SetActive(true);
    }

}
