using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;

    void Start()
    {
        transform.rotation = Quaternion.Euler(30f, 0, 0);
    }
    // void Update()

    // {
        
    // }
    void LateUpdate()
    {   
        if (target == null){
        return;
        }
        transform.position = new Vector3(target.position.x, target.position.y + 2.7f ,target.position.z -2f);
    }
}
