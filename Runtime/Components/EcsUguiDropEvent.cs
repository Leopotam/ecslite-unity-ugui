// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2024 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace Leopotam.EcsLite.Unity.Ugui {
    public struct EcsUguiDropEvent {
        public string WidgetName;
        public GameObject Sender;
        public PointerEventData.InputButton Button;
    }
}