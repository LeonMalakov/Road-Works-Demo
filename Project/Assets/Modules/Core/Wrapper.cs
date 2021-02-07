using System;

namespace WAL.Core
{
    /// <summary>
    /// A "Class-Wrapper". It can be used for wrapping types such as structs.<br/>
    /// Also, you can create serializable nested arrays by wrapping array-type with it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Wrapper<T>
    {
        public T Item;
    }
}