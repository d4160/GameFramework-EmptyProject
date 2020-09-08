using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class RequireTypeTest : MonoBehaviour, IRequireTypeTest<int>
    {
        [RequireType(typeof(IRequireTypeTest<int>))]
        public Object requireType0;

        public RequireTypeNest1 nest1;

        int IRequireTypeTest<int>.Something => 1;

        [Button]
        private void LogTag0()
        {
            Debug.Log((requireType0 as IRequireTypeTest<int>)?.Something);
        }
    }

    [System.Serializable]
    public class RequireTypeNest1
    {
        [RequireType(typeof(IRequireTypeTest<int>))]
        public Object requireType1;

        public RequireTypeNest2 nest2;
    }

    [System.Serializable]
    public struct RequireTypeNest2
    {
        [RequireType(typeof(IRequireTypeTest<int>))]
        public Object requireType2;
    }

    public interface IRequireTypeTest
    {
        string Something { get; }
    }

    public interface IRequireTypeTest<out T>
    {
        T Something { get; }
    }
}