using System;
using UnityEngine;

namespace Lance.Common
{
    /// <summary>
    /// Allows to pick a built-in tag value.
    /// Supported types: <see cref="string"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TagAttribute : PropertyAttribute
    {
    }
}