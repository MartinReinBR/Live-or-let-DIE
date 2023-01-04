using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentNumber = 0;

    public float rayLength;

    [SerializeField] private Renderer rend;
    [SerializeField] private MoveDice moveDice;
    [SerializeField] private Pathfinding pathfinding;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private GameObject rollParticles;


    public bool isFighting;
    private float waitAfterPlayerTurn;

    public bool isDead;

    private List<Vector3> path;


    private void Start()
    {
        isFighting = false;
        rayLength = 1f;
        currentNumber = 2;
        audioManager = GetComponent<AudioManager>();

        waitAfterPlayerTurn = 0.5f;
        isDead = false;
    }

    private void Update()
    {
        if (isDead)
            return;

        CastRays();

        if (StateManager.instance.state != State.EnemyTurn)
            return;

        if (isFighting)
        {
            StateManager.instance.TurnDone(this);
            return;
        }

        waitAfterPlayerTurn -= Time.deltaTime;
        if(waitAfterPlayerTurn > 0)
            return;
        waitAfterPlayerTurn = 0.5f;



        if (!moveDice.isRolling && !StateManager.instance.IsTurnDone(this))
        {
            path = pathfinding.AStar(transform.position, playerObject.transform.position);
            if(path != null && path.Count > 1)
            {
                // ---DEBUG---
                Vector3 lastPoint = path[0];
                foreach (Vector3 point in path)
                {
                    Debug.DrawLine(lastPoint, point, Color.green, 1.0f, false);
                    lastPoint = point;
                }
                //------------

                Vector3 direction = Vector3.Normalize(path[1] - path[0]);
                
                moveDice.MoveTheDice(direction, 
                () => {
                    audioManager.PlayRollSound();
                    StateManager.instance.TurnDone(this);
                    
                    GameObject psObject = Instantiate(rollParticles, path[1] + Vector3.up * 0.6f, rollParticles.transform.rotation);
                    ParticleSystem ps = psObject.GetComponent<ParticleSystem>();
                    Destroy(psObject, ps.main.duration + ps.main.startLifetime.constant);
                }, 
                (float progress) => {
                    return Mathf.Clamp(1f - progress, 0.3f, 1.0f); 
                });
            
            }
            else if(path == null)
            {
                // no path found, could not move
                StateManager.instance.TurnDone(this);
            }

        }


    }

    public void CastRays()
    {
        RaycastHit hit1, hit2, hit3, hit4, hit5, hit6;
        Ray ray1 = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        Ray ray2 = new Ray(transform.position, transform.TransformDirection(Vector3.back));
        Ray ray3 = new Ray(transform.position, transform.TransformDirection(Vector3.up));
        Ray ray4 = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        Ray ray5 = new Ray(transform.position, transform.TransformDirection(Vector3.left));
        Ray ray6 = new Ray(transform.position, transform.TransformDirection(Vector3.right));

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayLength, Color.yellow);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * rayLength, Color.blue);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * rayLength, Color.green);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * rayLength, Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * rayLength, Color.black);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * rayLength, Color.white);

        if (Physics.Raycast(ray1, out hit1, rayLength))
        {
            if (hit1.collider.tag == "Ground")
            {
                currentNumber = 3;
            }
        }

        if (Physics.Raycast(ray2, out hit2, rayLength))
        {
            if (hit2.collider.tag == "Ground")
            {
                currentNumber = 4;
            }
        }


        if (Physics.Raycast(ray3, out hit3, rayLength))
        {
            if (hit3.collider.tag == "Ground")
            {
                currentNumber = 5;
            }
        }

        if (Physics.Raycast(ray4, out hit4, rayLength))
        {
            if (hit4.collider.tag == "Ground")
            {
                currentNumber = 2;
            }
        }

        if (Physics.Raycast(ray5, out hit5, rayLength))
        {
            if (hit5.collider.tag == "Ground")
            {
                currentNumber = 1;
            }
        }

        if (Physics.Raycast(ray6, out hit6, rayLength))
        {
            if (hit6.collider.tag == "Ground")
            {
                currentNumber = 6;
            }
        }

    }
}
