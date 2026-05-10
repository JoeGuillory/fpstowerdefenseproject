using Steamworks;
using Unity.Netcode;
using UnityEngine;

public class SteamTestScript : MonoBehaviour
{
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> LobbyEnter;


    public ulong CurrentLobbyId;
    private const string HostAddressKey = "HostAddress";
    
    [SerializeField]
    private NetworkManager _networkManager;
    
    
    
    void Start()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }
        
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoined);
        LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        Debug.Log("Button Pressed ");
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 3);
    }


    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        Debug.Log("Started Lobby");
        _networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,SteamUser.GetSteamID().ToString());

    }

    private void OnLobbyJoined(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyId = callback.m_ulSteamIDLobby;


        if (NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        _networkManager.StartClient();
    }
    
    
}
