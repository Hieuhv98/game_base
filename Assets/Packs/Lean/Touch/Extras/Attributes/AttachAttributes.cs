namespace Lance.Common
{
    using System;
    using UnityEngine;

    #region single

    /// <summary>
    /// public BoxCollider boxCollider;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentAttribute : AttachPropertyAttribute
    {
    }

    /// <summary>
    /// public BoxCollider boxColliderInChildren;
    /// include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentInChildrenAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public string NameChildren { get; }

        public GetComponentInChildrenAttribute(string nameChildren) { NameChildren = nameChildren; }
        public GetComponentInChildrenAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// public BoxCollider boxColliderInChildrenOnly
    /// not include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentInChildrenOnlyAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public GetComponentInChildrenOnlyAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// public BoxCollider boxColliderInParent;
    /// include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentInParentAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public GetComponentInParentAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// public BoxCollider boxColliderInParentOnly;
    /// not include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentInParentOnlyAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public GetComponentInParentOnlyAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AddComponentAttribute : AttachPropertyAttribute
    {
    }

    /// <summary>
    /// [Find( "Main Camera" )] public GameObject cameraGameObject;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindAttribute : AttachPropertyAttribute
    {
        public string Name { get; }

        public FindAttribute(string name) { Name = name; }
    }

    /// <summary>
    /// [FindWithTag("MainCamera")] public GameObject cameraGameObjectWithTag;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindWithTagAttribute : AttachPropertyAttribute
    {
        public string Tag { get; }

        public FindWithTagAttribute(string tag) { Tag = tag; }
    }

    /// <summary>
    /// [FindChild("Main Camera")] public Transform transformChild;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindChildAttribute : AttachPropertyAttribute
    {
        public string Name { get; }

        public FindChildAttribute(string name) { Name = name; }
    }

    /// <summary>
    /// [FindObjectOfType] public Camera camera;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindObjectOfTypeAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public FindObjectOfTypeAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    #endregion

    #region multiple

    /// <summary>
    /// public BoxCollider[] boxColliders;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentsAttribute : AttachPropertyAttribute
    {
    }

    /// <summary>
    /// public BoxCollider[] boxCollidersInChildren;
    /// include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentsInChildrenAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public GetComponentsInChildrenAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// public BoxCollider[] boxCollidersInChildrenOnly
    /// not include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentsInChildrenOnlyAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public GetComponentsInChildrenOnlyAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// public BoxCollider[] boxCollidersInParent;
    /// include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentsInParentAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public GetComponentsInParentAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// public BoxCollider[] boxCollidersInParentOnly;
    /// not include self
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentsInParentOnlyAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public GetComponentsInParentOnlyAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    /// <summary>
    /// [FindObjectsOfType] public Camera[] cameras;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindObjectsOfTypeAttribute : AttachPropertyAttribute
    {
        public bool IncludeInactive { get; }

        public FindObjectsOfTypeAttribute(bool includeInactive = false) { IncludeInactive = includeInactive; }
    }

    #endregion

    public class AttachPropertyAttribute : PropertyAttribute
    {
    }
}