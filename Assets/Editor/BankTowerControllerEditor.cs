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

        public override void OnInspectorGUI() //Функция "Что покажет инспектор"
        {
            TowerData previousValue = _towerData.objectReferenceValue as TowerData; //??нашли кол-во скриптблобжектов? == нет

            
            serializedObject.Update();
            BankTowerController behaviour = (BankTowerController)target; // сказали, что будем обращаться к классу "BankTowerController" == не совсем это называется каст типов
            behaviour.init(); //обратился к функции с заменой содержимого в префабе == да

            /*if (previousValue.NameTower != (_towerData.objectReferenceValue as TowerData).NameTower) //??Мы сверху присвоиди переменной previousValue значение _towerData.intValue, почему мы тут проверяем на равенство? == чтобы узнать обнвлять или нет, по хорошему эти значения не всегда равны(по-логай)
            {
                BankTowerController behaviour = (BankTowerController)target; // сказали, что будем обращаться к классу "BankTowerController" == не совсем это называется каст типов
                behaviour.init(); //обратился к функции с заменой содержимого в префабе == да
            }*/
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
        }
    }
}