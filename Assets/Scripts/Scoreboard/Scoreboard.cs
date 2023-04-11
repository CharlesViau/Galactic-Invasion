using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public sealed class Scoreboard
{
    private const string Password = "n#a39dLm5WNDS#t3";
    private const string Username = "scoreboard";
    private static Scoreboard _instance;
    private static readonly object Lock = new object();
    private string _token;

    public static Scoreboard Instance
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new Scoreboard();
            }
        }
    }

    private static IEnumerator GetAccessToken(Action<string> callback)
    {
        var form = new WWWForm();

        form.AddField("username", Username);
        form.AddField("password", Password);

        var request = UnityWebRequest.Post("http://galacticinvasion.duckdns.org:8000/token", form);

        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
            callback(JsonUtility.FromJson<Token>(request.downloadHandler.text).access_token);

        request.Dispose();
    }

    public IEnumerator SetScore(string player, int score, Action<Score> callback, Action<string> errorCallback)
    {
        yield return GetAccessToken(accessToken => { _token = accessToken; });

        var request = new UnityWebRequest("http://galacticinvasion.duckdns.org:8000/scores", "POST");
        var body = Encoding.UTF8.GetBytes("{\"player\":\"" + player + "\", \"score\":\"" + score + "\"}");

        request.SetRequestHeader("Authorization", "Bearer " + _token);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            errorCallback(request.error);
        else
            callback(JsonUtility.FromJson<Score>(request.downloadHandler.text));

        request.Dispose();
    }

    public IEnumerator GetScores(Action<Scores> callback, int offset = 0, int limit = 10)
    {
        var request = UnityWebRequest.Get("http://galacticinvasion.duckdns.org:8000/scores?skip=" + offset +
                                          "&limit=" + limit);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
            callback(JsonUtility.FromJson<Scores>("{\"scores\":" + request.downloadHandler.text + "}"));

        request.Dispose();
    }


    [Serializable]
    private class Token
    {
        public string access_token;
    }

    [Serializable]
    public class Score
    {
        public string player;
        public int score;
        public int rank;
    }


    [Serializable]
    public class Scores
    {
        public List<Score> scores;
    }
}