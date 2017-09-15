using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

using System.Linq;
using System;
using System.Collections.Generic;

public class PlayerMovement : Hero
{
    private Vector3 movement;

    int floorMask;
    int uiMask;

    float camRayLength = 100f;

    int keys = 0;

    bool isWalking;
    bool isCharging;
    bool isKicking;

    GameObject indicatorCubePrefab;
    GameObject indicatorCube;

    GameObject pathHighlightHolder;

    AttackButton attackButton;
    CameraButton cameraButton;

    public Equipment equipment;

    PlayerHealth healthScript;

    CombatUI combatUI;

    void Start()
    {
        Initialize();

        floorMask = LayerMask.GetMask("Floor");
        uiMask = LayerMask.GetMask("UI");

        indicatorCubePrefab = Resources.Load("IndicatorCubeGreen", typeof(GameObject)) as GameObject;

        //attackFrequency = 1 / attackSpeed;
        
        attackButton = GameManager.instance.hud.Find("CombatUI").Find("Panel").Find("AttackButton").GetComponent<AttackButton>();
        cameraButton = GameManager.instance.hud.Find("CameraButton").Find("Panel").Find("CameraButton").GetComponent<CameraButton>();


        healthScript = GetComponent<PlayerHealth>();

        equipment = Inventory.instance.equipment;
        combatUI = equipment.weapon.combatUI;

        GameManager.instance.inputManager.touchStart += GetMoveTo;

        
    }

    void Update()
    {
        HighlightSquare();
        combatUI.Update();
    }

    void FixedUpdate()
    {
        //GetMoveTo();
        Move();
    }

    public void OnChangeWeapon(Weapon newWeapon)
    {
        combatUI = newWeapon.combatUI;
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

                if (path != null)
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

    void HighlightSquare()
    {
        if (Input.mousePresent)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
            {
                var pos = hit.point.ConvertToIPosition();

                if (Pathfinding.GetPathSquare(hit.point) == null)
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

    bool IsPointerOverUI(Touch touch)
    {
        if (touch.fingerId >= 0) {//Application.platform == RuntimePlatform.Android) {

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                //Debug.Log(touch.fingerId + ": " + touch.phase.ToString());
                return true;
            }
            return false;
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }

    private void GetMoveTo(Touch touch)
    {
        if (canMove 
            && !attackButton.isPressed
            && !cameraButton.isPressed
            && !IsPointerOverUI(touch))
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
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
                    if (hit.point.ConvertToIPosition().To2D() != transform.position.ConvertToIPosition().To2D())
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
