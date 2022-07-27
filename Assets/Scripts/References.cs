using System;
using UnityEngine;
using Variable;

namespace References
{
    #region Basic
    [Serializable] public class Bool : References<bool, Variable.Bool>{ }
    [Serializable] public class String : References<string, Variable.String>{ }
    [Serializable] public class Float : References<float, Variable.Float>{ }
    [Serializable] public class Int16 : References<short, Variable.Int16>{ }
    [Serializable] public class Int : References<int, Variable.Int>{ }
    [Serializable] public class Int64 : References<long, Variable.Int64>{ }
    [Serializable] public class Double : References<double, Variable.Double>{ }
    [Serializable] public class Char : References<char, Variable.Char>{ }
    #endregion
    #region Struct
    [Serializable] public class Vector2 : References<UnityEngine.Vector2, Variable.Vector2>{ }
    [Serializable] public class Vector3 : References<UnityEngine.Vector3, Variable.Vector3>{ }
    [Serializable] public class Quaternion : References<UnityEngine.Quaternion, Variable.Quaternion>{ }
    #endregion
    #region References
    [Serializable] public class GameObject : References<UnityEngine.GameObject, Variable.GameObject>{ }
    [Serializable] public class Transform : References<UnityEngine.Transform, Variable.Transform>{ }
    [Serializable] public class AnimationCurve : References<UnityEngine.AnimationCurve, Variable.AnimationCurve>{ }
    [Serializable] public class Gradient : References<UnityEngine.Gradient, Variable.Gradient>{ }
    [Serializable] public class Collider : References<UnityEngine.Collider, Variable.Collider>{ }
    [Serializable] public class Mesh : References<UnityEngine.Mesh, Variable.Mesh>{ }
    [Serializable] public class Rigidbody : References<UnityEngine.Rigidbody, Variable.Rigidbody>{ }
    [Serializable] public class CharacterController : References<UnityEngine.CharacterController, Variable.CharacterController>{ }
    #endregion
    
    public class References <T1, T2>
    {
        [SerializeField] public bool useConstant = true;
        [SerializeField] private  T1 _constantValue;
        [SerializeField] private T2 _variable;

        public References()
        {
        
        }
        
        public References(T1 value)
        {
            useConstant = true;
            _constantValue = value;
        }

        public T1 Value
        {
            get => useConstant ? _constantValue : _variable as GenericVariable<T1>;
            set
            {
                if (useConstant)
                {
                    _constantValue = value;
                }
                else
                {
                    (_variable as GenericVariable<T1>).Value = value;
                }
            }
        }

        public static implicit operator T1(References<T1, T2> reference) => reference.Value;
    }
}
