using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{
    [SerializeField]
    string Code;
     private async void Start()
     {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
     }

    public async void CreateRelay()
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            transport.SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData);

            NetworkManager.Singleton.StartHost();
        } catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    
    public async void JoinRelay()
    {
        try
        {
            JoinAllocation join =  await RelayService.Instance.JoinAllocationAsync(Code);

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            transport.SetClientRelayData(
                join.RelayServer.IpV4,
                (ushort)join.RelayServer.Port,
                join.AllocationIdBytes,
                join.Key,
                join.ConnectionData,
                join.HostConnectionData);


            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e) 
        {
            Debug.Log(e);
        }
    }
}
