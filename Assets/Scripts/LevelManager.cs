using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LevelManager : MonoBehaviour
{

    public List<Color> colors;
    public GameObject playerPrefab;
    public GameObject botPrefab;
    public Bounds spawnBounds;
    public Transform world;
    public TextAsset namesFile;

    private List<string> names;
    private List<UFOController> players;
    private PlayerController mainPlayer;

	public delegate void Win(UFOController winner);
	public static event Win OnWin;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        UIManager.OnStartGame += CanPlay;
        UFOController.OnKill += CantPlay;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        UIManager.OnStartGame -= CanPlay;
        UFOController.OnKill -= CantPlay;
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        names = namesFile.text.Split('\n').ToList();
        world.RotateAround(Vector3.zero, Vector3.up, Random.Range(0, 360));
        SpawnPlayers();
    }


    public List<UFOController> GetPlayers()
    {
        return players;
    }

    public PlayerController MainPlayer()
    {
        return mainPlayer;
    }

    public int IndexOfPlayer()
    {
        return SortedPlayers().IndexOf(mainPlayer);
    }

    public List<UFOController> SortedPlayers()
    {
        // players.Sort((x, y) => -1 * x.xp.CompareTo(y.xp));
        return players.OrderByDescending((x)=>x.xp).ToList();
    }

    private void CanPlay()
    {
        foreach (var player in players)
        {
            player.CanPlay = true;
            player.sucker.gameObject.SetActive(true);
        }
    }

    private void CantPlay(UFOController ufo, UFOController target)
    {
        if (target is PlayerController)
        {

            foreach (var player in players)
            {
                player.CanPlay = false;
                player.sucker.gameObject.SetActive(false);
            }
            // target.CanPlay = false;
            // target.gameObject.SetActive(false);
        }
        else
        {
			target.CanPlay = false;
            target.gameObject.SetActive(false);
			var playingPlayers = players.Where((x)=>x.CanPlay);
			if(playingPlayers.Count() == 1)
			{
				var player = playingPlayers.First();
				player.CanPlay = false;
				player.sucker.gameObject.SetActive(false);
				OnWin(player);
			}
        }


    }

    private void SpawnPlayers()
    {
        players = new List<UFOController>();
        var min = spawnBounds.min;
        var max = spawnBounds.max;
		var rot = 0;// Random.Range(0, 360);

        for (int i = 0; i < 9; i++)
        {
            SpawnPlayer(botPrefab, min, max, rot);
        }
        mainPlayer = (PlayerController)SpawnPlayer(playerPrefab, min, max, rot, PlayerPrefs.GetString("username"));// PlayerPrefs.GetString("player_name"));

        // UFOController.TOP_PLAYER = players[0];
    }

    private UFOController SpawnPlayer(GameObject prefab, Vector3 min, Vector3 max, float rotateAngle, string name = "")
    {
        var player = Instantiate(prefab, GetRandomPosition(min, max), Quaternion.Euler(Vector3.up * rotateAngle)).GetComponent<UFOController>();

        int r1 = Random.Range(0, colors.Count);
        player.Color = colors[r1];
        colors.RemoveAt(r1);

        if (name == "")
        {
            int r2 = Random.Range(0, names.Count);
            player.Name = names[r2];
            names.RemoveAt(r2);
        }
        else
        {
            player.Name = name;
        }
		player.playerNameText.text = player.Name;

        players.Add(player);
        return player;
    }

    private Vector3 GetRandomPosition(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(-min.x, -max.x), 3f, Random.Range(-min.z, -max.z));
    }
}
