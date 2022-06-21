using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class playerMovement : MonoBehaviour
{
    public bool isSprinting = false;

    [SerializeField] StudioEventEmitter dashSFXEmitter;
    [SerializeField] StudioEventEmitter stepsSFXEmitter;
    private Coroutine dashCoroutine;
    
    private float currentVelocity;
    [SerializeField] private float normalVelocity = 0.075f;
    [SerializeField] private float sprintVelocity = 0.12f;
    [SerializeField] Animator thisAnim;
    static GameObject instance;
    static public Vector2 getPlayerPosition()
    {
        return new Vector2(instance.transform.position.x, instance.transform.position.y);
    }
    
    private GameState gameState;
    private cameraFollow cam;
    void Start() {
        instance = gameObject;
        currentVelocity = normalVelocity;
        thisAnim = this.GetComponent<Animator>();
        gameState = FindObjectOfType<GameState>();
        cam = FindObjectOfType<cameraFollow>();
    }
    private void FixedUpdate()
    {        
    }
    void Update()
    {
        if (gameState.state == State.Play) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                currentVelocity = sprintVelocity;
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
                    isSprinting = true;
                } else {
                    isSprinting = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                currentVelocity = normalVelocity;
                isSprinting = false;
            }
            Vector2 movementInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f ) ;
            this.transform.position = new Vector2(
                this.transform.position.x + movementInput.x * currentVelocity * Time.deltaTime,
                this.transform.position.y + movementInput.y * currentVelocity * Time.deltaTime
            );
            if (movementInput.magnitude > 0 && stepsSFXEmitter.IsPlaying()) 
            {
                stepsSFXEmitter.Play();
            }
            thisAnim.SetFloat("speedX", Input.GetAxis("Horizontal"));
            thisAnim.SetFloat("speedY", Input.GetAxis("Vertical"));
            
            GetComponent<Animator>().speed = (isSprinting ? 4 : 2);

            if (isSprinting && dashCoroutine == null) {
                dashCoroutine = StartCoroutine(makeDashSound());
            }

            //Después de que se mueva, que la camara se ajuste
            cam._Update();
        }

        if (gameState.state == State.Pause) {
            thisAnim.SetFloat("speedX", 0);
            thisAnim.SetFloat("speedY", 0);
        }
    }
    
    IEnumerator makeDashSound() {
        while (isSprinting) {            
            dashSFXEmitter.Play();
            yield return new WaitForSeconds(0.4f);
        }
        dashCoroutine = null;
    }

}
