using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam { get; private set; }
    Transform thisTransform = null;
    
    Transform target = null;
    [SerializeField] float smoothDamp = 5f;
    [SerializeField] float smoothTime = 2f;
    [SerializeField] float camZOffset = -10f;
    Vector3 offset = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    [SerializeField] int[] camSizes = null;
    int camSizeIndex = 0;
    [SerializeField] float camSizeTransitionTimer = .75f;

    [SerializeField] float xPositionClamp = 0f;
    [SerializeField] float yPositionClamp = 0f;

    Coroutine ToggleCameraSizeCoroutine = null;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        thisTransform = transform;
        offset = Vector3.forward * camZOffset;
        camSizeIndex = 0;
        cam.orthographicSize = camSizes[camSizeIndex];
    }

    private void Start()
    {
        target = FindObjectOfType<BaseCharacter>().transform;
        thisTransform.position = new Vector3(target.position.x, target.position.y, camZOffset);
    }

    public void ToggleCamera()
    {
        camSizeIndex++;
        if (camSizeIndex >= camSizes.Length)
            camSizeIndex = 0;
        if(ToggleCameraSizeCoroutine != null)        
            StopCoroutine(ToggleCameraSizeCoroutine);
        
        ToggleCameraSizeCoroutine = StartCoroutine(CameraSizeTransition(camSizes[camSizeIndex]));        
    }

    private void LateUpdate()
    {

        Vector3 destinationPosition = target.GetComponent<Rigidbody>().position + offset;

        //Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));

        thisTransform.position = Vector3.Lerp(transform.position, destinationPosition, 1 - Mathf.Exp(-smoothDamp * Time.deltaTime));

        //Vector3 pos = Vector3.SmoothDamp(thisTransform.position, destinationPosition, ref velocity, smoothDamp, 50, smoothTime * Time.fixedDeltaTime);
        //pos.x = Mathf.Clamp(pos.x, xPositionClamp, 10000);
        //pos.y = Mathf.Clamp(pos.y, yPositionClamp, 10000);
        //thisTransform.position = pos;
        //thisTransform.position = Vector3.Lerp(thisTransform.position, destinationPosition, smoothTime * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Smoothly transition between camera sizes
    /// </summary>
    /// <param name="camSize"></param>
    /// <returns></returns>
    IEnumerator CameraSizeTransition(int camSize)
    {
        float currentSize = cam.orthographicSize;
        float time = 0;
        while(time < camSizeTransitionTimer)
        {
            float step = time / camSizeTransitionTimer;
            cam.orthographicSize = Mathf.Lerp(currentSize, camSize, step);
            time += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = camSize;
    }
}
