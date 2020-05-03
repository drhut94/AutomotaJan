using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialIndexController : MonoBehaviour
{
    public float speed;

    private SpatialIndex spatial;


    private void Awake() {
        spatial = FindObjectOfType<SpatialIndex>();
    }

    private void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
            Debug.Log(spatial.getFloorStatus(transform.position.x, transform.position.y));
        if(spatial.getFloorStatus(transform.position.x, transform.position.y) == SpatialIndex.FLOOR_STATUS.REGULAR) {
            transform.eulerAngles += new Vector3(0, 1, 0);
        }
    }
}
