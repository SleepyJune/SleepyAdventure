using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour {

    public PrefabManager prefabManager;

    public GameObject defaultSquare;

    public int width = 20;
    public int height = 20;


    GameObject selectedObject;
    Level level = new Level();

    int shootableMask;


    // Use this for initialization
    void Start () {

        shootableMask = LayerMask.GetMask("Floor");


        for (int y=0;y< width; y++)
        {
            for(int x=0;x< height; x++)
            {
                var add = new Vector3(x, 0, y);
                Instantiate(defaultSquare, transform.position+add, transform.rotation);
            }
        }
        
        //prefabManager = GetComponent<PrefabManager>();

        foreach(var collection in prefabManager.collections)
        {
            var firstObject = collection.objects[0];

            var newObject = Instantiate(firstObject, transform.position, transform.rotation);

            MonoBehaviour[] comps = newObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in comps)
            {
                c.enabled = false;
            }
            //newObject.GetComponent<Animator>().enabled = true;
            //newObject.GetComponent<Animator>().enabled = true;

            selectedObject = newObject;
        }

	}
	
    Vector3 GetRoundedPosition(Vector3 point)
    {
        return new Vector3(Mathf.Round(point.x), 0, Mathf.Round(point.z));
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, shootableMask))
            {
                Vector3 playerToMouse = hit.point - transform.position;
                playerToMouse.y = 0f;


                Instantiate(selectedObject, GetRoundedPosition(hit.point), new Quaternion());

                Debug.Log("hit");
            }


        }
    }
}
