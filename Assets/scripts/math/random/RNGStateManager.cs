using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RNGStateManager
{
    private static Stack<Random.State> history = new Stack<Random.State>();

    public static void Push()
    {
        //Push the current state
        history.Push(Random.state);
    }

    public static void Push(int seed)
    {
        //Set the seed
        if(seed < 0)
            seed = (int)(Random.value * 0xffffff);

        //First push the old state
        RNGStateManager.Push();

        //Then set the state and push this too
        Random.InitState(seed);
        RNGStateManager.Push();
    }

    public static Random.State Pop()
    {
        //Just pop the current state
        Random.State state = history.Pop();

        //Set the current state
        Random.state = state;

        //And return it
        return state;
    }

    //Get the currentstate
    public static Random.State state
    {
        get { return history.Peek(); }
    }
}
