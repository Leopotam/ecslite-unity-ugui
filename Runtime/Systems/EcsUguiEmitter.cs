// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    /// <summary>
    /// Emitter system for uGui events to ECS world.
    /// </summary>
    public class EcsUguiEmitter : MonoBehaviour {
        EcsWorld _world;
        readonly Dictionary<int, GameObject> _actions = new Dictionary<int, GameObject> (64);

        internal void SetWorld (EcsWorld world) {
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (_world != null) { throw new Exception ("World already attached."); }
#endif
            _world = world;
        }

        public virtual EcsWorld GetWorld () {
            return _world;
        }

        public virtual void SetNamedObject (string widgetName, GameObject go) {
            if (!string.IsNullOrEmpty (widgetName)) {
                var id = widgetName.GetHashCode ();
                if (_actions.ContainsKey (id)) {
                    if (!go) {
                        _actions.Remove (id);
                    }
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                    if (go) {
                        throw new Exception ($"Action with \"{widgetName}\" name already registered");
                    }
#endif
                } else {
                    if ((object) go != null) {
                        _actions[id] = go.gameObject;
                    }
                }
            }
        }

        /// <summary>
        /// Gets link to named GameObject to use it later from code.
        /// </summary>
        /// <param name="widgetName">Logical name.</param>
        public virtual GameObject GetNamedObject (string widgetName) {
            _actions.TryGetValue (widgetName.GetHashCode (), out var retVal);
            return retVal;
        }
    }
}