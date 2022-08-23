using UnityEditor;

namespace Editor
{
    [UnityEditor.CustomEditor(typeof(BankTowerController))] 
    public class BankTowerControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector()) 
            {
                BankTowerController behaviour = (BankTowerController)target; 
                behaviour.Init();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}