using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Token
{
    public string access_token;
    public string token_type;
}

public class Player : MonoBehaviour
{
    public string player;
    private int score;
    private string token;


    private void Reset()
    {
        score = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            score++;
            Debug.Log("Score: " + score);
        }

        if (Input.GetKeyDown(KeyCode.G)) GetScores();
        if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Authenticate());
        if (Input.GetKeyDown(KeyCode.P)) StartCoroutine(PostScores());
    }

    private void GetScores()
    {
        StartCoroutine(GetRequest("http://galacticinvasion.duckdns.org:8000/scores",
            response => { Debug.Log(response); }));
    }

    private IEnumerator Authenticate()
    {
        var formData = new Dictionary<string, string>();
        formData.Add("username", "scoreboard");
        formData.Add("password", "n#a39dLm5WNDS#t3");
        yield return StartCoroutine(PostRequestForm("http://galacticinvasion.duckdns.org:8000/token", formData,
            response =>
            {
                token = JsonUtility.FromJson<Token>(response).access_token;
                Debug.Log(token);
            }));
    }

    private IEnumerator PostScores()
    {
        if (token == null) yield return Authenticate();

        StartCoroutine(PostRequestAuth("http://galacticinvasion.duckdns.org:8000/scores",
            "{\"player\":\"" + player + "\", \"score\":\"" + score + "\"}", token,
            response => { Debug.Log(response); }));
    }

    private static IEnumerator GetRequest(string url, Action<string> callback)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError("API request failed: " + request.error);
            else
                callback(request.downloadHandler.text);
        }
    }

    private IEnumerator PostRequestForm(string url, Dictionary<string, string> formData, Action<string> callback)
    {
        var form = new WWWForm();
        foreach (var field in formData) form.AddField(field.Key, field.Value);

        var request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            callback(request.downloadHandler.text);
        else
            Debug.LogError("API request failed: " + request.error);
    }

    private static IEnumerator PostRequestAuth(string url, string body, string token, Action<bool> callback)
    {
        var request = new UnityWebRequest(url, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(body);

        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("API request failed: " + request.error);
        else
            callback(true);
    }
}