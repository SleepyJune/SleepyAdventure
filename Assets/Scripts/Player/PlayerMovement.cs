using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
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

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

        indicatorCubePrefab = Resources.Load("IndicatorCubeGreen", typeof(GameObject)) as GameObject;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        var vec = new Vector3(h, 0, v);

        //playerRigidbody.AddForce(vec * 1000);
        //Turning ();
        //Animating(h, v);

        GetMoveTo();
        Move(h, v);

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

    void Move(float h, float v)
    {
        if (destination != null)
        {
            float distance = Vector3.Distance(VectorTo2D(transform.position), VectorTo2D(destination));

            if (distance > 0.01)
            {
                Vector3 dir = (destination - transform.position).normalized;
                dir.y = 0;

                transform.position += dir * speed * Time.deltaTime;
                isWalking = true;
            }
            else
            {
                isWalking = false;
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
            if(indicatorCube != null)
            {
                Destroy(indicatorCube);
            }
        }
    }

    private void GetMoveTo()
    {
        if (Input.GetButton("Fire2") || Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
            {
                Vector3 playerToMouse = hit.point - transform.position;
                playerToMouse.y = 0f;

                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                playerRigidbody.MoveRotation(newRotation);

                if (Input.GetButton("Fire2"))
                {
                    destination = hit.point.ConvertToIPosition().To2D().ToVector();
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
