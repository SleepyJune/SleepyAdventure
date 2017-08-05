using UnityEngine;

public class MonsterLoots : MonoBehaviour
{
    public GameObject[] loots;

    void Start()
    {
        
    }

    void DropLoots()
    {
        //int spawnPointIndex = Random.Range(0, spawnPoints.Length);        
        
        foreach (var loot in loots)
        {
            Instantiate(loot, transform);
        }        
    }
}