using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextController : MonoBehaviour {

    public GameObject damageTextPrefab;

    GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
	}
	
	public void CreateDamageText(int damage, Vector3 worldPos)
    {
        Vector2 screenLocation = 
            Camera.main.WorldToScreenPoint(
                GameManager.instance.player.transform.position +
                new Vector3(Random.Range(-.2f, .2f), .5f + Random.Range(-.2f, .2f), Random.Range(-.2f, .2f)));

        var text = Instantiate(damageTextPrefab);
        text.transform.SetParent(canvas.transform, false);
        text.transform.position = screenLocation;
        text.GetComponentInChildren<Text>().text = damage.ToString();

        Destroy(text, 2);

    }
}
