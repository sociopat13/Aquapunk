using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLandscape : NetworkBehaviour
{
    public GameObject planePrefab;

    
    private void Start()
    {
        if (isServer)
        {
        }
    }
}
