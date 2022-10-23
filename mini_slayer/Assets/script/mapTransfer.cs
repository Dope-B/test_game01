using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapTransfer : MonoBehaviour
{
    public string depart;
    public string dest;
    public player_movement player;
    public fadeManager FadeManager;
    public camera cam;

    // Start is called before the first frame update
    void Start()
    {
        if (player.currentMap == depart && player.previousMap == dest)
        {
            player.rigid.velocity = Vector2.zero;
            player.transform.position = this.transform.position;
            cam.transform.position = player.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            StartCoroutine(mapTrans());
        }
    }
    IEnumerator mapTrans()
    {
        Time.timeScale = 0f;
        FadeManager.fadeOut();
        yield return new WaitUntil(() => FadeManager.fadeDone);
        SceneManager.LoadScene(dest);
        player.currentMap = dest;
        player.previousMap = depart;
        FadeManager.fadeIn();
        Time.timeScale = 1f;
    }
}
