using d4160.Core;
using Lean.Pool;
using UnityEngine;

public class PlayerProvider : MonoBehaviour, IUnityObjectProvider<GameObject>, IObjectDisposer<GameObject>
{
    [SerializeField] private LeanGameObjectPool[] _womanCharacterPools;
    [SerializeField] private LeanGameObjectPool[] _manCharacterPools;

    private bool _womanCharacterPoolSelected;
    private int _selectedPoolIndex;

    public LeanGameObjectPool SelectedPool =>
        _womanCharacterPoolSelected
            ? _womanCharacterPools[_selectedPoolIndex]
            : _manCharacterPools[_selectedPoolIndex];

    public void SelectPool(int role, string genre)
    {
        switch (genre)
        {
            case "F":
                _womanCharacterPoolSelected = true;
                break;
            case "M":
            default:
                _womanCharacterPoolSelected = false;
                break;
        }

        _selectedPoolIndex = role - 1;
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        return SelectedPool.Spawn(position, rotation);
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent, bool worldStays)
    {
        return SelectedPool.Spawn(position, rotation, parent, worldStays);
    }

    public GameObject Spawn()
    {
        return SelectedPool.Spawn();
    }

    public void Despawn(GameObject clone, float t = 0)
    {
        SelectedPool.Despawn(clone, t);
    }

    public void DespawnAll()
    {
        SelectedPool.DespawnAll();
    }
}
