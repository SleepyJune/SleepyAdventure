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

    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas/EmojiBarHolder");
        anim = transform.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        UpdateBarLocation();
    }

    public void UpdateBarLocation()
    {
        Vector2 screenLocation =
            Camera.main.WorldToScreenPoint(
                unit.emojiBarTransform.position);

        var size = unit.GetPixelSize() / 30;
              
        transform.position = screenLocation;
        transform.localScale = new Vector3(size, size, size);

    }
}
