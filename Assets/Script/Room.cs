using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameObject doorRight;
    private GameObject doorLeft;
    private GameObject doorUp;
    private GameObject doorDown;
    
    public bool openRight;
    public bool openLeft;
    public bool openUp;
    public bool openDown;

    public int stepFromStart;
    public int doorRoom;

    private void Awake()
    {
        doorRight = transform.Find("door_right").gameObject;
        doorLeft = transform.Find("door_left").gameObject;
        doorUp = transform.Find("door_up").gameObject;
        doorDown = transform.Find("door_down").gameObject;
    }

    private void Start()
    {
        doorRight.SetActive(openRight);
        doorLeft.SetActive(openLeft);
        doorUp.SetActive(openUp);
        doorDown.SetActive(openDown);
    }

    public void CalculateStep()
    {
        var position = transform.position;
        stepFromStart = (int) (Mathf.Abs(position.x / 30f) + Mathf.Abs(position.y / 20f));

        if (openRight)
            doorRoom++;
        if (openDown)
            doorRoom++;
        if (openLeft)
            doorRoom++;
        if (openUp)
            doorRoom++;
    }
}