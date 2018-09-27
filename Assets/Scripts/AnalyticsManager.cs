using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using GameAnalyticsSDK;
using System;

public class AnalyticsManager : MonoBehaviour
{

    public static AnalyticsManager Instance;

    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        GameAnalytics.Initialize();

        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }

        StartEvent();

    }


    #region FACEBOOK

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...


            var param = new Dictionary<string, object>();
            param[AppEventParameterName.ContentID] = "game";
            param[AppEventParameterName.Success] = "1";

            FB.LogAppEvent(
                AppEventName.CompletedRegistration,
                parameters: param
            );
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    #endregion


    #region GAME_ANALYTICS
    public void GameOverEvent(int score)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", score);

        var param = new Dictionary<string, object>();
        param[AppEventParameterName.ContentID] = "game";
        param[AppEventParameterName.Success] = "0";

        FB.LogAppEvent(
            AppEventName.CompletedRegistration,
            parameters: param
        );
    }
    public void GameOverPlaceEvent(int place)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "place", place);

        var param = new Dictionary<string, object>();
        param[AppEventParameterName.ContentID] = "place";
        param[AppEventParameterName.Success] = "1";
        param[AppEventParameterName.Level] = place+"";

        FB.LogAppEvent(
            AppEventName.CompletedRegistration,
            parameters: param
        );
    }

    public void StartEvent()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");

        
    }

    public void HighScoreEvent(int score)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "highscore", score);
    }


    #endregion

}
