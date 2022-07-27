using System;
using UnityEditor;
using UnityEngine;
using Variable;

namespace References
{
    #region Basic
    [CustomPropertyDrawer(typeof(Bool))]
    [CustomPropertyDrawer(typeof(String))]
    [CustomPropertyDrawer(typeof(Float))]
    [CustomPropertyDrawer(typeof(Int16))]
    [CustomPropertyDrawer(typeof(Int))]
    [CustomPropertyDrawer(typeof(Int64))]
    [CustomPropertyDrawer(typeof(Double))]
    [CustomPropertyDrawer(typeof(Char))]
    #endregion
    #region Struct
    [CustomPropertyDrawer(typeof(Vector2))]
    [CustomPropertyDrawer(typeof(Vector3))]
    [CustomPropertyDrawer(typeof(Quaternion))]
    #endregion
    #region Reference
    [CustomPropertyDrawer(typeof(GameObject))]
    [CustomPropertyDrawer(typeof(Transform))]
    [CustomPropertyDrawer(typeof(AnimationCurve))]
    [CustomPropertyDrawer(typeof(Gradient))]
    [CustomPropertyDrawer(typeof(Collider))]
    [CustomPropertyDrawer(typeof(Mesh))]
    [CustomPropertyDrawer(typeof(Rigidbody))]
    [CustomPropertyDrawer(typeof(CharacterController))]
    #endregion
    public class ReferenceDrawer : PropertyDrawer
    {
        private readonly string[] popupOptions = {"Use Constant", "Use Variable"};
    }
}

