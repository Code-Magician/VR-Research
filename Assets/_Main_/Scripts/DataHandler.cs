using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance;

    public Transform startXTr, endXTr, startZTr, endZTr;

    private void Start()
    {
        Instance = this;
    }
}
