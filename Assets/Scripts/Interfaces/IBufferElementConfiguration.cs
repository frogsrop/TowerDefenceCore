using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public interface IBufferElementConfiguration<T> : IBufferElementData where T : unmanaged
{
    IBufferElementData BuildComponent(T data);
}
