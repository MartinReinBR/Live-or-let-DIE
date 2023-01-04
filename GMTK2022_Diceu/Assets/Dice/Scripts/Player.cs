using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int currentNumber = 0;

    private float rayLength;

    public bool isRolling;
    public bool isFighting;

    [SerializeField] private MoveDice MoveDice;

    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerAudioManager audioManager;

    [SerializeField] private GameObject dustPrefab;

    public float explosionForce;
    public bool isDead;
    private float deathTimer;

    private void Start()
    {
        rayLength = 1f;
        isDead = false;
        deathTimer = 0.0f;
        audioManager = GetComponent<PlayerAudioManager>();
    }

    private void Update()
    {
        if(isDead)
        {
            deathTimer += Time.deltaTime;
            if(deathTimer > 2f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        CastRays();


        if (StateManager.instance.state != State.PlayerTurn && !PauseMenu.isPaused || isFighting)
            return;
        


        Vector3 direction = new Vector3(
            UtilityFunctions.BoolToInt(Input.GetKeyDown(KeyCode.D)) - UtilityFunctions.BoolToInt(Input.GetKeyDown(KeyCode.A)),
            0,
            UtilityFunctions.BoolToInt(Input.GetKeyDown(KeyCode.W)) - UtilityFunctions.BoolToInt(Input.GetKeyDown(KeyCode.S)));
        direction = direction.normalized;

        if(direction.magnitude > 0)
        {

            direction = cameraController.flatRotation * direction;

            Vector3 pos = transform.position + direction;
            Action whenDone = () => {
                audioManager.PlayRollSound();

                pos = transform.position;
                GameObject psObject = Instantiate(dustPrefab, pos + Vector3.down * 0.4f, dustPrefab.transform.rotation);
                ParticleSystem ps = psObject.GetComponent<ParticleSystem>();
                Destroy(psObject, ps.main.duration + ps.main.startLifetime.constant);
                
                StateManager.instance.TurnDone(this);
            };
            Vector3 flattenedPos = pos;
            flattenedPos.y = 0;
            if(Pathfinding.IsGround(flattenedPos))
            {
                MoveDice.MoveTheDice(direction, whenDone);
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

            if (hit1.collider.tag == "Enemy" && !MoveDice.isRolling && !hit1.transform.gameObject.GetComponent<MoveDice>().isRolling)
            {
                FightEnemy(hit1.transform.gameObject);
            }
        }

        if (Physics.Raycast(ray2, out hit2, rayLength))
        {
            if (hit2.collider.tag == "Ground")
            {
                currentNumber = 4;
            }

            if (hit2.collider.tag == "Enemy" && !MoveDice.isRolling && !hit2.transform.gameObject.GetComponent<MoveDice>().isRolling)
            {
                FightEnemy(hit2.transform.gameObject);
            }
        }


        if (Physics.Raycast(ray3, out hit3, rayLength))
        {
            if (hit3.collider.tag == "Ground")
            {
                currentNumber = 5;
            }

            if (hit3.collider.tag == "Enemy" && !MoveDice.isRolling && !hit3.transform.gameObject.GetComponent<MoveDice>().isRolling)
            {
                FightEnemy(hit3.transform.gameObject);
            }
        }

        if (Physics.Raycast(ray4, out hit4, rayLength))
        {
            if (hit4.collider.tag == "Ground")
            {
                currentNumber = 2;
            }

            if (hit4.collider.tag == "Enemy" && !MoveDice.isRolling && !hit4.transform.gameObject.GetComponent<MoveDice>().isRolling)
            {
                FightEnemy(hit4.transform.gameObject);
            }
        }

        if (Physics.Raycast(ray5, out hit5, rayLength))
        {
            if (hit5.collider.tag == "Ground")
            {
                currentNumber = 1;
            }

            if (hit5.collider.tag == "Enemy" && !MoveDice.isRolling && !hit5.transform.gameObject.GetComponent<MoveDice>().isRolling)
            {
                FightEnemy(hit5.transform.gameObject);
            }
        }

        if (Physics.Raycast(ray6, out hit6, rayLength))
        {
            if (hit6.collider.tag == "Ground")
            {
                currentNumber = 6;
            }

            if (hit6.collider.tag == "Enemy" && !MoveDice.isRolling && !hit6.transform.gameObject.GetComponent<MoveDice>().isRolling)
            {
                FightEnemy(hit6.transform.gameObject);
            }
        }

    }

    public void FightEnemy(GameObject currentEnemy)
    {
        if (!currentEnemy.gameObject.GetComponent<Enemy>().isFighting)
        {
            StartCoroutine(WaitForFight(currentEnemy));
        }
    }
    IEnumerator WaitForFight(GameObject currentEnemy)
    {
        isFighting = true;
        currentEnemy.gameObject.GetComponent<Enemy>().isFighting = true;

        yield return new WaitForSeconds(1);


        if (currentEnemy.GetComponent<Enemy>().currentNumber < currentNumber)
        {
            Debug.Log("WIN!");

            Rigidbody rb;
            if (!currentEnemy.TryGetComponent(out rb))
            {
                rb = currentEnemy.AddComponent<Rigidbody>();
            }
            currentEnemy.tag = "Untagged";
            rb.mass = 1;

            Vector3 direction = (currentEnemy.transform.position - transform.position);
            Vector3 center = transform.position + direction / 2f;
            float diff = currentNumber - currentEnemy.GetComponent<Enemy>().currentNumber;
            rb.AddExplosionForce(explosionForce, center, 1, 0.4f);
            direction = Vector3.Cross(direction, Vector3.up);
            rb.AddTorque(direction * rb.mass * 50 * diff, ForceMode.Impulse);
            Destroy(currentEnemy, 3);
            
            audioManager.PlayWinSound();

            StateManager.instance.RemoveEnemy(currentEnemy.GetComponent<Enemy>());
            currentEnemy.GetComponent<Enemy>().isDead = true;
        }

        else if (currentEnemy.GetComponent<Enemy>().currentNumber > currentNumber)
        {
            Debug.Log("You Lose!");

            Rigidbody rb;
            if (!gameObject.TryGetComponent(out rb))
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            tag = "Untagged";
            rb.mass = 1;

            rb.useGravity = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = false;

            Vector3 direction = (currentEnemy.transform.position - transform.position);
            Vector3 center = transform.position + direction / 2f;
            float diff = currentEnemy.GetComponent<Enemy>().currentNumber - currentNumber;
            rb.AddExplosionForce(explosionForce, center, 1, 0.4f);
            direction = Vector3.Cross(direction, Vector3.up);
            rb.AddTorque(direction * rb.mass * 50 * diff, ForceMode.Impulse);
            cameraController.isFrozen = true;
            isDead = true;

            audioManager.PlayWinSound();

        }

        else
        {
            Debug.Log("Tie");
            Vector3 direction = (transform.position - currentEnemy.transform.position).normalized;
            Vector3 playerTarget = transform.position + direction;
            Vector3 enemyTarget = currentEnemy.transform.position - direction;

            audioManager.PlayDrawSound();
            
            // Push player
            Vector3 flattened = playerTarget;
            flattened.y = 0;
            bool groundBehindPlayer = Pathfinding.IsGround(flattened);
            
            // Push enemy
            flattened = enemyTarget;
            flattened.y = 0;
            bool groundBehingEnemy = Pathfinding.IsGround(flattened);
            if(groundBehindPlayer && groundBehingEnemy)
            {
                StartCoroutine(MoveDice.PushCube(playerTarget));
                yield return currentEnemy.GetComponent<MoveDice>().PushCube(enemyTarget);
            }
            else if(groundBehindPlayer)
            {
                yield return MoveDice.PushCube(playerTarget);
            }
            else if (groundBehingEnemy)
            {
                yield return currentEnemy.GetComponent<MoveDice>().PushCube(enemyTarget);
            }


        }

        if (currentEnemy)
        {
            currentEnemy.gameObject.GetComponent<Enemy>().isFighting = false;
        }

        isFighting = false;
    }

}