using UnityEngine;
using Unity.Cinemachine;

public class CameraDirector : MonoBehaviour
{
    public CinemachineCamera lakeWide;
    public CinemachineCamera beerCloseup;
    public CinemachineCamera lakeWide2;
    public CinemachineCamera sunCam;
    public CinemachineCamera playerCloseup;

    public CinemachineCamera playerCam;
    public CinemachineCamera targetCam;

    public Transform defaultLookAt;

    void Start()
    {
        if (defaultLookAt == null)
            defaultLookAt = playerCam.LookAt;
    }

    public void ActivateCamera(string camName)
    {
        ResetCameraPriority();

        switch (camName)
        {
            case "WideLakeShot":
                lakeWide.Priority = 10;
                break;

            case "BeerCloseup":
                beerCloseup.Priority = 10;
                break;

            case "WideLakeShot2":
                lakeWide2.Priority = 10;
                break;

            case "FocusSun":
                sunCam.Priority = 10;
                break;

            case "PlayerCam":
                playerCam.Priority = 10;
                break;

            case "PlayerCloseup":
                playerCloseup.Priority = 10;
                break;
        }
    }

    public void FocusTarget(GameObject target)
    {
        targetCam.Follow = target.transform;
        targetCam.LookAt = target.transform;

        ResetCameraPriority();
        targetCam.Priority = 100;
    }

    public void ReturnPlayer()
    {
        ResetCameraPriority();

        playerCam.Priority = 100;
    }

    private void ResetCameraPriority()
    {
        lakeWide.Priority = 0;
        beerCloseup.Priority = 0;
        lakeWide2.Priority = 0;
        sunCam.Priority = 0;
        playerCloseup.Priority = 0;

        playerCam.Priority = 0;
        targetCam.Priority = 0;
    }

    public void LookAtTarget(GameObject target)
    {
        ResetCameraPriority();
        targetCam.Priority = 100;
        targetCam.LookAt = target.transform;
    }

    public void ResetLookAt()
    {
        ResetCameraPriority();
        playerCam.Priority = 100;
        playerCam.LookAt = defaultLookAt;
    }
}