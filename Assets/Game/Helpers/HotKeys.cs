using UnityEngine;

class HotKeys : MonoBehaviour
{

    bool inventoryKeyPress = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Inventory.instance.Save();
            GetComponent<SceneChanger>().OnLoadButtonPressed("IntroScreen");
        }

        if (GameManager.instance.player && !GameManager.instance.player.isDead)
        {
            if (Input.GetKeyDown(KeyCode.I) && !inventoryKeyPress)
            {
                GameManager.instance.OnOpenInventory();
                inventoryKeyPress = true;
            }

            if (Input.GetKeyUp(KeyCode.I))
            {
                inventoryKeyPress = false;
            }
        }
    }
}
