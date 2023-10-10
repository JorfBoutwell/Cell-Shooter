using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;

    enum state {Patrol, Follow};

    bool onLink = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");

        agent.autoTraverseOffMeshLink = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.transform.position;

        if(agent.isOnOffMeshLink && !onLink)
        {
            Debug.Log("On Off-Mesh Link");
            onLink = true;
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos; // + Vector3.up * agent.baseOffset
        float duration = Vector3.Distance(startPos, endPos) / agent.speed;
        Keyframe vertex = new Keyframe(duration / 2f, 0.5f * 9.8f * (duration / 2f) * (duration / 2f));
        AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), vertex, new Keyframe(duration, 0f));

        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        agent.CompleteOffMeshLink();
        onLink = false;
    }
}
