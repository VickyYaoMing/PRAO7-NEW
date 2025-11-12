using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

	[SerializeField] private AudioSource sourceOfSound;

	[SerializeField] private float masterVolume = 1.0f;

	// guy on youtube told me to have this Awake function 💀
	// https://youtu.be/DU7cgVsU2rM?si=tl4a7eB1OQXAYYD-&t=232
	public void Awake()
	{
		if(instance == null)
			instance = this;
	}

	public void PlaySound(AudioClip sound, Transform spawn)
	{
		AudioSource source = Instantiate(sourceOfSound, spawn.position, Quaternion.identity);

		source.clip = sound;
		source.volume = masterVolume;
		source.Play();

		Destroy(source.gameObject, source.clip.length);
	}
}
