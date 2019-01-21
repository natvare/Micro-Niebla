using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;


namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        [SerializeField]
        private Text txt;
        [SerializeField]
        private Text nombreSala;

        private Scene currentScene;
        private string SceneName;

        private static bool createRoom;


        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
            {
                SceneManager.LoadScene(0);
            }


            public override void OnPlayerEnteredRoom(Player other)
            {
                Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                    //LoadArena();
                }
            }


            public override void OnPlayerLeftRoom(Player other)
            {
                Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                    //LoadArena();
                }
            }


        #endregion


        #region MonoBehaviour Callbacks
        void Start()
        {
            currentScene = SceneManager.GetActiveScene();
            SceneName = currentScene.name;
            
        }

        void Update()
        {

            if(SceneName == "02 Lobby")
            {
                if (PhotonNetwork.IsConnected) RoomsInfo();

            }
            
            if (SceneName == "05 Espera")
            {                
                Debug.Log(Parametros.roomname);
                if (PhotonNetwork.IsMasterClient) PlayersInRoom();
                else txt.text = "Esperando Jugadores... "+ PhotonNetwork.CurrentRoom.PlayerCount+" de "+ PhotonNetwork.CurrentRoom.MaxPlayers;
                nombreSala.text = "Sala: " + PhotonNetwork.CurrentRoom.Name;
            }
            



        }

        #endregion




        #region Public Methods

            public void LeaveRoom()
            {
                PhotonNetwork.LeaveRoom();
            }


            public void SwitchScenes(int idScene)
            {
                SceneManager.LoadScene(idScene);
            }


            public void CrearSala()
            {
                PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, new RoomOptions { MaxPlayers = System.Convert.ToByte(Parametros.param.cantidad) });
            }


            public void JoinRandomRoom()
            {
                PhotonNetwork.JoinRandomRoom();
            }

        public void GoLogin(bool create)
        {
            createRoom = create;
            Debug.Log(createRoom);
            SwitchScenes(1);
        }

        public void CreateOrJoin()
        {
            Debug.Log("CreateOrJoin " + createRoom);
            if (createRoom==true) SwitchScenes(3);
            else SwitchScenes(3);
        }



            public void ComenzarJuego()
            {
                if(PhotonNetwork.IsMasterClient) LoadArena();
            }



        #endregion


        #region Private Methods

            void LoadArena()
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                }
                Debug.LogFormat("PhotonNetwork : Player on the Room : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
                //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
                PhotonNetwork.LoadLevel("06 Sala");
            }

            

            private void RoomsInfo()
            {                            
                txt.text = PhotonNetwork.CurrentRoom.Name;
                Debug.Log("Sala: "+PhotonNetwork.CountOfPlayers);
            }

            private void PlayersInRoom()
            {
                int i = System.Convert.ToInt32(PhotonNetwork.CurrentRoom.PlayerCount);            
                txt.text = "Jugadores Conectados:\n"+System.Convert.ToString(PhotonNetwork.CurrentRoom.PlayerCount) 
                                    + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
                while (i != 0) {
                    i--;
                    Debug.Log(PhotonNetwork.PlayerList[i].NickName);
                
                }
            }

        #endregion
    }
}

