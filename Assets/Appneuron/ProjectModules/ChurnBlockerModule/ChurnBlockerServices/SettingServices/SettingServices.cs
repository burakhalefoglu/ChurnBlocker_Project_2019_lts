﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.ConfigServices;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.SettingServices
{
    public class SettingServices : MonoBehaviour
    {

        [Enumeration(new string[] { "HyperCasual", "Arcade", "Strategy", "Other" })]
        [SerializeField]
        private int gameType;

        [Enumeration(new string[] { "PixelArt2D", "Stylized2D", "stylized3D", "Other" })]
        [SerializeField]
        private int graphStyle;


        private void Start()
        {
            ComponentsConfigServices.GameType = gameType;
            ComponentsConfigServices.GraphStyle = graphStyle;
        }

    }




    public class EnumerationAttribute : PropertyAttribute
    {

        public readonly string[] Items;
        public int Selected { get; set; }

        public EnumerationAttribute(params string[] enumerations) { Items = enumerations; }

    }



    [CustomPropertyDrawer(typeof(EnumerationAttribute))]
    public class EnumerationDrawer : PropertyDrawer
    {

        EnumerationAttribute enumeration { get { return (EnumerationAttribute)attribute; } }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            enumeration.Selected = EditorGUI.Popup(EditorGUI.PrefixLabel(position, label), enumeration.Selected, enumeration.Items);
            property.intValue = enumeration.Selected;
        }

    }
}