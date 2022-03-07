using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    Lane UpperLane;
    Lane MiddleLane;
    Lane LowerLane;
    public Lane CurrentLane;

    void Awake()
    {
        UpperLane = new Lane("Lane1", -0.5f);
        MiddleLane = new Lane("Lane2", -1.5f);
        LowerLane = new Lane("Lane3", -2.5f);
        UpperLane.LaneBelow = MiddleLane;
        MiddleLane.LaneAbove = UpperLane;
        MiddleLane.LaneBelow = LowerLane;
        LowerLane.LaneAbove = MiddleLane;
        CurrentLane = MiddleLane;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Lane SwitchLane(bool isUp)
    {
        Lane newLane;
        if(isUp)
        {
            newLane = CurrentLane.LaneAbove;
        }
        else
        {
            newLane = CurrentLane.LaneBelow;
        }
        if(newLane != null)
        {
            CurrentLane = newLane;
        }
        return CurrentLane;
    }
}


public class Lane
{
    public string Name;
    public float LaneYPosition { get; }
    public Lane LaneAbove = null;
    public Lane LaneBelow = null;

    public Lane(string name, float laneYPosition)
    {
        Name = name;
        LaneYPosition = laneYPosition;
    }
}
