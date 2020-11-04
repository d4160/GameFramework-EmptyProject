using UnityEngine.EventSystems;

namespace UI_Input
{
	public interface ISimpleInputDraggable
	{
		void OnPointerDown( PointerEventData eventData );
		void OnDrag( PointerEventData eventData );
		void OnPointerUp( PointerEventData eventData );
	}
}