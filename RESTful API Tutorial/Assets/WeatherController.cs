using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using Assets;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour
{
    private const string API_KEY= "256d5c500a9b88887ec0a35240e560ce";
    private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes

    public string CityId = "Phoenix";
    public GameObject SnowSystem;
    public GameObject RainSystem;
    public Camera Camera;
    public Material ClearSkybox;
    public Material CloudySkybox;
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    private String lastFoundWeather;


    // Start is called before the first frame update
    void Start()
    {
        CheckSnowStatus();
    }

    // Update is called once per frame
    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CheckSnowStatus();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }
    }

    private async Task<WeatherInfo> GetWeather()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID={1}", CityId, API_KEY));
        HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        return info;
    }

    public async void CheckSnowStatus()
    {
        bool snowing = (await GetWeather()).weather[0].main.Equals("Snow");
        if (snowing)
            SnowSystem.SetActive(true);
        else
            SnowSystem.SetActive(false);
    }


    public async void CheckAndGetStatus(string city, Text OutputField)
    {
        CityId = city;
        apiCheckCountdown = API_CHECK_MAXTIME;
        string weatherString = (await GetWeather()).weather[0].main.ToString();
        Debug.Log(weatherString);
        OutputField.text = weatherString;
        if (weatherString.Equals("Snow"))
        {
            SnowSystem.SetActive(true);
            RainSystem.SetActive(false);
            RenderSettings.skybox = CloudySkybox;
        }
        else if (weatherString.Equals("Clear"))
        {
            SnowSystem.SetActive(false);
            RainSystem.SetActive(false);
            RenderSettings.skybox = ClearSkybox;
        }
        else if (weatherString.Equals("Rain"))
        {
            SnowSystem.SetActive(false);
            RainSystem.SetActive(true);
            RenderSettings.skybox = CloudySkybox;
        }
        else if (weatherString.Equals("Clouds"))
        {
            SnowSystem.SetActive(false);
            RainSystem.SetActive(false);
            RenderSettings.skybox = CloudySkybox;
        }
    }
}
