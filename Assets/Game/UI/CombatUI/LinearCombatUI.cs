using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearCombatUI : CombatUI
{
    public GameObject linePrefab;

    LineRenderer line;

    Vector3 mouseStartPosion;

    // Use this for initialization
    public override void Initialize()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        var player = GameManager.instance.player;

        if (player)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                line.SetPosition(0, player.transform.position);

                mouseStartPosion = Input.mousePosition;
            }

            if (Input.GetKey(KeyCode.F))
            {

                //trail.SetActive(true);

                line.SetPosition(0, player.transform.position);

                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //var distance = Camera.main.transform.position.y - player.transform.position.y;
                //Vector3 pos = ray.GetPoint(Camera.main.transform.position.y);                

                Vector3 diff2d = (Input.mousePosition - mouseStartPosion).normalized;
                Vector3 diff = new Vector3(diff2d.x, 0, diff2d.y);

                if(diff == Vector3.zero)
                {
                    diff = player.transform.forward;
                }

                //Debug.Log(diff);

                Vector3 pos = player.transform.position + diff * 2;


                line.SetPosition(1, pos);
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                Inventory.instance.equipment.weapon.Attack(
                    GameManager.instance.player,
                    GameManager.instance.player.transform.position,
                    line.GetPosition(1));
                Destroy(line.gameObject);
            }
        }
    }
}
