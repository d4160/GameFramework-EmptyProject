using System.Linq;
using d4160.GameFramework.DataPersistence;
using Data.Models;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Scriptables
{
    [CreateAssetMenu(menuName = "Game Framework/Title Database")]
    public class TitleDatabaseScriptable : BaseTitleDataScriptable<TitleDatabaseScriptable>
    {
        [ReorderableList]
        [SerializeField] private Genre[] _genres;

        [ReorderableList]
        [SerializeField] private Role[] _roles;

        [ReorderableList]
        [SerializeField] private PermissionCategory[] _permissionCategories;

        [ReorderableList]
        [SerializeField] private Permission[] _permissions;

        [ReorderableList]
        [SerializeField] private RolePermission[] _rolePermissionRel;

        [ReorderableList]
        [SerializeField] private CategoryPermission[] _categoryPermissionRel;

        public Genre[] Genres => _genres;

        public Role[] Roles => _roles;

        public PermissionCategory[] PermissionCategories => _permissionCategories;

        public Permission[] Permissions => _permissions;

        public RolePermission[] RolePermissionRel => _rolePermissionRel;

        public CategoryPermission[] CategoryPermissionRel => _categoryPermissionRel;

        private string _jsonData;

        [Button("Serialize")]
        private void SerializeThis()
        {
            SetSerializer(new UnityJsonDataSerializer());

            _jsonData = Serialize();

            Debug.Log($"Data serialized: {_jsonData}");
        }

        [Button]
        private void Deserialize()
        {
            Deserialize(_jsonData);

            Debug.Log($"Data deserialized.");
        }

        public int[] GetPermissions(int role, int category)
        {
            var permissionsByRole = _rolePermissionRel.Where((x) => x.RoleId == role);
            var permissionsByCategory = permissionsByRole.Where((x) =>
                _categoryPermissionRel.FirstOrDefault((y) => x.PermissionId == y.PermissionId)?.CategoryId == category);

            return permissionsByCategory.Select(x => x.PermissionId).ToArray();
        }
    }
}