using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public abstract class AbstractEffectConfig : ScriptableObject
{
    public static readonly Dictionary<int, AbstractEffectConfig> Mapping = new(); //прописали ссылку создающую словарь
    private static int _curId; //текущий айди, общее значение для всех экземпляров класса
    public int Id { get; private set; } //прописываемый айди

    private void OnEnable() //при включении
    {
        Id = _curId; //Айди приравнивается текущий айди
        _curId++; //текущий айди увеличивается
        Mapping[Id] = this; //создается словарь
    }

    public abstract void AppendToBuffer(Entity entity, EntityCommandBuffer ecb);

}