using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Assets.Appneuron.Project.ChurnBlockerModule.Services.ConfigServices;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Services.SettingServices
{
    public class SettingServices : MonoBehaviour
    {

        [Enumeration(new string[] { "HyperCasual", "Arcade", "Strategy", "Other" })]
        [SerializeField]
        private int GameType;

        [Enumeration(new string[] { "PixelArt2D", "Stylized2D", "stylized3D", "Other" })]
        [SerializeField]
        private int GraphStyle;


        private void Start()
        {
            ComponentsConfigServices.GameType = GameType;
            ComponentsConfigServices.GraphStyle = GraphStyle;
        }


    }



    public class Enumeration : PropertyAttribute
    {

        public readonly string[] items;
        public int selected = 0;

        public Enumeration(params string[] enumerations) { this.items = enumerations; }

    }



    [CustomPropertyDrawer(typeof(Enumeration))]
    public class EnumerationDrawer : PropertyDrawer
    {

        Enumeration enumeration { get { return (Enumeration)attribute; } }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {

            enumeration.selected = EditorGUI.Popup(EditorGUI.PrefixLabel(position, label), enumeration.selected, enumeration.items);
            prop.intValue = enumeration.selected;
        }

    }
}