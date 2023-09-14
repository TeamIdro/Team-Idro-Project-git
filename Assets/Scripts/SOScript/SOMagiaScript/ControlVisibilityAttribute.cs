using UnityEngine;

public class ControlVisibilityAttribute : PropertyAttribute
{
    public string condition;

    public ControlVisibilityAttribute(string condition)
    {
        this.condition = condition;
    }
}