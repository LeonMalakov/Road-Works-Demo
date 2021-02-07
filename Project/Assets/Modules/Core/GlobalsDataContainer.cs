using UnityEngine;

namespace WAL.Core
{
    [CreateAssetMenu(fileName = "GlobalsDataContainer", menuName = "Data/GlobalsDataContainer")]
    public class GlobalsDataContainer : ScriptableObject
    {
        public ScriptableObject[] Elements;
    }
}