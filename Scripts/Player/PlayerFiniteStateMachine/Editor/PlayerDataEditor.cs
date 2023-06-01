using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [CustomEditor(typeof(PlayerDataSO))]
    public class PlayerDataEditor : Editor
    {
        private static List<Type> _dataTypes = new List<Type>();

        private PlayerDataSO _playerDataSo;

        private bool _showForceUpdateButtons;
        private bool _showAddDataButtons;
        
        private void OnEnable()
        {
            _playerDataSo = target as PlayerDataSO;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _showAddDataButtons = EditorGUILayout.Foldout(_showAddDataButtons, "Add State Data");


            if (_showAddDataButtons)
            {
                foreach (var dataType in _dataTypes)
                {
                    if (GUILayout.Button(dataType.Name))
                    {
                        var comp = Activator.CreateInstance(dataType) as PlayerStateData;

                        if (comp == null)
                        {
                            return;
                        }
                        
                        comp.SetElementName();
                        
                        _playerDataSo.AddData(comp);
                        
                        EditorUtility.SetDirty(_playerDataSo);
                    }
                }                
            }
            
            _showForceUpdateButtons = EditorGUILayout.Foldout(_showForceUpdateButtons, "Force Update Data");

            if (_showForceUpdateButtons)
            {
                if (GUILayout.Button("Force Update Data Names"))
                {
                    foreach (var stateData in _playerDataSo.StateData)
                    {
                        stateData.SetElementName();
                    }
                }
            }





            // if (GUILayout.Button("Debug All State Data"))
            // {
            //     foreach (var playerStateData in _playerData.StateData)
            //     {
            //         Debug.Log(playerStateData);
            //     }
            // }
        }

        [DidReloadScripts]
        private static void OnRecompile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var filteredTypes = types.Where(type =>
                type.IsSubclassOf(typeof(PlayerStateData)) && !type.ContainsGenericParameters && type.IsClass);
            _dataTypes = filteredTypes.ToList();
        }
    }
}