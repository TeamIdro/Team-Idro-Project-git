using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(ControlVisibilityAttribute))]
public class ControlVisibilityDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ControlVisibilityAttribute attribute = (ControlVisibilityAttribute)base.attribute;

        // Esegui la condizione specificata nell'attributo
        bool conditionMet = EvaluateCondition(attribute.condition, property);

        // Mostra o nascondi la variabile in base alla condizione
        if (conditionMet)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!IsVisible(property))
        {
            // Nascondi la variabile impostando l'altezza a zero
            return 0;
        }

        // Altrimenti, mantieni l'altezza predefinita della variabile
        return EditorGUI.GetPropertyHeight(property, label);
    }


    private bool EvaluateCondition(string condition, SerializedProperty property)
    {
        TipoComportamentoMagia magicBehaviourType = (TipoComportamentoMagia)property.serializedObject.FindProperty("magicBehaviourType").enumValueIndex;

        // Dividi la condizione in due parti: il nome della variabile e il valore
        string[] parts = condition.Split(new[] { "==" }, StringSplitOptions.RemoveEmptyEntries);
        string variableName = parts[0].Trim();
        string expectedValue = parts[1].Trim();

        // Se il nome della variabile è "magicBehaviourType" e il valore atteso è "TipoComportamentoMagia.LineCast"
        // confronta con il valore effettivo di magicBehaviourType
        if (variableName == "magicBehaviourType" && expectedValue == "TipoComportamentoMagia.LineCast")
        {
            return magicBehaviourType == TipoComportamentoMagia.LineCast;
        }

        // Altrimenti, restituisci false se la condizione non è stata gestita
        return false;
    }
    private bool IsVisible(SerializedProperty property)
    {
        ControlVisibilityAttribute attribute = (ControlVisibilityAttribute)base.attribute;
        return EvaluateCondition(attribute.condition, property);
    }
}
