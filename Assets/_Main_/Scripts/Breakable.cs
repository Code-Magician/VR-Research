using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] List<GameObject> brokenPieces;
    [SerializeField] float timeToBreak;

    private float timer = 0;

    private void Start()
    {
        foreach(var piece in brokenPieces)
        {
            piece.SetActive(false);
        }
    }

    private void Break()
    {
        timer += Time.deltaTime;
        if(timer > timeToBreak)
        {
            foreach (var piece in brokenPieces)
            {
                piece.SetActive(true);
                piece.transform.parent = null;
            }
            Destroy(gameObject);
        }
    }
}
