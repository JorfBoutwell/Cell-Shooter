using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public enum playerState
    {
        idle,
        walk,
        walkJump,
        run,
        runJump,
        jump,
        wallRun,

    }

    public playerState currentState;

    

    // Start is called before the first frame update
    void Start()
    {
        setCurrentState(playerState.idle);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        switch (currentState)
        {
            case playerState.idle:

        }
        */

        if(currentState == playerState.idle)
        {
            idle();
        } else if(currentState == playerState.walk)
        {
            print("Walk");
        }
    }

    void setCurrentState(playerState state)
    {
        currentState = state;
    }

    void idle()
    {
        print("Idle State");
    }
}
