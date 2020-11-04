using UnityEngine;

namespace Data.Models
{
    [System.Serializable]
    public class Genre
    {
        [SerializeField] private string _name;
        [SerializeField] public char _id;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public char Id
        {
            get => _id;
            set => _id = value;
        }
    }
}
