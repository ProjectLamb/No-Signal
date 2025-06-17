using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCam : MonoBehaviour
{
    public Transform target;

    public void SetCamPosition()
    {
        this.transform.position = new Vector3(target.position.x + 5.46f, target.position.y + 5.7f, target.position.z - 70f);
    }
}
