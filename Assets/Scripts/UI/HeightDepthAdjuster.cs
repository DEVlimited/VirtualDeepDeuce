using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adjusts the height/z-axis of the table and book(s), using a slider. This way the 
/// table is at a good height regardless of the user's height, or if they're
/// standing/sitting.
/// </summary>
public class HeightDepthAdjuster : MonoBehaviour
{
    public GameObject Floor;
    public void AdjustHeight(float newY)
    {
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    /// <summary>
    /// Wasn't sure how to describe this, but "depth" is how far away the table is
    /// from the user in the z-axis.
    /// </summary>
    /// <param name="newZ"></param>
    public void AdjustDepth(float newZ)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    public void AdjustFloor(float floorY)
    {
        Floor.transform.position = new Vector3(Floor.transform.position.x, floorY, Floor.transform.position.z);
    }
}
