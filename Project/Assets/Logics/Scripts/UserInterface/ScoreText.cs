using UnityEngine;
using UnityEngine.UI;

namespace Combine
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private Text _text;


        public void SetScore(int score)
        {
            _text.text = score.ToString();
        }    
    }
}
