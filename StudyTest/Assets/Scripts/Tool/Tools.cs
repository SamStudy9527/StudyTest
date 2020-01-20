using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sam
{
    //------------------------【工具方法】--------------------------------------
    public static class Tools
    {

        /// <summary>计算向量 A 和向量 B 在轴 axis 确定的平面上投影（分量）的夹角 </summary>
        /// <param name="dirA">向量 A</param>
        /// <param name="dirB">向量 B</param>
        /// <param name="axis">平面的法线向量</param>
        /// <returns>A、B在平面上的夹角</returns>
        public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
        {
            // A、B减去在轴上的投影后，剩下的部分是在轴确定的平面上的分量
            dirA -= Vector3.Project(dirA, axis);
            dirB -= Vector3.Project(dirB, axis);

            // 计算夹角，返回的角度总是两个向量之间的锐角(也就是说,它们之间2个可能的角度中较小的那个,不大于180度)。 
            float angle = Vector3.Angle(dirA, dirB);

            // 确定从 A 到 B 是正转还反转
            return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
        }

        /// <summary>
        /// 计算欧拉角进行合理化处理
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static float CheckAngle(this float Value)
        {
            float angle = Value - 180f;
            return angle > 0 ? angle - 180f : angle + 180f;
        }

    }

    /// <summary> 这是用于在检视面板只读属性的组件 </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif

    //------------------------【扩展方法】--------------------------------------
    /// <summary> 扩展方法 </summary>
    public static class EMethods
    {

        /// <summary> 省去[RequireComponent]诸多麻烦的、安全地取得游戏对象上组件的方法，一般用在初始化的时候 </summary>
        public static T GetSafeComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (!component)
                Debug.LogError("想要查找 " + typeof(T) + " 类型的组件，但是没找到。", go);
            return component;
        }

        /// <summary> /// 设置：修改Transform.EulerAngles.y分量的拓展方法 /// </summary>
        /// <param name="tr"></param>
        /// <param name="y"></param>
        public static void SetYOfEulerAngles(this Transform tr, float y)
        {
            Vector3 euler = tr.eulerAngles;
            euler.y = y;
            tr.eulerAngles = euler;
        }
    }
    //--------------------------------------------------------------------------
}
