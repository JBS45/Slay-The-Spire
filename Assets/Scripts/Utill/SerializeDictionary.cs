using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializeDictionary<TKey,TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> m_keys = new List<TKey>();
    [SerializeField]
    List<TValue> m_values = new List<TValue>();



    public void OnBeforeSerialize()
    {
        m_keys.Clear();
        m_values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            m_keys.Add(pair.Key);
            m_values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < Mathf.Min(m_keys.Count,m_values.Count); ++i)
        {
            this.Add(m_keys[i], m_values[i]);
        }
    }
}
