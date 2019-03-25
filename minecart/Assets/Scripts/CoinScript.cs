using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ENTER");
        if (other.name == "Character")
        {
            SoundManagerScript.PlaySound(SoundManagerScript.COIN);
            Destroy(gameObject);
        }
    }
}
