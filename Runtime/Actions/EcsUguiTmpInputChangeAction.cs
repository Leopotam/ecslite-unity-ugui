// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using TMPro;
using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    [RequireComponent (typeof (TMP_InputField))]
    public sealed class EcsUguiTmpInputChangeAction : EcsUguiActionBase<EcsUguiTmpInputChangeEvent> {
        TMP_InputField _input;

        protected override void Awake () {
            base.Awake ();
            _input = GetComponent<TMP_InputField> ();
            _input.onValueChanged.AddListener (OnInputValueChanged);
        }

        void OnInputValueChanged (string value) {
            if (IsValidForEvent ()) {
                ref var msg = ref CreateEvent ();
                msg.WidgetName = GetWidgetName ();
                msg.Sender = _input;
                msg.Value = value;
            }
        }
    }
}