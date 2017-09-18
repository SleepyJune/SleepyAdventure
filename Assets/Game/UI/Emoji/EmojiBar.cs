using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiBar : MonoBehaviour
{
    [System.NonSerialized]
    public Animator anim;

    GameObject canvas;

    [System.NonSerialized]
    public Monster unit;

    float lastSize;
    
    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas/EmojiBarHolder");
        anim = transform.GetComponentInChildren<Animator>();

        lastSize = unit.GetBoundsSize();
    }

    void Update()
    {
        if (unit.isDead)
        {
            Destroy(gameObject);
            return;
        }

        UpdateBarLocation();
    }


    public void UpdateBarLocation()
    {
        Vector2 screenLocation =
            Camera.main.WorldToScreenPoint(
                unit.emojiBarTransform.position);

        //var currentSize = unit.GetBoundsSize();
        var sizeRatio = unit.GetRelativeSizeRatio();

        transform.position = screenLocation;
        transform.localScale = new Vector3(sizeRatio, sizeRatio, sizeRatio);

        //lastSize = currentSize;

    }
}
