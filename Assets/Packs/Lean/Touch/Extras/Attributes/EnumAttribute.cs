using System;
using UnityEngine;

namespace Lance.Common
{
    /// <summary>
    /// Creates a popup window with an input field. Allows to search for enum values for its name.
    /// Supported types: any <see cref="Enum"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumAttribute : PropertyAttribute
    {
    }
}