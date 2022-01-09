using System;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
// value-type container that holds exactly one value per named member of an enum
// usage: declare [Serializable] class that implements typed version. in an editor folder, declare a CustomPropertyEditor for that class that implements EnumBoxDrawer
// example: for enum T boxing an int
    // [Serializable] public class TIntBox : EnumBox<T, int> {}
    // [CustomPropertyDrawer(typeof(TIntBox))] public class TIntBoxDrawer : EnumBoxDrawer<T, int> {}
[Serializable]
public class EnumBox<TEnum, TValue> : ISerializationCallbackReceiver
    where TEnum : Enum
    where TValue : struct // TODO: would like to remove this
{
    static IEnumerable<TEnum> _cachedEnumValues;
    static IEnumerable<TEnum> cachedEnumValues => _cachedEnumValues ?? (_cachedEnumValues = EnumUtil.AllValues<TEnum>());

    static int _cachedEnumCount = -1;
    static int cachedEnumCount => _cachedEnumCount == -1 ? (_cachedEnumCount = EnumUtil.NameCount<TEnum>()) : _cachedEnumCount;

    public TValue this [TEnum e]
    {
        get
        {
            initDictIfNecessary();
            return valueDict[e];
        }

        set
        {
            initDictIfNecessary();
            valueDict[e] = value;
        }
    }

    [SerializeField]
    TValue[] _values; // exclusively for Unity serialization engine. do not touch in the code except to serialize

    Dictionary<TEnum, TValue> valueDict;

	public void OnBeforeSerialize ()
	{
        initDictIfNecessary();
        initValuesIfNecessary();

        int counter = 0;

        foreach (TEnum evalue in cachedEnumValues)
        {
            _values[counter++] = valueDict[evalue];
        }
	}

	public void OnAfterDeserialize ()
	{
        initDictIfNecessary();
        initValuesIfNecessary();

        int counter = 0;

        foreach (TEnum evalue in cachedEnumValues)
        {
            valueDict[evalue] = _values[counter++];
        }
	}

    void initDictIfNecessary ()
    {
        if (valueDict == null)
        {
            valueDict = new Dictionary<TEnum, TValue>();

            foreach (TEnum evalue in cachedEnumValues)
            {
                valueDict[evalue] = default(TValue);
            }
        }
    }

    void initValuesIfNecessary ()
    {
        if (_values == null || _values.Length != cachedEnumCount) _values = new TValue[cachedEnumCount];
    }
}
}
