using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static string JUMP = "jump";
    public static string FALL = "redneck laugh";
    public static string COIN = "coin";
    private string[] sounds = { JUMP, FALL, COIN };
    private static Dictionary<string, AudioClip> map = new Dictionary<string, AudioClip>();
    static AudioSource source;
    static AudioClip jmp;

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            map.Add(sounds[i], Resources.Load<AudioClip>("Sound/" + sounds[i]));
        }
        source = GetComponent<AudioSource>();
    }

    public static void PlaySound (string sound)
    {
        source.PlayOneShot(map[sound]);
    }
}
