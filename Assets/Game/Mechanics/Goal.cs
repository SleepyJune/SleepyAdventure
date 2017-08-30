using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Animator>().SetTrigger("Victory");
            GameManager.instance.SetSceneWithWait("LevelComplete", 2);
            this.transform.gameObject.SetActive(false);
        }
    }
}
