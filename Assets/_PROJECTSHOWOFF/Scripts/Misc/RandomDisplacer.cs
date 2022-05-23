using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDisplacer : MonoBehaviour
{
    [SerializeField] float xMin;
    [SerializeField] float xMax;

    [SerializeField] float yMin;
    [SerializeField] float yMax;

    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0);
    }
}
