using UnityEditor;

namespace Editor
{
    [UnityEditor.CustomEditor(typeof(BankTowerController))] //�o�������� ��� ���� �������� �����
    public class BankTowerControllerEditor : UnityEditor.Editor
    {
        private SerializedProperty _towerData; //������ ���������� _towerData ������ SerializedProperty. ��� �� ����� ����� ��������������� serializedObject.ApplyModifiedProperties()

        void OnEnable() //����������� ��� ��������� �������
        {
            _towerData = serializedObject.FindProperty("_towerData"); //??������� ��������������� ? == ���
        }

        public override void OnInspectorGUI() //������� "��� ������� ���������"
        {
            int previousValue = _towerData.intValue; //??����� ���-�� ����������������? == ���

            DrawDefaultInspector();//���������� ����������� ��������� == ��
            serializedObject.Update();//������ ������ == �� ����

            if (previousValue != _towerData.intValue) //??�� ������ ��������� ���������� previousValue �������� _towerData.intValue, ������ �� ��� ��������� �� ���������? == ����� ������ �������� ��� ���, �� �������� ��� �������� �� ������ �����(��-�����)
            {
                BankTowerController behaviour = (BankTowerController)target; // �������, ��� ����� ���������� � ������ "BankTowerController" == �� ������ ��� ���������� ���� �����
                behaviour.init(); //��������� � ������� � ������� ����������� � ������� == ��
            }

            serializedObject.ApplyModifiedProperties();//��������� ����������� == ��
        }
    }
}