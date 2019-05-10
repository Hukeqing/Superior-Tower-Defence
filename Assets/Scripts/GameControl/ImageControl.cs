using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageControl : MonoBehaviour
{
    public int Call;

    private CameraControl CC;
    private PolygonCollider2D PC;
    private Image Im;
    private Color NormalColor;

    private void Start()
    {
        CC = Camera.main.GetComponent<CameraControl>();
        PC = GetComponent<PolygonCollider2D>();
        Im = GetComponent<Image>();
        NormalColor = Im.color;
    }

    void Update()
    {
        if (PC.OverlapPoint(Input.mousePosition))
        {
            Im.color = Color.cyan;
            if (Input.GetMouseButtonDown(0))
            {
                CC.BuildTower(Call);
            }
        }
        else
        {
            Im.color = NormalColor;
        }
    }
}
