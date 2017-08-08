using UnityEngine;

public class MonsterLoots : MonoBehaviour
{
    public GameObject[] loots;

    void Start()
    {
        
    }

    public void DropLoots()
    {
        //int spawnPointIndex = Random.Range(0, spawnPoints.Length);        
        
        foreach (var loot in loots)
        {
            Instantiate(loot, transform.position, new Quaternion());
        }        
    }
}