using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Robot>() != null)
        {
            other.GetComponent<Robot>().Complite();
        }
    }
}
