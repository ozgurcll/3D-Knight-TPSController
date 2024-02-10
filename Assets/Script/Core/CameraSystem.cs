using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 1.0f;
    public float damping = 1.0f;
    private Vector3 offset;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    private float maxTargetFov = 60f;
    private float minTargetFov = 20f;
    private float targetFov = 60f;

    private Vector3 followOfset;
    private float minFollowOfset = 3f;
    private float maxFollowOfset = 15f;

    float minFollowOfsetY = 0.2f;
    float maxFollowOfsetY = 2f;

    private void Awake()
    {
        followOfset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        offset = transform.position - target.position;

    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            CameraRotate();
        }
        transform.position = Vector3.Lerp(transform.position, target.position + offset, 1 * Time.deltaTime);


        CameraZoomMoveFw();
    }

    private void CameraRotate()
    {

        float rotateDir = 0f;
        if (Input.GetAxis("Mouse X") < 0) rotateDir = -1;
        if (Input.GetAxis("Mouse X") > 0) rotateDir = +1;

        float rotateSpeed = 700f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);


    }

    void CameraZoomFov()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFov += 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFov -= 5;
        }

        targetFov = Mathf.Clamp(targetFov, minTargetFov, maxTargetFov);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFov, Time.deltaTime * zoomSpeed);
    }

    void CameraZoomMoveFw()
    {
        Vector3 zoomDir = followOfset.normalized;

        float zoomAmount = 3f;
        if (Input.mouseScrollDelta.y > 0)
        {
            followOfset += zoomDir * zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOfset -= zoomDir * zoomAmount;
        }

        if (followOfset.magnitude < minFollowOfset)
        {
            followOfset = zoomDir * minFollowOfset;
        }
        if (followOfset.magnitude > maxFollowOfset)
        {
            followOfset = zoomDir * maxFollowOfset;
        }
        float zoomSpeed = 10f;

        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
       Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOfset, Time.deltaTime * zoomSpeed);


    }

    void CameraZoomLowerY()
    {

        float zoomAmount = 3f;
        if (Input.mouseScrollDelta.y > 0)
        {
            followOfset.y += zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOfset.y -= zoomAmount;
        }

        followOfset.y = Mathf.Clamp(followOfset.y, minFollowOfsetY, maxFollowOfsetY);
        float zoomSpeed = 10f;

        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
       Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOfset, Time.deltaTime * zoomSpeed);

    }

    private void CameraMove() // Burayı kullanmıcaz rotation ihtiyacımız olan
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
