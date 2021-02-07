using System;
using System.Collections.Generic;
using UnityEngine;

namespace WAL.Core
{
    public class Globals : MonoBehaviour
    {
        #region Singleton
        private static Globals _instance;

        public static Globals Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GlobalsFactory.Create();

                return _instance;
            }
        }

        internal static void Initialize()
        {
            if (_instance == null)
            {
                _instance = GlobalsFactory.Create();
            }
        }

        internal static void Initialize(ICollection<Component> elements)
        {
            if (_instance == null)
            {
                _instance = GlobalsFactory.Create();
            }

            // Scene Container assigning.
            _instance._sceneElements = GlobalsFactory.CreateDataElementsDictionary(elements);
        }
        #endregion

        private Dictionary<Type, ScriptableObject> _dataElements;

        private Dictionary<Type, Component> _sceneElements;

        internal void Construct(Dictionary<Type, ScriptableObject> dataElements)
        {
            _dataElements = dataElements;
        }

        public T GetData<T>() where T : ScriptableObject
        {
            if(_dataElements == null)
            {
                throw new Exception($"{nameof(Globals)} has no {nameof(GlobalsDataContainer)}.");
            }

            T data = _dataElements[typeof(T)] as T;

            if (data == null)
            {
                throw new Exception($"{nameof(Globals)} not contains {typeof(T)}.");
            }

            return data;
        }

        public T GetDependency<T>() where T : Component
        {
            if (_sceneElements == null)
            {
                throw new Exception($"{nameof(Globals)} has no scene dependencies.");
            }

            T data = _sceneElements[typeof(T)] as T;

            if (data == null)
            {
                throw new Exception($"{nameof(Globals)} scene dependencies not contains {typeof(T)}.");
            }

            return data;
        }
    }
}