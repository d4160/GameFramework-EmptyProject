using d4160.Core;
using UnityEngine;

namespace Game.Character_Inventory
{
    public class CharacterInventorySystem : MonoBehaviourData<CharacterInventory>
    {
        [Header("RENDERERS")] 
        [SerializeField] private SpriteRenderer _headRen;
        [SerializeField] private SpriteRenderer _bodyRen;
        [SerializeField] private SpriteRenderer _eyeRen;
        [SerializeField] private SpriteRenderer _gloveBackRen;
        [SerializeField] private SpriteRenderer _gloveFrontRen;
        [SerializeField] private SpriteRenderer _bootBackRen;
        [SerializeField] private SpriteRenderer _bootFrontRen;

        //[Header("LIBRARIES")]
        //[SerializeField] private SpriteLibrary _gloveBackLib;
        //[SerializeField] private SpriteLibrary _gloveFrontLib;
        //[SerializeField] private SpriteLibrary _bootBackLib;
        //[SerializeField] private SpriteLibrary _bootFrontLib;

        public void ChangeHeadColor(Color color)
        {
            _headRen.color = color;
        }

        public void ChangeBodyColor(Color color)
        {
            _bodyRen.color = color;
        }

        public void ChangeEyeColor(Color color)
        {
            _eyeRen.color = color;
        }

        public void SetGloves(string label)
        {
            //var sprite = _gloveBackLib.GetSprite("GloveBack", label);
            //_gloveBackRen.sprite = sprite;
            //_gloveFrontRen.sprite = _gloveFrontLib.GetSprite("GloveFront", label);
        }

        public void SetBoots(string label)
        {
            //var backSprite = _bootBackLib.GetSprite("BootBack", label);
            //_bootBackRen.sprite = backSprite;

            //var frontSprite = _bootFrontLib.GetSprite("BootFront", label);
            //_bootFrontRen.sprite = frontSprite;
        }
    }
}
