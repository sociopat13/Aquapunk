using Aquapunk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeVisMob : MonoBehaviour
{
    public Collider vis;
    public Mob mob;

    private void OnTriggerEnter(Collider other)
    {
        if (mob.trigger == null && other.GetComponent<Entity>() && other.GetComponent<Entity>().GetType().ToString() != "Aquapunk.Mob" && mob.agreed)
        {
            StopCoroutine(mob.TerritoryPatrol());
            mob.trigger = other.gameObject;
            mob.enemys.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (mob.enemys.Contains(other.gameObject))
        {
            StartCoroutine(mob.TerritoryPatrol());
            mob.enemys.Remove(mob.trigger);
            mob.trigger = null;
            mob.SortTrigger();
        }

    }
}
