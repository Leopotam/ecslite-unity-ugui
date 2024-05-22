// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2024 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine.UI;

namespace Leopotam.EcsLite.Unity.Ugui {
    public struct EcsUguiSliderChangeEvent {
        public string WidgetName;
        public Slider Sender;
        public float Value;
    }
}