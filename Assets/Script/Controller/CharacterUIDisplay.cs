using UnityEngine;

namespace Script.Controller
{
    public class CharacterUIDisplay : MonoBehaviour
    {
        [Header("Animation UI")] 
        [SerializeField] private GameObject uiIdle;
        [SerializeField] private GameObject uiHold;
        [SerializeField] private GameObject uiTired;
        [SerializeField] private GameObject uiDead;

        private GameObject current;
        
        public void ChangeCharacterUI(CharacterUIState state)
        {
            if(current != null)
                current.SetActive(false);
            
            switch (state)
            {
                case CharacterUIState.idle:
                    current = uiIdle;
                    break;
                case CharacterUIState.hold:
                    current = uiHold;
                    break;
                case CharacterUIState.tired:
                    current = uiTired;
                    break;
                case CharacterUIState.dead:
                    current = uiDead;
                    break;
            }
            
            current.SetActive(true);
        }
    }
}