using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;

    private CameraMixer mixer;
    public float time = 2;
    public int camPos = 0;


    private void Awake() {
        mixer = GetComponent<CameraMixer>();
        foreach(Camera cam in cameras) {
            cam.enabled = false;
        }
    }

    private void Start() {
        StartCoroutine(BlendCameraCoroutine());
    }

    IEnumerator BlendCameraCoroutine() {

        mixer.blendCamera(cameras[camPos], time, Interpolators.quartIn);

        yield return new WaitForSeconds(time);

        //camPos = camPos >= cameras.Length ? 0 : camPos++;
        camPos++;
        if(camPos >= cameras.Length) {
            camPos = 0;
        }

        StartCoroutine(BlendCameraCoroutine());
    }
}
