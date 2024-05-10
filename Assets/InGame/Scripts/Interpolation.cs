using UnityEngine;
using Photon.Pun;

public class Interpolation : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Vector3 lastReceivedPosition;
    private float interpolationFactor = 0.1f; // Adjust as needed for smoothness

    void Update()
    {
        if (!photonView.IsMine)
        {
            // Interpolate towards the network position
            transform.position = Vector3.Lerp(transform.position, networkPosition, interpolationFactor);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // If this is the owner of the object, send the current position to others
            stream.SendNext(transform.position);
        }
        else
        {
            // If this is not the owner, receive the position from the owner
            lastReceivedPosition = (Vector3)stream.ReceiveNext();
            networkPosition = lastReceivedPosition;
        }
    }
}
