using UnityEngine;

/// <summary>Class <c>LaneController</c> initilizes three lanes and handles the current lane the player uses
/// and switching the lane.
/// The script also includes the class <c>Lane</c> which models a lane in the level.</summary>
///

public class LaneController : MonoBehaviour
{
    Lane UpperLane;
    Lane MiddleLane;
    Lane LowerLane;
    [SerializeField] float lowerLanePosition; // -4.2
    [SerializeField] float laneHeight; // 1.26

    public Lane CurrentLane;

    void Awake()
    {
        UpperLane = new Lane("Lane1", lowerLanePosition+laneHeight*2);
        MiddleLane = new Lane("Lane2", lowerLanePosition+laneHeight);
        LowerLane = new Lane("Lane3", lowerLanePosition);
        UpperLane.LaneBelow = MiddleLane;
        MiddleLane.LaneAbove = UpperLane;
        MiddleLane.LaneBelow = LowerLane;
        LowerLane.LaneAbove = MiddleLane;
        CurrentLane = LowerLane;
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

/// <summary>Class <c>Lane</c> models a lane in the level.</summary>
///
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
