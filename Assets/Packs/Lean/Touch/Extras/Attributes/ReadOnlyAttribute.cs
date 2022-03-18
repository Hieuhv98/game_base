using System;
using UnityEngine;

namespace Lance.Common
{
    /// <summary>
    /// Marks field as read-only.
    /// Supported types: all.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}