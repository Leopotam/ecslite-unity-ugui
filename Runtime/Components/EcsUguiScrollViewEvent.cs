// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2024 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Leopotam.EcsLite.Unity.Ugui {
    public struct EcsUguiScrollViewEvent {
        public string WidgetName;
        public ScrollRect Sender;
        public Vector2 Value;
    }
}