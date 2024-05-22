// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2024 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using TMPro;

namespace Leopotam.EcsLite.Unity.Ugui {
    public struct EcsUguiTmpInputChangeEvent {
        public string WidgetName;
        public TMP_InputField Sender;
        public string Value;
    }
}