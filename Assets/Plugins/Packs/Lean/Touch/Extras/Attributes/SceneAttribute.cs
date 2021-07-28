using System;
using UnityEngine;

namespace Lance.Common
{
    /// <summary>
    /// Allows to pick a name scene value.
    /// Supported types: <see cref="string"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SceneAttribute : PropertyAttribute
    {
    }
}