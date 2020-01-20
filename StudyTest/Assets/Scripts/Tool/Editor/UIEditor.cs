using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace HelpProject
{
    public class UIEditor : MonoBehaviour
    {

        //将UI下的所有image text里默认的material替换成自己的写的材质球
        [MenuItem("Tools/替换所有UI的材质球")]
        public static void ReplaceUIMaterial()
        {
            GameObject go = Selection.activeGameObject;

            string path = "Assets/Materials/JayW/WorldUIMaterial.mat";
            material = AssetDatabase.LoadAssetAtPath<Material>(path);
            //Debug.Log(material.name);
            GetChildAndReplaceMaterial(go.transform);
            AssetDatabase.SaveAssets();
            Debug.Log("已经将选择的UI的材质球设置为WorldUIMaterial");
        }


        [MenuItem("Tools/恢复所有UI的材质球为空")]
        public static void ReplaceUIMaterialNull()
        {
            GameObject go = Selection.activeGameObject;
            material = null;
            GetChildAndReplaceMaterial(go.transform);
            AssetDatabase.SaveAssets();
            Debug.Log("已经将选择的UI的材质球设置为空");
        }



        static Material material = null;
        static void GetChildAndReplaceMaterial(Transform t)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Undo.RecordObject(t.gameObject, t.gameObject.name);
                if (t.GetChild(i).GetComponent<Image>()) t.GetChild(i).GetComponent<Image>().material = material;
                if (t.GetChild(i).GetComponent<Text>()) t.GetChild(i).GetComponent<Text>().material = material;
                EditorUtility.SetDirty(t.gameObject);
                GetChildAndReplaceMaterial(t.GetChild(i));
            }
        }

        static List<string> NameArray = new List<string>();
        [MenuItem("Tools/复制名字")]
        public static void CopyName()
        {
            NameArray.Clear();
            GameObject go = Selection.activeGameObject;
            NameArray.Add(go.name);
            Transform[] coms = go.GetComponentsInChildren<Transform>();
            foreach (Transform item in coms)
            {
                NameArray.Add(item.name);
            }
        }

        [MenuItem("Tools/粘贴名字")]
        public static void PasteName()
        {
            GameObject go = Selection.activeGameObject;

            go.name = NameArray[0];

            Transform[] coms = go.GetComponentsInChildren<Transform>();
            for (int i = 0; i < coms.Length; i++)
            {
                coms[i].name = NameArray[i + 1];
            }
        }
    }
}