using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	// Sounds array, set up through the inspector, useful for looping through audio instances
	public Sound[] sounds;

	// Dictionary of sounds to their name, useful for playing without having to find the audio track
	Dictionary<string, Sound> library;

	// Instance variable of the current BGM
	private Sound currentBGM;

	// Volume values
	public float masterVolume = 1f;
	public float musicVolume = 1f;
	public float effectsVolume = 1f;

	// Default pitch value, useful for changing pitch of sound effects randomly based on the default value
	private float defaultPitch = 1f;

	// Initialize manager
	void Awake() 
	{
		// Initialize library from all sounds in sound array
		// Sounds array must be populated from inspector
		this.initLibrary();

		// Load any saved sound settings from player prefs
		this.loadSavedSoundSettings();
	}

	// Plays sound by name
	public void Play(string name) 
	{
		if(this.library.ContainsKey(name)) 
		{
			// Get the sound by name
			Sound s = this.library[name];

			// Check if the sound is SFX or BGM
			if(s.isSFX) 
			{
				// Randomize the effects volume and pitch by their random values
				s.source.volume = this.effectsVolume * (1 + Random.Range(-s.randomVolumeValue / 2f, s.randomVolumeValue / 2f));
				s.source.pitch = this.defaultPitch * (1 + Random.Range(-s.randomPitchValue / 2f, s.randomPitchValue / 2f));
				
				// Play the effect
				s.source.Play();
			} 
			else 
			{
				// Stop all other songs
				this.StopPlayingBGMS();
				
				// Play the song
				s.source.Play();
			}

			// Return early
			return;
		}

		// If we get to this point, then we have no sound file for the given name
		Debug.Log("No sound file found for name: " + name);
	}

	// Function to change the play state of the current bgm
	public void ChangePlayStateOfBGM() 
	{
		// Get current play state of the current BGM in boolean form
		bool isPlaying = this.currentBGM.source.isPlaying;

		// Check if the current BGM is playing
		if(isPlaying) 
		{
			// If so, then we pause its source
			this.currentBGM.source.Pause();
		} 
		else 
		{
			// If not, then we play its source
			this.currentBGM.source.Play();
		}
	}

	// Function called to set the audio managers master volume
	public void setMasterVolume(float volume)
	{
		this.masterVolume = volume;

		this.updateVolume();
	}

	// Function called to set the audio managers music volume
	public void setMusicVolume(float volume) 
	{
		// Set new music volume value
		this.musicVolume = volume;

		// Change all music sound instances to new volume
		this.updateVolume();
	}

	// Function called to set the audio managers effects volume
	public void setEffectsVolume(float volume) 
	{
		// Set new music volume value
		this.effectsVolume = volume;

		// Change all effects sound instances to new volume
		this.updateVolume();
	}

	// Loops through and resets the sound sources to the correct current volume
	private void updateVolume() 
	{
		// Loop through each sound
		foreach(Sound s in sounds) 
		{
			// Check whether the sound is SFX
			if(s.isSFX) 
			{
				// If so we set the sounds volume to the effects volume
				s.source.volume = this.effectsVolume * this.masterVolume;
			} 
			else 
			{
				// If not, then we set the sounds volume to the music volume
				s.source.volume = this.musicVolume * this.masterVolume;
			}
		}

		this.saveSoundSettings();
	}

	// Initializes sound dictionary "library" using the sounds array set through inspector
	private void initLibrary() 
	{
		// Instantiate a new dictionary
		this.library = new Dictionary<string, Sound>();

		// Add all sound instances into dictionary
		foreach(Sound s in this.sounds) 
		{
			// Add sound to library
			this.library[s.name] = s;

			// We also want to create all the audio sources here if they have not been created already
			if(!s.source) 
			{
				// Add a source object
				s.source = this.gameObject.AddComponent<AudioSource>();

				// Set source variables
				s.source.clip = s.clip;
				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.isLooping;
			}
		}
	}

	private void StopPlayingBGMS()
	{
		foreach(Sound s in this.sounds) 
		{
			if(!s.isSFX)
			{
				s.source.Stop();
			}
		}
	}

	// Gets settings related to audio, if none saved, then everything is still set to default
	private void loadSavedSoundSettings() {
		// Getting volume settings from player prefs
		this.masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
		this.musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
		this.effectsVolume = PlayerPrefs.GetFloat("effectsVolume", 1f);
	}

	// Saves sound settings to player prefs
	private void saveSoundSettings() {
		// Saving volume settings into player prefs
		PlayerPrefs.SetFloat("masterVolume", this.masterVolume);
		PlayerPrefs.SetFloat("musicVolume", this.musicVolume);
		PlayerPrefs.SetFloat("effectsVolume", this.effectsVolume);
		PlayerPrefs.Save();
	}
}