//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour
{
    WeaponManager m_weapon;
    [SerializeField] string[] m_neuro = new string[] { "adrenaline", "gaba", "dopamine", "glutamate" };
    [SerializeField] GameObject[] m_neuroObjects = new GameObject[4];
    public string currentNeuro;
    private int m_index = 0;
    private Vector3 m_destination;
    [SerializeField] float m_projectileSpeed = 500f;

    private void Awake()
    {
        currentNeuro = m_neuro[0];
        m_weapon = GetComponent<WeaponManager>();
    }

    public void SecondaryFire() // Fire Neurotransmitter
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            m_destination = hit.point;
        else
            m_destination = ray.GetPoint(1000);

        InstantiateProjectile(m_weapon.bulletTransform);

    }

    public void Ability1() // Switch neuro
    {
        if (m_index < 3)
            m_index++;
        else
            m_index = 0;

        currentNeuro = m_neuro[m_index];
    }

    private void InstantiateProjectile(Transform firePoint)
    {
        var currentNeuroObj = Instantiate(m_neuroObjects[m_index], firePoint.position, Quaternion.identity) as GameObject;
        currentNeuroObj.GetComponent<Rigidbody>().velocity = (m_destination - firePoint.position).normalized * m_projectileSpeed;
    }

    public void Passive()
    {
        //Need to implement health first
    }
}
