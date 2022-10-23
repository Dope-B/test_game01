using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeManager :MonoBehaviour
{
    private static fadeManager FadeManager;
    public SpriteRenderer sprite;
    private Color color;
    private WaitForSecondsRealtime wait;
    public bool fadeDone;

    private void Start()
    {
        if (FadeManager == null)
        {
            wait = new WaitForSecondsRealtime(0.002f);
            FadeManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void fadeOut(float speed=0.4f)
    {
        StopAllCoroutines();
        StartCoroutine(CfadeOut(speed));
    }
    public void fadeIn(float speed=0.4f)
    {
        StopAllCoroutines();
        StartCoroutine(CfadeIn(speed));
    }
    IEnumerator CfadeOut(float speed)
    {
        fadeDone = false;
        color = sprite.color;
        while (sprite.color.a <1f)
        {
            color.a += speed;
            sprite.color = color;
            yield return wait;
        }
        fadeDone = true;
    }
    IEnumerator CfadeIn(float speed)
    {
        fadeDone = false;
        color = sprite.color;
        while (sprite.color.a > 0f)
        {
            color.a -= speed;
            sprite.color = color;
            yield return wait;
        }
        fadeDone = true;
    }
}
