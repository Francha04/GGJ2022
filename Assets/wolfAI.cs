using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using UnityEngine.Serialization;
using FMODUnity;

public class wolfAI : MonoBehaviour
{
    AIDestinationSetter seeker;
    AIPath thisPathAI;
    [SerializeField] StudioEventEmitter stepsSFXEmitter;
    [SerializeField] StudioEventEmitter growlSFXEmitter;
    [SerializeField] private CircleCollider2D childCollider;    
    [SerializeField] [FormerlySerializedAs("maxRange")] float maxDistancePatrol;
    [SerializeField] [FormerlySerializedAs("minRange")] float minDistanceToPatrol;
    [SerializeField] [FormerlySerializedAs("exitRadius")] float sensorEscapeRadius;
    [SerializeField] [FormerlySerializedAs("enterRadius")] float sensorEnterRadius;    
    [SerializeField] float minTimerRange;
    [SerializeField] float maxTimerRange;
    [SerializeField] float maxDistanceFromPlayer;
    [SerializeField] float sprintingSpeed;
    [SerializeField] float patrolSpeed;
    Coroutine patrolTimer;
    Transform patrolTarget;
    Transform playerTransform;
    Animator thisAnim;
    bool chasingPlayer = false;
    private void Start()
    {
        patrolTarget = new GameObject("wolfDummyTarget").transform;
        childCollider.radius = sensorEnterRadius;
        seeker = this.GetComponent<AIDestinationSetter>();
        thisPathAI = this.GetComponent<AIPath>();
        FindObjectOfType<spawnManager>().addToList(this.gameObject);
        thisAnim = this.GetComponent<Animator>();
        startPatrolTimer();        
    }
    private void FixedUpdate()
    {
        if (thisPathAI.velocity.magnitude > 0 && !stepsSFXEmitter.IsPlaying()) 
        {
            stepsSFXEmitter.Play();
        }
    }

    private void generateNewPatrolTarget()
    {
        Vector2 targetPos = new Vector2(0f,0f);
        /*do
        {*/
            float distanceToMove = Random.Range(minDistanceToPatrol, maxDistancePatrol);
            float angle = Random.value * 2 * Mathf.PI;
            targetPos = new Vector2(distanceToMove * Mathf.Cos(angle), distanceToMove * Mathf.Sin(angle));
       /* } while (Vector2.Distance(targetPos, playerMovement.getPlayerPosition()) < maxDistanceFromPlayer);*///Aca comente un do while for design reasons.
        patrolTarget.transform.position = targetPos;
        seeker.target = patrolTarget;
        startPatrolTimer();
    }

    private void startPatrolTimer()
    {
        patrolTimer = StartCoroutine(patrol());
    }
    private IEnumerator patrol()
    {
        yield return new WaitUntil(() => seeker.target == null || thisPathAI.reachedDestination);
        //print("Reached destination.");
        float timeToWait = Random.Range(minTimerRange, maxTimerRange);
        //Debug.Log($"{name} time to wait: {timeToWait}", this);
        yield return new WaitForSeconds(timeToWait);
        generateNewPatrolTarget();
    }
    public void onPlayerEnter(GameObject player)
    {
        childCollider.radius = sensorEscapeRadius;
        seeker.target = player.transform;
        if (patrolTimer != null) {
            StopCoroutine(patrolTimer);
        }
        growlSFXEmitter.Play();
        playerTransform = player.transform;
        thisPathAI.maxSpeed = sprintingSpeed; 
    }
    public void onPlayerExit(GameObject player)
    {
        childCollider.radius = sensorEnterRadius;
        goToPosition(playerTransform.position); 
        print("Player exit");
        thisPathAI.maxSpeed = patrolSpeed;
    }

  

    private void goToPosition (Vector2 position) 
    {
        patrolTarget.position = position;
        seeker.target = patrolTarget;
        startPatrolTimer();        
    }
    private void Update()
    {
        thisAnim.SetFloat("Blend", Mathf.Sign(thisPathAI.velocity.y));   
    }
}