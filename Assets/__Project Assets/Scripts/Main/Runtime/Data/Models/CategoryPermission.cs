using UnityEngine;

namespace Data.Models
{
    [System.Serializable]
    public class CategoryPermission
    {
        [SerializeField] private int _categoryId;
        [SerializeField] private int _permissionId;

        public int CategoryId
        {
            get => _categoryId;
            set => _categoryId = value;
        }

        public int PermissionId
        {
            get => _permissionId;
            set => _permissionId = value;
        }
    }
}