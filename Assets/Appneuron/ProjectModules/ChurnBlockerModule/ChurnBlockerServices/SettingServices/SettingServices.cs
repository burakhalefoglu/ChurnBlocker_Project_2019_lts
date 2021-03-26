using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Appneuron.Services;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.SettingServices
{

    public static class ProgramList
    {
       public readonly static string SURVEY_INPUT = "burak";
        public readonly static string SURVEY_2 = "burak";

    }

    public class SettingServices : MonoBehaviour
    {
        


        [Enumeration(new string[] { "HyperCasual", "Arcade", "Strategy", "Other" })]
        [SerializeField]
        private int gameType;

        [Enumeration(new string[] { "PixelArt2D", "Stylized2D", "stylized3D", "Other" })]
        [SerializeField]
        private int graphStyle;

        public int GetGameType()
        {
            return gameType;
        }

        public int GetGraphStyle()
        {
            return graphStyle;
        }
    }




    public class EnumerationAttribute : PropertyAttribute
    {

        public string[] Items { get; set; }
        public int Selected { get; set; }

        public EnumerationAttribute(params string[] enumerations) { Items = enumerations; }

    }


#if UNITY_EDITOR

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
#endif

}