using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MiniCam : MonoBehaviour
{

    private Vector2 lastMousePos;
    private float mouseX;
    private float mouseY;
    private float rotY;

    public float sensitivity;
    public Transform target;

    private Camera cam;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        RenderPipelineManager.beginCameraRendering += BeginRender;
        RenderPipelineManager.endCameraRendering += EndRender;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= BeginRender;
        RenderPipelineManager.endCameraRendering -= EndRender;
    }

    private void BeginRender(ScriptableRenderContext context, Camera renderingCamera)
    {
        if (renderingCamera == cam)
        {
            RenderSettings.fog = false;
        }
    }

    private void EndRender(ScriptableRenderContext context, Camera renderingCamera)
    {
        if (renderingCamera == cam)
        {
            RenderSettings.fog = true;
        }
    }


    void Start()
    {
        lastMousePos = Mouse.current.position.ReadValue();
    }
    void Update()
    {
        MiniCamFollow();
    }

    void MiniCamFollow()
    {
        //카메라 회전
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity;

        mouseX = delta.x;
        mouseY = delta.y;

        rotY += mouseX;

        //카메라 플레이어 추적
        transform.position = new Vector3(target.position.x, target.position.y + 150f, target.position.z);
    }
}
