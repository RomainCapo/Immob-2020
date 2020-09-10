/***
 * Immob-2020
 * Romain Capocasale, Jonas Freiburghaus and Vincent Moulin
 * Infography course
 * He-Arc, INF3dlm-a
 * 2019-2020
 * **/

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plan
{
    public float wallHeight;
    public float wallWidth;
    public float windowH1;
    public float windowH2;
    public float doorH2;
    public List<float> entryPoint;
    public List<Segment> segments;
    public List<Area> areas;
    public List<Door> doors;

    /// <summary>
    /// Extrude the wall above the door, extrude the door
    /// </summary>
    public void Extrude()
    {
        foreach(Segment s in segments)
        {
            s.Extrude(this);
        }

        foreach(Door d in doors)
        {
            d.Extrude(this);
        }

        foreach(Area a in areas)
        {
            GameObject light = a.InstanciateLights(this);
            GameObject floor = a.InstanciateCeilingAndFloor(this);
            light.transform.parent = floor.transform;
        }

        GameObject.Destroy(Segment.wallPrefab);
        GameObject.Destroy(Door.wallPrefab);
    }

    /// <summary>
    /// Allow to destroy the door collider after the furniture placement. 
    /// </summary>
    public void DestroyDoorCollider()
    {
        foreach(BoxCollider b in Door.doorsCollider)
        {
            GameObject.Destroy(b);
        }
    }

    /// <summary>
    /// Allow to destroy the window collider after the furniture placement. 
    /// </summary>
    public void DestroyWindowCollider()
    {
        foreach(BoxCollider b in Window.windowsCollider)
        {
            GameObject.Destroy(b);
        }
    }
}
