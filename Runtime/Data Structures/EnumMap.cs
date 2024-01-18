using System;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
    [Serializable]
    public class EnumMap<TEnum, TValue> : ISerializationCallbackReceiver
        where TEnum : Enum
    {
        public TValue this[TEnum e]
        {
            get => m_values[getOrAddEnumPosition(e)];
            set => m_values[getOrAddEnumPosition(e)] = value;
        }

        [SerializeField]
        List<TEnum> m_enums;

        [SerializeField]
        List<TValue> m_values;

        public void OnBeforeSerialize()
        {
            createAndResizeLists();
        }

        public void OnAfterDeserialize()
        {
            createAndResizeLists();
        }

        void createAndResizeLists()
        {
            if (m_enums == null)
                m_enums = new List<TEnum>();
            if (m_values == null)
                m_values = new List<TValue>();

            foreach (var e in EnumUtil.AllValues<TEnum>())
            {
                getOrAddEnumPosition(e);
            }
        }

        int getOrAddEnumPosition(TEnum e)
        {
            int position = m_enums.IndexOf(e);

            if (position != -1)
            {
                return position;
            }
            else
            {
                m_enums.Add(e);
                m_values.Add(default);
                return m_enums.Count - 1;
            }
        }
    }
}
