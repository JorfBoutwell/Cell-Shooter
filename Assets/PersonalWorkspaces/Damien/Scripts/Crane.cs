using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    public bool Direction = false;
    public float Speed = 1;
    public GameObject Pole;
    public GameObject Wire0;
    public GameObject Wire1;
    public GameObject Wire2;
    public GameObject Wire3;
    public GameObject Goober0;
    public GameObject Goober1;
    public GameObject Goober2;
    public GameObject Goober3;
    private float Delta = 0;
    private float Pause = 0;
    private List<GameObject> Players = new List<GameObject>();
    private GameObject[] All;

    // Start is called before the first frame update
    void Start()
    {
        All = GameObject.FindObjectsOfType<GameObject>();
        for (int i = 0; i < All.Length; i++)
        {
            if (All[i].name.Contains("PlayerGENERIC"))
            {
                Players.Add(All[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Delta = Time.deltaTime;

        if (transform.position.z > -351.82 || transform.position.z < -393.58)
        {
            Direction = !Direction;
            Pause = 1;

            if (transform.position.z > -351.82)
            {
                transform.position -= new Vector3(0, 0, .1f);
                Pole.transform.position -= new Vector3(0, 0, .1f);
                Wire0.transform.position -= new Vector3(0, 0, .1f);
                Wire1.transform.position -= new Vector3(0, 0, .1f);
                Wire2.transform.position -= new Vector3(0, 0, .1f);
                Wire3.transform.position -= new Vector3(0, 0, .1f);
                Goober0.transform.position -= new Vector3(0, 0, .1f);
                Goober1.transform.position -= new Vector3(0, 0, .1f);
                Goober2.transform.position -= new Vector3(0, 0, .1f);
                Goober3.transform.position -= new Vector3(0, 0, .1f);
            }
            else
            {
                transform.position += new Vector3(0, 0, .1f);
                Pole.transform.position += new Vector3(0, 0, .1f);
                Wire0.transform.position += new Vector3(0, 0, .1f);
                Wire1.transform.position += new Vector3(0, 0, .1f);
                Wire2.transform.position += new Vector3(0, 0, .1f);
                Wire3.transform.position += new Vector3(0, 0, .1f);
                Goober0.transform.position += new Vector3(0, 0, .1f);
                Goober1.transform.position += new Vector3(0, 0, .1f);
                Goober2.transform.position += new Vector3(0, 0, .1f);
                Goober3.transform.position += new Vector3(0, 0, .1f);
            }
        } 

        if (Pause <= 0)
        {
            if (Direction)
            {
                transform.position += new Vector3(0, 0, Speed * Delta);
                Pole.transform.position += new Vector3(0, 0, Speed * Delta);
                Wire0.transform.position += new Vector3(0, 0, Speed * Delta);
                Wire1.transform.position += new Vector3(0, 0, Speed * Delta);
                Wire2.transform.position += new Vector3(0, 0, Speed * Delta);
                Wire3.transform.position += new Vector3(0, 0, Speed * Delta);
                Goober0.transform.position += new Vector3(0, 0, Speed * Delta);
                Goober1.transform.position += new Vector3(0, 0, Speed * Delta);
                Goober2.transform.position += new Vector3(0, 0, Speed * Delta);
                Goober3.transform.position += new Vector3(0, 0, Speed * Delta);
            }
            else
            {
                transform.position -= new Vector3(0, 0, Speed * Delta);
                Pole.transform.position -= new Vector3(0, 0, Speed * Delta);
                Wire0.transform.position -= new Vector3(0, 0, Speed * Delta);
                Wire1.transform.position -= new Vector3(0, 0, Speed * Delta);
                Wire2.transform.position -= new Vector3(0, 0, Speed * Delta);
                Wire3.transform.position -= new Vector3(0, 0, Speed * Delta);
                Goober0.transform.position -= new Vector3(0, 0, Speed * Delta);
                Goober1.transform.position -= new Vector3(0, 0, Speed * Delta);
                Goober2.transform.position -= new Vector3(0, 0, Speed * Delta);
                Goober3.transform.position -= new Vector3(0, 0, Speed * Delta);
            }

            for (int i = 0; i < Players.Count; i++)
            {
                GameObject Player = Players[i];
                RaycastHit RayHit;
                if (Physics.Raycast(Player.transform.position, transform.TransformDirection(Vector3.down), out RayHit, maxDistance: 1.2f))
                {
                    if (RayHit.transform.gameObject.name == transform.gameObject.name)
                    {
                        float AddedVel = Direction == false ? -.73f : .45f;
                        Player.GetComponent<Rigidbody>().AddForce(new Vector3(.40f, 0, .14f + (AddedVel)));
                    }
                }
            }
        }
        else
        {
            Pause -= Delta;
        }
    }
}
