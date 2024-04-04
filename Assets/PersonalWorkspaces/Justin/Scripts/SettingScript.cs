using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SettingScript : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    public GameObject player;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivityX = player.GetComponent<PlayerControllerNEW>().m_sensitivityX;
        mouseSensitivityY = player.GetComponent<PlayerControllerNEW>().m_sensitivityY;

        sensitivitySlider.onValueChanged.AddListener((v) => {
            sensitivityText.text = v.ToString("0.0");
        });

        //mouseSensitivity = PlayerPrefs.GetFloat("currentSensitivity", 100f);
        //sensitivitySlider.value = mouseSensitivity / 10;

        sensitivitySlider.onValueChanged.AddListener((v) => {
            
            adjustSensitivity(sensitivitySlider.value);
        });

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("hm" + mouseSensitivity);
    }

    public void adjustSensitivity(float sliderValue)
    {
        //mouseSensitivity = (mouseSensitivity * (sliderValue*1.25f));
        mouseSensitivityX = sliderValue * 60f;
        mouseSensitivityY = sliderValue * 60f;
    }
}
