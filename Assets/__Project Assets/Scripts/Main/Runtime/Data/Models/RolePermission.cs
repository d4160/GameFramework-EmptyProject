using UnityEngine;

namespace Data.Models
{
    [System.Serializable]
    public class RolePermission
    {
        [SerializeField] private int _roleId;
        [SerializeField] private int _permissionId;
        [SerializeField] private bool _isActive;

        public int RoleId
        {
            get => _roleId;
            set => _roleId = value;
        }

        public int PermissionId
        {
            get => _permissionId;
            set => _permissionId = value;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }
    }
}
