using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public Transform playerTr;

    void Update()
    {
        transform.position = new Vector3(playerTr.position.x, 2f, playerTr.position.z + 1f);
    }
}
