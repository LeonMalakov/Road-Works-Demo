using System;
using System.Collections.Generic;
using UnityEngine;

namespace WAL.Core
{
    [Serializable]
    public abstract class EnumMatchingBase
    {
    }

    /// <summary>
    /// Serializable matching between Enum and Class.<br/>
    /// You can create nested matchings.<br/>
    /// NOTE that Unity can't serialize nested arrays, so you can't use array as TData, 
    /// BUT you can wrap array with "Wrapper"-class, and it's work. 
    /// </summary>
    /// <typeparam name="TEnum">Key.</typeparam>
    /// <typeparam name="TData">Value.</typeparam>
    [Serializable]
    public sealed class EnumMatching<TEnum, TData> : EnumMatchingBase
        where TEnum : Enum
    {
        [SerializeField]
        private TData[] _elements;

        public IReadOnlyList<TData> Elements => _elements;

        public TData Get(TEnum e) => Elements[Convert.ToInt32(e)];
    }
}