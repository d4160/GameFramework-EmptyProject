using d4160.Core.MonoBehaviours;
using UnityEngine;

namespace Game.World
{
    public class CampusManager : Singleton<CampusManager>
    {
        [SerializeField] private GameObject _classRoom;

        public void SetActiveClassRoom(bool active)
        {
            _classRoom.SetActive(active);
        }
    }
}
