using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // animator.SetFloat("New Float", 3.1f);
        // animator.SetInteger("New Int", 1);
        // animator.SetBool("New Bool", true);
        // animator.SetTrigger("New Trigger");
    }
    public float speed;
    public float rotationSpeed = 10f;
    float hAxis;
    float vAxis;
    private Vector3 lastMoveVec = Vector3.forward;
    Vector3 moveVec;

    void Start()
    {
        
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0,vAxis);
        bool isMove = moveVec.magnitude > 0;
        bool isLastMove = lastMoveVec.magnitude > 0;
        animator.SetBool("IsMove", isMove);
        
        // if (isMove){
        //     animator.transform.forward = moveVec;
        // }

        if (isMove)
        {
            transform.position += moveVec.normalized * speed * Time.deltaTime;
            lastMoveVec = moveVec;
        }
        if (isLastMove)
        {
        Quaternion targetRotation = Quaternion.LookRotation(lastMoveVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
       
           
  
       
}
