using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [CustomEditor(typeof(PlayerAbilityDataSO))]
    public class PlayerAbilityDataSOEditor : Editor
    {
        private static List<Type> _dataCompTypes = new List<Type>();

        private PlayerAbilityDataSO _dataSo;

        private bool _showForceUpdateButtons;
        private bool _showAddComponentButtons;

        private void OnEnable()
        {
            _dataSo = target as PlayerAbilityDataSO;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Set Number of Phases"))
            {
                foreach (var item in _dataSo.ComponentData)
                {
                    item.InitializePhaseData(_dataSo.NumberOfPhases);
                }
            }

            _showAddComponentButtons = EditorGUILayout.Foldout(_showAddComponentButtons, "Add Components");

            if (_showAddComponentButtons)
            {
                foreach (var dataCompType in _dataCompTypes)
                {
                    if (GUILayout.Button(dataCompType.Name))
                    {
                        var comp = Activator.CreateInstance(dataCompType) as PlayerAbilityComponentData;

                        if (comp == null) return;
                        
                        comp.InitializePhaseData(_dataSo.NumberOfPhases);
                        
                        _dataSo.AddData(comp);
                        
                        EditorUtility.SetDirty(_dataSo);
                    }
                }
            }

            _showForceUpdateButtons = EditorGUILayout.Foldout(_showForceUpdateButtons, "Force Update Buttons");

            if (_showForceUpdateButtons)
            {
                if (GUILayout.Button("Force Update Component Names"))
                {
                    foreach (var item in _dataSo.ComponentData)
                    {
                        item.SetComponentName();
                    }
                }

                if (GUILayout.Button("Force Update Phase Names"))
                {
                    foreach (var item in _dataSo.ComponentData)
                    {
                        item.SetPhaseDataName();
                    }
                }
            }
        }

        [DidReloadScripts]
        private static void OnRecompile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var filteredTypes = types.Where(type =>
                type.IsSubclassOf(typeof(PlayerAbilityComponentData)) && !type.ContainsGenericParameters &&
                type.IsClass);
            _dataCompTypes = filteredTypes.ToList();
        }
    }
}