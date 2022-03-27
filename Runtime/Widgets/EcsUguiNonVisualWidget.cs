// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Leopotam.EcsLite.Unity.Ugui {
    [RequireComponent (typeof (CanvasRenderer))]
    [RequireComponent (typeof (RectTransform))]
    public class EcsUguiNonVisualWidget : Graphic {
        public override void SetMaterialDirty () { }
        public override void SetVerticesDirty () { }
        public override Material material { get => defaultMaterial; set { } }
        public override void Rebuild (CanvasUpdate update) { }
    }
}