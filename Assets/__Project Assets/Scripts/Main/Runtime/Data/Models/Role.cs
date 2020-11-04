using UnityEngine;

namespace Data.Models
{
    [System.Serializable]
    public class Role
    {
        [SerializeField] private string _name;
        [SerializeField] public int _id;

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
