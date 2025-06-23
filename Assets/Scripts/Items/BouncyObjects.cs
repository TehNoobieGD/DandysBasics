using System.Collections.Generic;
using UnityEngine;

public class BouncyObjects : MonoBehaviour
{
    [Header("Bounce Settings")]
    public string targetTag = "Bounce";         // Tag of objects to bounce
    public float bounceHeight = 0.25f;          // How high the objects bounce
    public float bounceSpeed = 2f;              // Speed of bouncing

    private List<Transform> bounceObjects = new List<Transform>();
    private Dictionary<Transform, float> baseY = new Dictionary<Transform, float>();

    void Start()
    {
        // Find all objects with the specified tag
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in taggedObjects)
        {
            Transform t = obj.transform;
            bounceObjects.Add(t);
            baseY[t] = t.position.y; // Save the original Y position
        }
    }

    void Update()
    {
        float time = Time.time;

        foreach (Transform t in bounceObjects)
        {
            if (t == null) continue;

            float newY = baseY[t] + Mathf.Sin(time * bounceSpeed) * bounceHeight;
            t.position = new Vector3(t.position.x, newY, t.position.z);
        }
    }
}
