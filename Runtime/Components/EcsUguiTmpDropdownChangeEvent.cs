// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2024 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using TMPro;

namespace Leopotam.EcsLite.Unity.Ugui {
    public struct EcsUguiTmpDropdownChangeEvent {
        public string WidgetName;
        public TMP_Dropdown Sender;
        public int Value;
    }
}