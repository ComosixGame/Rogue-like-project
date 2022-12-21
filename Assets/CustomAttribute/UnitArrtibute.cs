using UnityEngine;

namespace MyCustomAttribute
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ReadOnlyAttribute : PropertyAttribute { }
}
