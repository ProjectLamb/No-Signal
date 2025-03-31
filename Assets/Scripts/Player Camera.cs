using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;

    void Start()
    {
        transform.rotation = Quaternion.Euler(25f, 0, 0);
    }
    // void Update()
    // {
        
    // }
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y + 1.5f ,target.position.z -3f);
    }
}
