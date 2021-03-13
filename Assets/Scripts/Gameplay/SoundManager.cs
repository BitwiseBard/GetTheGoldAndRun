using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static AudioClip downStairs, treasure, gunShot, monster, start, tileMove, upStairs;
    static AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        downStairs = Resources.Load<AudioClip>("downStairs");
        treasure = Resources.Load<AudioClip>("treasure");
        gunShot = Resources.Load<AudioClip>("gunShot");
        monster = Resources.Load<AudioClip>("monster");
        start = Resources.Load<AudioClip>("start");
        tileMove = Resources.Load<AudioClip>("tileMove");
        upStairs = Resources.Load<AudioClip>("upStairs");

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip) {
        switch(clip) {
            case "downStairs":
                audioSource.PlayOneShot(downStairs);
                break;
            case "treasure":
                audioSource.PlayOneShot(treasure);
                break;
            case "gunShot":
                audioSource.PlayOneShot(gunShot);
                break;
            case "monster":
                audioSource.PlayOneShot(monster);
                break;
            case "start":
                audioSource.PlayOneShot(start);
                break;
            case "tileMove":
                audioSource.PlayOneShot(tileMove);
                break;
            case "upStairs":
                audioSource.PlayOneShot(upStairs);
                break;
            default:
                break;
        }
    }
}
