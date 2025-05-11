using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    // overriding server authority to give clients authority
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

}
