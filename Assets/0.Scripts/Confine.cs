using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confine : MonoBehaviour
{
    PolygonCollider2D collider;
    [SerializeField] Vector2[] pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<PolygonCollider2D>().points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
