using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraPlayer;
    [SerializeField] GameObject leftCamera;
    [SerializeField] GameObject rightCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchCameras(int value)
    {
        switch (value)
        {
            case 0:
                cameraPlayer.SetActive(true);
                leftCamera.SetActive(false);
                rightCamera.SetActive(false);
                break;
            case 1:
                cameraPlayer.SetActive(false);
                leftCamera.SetActive(false);
                rightCamera.SetActive(true);
                break;
            case 2:
                cameraPlayer.SetActive(false);
                leftCamera.SetActive(true);
                rightCamera.SetActive(false);
                break;
        }
    }
}
