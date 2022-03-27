// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Leopotam.EcsLite.Unity.Ugui {
    [RequireComponent (typeof (ScrollRect))]
    public sealed class EcsUguiScrollViewAction : EcsUguiActionBase<EcsUguiScrollViewEvent> {
        ScrollRect _scrollView;

        protected override void Awake () {
            base.Awake ();
            _scrollView = GetComponent<ScrollRect> ();
            _scrollView.onValueChanged.AddListener (OnScrollViewValueChanged);
        }

        void OnScrollViewValueChanged (Vector2 value) {
            if (IsValidForEvent ()) {
                ref var msg = ref CreateEvent ();
                msg.WidgetName = GetWidgetName ();
                msg.Sender = _scrollView;
                msg.Value = value;
            }
        }
    }
}