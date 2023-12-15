// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    public class EcsUguiProxyEmitter : EcsUguiEmitter {
        public enum SearchType {
            InGlobal,
            InHierarchy
        }

        [SerializeField] EcsUguiEmitter _parent;
        [SerializeField] SearchType _searchType = SearchType.InGlobal;

        public override EcsWorld GetWorld () {
            return ValidateEmitter () ? _parent.GetWorld () : default;
        }

        public override void SetNamedObject (string widgetName, GameObject go) {
            if (ValidateEmitter ()) { _parent.SetNamedObject (widgetName, go); }
        }

        public override GameObject GetNamedObject (string widgetName) {
            return ValidateEmitter () ? _parent.GetNamedObject (widgetName) : default;
        }

        bool ValidateEmitter () {
            if (_parent) { return true; }
            // parent was killed.
            if ((object) _parent != null) { return false; }

            EcsUguiEmitter parent = default;
            if (_searchType == SearchType.InGlobal) {
                var validType = typeof (EcsUguiEmitter);
                foreach (var em in FindObjectsOfType<EcsUguiEmitter> ()) {
                    if (em.GetType () == validType) {
                        parent = em;
                        break;
                    }
                }
            } else {
                parent = GetComponentInParent<EcsUguiEmitter> ();
            }
            // fix for GetComponentInParent.
            if (parent == this) { parent = null; }
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (parent == null) {
                Debug.LogError ("EcsUiEmitter not found in hierarchy", this);
                return false;
            }
#endif
            _parent = parent;
            return true;
        }
    }
}