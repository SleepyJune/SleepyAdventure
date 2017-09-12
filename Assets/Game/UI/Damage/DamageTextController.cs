using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextController : MonoBehaviour {

    public GameObject damageTextPrefab;

    GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas/DamageTextHolder");
	}
	
	public void CreateDamageText(Unit unit, int damage)
    {
        Vector2 screenLocation = 
            Camera.main.WorldToScreenPoint(
                unit.transform.position +
                new Vector3(Random.Range(-.2f, .2f), .5f + Random.Range(-.2f, .2f), Random.Range(-.2f, .2f)));

        var size = unit.GetPixelSize()/30;
        
        var text = Instantiate(damageTextPrefab);
        text.transform.SetParent(canvas.transform, false);
        text.transform.position = screenLocation;
        text.GetComponentInChildren<Text>().text = damage.ToString();

        text.transform.localScale = Vector3.Scale(text.transform.localScale, new Vector3(size, size, size));

        Destroy(text, 2);

    }
}
