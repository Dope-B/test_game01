using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    static private camera this_cam;
    public player_movement player;
    public BoxCollider2D box;
    private Camera cam;
    private Vector3 minBound;
    private Vector3 maxBound;
    private Vector3 target;
    private float speed=1.5f;
    private float clampX;
    private float clampY;
    private float halfWidth;
    private float halfHeight;

    // Start is called before the first frame update
    void Start()
    {
        if (this_cam == null)
        {
            this_cam = this;
            cam = gameObject.GetComponent<Camera>();
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * Screen.width / Screen.height;
            minBound = box.bounds.min;
            maxBound = box.bounds.max;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            target.Set(player.transform.position.x, player.transform.position.y-1f, this.transform.position.z);
            transform.position = Vector3.Lerp(this.transform.position, target, speed * Time.deltaTime);
            if (box.bounds.size.x < halfWidth * 2) { clampX = box.bounds.size.x / 2; Debug.Log("xx"); }
            else { clampX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth); }
            if (box.bounds.size.y < halfHeight * 2) { clampY = box.bounds.size.y / 2; Debug.Log("yy"); }
            else { clampY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight); }
            transform.position = new Vector3(clampX, clampY, transform.position.z);
        }
    }
    void setBox(BoxCollider2D coll)
    {
        this.box = coll;
        minBound = coll.bounds.min;
        maxBound = coll.bounds.max;
    }
}
