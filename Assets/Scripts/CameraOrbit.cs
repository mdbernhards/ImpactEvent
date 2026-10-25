using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;

    public float orbitRadius = 25f;
    public float height = 12f;
    public float orbitSpeed = 3f;

    private float angle;

    void Update()
    {
        angle += orbitSpeed * Time.deltaTime;

        float x = Mathf.Sin(angle) * orbitRadius;
        float z = Mathf.Cos(angle) * orbitRadius;

        transform.position = target.position + new Vector3(x, height, z);

        transform.LookAt(target.position);
    }
}