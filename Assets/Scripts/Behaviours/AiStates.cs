using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AiStates : MonoBehaviour
{
    // the states
    public enum States
    {
       Roaming, Alert1 , Alert2
    }
    // the delegate dictates what the functions for each state will look like
    public delegate void StateDelegate();
    // varibles
    [SerializeField] private States currentState = States.Roaming;
    [SerializeField] GameObject player;
    [SerializeField] private NavMeshAgent agent;
    private bool hasPath = false;
    private bool unreachable = false;
    private bool inRange = false;
    private bool alertRise = false;
    private Dictionary<States, StateDelegate> states = new Dictionary<States, StateDelegate>();
    [SerializeField] public int Alert = 1;
   [SerializeField] private GameManger _gameManger;

    public static Vector3 startPos;
    // gives state if there isnt one
    public void ChangeState(States _newState)
    {
        if (currentState != _newState)
            currentState = _newState;
    }
    
    // adds states
    private void Start()
    {
        startPos = agent.transform.position;
        states.Add(States.Roaming, Roaming);
        states.Add(States.Alert1, Alert1);
        states.Add(States.Alert2, Alert2);
        
    }

    // debugs log if not finding state
    private void Update()
    {
        if (states.TryGetValue(currentState, out StateDelegate state))
            state.Invoke();
        else
            Debug.LogError($"No state function set for state{currentState}");
        
        if (Alert < 0)
        {
            Alert = 1;
        }
        // calculates players distance from agent and increases alert if in range
        float playerDistance = (player.transform.position - agent.transform.position).magnitude;

        if (playerDistance < 50)
        {
            inRange = true;
            if (!alertRise)
            {
                Alert++;
                StartCoroutine(AlertRising());
            }
            
        }

    }
    // sets AI to roam randomly
    private void Roaming()
    {
        agent.speed = 10;
        if (!hasPath)
        {
            agent.SetDestination(new Vector3(Random.Range(-50,50),0,Random.Range(-50,50)));
            hasPath = true;
            unreachable = false;
            StartCoroutine(PathTimelimit());
        }
        if (unreachable)
        {
            hasPath = false;     
        }

        if (agent.remainingDistance <= .1f && !agent.pathPending)
        {
            hasPath = false;
        }

            if (Alert >= 100 && Alert < 200)
            {
            hasPath = false;
            ChangeState(States.Alert1);
            
            }
            else if (Alert >= 200)
            {
            hasPath = false;
            ChangeState(States.Alert2);
            }
        
    }
    // sets AI to run to player
    private void Alert1()
    {
        float playerDistance = (player.transform.position - agent.transform.position).magnitude;
        agent.speed = 10;
        if (!hasPath)
        {
            if (playerDistance > 20)
            {
                agent.SetDestination(new Vector3(Random.Range(-50,50),0,Random.Range(-50,50)));
                hasPath = true;
                unreachable = false;
                StartCoroutine(PathTimelimit());
            }
            if (playerDistance < 20)
            {
                agent.SetDestination(player.transform.position);
                hasPath = true;
            }
            if (unreachable)
            {
                hasPath = false;     
            }
           
        }

        if (agent.remainingDistance <= .1f && !agent.pathPending)
        {
            hasPath = false;
            if (Alert < 100)
            {
                ChangeState(States.Roaming);
            }
            else if (Alert >= 200)
            {
                ChangeState(States.Alert2);
            }
        }
    }
    // sets AI to run to player 
    private void Alert2()
    {
        agent.speed = 20;
        if (!hasPath)
        {
            agent.SetDestination(player.transform.position);
            hasPath = true;
        }

        if (agent.remainingDistance <= .1f && !agent.pathPending)
        {
            hasPath = false;
            if (Alert >= 100 && Alert < 200)
            {
                ChangeState(States.Alert1);
            }
            else if (Alert < 100)
            {
                ChangeState(States.Roaming);
            }
        }
    }
    // couts #0seconds to see if path isnt reachable
    IEnumerator PathTimelimit()
    {
        while (unreachable == false)
        {
            yield return new WaitForSeconds(10);
        }
        hasPath = false;
    }

    IEnumerator AlertRising()
    {
        while (inRange && Alert > 0)
        {
            alertRise = true;
            Alert = Alert + 5;
            
            yield return new WaitForSeconds(1);
        }

        alertRise = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            ChangeState(States.Roaming);
            agent.SetDestination(startPos);
            Alert = 1;
            _gameManger.MonsterHit();
            agent.transform.position = startPos;
            
        }
    }
}
