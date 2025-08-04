using Unity.VisualScripting;
using UnityEngine;
using FMOD.Studio;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public float speed;
    public float rotationSpeed = 10f;

    float hAxis;
    float vAxis;

    private Vector3 lastMoveVec = Vector3.forward;
    Vector3 moveVec;

    private EventInstance playerFootsteps;
    private bool IsCarContact;

    void Start()
    {
        playerFootsteps = AudioManager.Instance.CreateInstance(FMODEvents.instance.playerFootSteps);
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0,vAxis);
        bool isMove = moveVec.magnitude > 0;
        bool isLastMove = lastMoveVec.magnitude > 0;
        animator.SetBool("IsMove", isMove);

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

        UpdateSound();
    }

    private void UpdateSound()
    {
        if (moveVec.magnitude > 0 && !IsCarContact)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else
        {
            playerFootsteps.stop(STOP_MODE.IMMEDIATE);
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Car"))
        {
            IsCarContact = true;
            playerFootsteps.stop(STOP_MODE.IMMEDIATE);
        }
    }
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Car"))
        {
            IsCarContact = true;
            playerFootsteps.stop(STOP_MODE.IMMEDIATE);
            if(Input.GetKeyDown(KeyCode.E))
            {
                playerFootsteps.stop(STOP_MODE.IMMEDIATE);
            }
        }
    }
       
           
  
       
}
