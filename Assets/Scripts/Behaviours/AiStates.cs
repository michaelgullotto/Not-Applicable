using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private Dictionary<States, StateDelegate> states = new Dictionary<States, StateDelegate>();
    public static int Alert = 0;
    // gives state if there isnt one
    public void ChangeState(States _newState)
    {
        if (currentState != _newState)
            currentState = _newState;
    }
    
    // adds states
    private void Start()
    {
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
            Alert = 0;
        }
    }
    // sets AI to roam randomly
    private void Roaming()
    {
        if (!hasPath)
        {
            agent.SetDestination(new Vector3(Random.Range(1,200), Random.Range(1,200)));
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

        if (!hasPath)
        {
            agent.SetDestination(player.transform.position);
            hasPath = true;
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
            yield return new WaitForSeconds(30);
        }
        hasPath = false;
    }
}
