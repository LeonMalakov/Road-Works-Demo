using System;
using UnityEngine;

namespace Combine
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _content;


        public event Action RestartClicked;


        public void Show()
        {
            _content.SetActive(true);
        }


        public void UIEvent_Restart()
        {
            RestartClicked?.Invoke();
        }
    }
}