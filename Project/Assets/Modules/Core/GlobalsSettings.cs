using UnityEngine;

namespace WAL.Core
{
    //[CreateAssetMenu(fileName = "GlobalsSettings", menuName = "Data(Single)/GlobalsSettings")]
    public class GlobalsSettings : ScriptableObject
    {
        public GlobalsDataContainer DataContainer;

        public static GlobalsSettings Load()
        {
            return Resources.Load<GlobalsSettings>(nameof(GlobalsSettings));
        }
    }
}