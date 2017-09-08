using UnityEngine;

public class TrapFloor : Trap
{
    [System.NonSerialized]
    public new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider obj)
    {
        var unit = obj.GetComponent<Unit>();
        if(unit is Hero)
        {
            //anim.SetTrigger("Activate");
            rigidbody.isKinematic = false;
                        
            unit.collider.isTrigger = true;
            unit.rigidbody.constraints = RigidbodyConstraints.None;
            unit.anim.SetTrigger("Fall");

            GameManager.instance.GameOver();
            GameManager.instance.SetSceneWithWait("LevelFailed", 2.5f);

            Destroy(gameObject,2);
        }
    }
}
