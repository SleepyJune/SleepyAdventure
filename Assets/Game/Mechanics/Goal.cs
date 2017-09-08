using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : Trap {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Animator>().SetTrigger("Victory");
            GameManager.instance.GameOver();
            GameManager.instance.SetSceneWithWait("LevelComplete", 2);

            this.transform.gameObject.SetActive(false);
        }
    }
}
