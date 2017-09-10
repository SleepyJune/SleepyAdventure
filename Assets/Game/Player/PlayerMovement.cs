using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

using System.Linq;

public class PlayerMovement : Hero
{
    private Vector3 movement;

    int floorMask;
    float camRayLength = 100f;

    int keys = 0;

    bool isWalking;
    bool isCharging;
    bool isKicking;

    GameObject indicatorCubePrefab;
    GameObject indicatorCube;

    GameObject pathHighlightHolder;

    Button attackButton;

    public Equipment equipment;

    PlayerHealth healthScript;

    bool cleaverEquiped = false;

    void Start()
    {
        Initialize();

        floorMask = LayerMask.GetMask("Floor");
        indicatorCubePrefab = Resources.Load("IndicatorCubeGreen", typeof(GameObject)) as GameObject;

        //attackFrequency = 1 / attackSpeed;

        /*
#if UNITY_EDITOR
        Debug.Log("Unity Editor");
#elif UNITY_ANDROID
        Debug.Log("Unity Editor");
#elif UNITY_IOS
    Debug.Log("Unity iPhone");
#else
    Debug.Log("Any other platform");
#endif
*/
        attackButton = GameManager.instance.hud.Find("CombatUI").Find("Panel").Find("AttackButton").GetComponent<Button>();
        attackButton.onClick.AddListener(Attack);

        healthScript = GetComponent<PlayerHealth>();

        equipment = Inventory.instance.equipment;
    }

    void FixedUpdate()
    {
        GetMoveTo();
        Move();
        HighlightSquare();
        OnKeyPress();
    }

    void OnKeyPress()
    {
        if (Input.GetKey("space"))
        {
            Attack();            
        }
    }

    public void OnChangeWeapon()
    {
        cleaverEquiped = !cleaverEquiped;
    }

    public void Attack()
    {
        if (equipment.weapon)
        {
            equipment.weapon.Attack(this);
        }
        else
        {
            AttackPattern1();
        }
    }

    public void AttackPattern1()
    {
        if (GameManager.time - lastAttack > attackFrequency)
        {
            var enemies = GameManager.instance.units.Values.Where(u => u is Monster);

            foreach (Monster enemy in enemies)
            {
                if (enemy.transform.position.ConvertToIPosition().To2D()
                    .Distance(transform.position.ConvertToIPosition().To2D()) < 2)
                {
                    var dir = enemy.transform.position - transform.position;
                    dir.y = 0;

                    enemy.transform.GetComponent<Rigidbody>().AddForce(1000 * dir);

                    enemy.TakeDamage(this, 100);

                }
            }

            anim.SetTrigger("Punch");
            lastAttack = GameManager.time;
        }
    }

    void Move()
    {
        if (path != null && path.points.Count > 0)
        {
            var next = path.points.First();

            if (next != null)
            {
                if (transform.position.ConvertToIPosition().To2D() == next)
                {
                    path.points.Remove(next);

                    if (path.points.Count > 0)
                    {
                        next = path.points.First();
                        if (next != null)
                        {
                            nextPos = next;
                        }
                    }
                }
                else
                {
                    nextPos = next;
                }
            }

        }
        else
        {
            path = null;
            Destroy(pathHighlightHolder);
        }

        if (nextPos != IPosition.zero)
        {
            float distance = Vector3.Distance(transform.position.To2D(), nextPos.ToVector());

            if (distance > 0.05)
            {
                Vector3 dir = (nextPos.ToVector() - transform.position.To2D()).normalized;
                dir.y = 0;

                if (distance >= .1)
                {
                    transform.position += dir * speed * Time.deltaTime;

                    anim.SetFloat("Speed", speed * Time.deltaTime);                    
                }
                else
                {
                    transform.position = new Vector3(0, transform.position.y, 0)
                            + transform.position.ConvertToIPosition().To2D().ToVector();

                    //playerRigidbody.velocity = Vector3.zero;
                    //playerRigidbody.angularVelocity = Vector3.zero;
                }
                isWalking = true;

                if(path != null)
                {
                    LookAt(nextPos.ToVector());
                }

            }
            else
            {
                isWalking = false;
                anim.SetFloat("Speed", 0);
            }
        }
    }

    bool testTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2)
            {
                return true;
            }
        }
        return false;
    }

    void HighlightSquare()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
        {
            var pos = hit.point.ConvertToIPosition();

            if(Pathfinding.GetPathSquare(hit.point) == null)
            {
                return;
            }

            if (indicatorCube == null)
            {
                indicatorCube = Instantiate(indicatorCubePrefab, new Vector3(pos.x, 0, pos.z), Quaternion.identity);
            }
            else if (indicatorCube.transform.position.ConvertToIPosition() != pos)
            {
                Destroy(indicatorCube);
                indicatorCube = Instantiate(indicatorCubePrefab, new Vector3(pos.x, 0, pos.z), Quaternion.identity);
            }
        }
        else
        {
            if (indicatorCube != null)
            {
                Destroy(indicatorCube);
            }
        }
    }

    void GeneratePathHighlight()
    {
        if (path != null)
        {
            if (pathHighlightHolder != null)
            {
                Destroy(pathHighlightHolder);
            }

            pathHighlightHolder = new GameObject();

            foreach (var pos in path.points)
            {
                Instantiate(indicatorCubePrefab, new Vector3(pos.x, 0, pos.z), Quaternion.identity, pathHighlightHolder.transform);
            }

        }
        else
        {
            if (pathHighlightHolder != null)
            {
                Destroy(pathHighlightHolder);
            }
        }
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pData = (PointerEventData)data;
        var end = pData.pointerCurrentRaycast.worldPosition.ConvertToIPosition().To2D().ToVector();
    }

    private void GetMoveTo()
    {
        if (canMove && EventSystem.current.IsPointerOverGameObject() == false && Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
            {
                var end = hit.point.ConvertToIPosition().To2D().ToVector();

                //check if current path is the same
                if (path != null && path.end == end.ConvertToIPosition())
                {
                    return;
                }


                path = GameManager.instance.UnitMoveTo(this, transform.position, end);

                if (path != null)
                {
                    GeneratePathHighlight();
                }
                else
                {
                    if(hit.point.ConvertToIPosition().To2D() != transform.position.ConvertToIPosition().To2D())
                    {
                        LookAt(hit.point);
                    }
                }

                //transform.position = hit.point;
                //Instantiate(mouseHitParticle, hit.transform);

                //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
                //Debug.Log(hit);
            }
        }
    }

    public override void TakeDamage(Unit source, int amount)
    {
        healthScript.TakeDamage(source, amount);
    }

}
