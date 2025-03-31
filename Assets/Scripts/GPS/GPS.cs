using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public Transform playerTr;

    void Update()
    {
        transform.position = new Vector3(playerTr.position.x + 0.7f, 1f, playerTr.position.z + 0.9f);
    }
}
