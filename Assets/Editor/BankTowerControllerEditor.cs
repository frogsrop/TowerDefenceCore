using UnityEditor;

namespace Editor
{
    [UnityEditor.CustomEditor(typeof(BankTowerController))] 
    public class BankTowerControllerEditor : UnityEditor.Editor
    {
        private SerializedProperty _towerData; //

        void OnEnable() 
        {
            _towerData = serializedObject.FindProperty("_towerData"); 
        }

        public override void OnInspectorGUI() //������� "��� ������� ���������"
        {
            TowerData previousValue = _towerData.objectReferenceValue as TowerData; //??����� ���-�� ����������������? == ���

            
            serializedObject.Update();
            BankTowerController behaviour = (BankTowerController)target; // �������, ��� ����� ���������� � ������ "BankTowerController" == �� ������ ��� ���������� ���� �����
            behaviour.init(); //��������� � ������� � ������� ����������� � ������� == ��

            /*if (previousValue.NameTower != (_towerData.objectReferenceValue as TowerData).NameTower) //??�� ������ ��������� ���������� previousValue �������� _towerData.intValue, ������ �� ��� ��������� �� ���������? == ����� ������ �������� ��� ���, �� �������� ��� �������� �� ������ �����(��-�����)
            {
                BankTowerController behaviour = (BankTowerController)target; // �������, ��� ����� ���������� � ������ "BankTowerController" == �� ������ ��� ���������� ���� �����
                behaviour.init(); //��������� � ������� � ������� ����������� � ������� == ��
            }*/
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}