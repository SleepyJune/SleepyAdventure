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

    void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();

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
        attackButton = GameManager.instance.hud.Find("CombatUI").Find("AttackButton").GetComponent<Button>();
        attackButton.onClick.AddListener(Attack);
    }

    void FixedUpdate()
    {
        GetMoveTo();
        Move();
        HighlightSquare();
        OnAnimation();
    }

    void OnAnimation()
    {
        if (Input.GetKey("space"))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (Time.time - lastAttack > attackFrequency)
        {
            var enemies = GameManager.instance.units.Values.Where(u => u is Monster);

            foreach (var enemy in enemies)
            {
                if (enemy.transform.position.ConvertToIPosition().To2D()
                    .Distance(transform.position.ConvertToIPosition().To2D()) < 2)
                {
                    var dir = enemy.transform.position - transform.position;
                    dir.y = 0;

                    enemy.transform.GetComponent<Rigidbody>().AddForce(1000 * dir);
                }
            }

            anim.SetTrigger("Punch");
            lastAttack = Time.time;
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

                Quaternion newRotation = Quaternion.LookRotation(dir);
                rb.MoveRotation(newRotation);
            }
            else
            {
                isWalking = false;
                anim.SetFloat("Speed", 0);
            }
        }
    }

    public void LookAt(Unit source)
    {
        Vector3 dir = (source.transform.position - transform.position).normalized;
        dir.y = 0;

        Quaternion newRotation = Quaternion.LookRotation(dir);
        rb.MoveRotation(newRotation);

        var rot = source.transform.rotation;
        //transform.rotation = Quaternion.Euler(new Vector3(rot.x, rot.y + 180, rot.z));
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
        if (canMove && Input.GetButton("Fire1"))
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

                //transform.position = hit.point;
                //Instantiate(mouseHitParticle, hit.transform);

                //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
                //Debug.Log(hit);
            }
        }
    }

}
