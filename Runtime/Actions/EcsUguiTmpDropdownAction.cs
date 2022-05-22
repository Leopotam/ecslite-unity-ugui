// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using TMPro;
using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    [RequireComponent (typeof (TMP_Dropdown))]
    public sealed class EcsUguiTmpDropdownAction : EcsUguiActionBase<EcsUguiTmpDropdownChangeEvent> {
        TMP_Dropdown _dropdown;

        protected override void Awake () {
            base.Awake ();
            _dropdown = GetComponent<TMP_Dropdown> ();
            _dropdown.onValueChanged.AddListener (OnDropdownValueChanged);
        }

        void OnDropdownValueChanged (int value) {
            if (IsValidForEvent ()) {
                ref var msg = ref CreateEvent ();
                msg.WidgetName = GetWidgetName ();
                msg.Sender = _dropdown;
                msg.Value = value;
            }
        }
    }
}