using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLandscape : MonoBehaviour
{
    public GameObject planePrefab;


    [SerializeField]
    private int _rols;
    [SerializeField]
    private int _cols;

    private void Start()
    {
        for(float obj = 0, x = 0 - planePrefab.transform.localScale.x * 10 * _rols/ 2; obj <= _rols; obj++, x += planePrefab.transform.localScale.x * 10)
        {
            for (float objZ = 0, z = 0 - planePrefab.transform.localScale.z * 10 * _cols/ 2; objZ <= _cols; objZ++, z += planePrefab.transform.localScale.z * 10)
            {
                Transform plane = Instantiate(planePrefab).transform;

                plane.position = new Vector3(x, 0, z);

                plane.SetParent(transform);
            }
        }
        transform.Rotate(new Vector3(0, 45, 0));
    }
}
