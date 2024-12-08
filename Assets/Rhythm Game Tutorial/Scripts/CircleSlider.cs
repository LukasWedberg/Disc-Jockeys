using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSlider : MonoBehaviour
{
    //head material to modify color
    [SerializeField] Material HeadMaterial;
    //shader value to change
    string nh = "_ColorToShiftTo";
    Color newColor;

    [SerializeField] Transform Knob;
    [SerializeField] TMPro.TextMeshProUGUI ValueText;
    [SerializeField] Transform CircleCenter; // Reference to the center of the circle
    Vector3 mousePos;

    public void onHandleDrag()
    {
        //get mouse position and calculate angle relative to circle center
        mousePos = Input.mousePosition;
        Vector2 centerScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, CircleCenter.position);
        Vector2 dir = mousePos - (Vector3)centerScreenPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle <= 0) ? (360 + angle) : angle;

        //rotate knob around the circle center
        Knob.position = CircleCenter.position + (Quaternion.Euler(0, 0, angle) * Vector3.right * (Knob.position - CircleCenter.position).magnitude);
        Knob.rotation = Quaternion.Euler(0, 0, angle + 135f);

        //set color of head to change with angle
        newColor = Color.HSVToRGB(angle / 360, 1, 1);
        HeadMaterial.SetColor(nh, newColor);

        //saving new color to playerprefs
        /*
        Color colorOfHelmet = HeadMaterial.GetColor(nh);
        PlayerPrefs.SetFloat("rValue", colorOfHelmet.r);
        PlayerPrefs.SetFloat("gValue", colorOfHelmet.g);
        PlayerPrefs.SetFloat("bValue", colorOfHelmet.b);
        PlayerPrefs.SetFloat("aValue", colorOfHelmet.a); */
    }
}