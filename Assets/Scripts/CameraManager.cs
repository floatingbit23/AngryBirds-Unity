using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{

    // Declarar las 2 cámaras empleadas
    [SerializeField] private CinemachineCamera _idleCam;
    [SerializeField] private CinemachineCamera _followCam;

    private void Awake()
    {
        SwithToIdleCam(); // la escena siempre comenzará con la IdleCamera
    }


    public void SwithToIdleCam()
    {
        _idleCam.enabled = true;
        _followCam.enabled = false;
    }


    public void SwithToFollowCam(Transform followTransform)
    {
        _followCam.Follow = followTransform;

        _idleCam.enabled = false;
        _followCam.enabled = true;

    }

}
