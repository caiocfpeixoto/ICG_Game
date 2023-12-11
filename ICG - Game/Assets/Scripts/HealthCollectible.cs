using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;

     [Header("SFX")]
    [SerializeField] private AudioClip collectiblesSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().AddHealth(healthValue);
            SoundManager.instance.PlaySound(collectiblesSound);
            gameObject.SetActive(false);
        }
    }
}
