using System;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;

public class SpawnDespawnLogger : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        UnityEngine.Debug.Log($"[SpawnDespawnLogger] OnNetworkSpawn -> {gameObject.name} | IsServer:{IsServer} | IsClient:{IsClient} | IsOwner:{IsOwner} | NetId:{NetworkObjectId} | Time:{Time.time}");
    }

    public override void OnNetworkDespawn()
    {
        UnityEngine.Debug.LogWarning($"[SpawnDespawnLogger] OnNetworkDespawn -> {gameObject.name} | NetId:{NetworkObjectId} | Time:{Time.time}\nStack:\n{GetStack()}");
        base.OnNetworkDespawn();
    }

    private void OnDestroy()
    {
        UnityEngine.Debug.LogError($"[SpawnDespawnLogger] OnDestroy -> {gameObject.name} | Time:{Time.time}\nStack:\n{GetStack()}");
    }

    string GetStack()
    {
        try
        {
            var st = new StackTrace(2, true);
            return st.ToString();
        }
        catch (Exception e)
        {
            return "Could not get stack: " + e.Message;
        }
    }
}
