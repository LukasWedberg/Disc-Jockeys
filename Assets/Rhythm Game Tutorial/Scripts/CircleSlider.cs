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
    Vector3 mousePos;
    public void onHandleDrag()
    {
        //get mouse position and calculate angle
        mousePos = Input.mousePosition;
        Vector2 dir = mousePos - Knob.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle <= 0) ? (360 + angle) : angle;

        //rotate knob to angle
        Quaternion r = Quaternion.AngleAxis(angle + 135f, Vector3.forward);
        Knob.rotation = r;

        //set value text to display angle
       // ValueText.text = angle.ToString("0");

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
