using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelManager : MonoBehaviour
{
    public List<FastTravelController> travelPoints;

    public bool GetTravelPointActive(int tp)
    {
        return travelPoints[tp].canTravel;
    }

    public void TravelNow(int tp)
    {
        GameManager_New.instance.playerInstance.transform.position = travelPoints[tp].transform.position;
    }
}
