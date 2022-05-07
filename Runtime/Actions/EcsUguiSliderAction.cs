// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Leopotam.EcsLite.Unity.Ugui {
    [RequireComponent (typeof (Slider))]
    public sealed class EcsUguiSliderAction : EcsUguiActionBase<EcsUguiSliderChangeEvent> {
        Slider _slider;

        protected override void Awake () {
            base.Awake ();
            _slider = GetComponent<Slider> ();
            _slider.onValueChanged.AddListener (OnSliderValueChanged);
        }

        void OnSliderValueChanged (float value) {
            if (IsValidForEvent ()) {
                ref var msg = ref CreateEvent ();
                msg.WidgetName = GetWidgetName ();
                msg.Sender = _slider;
                msg.Value = value;
            }
        }
    }
}