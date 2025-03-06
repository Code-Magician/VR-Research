using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TriggerZone>().OnEventTrigger.AddListener(InsideTrash);
    }

    private void InsideTrash(GameObject go)
    {
        go.SetActive(false);
    }
}
