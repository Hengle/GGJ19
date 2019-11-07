using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Controller
{
    public class StuffController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public AreaController activeAreaSide;

        public int mass;
        public int countHold;

        public int rate;

        [Range(1, 100)] public int stuffValue = 1;

        private List<PlayerController> _listPlayer = new List<PlayerController>();

        public bool isGone;
        public bool isGold;

        private SpriteRenderer _spriteRenderer;

        private const int GOLD_MULTPLY = 2;
    
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.mass = mass;
        }

        public void ConvertGold(float time)
        {
            isGold = true;
            stuffValue *= GOLD_MULTPLY;
            _spriteRenderer.color = Color.yellow;
            
            
            IEnumerator ConvertNormaly(float delay)
            {
                yield return new WaitForSeconds(delay);
                isGold = false;
                _spriteRenderer.color = Color.white;
                stuffValue /= GOLD_MULTPLY;
            }
            StartCoroutine(ConvertNormaly(time));
        }


        public void Break(PlayerController playerController)
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }

            if (_listPlayer.Contains(playerController)) _listPlayer.Remove(playerController);

            countHold--;
            SettingMass(countHold);

            //Draw Listener
            MiniGameManager.Instance.StopDrawListener();
        }

     

        public void Hold(PlayerController playerController)
        {
            countHold++;

            if (!_listPlayer.Contains(playerController)) _listPlayer.Add(playerController);

            SettingMass(countHold);

            //Draw Listener
            if(countHold == PlayerManager.Instance.playerList.Count)
                MiniGameManager.Instance.StartDrawListener(this);
        }

       
        

        void SettingMass(int count)
        {
            int value = mass;
            int factor = count * rate;
            int x = mass - factor;
            if (x < 0)
            {
                _rigidbody.mass = 1;
            }
            else
            {
                if (_rigidbody != null)
                {
                    _rigidbody.mass = x;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Stuff"))
            {
                print("Enter");
            }
            else if (other.name == "LeftSide" || other.name == "RightSide")
            {
                activeAreaSide = other.GetComponent<AreaController>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            switch (other.name)
            {
                case "Neutral":
                    activeAreaSide.Add(this);
                    break;
                case "LeftSide":
                    activeAreaSide = null;
                    break;
                case "RightSide":
                    activeAreaSide = null;
                    break;
            }
        }

        public void BreakAllPlayers()
        {
            for (int i = 0; i < _listPlayer.Count; i++)
            {
                _listPlayer[0].isEnter = false;
                _listPlayer[0].Break(transform);
            }
        }
    }
}