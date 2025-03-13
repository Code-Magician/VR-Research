using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance;

    public Transform startXTr, endXTr, startZTr, endZTr;
    [SerializeField] GameObject landPointMarker;

    private void Start()
    {
        Instance = this;
    }

    public void SetMarker(Vector3 pos)
    {
        pos.y = 0.1f;
        landPointMarker.transform.position = pos;
    }
}
