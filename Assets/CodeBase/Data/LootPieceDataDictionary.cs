using System;
using CodeBase.Infrastructure.States;

namespace CodeBase.Data
{
    [Serializable]
    public class LootPieceDataDictionary : SerializableDictionary<string, LootPieceData>
    {
    }
}