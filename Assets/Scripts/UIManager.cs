using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject xpTextPrefab;
    public GameObject lvlupTextPrefab;
    public GameObject killTextPrefab;
    public Transform lvlupContainer;
    public Transform playersContainer;
    public GameObject WaitPanel;
    public Text WaitText;
    public GameObject GameOverPanel;
    public Text PlaceText;
    public GameObject Crown;
    // public GameObject WinPanel;
    // public Text WinnerText;

    public GameObject arrowPrefab;
    public Transform arrowContainer;

    public LevelManager levelManager;

    private List<RectTransform> arrows;

    private List<UFOController> players;

    public static event System.Action OnStartGame;

    void Awake()
    {
        OnStartGame += StartGame;

        LevelManager.OnWin += Win;
        UFOController.OnKill += GameOver;
        UFOController.OnStatsUpdate += AddXp;
        arrows = new List<RectTransform>();

    }

    void OnDestroy()
    {
        OnStartGame -= StartGame;
        LevelManager.OnWin -= Win;
        UFOController.OnKill -= GameOver;
        UFOController.OnStatsUpdate -= AddXp;
    }

    IEnumerator Start()
    {
        for (int i = 3; i > 0; i--)
        {
            WaitText.text = i + "";
            yield return new WaitForSeconds(1f);
        }
        OnStartGame();

        players = levelManager.GetPlayers();


        for (int i = 0; i < 10; i++)
        {
            var g = Instantiate(arrowPrefab, arrowContainer);
            g.GetComponent<Image>().color = players[i].Color;
            //TODO name

            arrows.Add(g.GetComponent<RectTransform>());
        }
    }



    private void StartGame()
    {
        WaitPanel.SetActive(false);
        playersContainer.gameObject.SetActive(true);

        StatsUpdate();

    }

    private void GameOver(UFOController ufo, UFOController target)
    {
        if (target is PlayerController)
        {
            ShowFinalPanel();
        }else if(ufo is PlayerController){
            Destroy(Instantiate(killTextPrefab, lvlupContainer), 1f);
        }
        StatsUpdate();
    }

    private void Win(UFOController winner)
    {

        ShowFinalPanel();
        Crown.SetActive(true);

    }

    private void ShowFinalPanel()
    {
        playersContainer.gameObject.SetActive(false);
        var place = levelManager.IndexOfPlayer() + 1;
        var sufix = "";
        switch (place)
        {
            case 1:
                sufix = "st";
                break;
            case 2:
                sufix = "nd";
                break;
            case 3:
                sufix = "rd";
                break;
            default:
                sufix = "th";
                break;

        }
        PlaceText.text = place + sufix + " place";

        GameOverPanel.SetActive(true);
    }

    private void AddXp(UFOController ufo, int xp, bool up)
    {
        if (ufo is PlayerController)
        {
            var t = Instantiate(xpTextPrefab, ((PlayerController)ufo).xpContainer);
            t.GetComponent<Text>().text = "+" + xp;
            Destroy(t, 1f);
            if (up)
                Destroy(Instantiate(lvlupTextPrefab, lvlupContainer), 1f);
        }
        StatsUpdate();
    }

    private void StatsUpdate()
    {
        List<UFOController> players = levelManager.SortedPlayers();

        // UFOController.TOP_PLAYER = players[0];
        players[0].SetTopPlayer();
        int c = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if(c == 5)
                break;

            var child = playersContainer.GetChild(c);
            var player = players[i];
            if (player.CanPlay)
            {
                c++;
                child.gameObject.SetActive(true);

                child.GetComponentInChildren<Image>().color = player.Color;
                child.GetComponentInChildren<Text>().text = string.Format("{0} {1}-{2} xp", (c + 1), player.Name, player.xp);
                
            }
            else
            {
                child.gameObject.SetActive(false);
            }

            
        }


    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        for (int i = 0; i < arrows.Count; i++)
        {
            if (players[i].CanPlay)
            {
                arrows[i].gameObject.SetActive(true);

                var screenPos = Camera.main.WorldToViewportPoint(players[i].transform.position);

                if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
                {
                    // Debug.Log("already on screen, don't bother with the rest!");
                    arrows[i].gameObject.SetActive(false);
                    continue;
                }

                arrows[i].position = UpdateTargetIconPosition(screenPos);
                
                // var screenPos = Camera.main.WorldToViewportPoint(players[i].transform.position); //get viewport positions


                // // Debug.Log(screenPos);

                // // var worldToViewportPoint = Camera.main.WorldToViewportPoint (players[i].transform.position);
                // // returns coming from upper left
                // //worldToViewportPoint = (-0.1, 0.5, 14.8), viewportToScreenPoint =(-66.1, 361.3, 14.8)
                                       
                // var screenPosClamped  = new Vector3(Mathf.Clamp(screenPos.x, 0.0f, 1.0f), Mathf.Clamp(screenPos.y, 0.0f, 1.0f), 0);
                // Debug.Log(screenPosClamped);
               
                // var v3 = new Vector3(screenPosClamped.x * Screen.width, screenPosClamped.y * Screen.height, 0);
                                                                       
                // arrows[i].position = v3;

                // no
                //Debug.Log("enemy " + allenemies.name + " out of view at worldToViewportPoint = " + worldToViewportPoint + ", v3 =" + v3);
                // break;

                // var onScreenPos = new Vector2(screenPos.x - 0.5f, screenPos.y - 0.5f) * 2; //2D version, new mapping
                // var max = Mathf.Max(Mathf.Abs(onScreenPos.x), Mathf.Abs(onScreenPos.y)); //get largest offset
                // onScreenPos = (onScreenPos / (max * 2)) + new Vector2(0.5f, 0.5f); //undo mapping
                // // Debug.Log(onScreenPos);
                // arrows[i].position = Camera.main.ViewportToScreenPoint(onScreenPos);
            }
            else
            {
                // Debug.LogError(i);
                arrows[i].gameObject.SetActive(false);
            }
        }
    }


    private Vector3 UpdateTargetIconPosition(Vector3 newPos)
    {
        if(newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 0;
        newPos = Vector3Maxamize(newPos);
        }
        newPos = Camera.main.ViewportToScreenPoint(newPos);
        newPos.x = Mathf.Clamp(newPos.x, m_edgeBuffer, Screen.width - m_edgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, m_edgeBuffer, Screen.height - m_edgeBuffer);
        newPos.z = 0f;
        return newPos;
    }

    public Vector3 Vector3Maxamize(Vector3 vector)
       {
       Vector3 returnVector = vector;
       float max = 0;
       max = vector.x > max ? vector.x : max;
       max = vector.y > max ? vector.y : max;
       max = vector.z > max ? vector.z : max;
       returnVector /= max;
       return returnVector;
       }

    [Range(0, 100)]
    public float m_edgeBuffer;
    
}
