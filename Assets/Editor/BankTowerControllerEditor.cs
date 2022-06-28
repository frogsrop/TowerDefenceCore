using UnityEditor;

namespace Editor
{
    [UnityEditor.CustomEditor(typeof(BankTowerController))] //пoказываем для кого редактор пишем
    public class BankTowerControllerEditor : UnityEditor.Editor
    {
        private SerializedProperty _towerData; //задаем переменную _towerData класса SerializedProperty. Что бы потом могли воспользоваться serializedObject.ApplyModifiedProperties()

        void OnEnable() //срабатывает при появлении скрипта
        {
            _towerData = serializedObject.FindProperty("_towerData"); //??находим скриптблобжекты ? == нет
        }

        public override void OnInspectorGUI() //Функция "Что покажет инспектор"
        {
            int previousValue = _towerData.intValue; //??нашли кол-во скриптблобжектов? == нет

            DrawDefaultInspector();//Показывает стандартный инспектор == да
            serializedObject.Update();//Походу лишнее == не знаю

            if (previousValue != _towerData.intValue) //??Мы сверху присвоиди переменной previousValue значение _towerData.intValue, почему мы тут проверяем на равенство? == чтобы узнать обнвлять или нет, по хорошему эти значения не всегда равны(по-логай)
            {
                BankTowerController behaviour = (BankTowerController)target; // сказали, что будем обращаться к классу "BankTowerController" == не совсем это называется каст типов
                behaviour.init(); //обратился к функции с заменой содержимого в префабе == да
            }

            serializedObject.ApplyModifiedProperties();//применить измененения == да
        }
    }
}