using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	// Manager instances
	private InputManager inputManager;
	private AudioManager audioManager;
	private GameManager gameManager;

	// Variable for holding currently selected item in menu
	public GameObject currentlySelectedObject;

	// EventSystem variable, in-built unity stuff
	public EventSystem eventSystem;	

	// Boolean variable to hold information on whether or not a button has been selected
	private bool buttonSelected = false;


	// For use with options menu
	public Slider masterVolumeSlider;
	public Slider musicVolumeSlider;
	public Slider effectsVolumeSlider;

	// Even earlier initialization
	void Awake () {
		// Get singleton manager instances
		this.inputManager = Object.FindObjectOfType<InputManager>();
		this.audioManager = Object.FindObjectOfType<AudioManager>();
		//this.audioManager.Play("MainMenuTheme");
		this.gameManager = Object.FindObjectOfType<GameManager>();

		if(masterVolumeSlider)
		{
			this.masterVolumeSlider.value = this.audioManager.masterVolume;
		}

		if(musicVolumeSlider)
		{
			this.musicVolumeSlider.value = this.audioManager.musicVolume;
		}

		if(effectsVolumeSlider)
		{
			this.effectsVolumeSlider.value = this.audioManager.effectsVolume;
		}
	}

	// Update is called once per frame
	void Update () {
		// Check for input on vertical axis
		if(this.inputManager.vertical.GetInputRaw() != 0 && buttonSelected == false) {
			this.eventSystem.SetSelectedGameObject(currentlySelectedObject);
			this.buttonSelected = true;
		}
	}

		// When disabled
	private void OnDisable() {
		// button selection is set to false
		buttonSelected = false;
		if(this.audioManager) {
			//this.audioManager.Play("Fire!");
		}
	}

	public void Play() {
		this.gameManager.StartMatch();
	}

	public void Menu() {
		this.gameManager.Menu();
	}

	public void Replay()
	{
		this.gameManager.Replay();
	}

	public void ChangeMasterVolumeLevels()
	{
		this.audioManager.setMasterVolume(this.masterVolumeSlider.value);
	}

	public void ChangeMusicVolumeLevels()
	{
		this.audioManager.setMusicVolume(this.musicVolumeSlider.value);
	}

	public void ChangeEffectsVolumeLevels()
	{
		this.audioManager.setEffectsVolume(this.effectsVolumeSlider.value);
	}

	// Quit() function triggered on click of Quit menu button
	public void Quit() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}