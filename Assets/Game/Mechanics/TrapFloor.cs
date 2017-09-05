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

            unit.Stop();
            unit.collider.isTrigger = true;
            unit.rigidbody.constraints = RigidbodyConstraints.None;
            Destroy(gameObject,2);
        }
    }
}
