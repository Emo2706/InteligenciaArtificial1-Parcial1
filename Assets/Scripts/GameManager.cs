using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float width = 30; 
    public float height = 20;
    

    public List<SteeringAgent> boids = new List<SteeringAgent>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    public void ShiftPositionOnBounds(Transform transform)
    {
        Vector3 pos = transform.position;
        float w = width / 2;
        float h = height / 2;

        if (pos.y > h)  pos.y = -h;
        if (pos.y < -h) pos.y = h;
        if (pos.x > w)  pos.x = -w;
        if (pos.x < -w) pos.x = w;

        transform.position = pos;

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.right * width + Vector3.up * height);
    }
}
