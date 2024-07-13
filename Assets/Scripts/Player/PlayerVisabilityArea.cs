using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisabilityArea : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float searchRadius;

    private void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        searchRadius = gameObject.GetComponent<PlayerEnemyFinder>().searchRadius;
        DrawSearchRadius();
    }

    private void Update()
    {
        UpdateCirclePosition();
    }

    private void DrawSearchRadius()
    {
        Debug.Log("Радиус поиска: " + searchRadius);
        int segments = 100;
        lineRenderer.positionCount = segments + 1;
        float angle = 0f;
        float scaleFactor = 0.333333f;

        lineRenderer.sortingOrder = 10;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * searchRadius * scaleFactor;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * searchRadius * scaleFactor;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / segments);
        }
    }

    private void UpdateCirclePosition()
    {
        Vector3 playerPosition = transform.position;
        lineRenderer.transform.position = playerPosition;
    }
}