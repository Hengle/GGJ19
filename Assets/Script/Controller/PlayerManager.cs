using System.Collections.Generic;
using UnityEngine;

namespace Script.Controller
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        [HideInInspector] public List<PlayerController> playerList = new List<PlayerController>();
    
        private void Awake()
        {
            Instance = this;
        }
    }
}