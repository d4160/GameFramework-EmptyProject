using NaughtyAttributes;
using UnityEngine;

namespace MyBox
{
	public class ActiveStateOnStart : MonoBehaviour
	{
		public bool Active;
		[Required] public GameObject Target;

		private void Awake()
		{
			Target.gameObject.SetActive(Active);
		}
	}
}