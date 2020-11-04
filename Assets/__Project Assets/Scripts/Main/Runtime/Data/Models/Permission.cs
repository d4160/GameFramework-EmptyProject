using UnityEngine;

namespace Data.Models
{
    [System.Serializable]
    public class Permission
    {
        /// <summary>
        /// The option that can user have
        /// </summary>
        [SerializeField] private string _name;
        [SerializeField] private int _id;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }
    }
}
