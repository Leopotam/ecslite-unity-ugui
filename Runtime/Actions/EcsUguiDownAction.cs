// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2024 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine.EventSystems;

namespace Leopotam.EcsLite.Unity.Ugui {
    public sealed class EcsUguiDownAction : EcsUguiActionBase<EcsUguiDownEvent>, IPointerDownHandler {
        public void OnPointerDown (PointerEventData eventData) {
            if (IsValidForEvent ()) {
                ref var msg = ref CreateEvent ();
                msg.WidgetName = GetWidgetName ();
                msg.Sender = gameObject;
                msg.Position = eventData.position;
                msg.PointerId = eventData.pointerId;
                msg.Button = eventData.button;
            }
        }
    }
}