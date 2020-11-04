//using d4160.Core.MonoBehaviours;
//using Photon.Realtime;
//using UnityEngine;

//namespace Game.UI
//{
//    public class UserTableUI : UnityObjectList<UserRowUI, Player>
//    {
//        public void Remove(Player player)
//        {
//            for (int i = 0; i < _items.Count; i++)
//            {
//                if (_items[i].IsPlayer(player))
//                {
//                    Remove(_items[i]);
//                }
//                else if (_items[i].IsPlayerNull())
//                {
//                    Remove(_items[i]);
//                }
//            }
//        }

//        public void SetMuteToggleState(Player player, bool muteState)
//        {
//            for (int i = 0; i < _items.Count; i++)
//            {
//                if (_items[i].IsPlayer(player))
//                {
//                    _items[i].SetMuteToggleState(muteState);
//                    break;
//                }
//            }
//        }
//    }
//}