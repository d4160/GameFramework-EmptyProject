//using NaughtyAttributes;
//using StandardAssets.Characters.Effects;
//using StandardAssets.Characters.Physics;
//using StandardAssets.Characters.ThirdPerson;
//using UnityEngine;

//namespace Game.Character
//{
//    public class CharacterConfigurator : MonoBehaviour
//    {
//        [SerializeField] private OpenCharacterController _openCharacter;
//        [SerializeField] private ThirdPersonBrain _personBrain;
//        [SerializeField] private GameObject _camTarget;

//        [SerializeField] private GameObject _doneCharacter;
//        [SerializeField] private GameObject _doneLeftFoot;
//        [SerializeField] private GameObject _doneRightFoot;

//        [SerializeField] private GameObject _newCharacterRoot;
//        [SerializeField] private GameObject _newLeftFoot;
//        [SerializeField] private GameObject _newRightFoot;
//        [SerializeField] private GameObject newRootTransform;

//        [Button]
//        public void Configure()
//        {
//            _newCharacterRoot.transform.position = _doneCharacter.transform.position;
//            _newCharacterRoot.transform.rotation = _doneCharacter.transform.rotation;

//            var doneCollider = _doneLeftFoot.GetComponent<SphereCollider>();
//            var newCollider = _newLeftFoot.AddComponent<SphereCollider>();
//            newCollider.center = doneCollider.center;
//            newCollider.radius = doneCollider.radius;
//            newCollider.isTrigger = true;

//            _newLeftFoot.AddComponent<Rigidbody>().isKinematic = true;
//            _newRightFoot.AddComponent<Rigidbody>().isKinematic = true;

//            doneCollider = _doneRightFoot.GetComponent<SphereCollider>();
//            newCollider = _newRightFoot.AddComponent<SphereCollider>();
//            newCollider.center = doneCollider.center;
//            newCollider.radius = doneCollider.radius;
//            newCollider.isTrigger = true;

//            _newLeftFoot.AddComponent<ColliderMovementDetection>();
//            _newRightFoot.AddComponent<ColliderMovementDetection>();

//            _camTarget.transform.SetParent(newRootTransform.transform, true);
//        }
//    }
//}
