using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	public Button startButton;
	public InputField usernameInput;
	public Slider slider;
	public Text connectingText;

	public GameObject StartPanel;
	public GameObject LoadingPanel;

	private float dest;
	// Use this for initialization
	void Start () {
		dest = .1f;
		usernameInput.text = PlayerPrefs.GetString("username", "Player");

		startButton.onClick.AddListener(()=>{
			PlayerPrefs.SetString("username", usernameInput.text);
			connectingText.text = string.Format(connectingText.text, usernameInput.text);
			StartPanel.SetActive(false);
			LoadingPanel.SetActive(true);
			StartCoroutine(Loading());
		});
		
	}

	IEnumerator Loading()
	{
		yield return new WaitForSeconds(.2f);
		dest = .2f;
		yield return new WaitForSeconds(1f);
		dest = .6f;
		yield return new WaitForSeconds(.4f);
		dest = .9f;
		yield return new WaitForSeconds(1f);
		dest = 1f;
		yield return new WaitForSeconds(.3f);

		SceneManager.LoadScene("Game");

	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		slider.value = Mathf.Lerp(slider.value, dest, Time.deltaTime*3);
	}
	
	
}
