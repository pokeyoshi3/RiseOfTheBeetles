using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform LookAt;
    
    public bool LookForwardByInput = true;
    public float LookDistance = 1.0f;
    [Range(1, 10)]
    public float LookSpeed = 0;

    public Vector3 offset;
    private Vector3 lookPosition;

    private void Start()
    {
        //offset = transform.localPosition;
    }

    void FixedUpdate()
    {
        if (LookAt == null)
            return;
  
        if(LookForwardByInput && GameManager_New.instance.GetGameState() == eGameState.running)
        {
            lookPosition = LookAt.position + new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), -1) * LookDistance;
        }
        else { lookPosition = LookAt.position; }

        transform.localPosition = Vector3.Lerp(transform.localPosition, offset + lookPosition, LookSpeed * Time.fixedDeltaTime);
    }

    public void SetCameraLookAt(Transform look, bool jumpTowards)
    {
        LookAt = look;

        if(jumpTowards)
        {
            SetCameraPosition(look.position);
        }
    }

    public void SetCameraPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetCameraRecoil(Vector3 recoil)
    {
        transform.position += recoil;
    }
}
