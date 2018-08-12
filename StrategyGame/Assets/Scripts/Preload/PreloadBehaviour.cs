using UnityEngine;
using UnityEngine.SceneManagement;

// Class that defines the behaviour of the preload scene
// The preload scene hosts the __app object that contains all game related manager classes
public class PreloadBehaviour : MonoBehaviour {

	//TODO: After implementing game manager, use it to switch scenes and update game state
	//private GameManager gameManager;

	void Awake() 
	{
		// Keep the object, ie. the __app game object, alive throughout all scene changes
		DontDestroyOnLoad(gameObject);
	}

	void Start () {
		//TODO: After implementing game manager, ...
		//this.gameManager = GetComponent<GameManager>();
		
		//TODO: After implementing game manager, ... use that basically instead of this
		SceneManager.LoadScene("Match");
	}
}