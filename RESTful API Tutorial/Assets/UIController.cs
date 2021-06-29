using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject WeatherController;
    public Text OutputField;
    public InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitText()
    {
        WeatherController.GetComponent<WeatherController>().CheckAndGetStatus(inputField.text, OutputField);
    }
}
