//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TCell : MonoBehaviour
{
    [Header("Input Variables")]
    InputActions m_input;

    [Header("Ability Variables")]
    Camera m_cam;
    PlayerController m_player;
    private bool m_isScoped = false;
    [SerializeField] Image m_scopeOverlay;
    [SerializeField] Image m_crosshairUI;
    [SerializeField] GameObject m_gunModel;

    [SerializeField] LayerMask m_enemyMask;

    void Awake()
    {
        m_cam = Camera.main;
        m_input = new InputActions();
        m_player = GetComponent<PlayerController>();

        m_input.Ability.Ability1.performed += ctx => Ability1();
        m_input.Ability.SecondaryFire.performed += ctx => SecondaryFire();
    }

    private void Start()
    {
        m_gunModel = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        m_input.Enable();
    }

    private void OnDisable()
    {
        m_input.Disable();
    }

    private void Ability1()
    {
        RaycastHit hit;

        if(Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, 1000f, m_enemyMask))
        {
            hit.transform.GetComponent<EnemyStats>().marked = true;
        }

    }

    private void SecondaryFire()
    {
        m_isScoped = !m_isScoped;

        if(m_isScoped)
        {
            m_player.canFOV = false;
            m_scopeOverlay.enabled = true;
            m_crosshairUI.enabled = false;
            m_gunModel.SetActive(false);
            m_cam.fieldOfView = 30;
        }
        else
        {
            m_player.canFOV = true;
            m_scopeOverlay.enabled = false;
            m_crosshairUI.enabled = true;
            m_gunModel.SetActive(true);
            m_cam.fieldOfView = 60;
        }
    }

}
