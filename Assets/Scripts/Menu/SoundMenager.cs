using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenager : MonoBehaviour
{

    public GameObject audioPrefab;
    public AudioClip playerDamage;
    public AudioClip buttonClicked;
    public AudioClip purchaseSound;
    public AudioClip submarineExplosion;
    public AudioClip pickUp;
    public AudioClip repairSound;
    public AudioClip frictionSound;

    public void PlayerDamagedSound()
    {
        GameObject temp = Instantiate(audioPrefab);
        temp.GetComponent<AudioSource>().clip = playerDamage;
        temp.GetComponent<AudioSource>().Play();
        StartCoroutine(destroySource(temp));
    }

    public void ButtonClickSound()
    {
        GameObject temp = Instantiate(audioPrefab);
        temp.GetComponent<AudioSource>().clip = buttonClicked;
        temp.GetComponent<AudioSource>().Play();
        StartCoroutine(destroySource(temp));
    }

    public void PurchaseSound()
    {
        GameObject temp = Instantiate(audioPrefab);
        temp.GetComponent<AudioSource>().clip = purchaseSound;
        temp.GetComponent<AudioSource>().Play();
        StartCoroutine(destroySource(temp));
    }

    public void SubmarineCrashSound()
    {
        GameObject temp = Instantiate(audioPrefab);
        temp.GetComponent<AudioSource>().clip = submarineExplosion;
        temp.GetComponent<AudioSource>().Play();
        StartCoroutine(destroySource(temp));
    }

    public void PickUpSound()
    {
        GameObject temp = Instantiate(audioPrefab);
        temp.GetComponent<AudioSource>().clip = pickUp;
        temp.GetComponent<AudioSource>().Play();
        StartCoroutine(destroySource(temp));
    }

    public void FrictionSound()
    {
        GameObject temp = Instantiate(audioPrefab);
        temp.GetComponent<AudioSource>().clip = frictionSound;
        temp.GetComponent<AudioSource>().Play();
        StartCoroutine(destroySource(temp));
    }

    public void RepairSound()
    {
        GameObject temp = Instantiate(audioPrefab);
        temp.GetComponent<AudioSource>().clip = repairSound;
        temp.GetComponent<AudioSource>().Play();
        StartCoroutine(destroySource(temp));
    }

    IEnumerator destroySource(GameObject temp)
    {
        yield return new WaitForSecondsRealtime(temp.GetComponent<AudioSource>().clip.length);
        Destroy(temp);
    }
}
