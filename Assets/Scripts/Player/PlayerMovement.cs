using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Linq;

public class PlayerMovement : Unit
{
    public Image joystick;

    public GameObject victoryParticle;

    private Vector3 movement;
    private Vector3 destination;

    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    int keys = 0;

    bool isWalking;
    bool isCharging;
    bool isKicking;

    GameObject indicatorCubePrefab;
    GameObject indicatorCube;
    
    GameObject pathHighlightHolder;

    void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

        indicatorCubePrefab = Resources.Load("IndicatorCubeGreen", typeof(GameObject)) as GameObject;
    }

    void FixedUpdate()
    {
        GetMoveTo();
        Move();

        HighlightSquare();
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            collision.gameObject.transform.GetComponent<Collider>().isTrigger = false;

            GameObject.Destroy(collision.gameObject, 0.25f);
            keys += 1;

            /*if (keys >= 2 && gameObject.GetComponent<PlayerHealth>().currentHealth > 0)
            {
                Instantiate(victoryParticle, collision.gameObject.transform);
                gameObject.GetComponent<PlayerHealth>().currentHealth = 0;
            }*/
        }
        else if (collision.gameObject.tag == "Door")
        {
            if (keys > 0)
            {
                GameObject.Destroy(collision.gameObject, 0.25f);
            }
        }
        else if (collision.gameObject.tag == "Goal")
        {
            SceneManager.LoadScene("LevelComplete");
        }
    }

    Vector3 VectorTo2D(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
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
                    destination = next.ToVector();
                }
            }

        }
        else
        {
            path = null;
            Destroy(pathHighlightHolder);
        }

        if (destination != Vector3.zero)
        {
            float distance = Vector3.Distance(VectorTo2D(transform.position), VectorTo2D(destination));

            if (distance > 0.05)
            {
                Vector3 dir = (destination - transform.position).normalized;
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
                }
                isWalking = true;

                Quaternion newRotation = Quaternion.LookRotation(dir);
                playerRigidbody.MoveRotation(newRotation);

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

    private void GetMoveTo()
    {
        if (Input.GetButton("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
            {
                var end = hit.point.ConvertToIPosition().To2D().ToVector();

                //path = Pathfinding.GetShortestPath(transform.position, end);

                path = GameManager.instance.UnitMoveTo(transform.position, end);

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

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
