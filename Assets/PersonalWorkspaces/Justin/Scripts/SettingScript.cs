using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SettingScript : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    [SerializeField] private Slider soundSlider;
    [SerializeField] private TextMeshProUGUI soundText;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        sensitivitySlider.onValueChanged.AddListener((v) => {
            sensitivityText.text = v.ToString("0.0");
        });

        soundSlider.onValueChanged.AddListener((v) => {
            soundText.text = v.ToString("0.0");
        });

        sensitivitySlider.onValueChanged.AddListener((v) => {
            
            adjustSensitivity(sensitivitySlider.value);
        });

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("hm" + player.GetComponent<PlayerControllerNEW>().m_sensitivityX);
        //currentPlayer.transform.GetChild(2).GetChild(1).GetChild(8).GetChild(3).GetChild(0).gameObject.SetActive(false);
        //currentPlayer.transform.GetChild(2).GetChild(1).GetChild(8).GetChild(3).GetChild(1).gameObject.SetActive(true);
    }

    public void adjustSensitivity(float sliderValue)
    {
        //mouseSensitivity = (mouseSensitivity * (sliderValue*1.25f));
        player.GetComponent<PlayerControllerNEW>().m_sensitivityX = sliderValue * 60f;
        player.GetComponent<PlayerControllerNEW>().m_sensitivityY = sliderValue * 60f;
    }
}
