// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using System.Reflection;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    public sealed class EcsUguiNamedAttribute : Attribute {
        public readonly string Name;

        public EcsUguiNamedAttribute (string name) {
            Name = name;
        }
    }

    public static class EcsSystemsExtensions {
        /// <summary>
        /// Injects named UI objects and Emitter to all systems added to EcsSystems.
        /// </summary>
        /// <param name="ecsSystems">EcsSystems group.</param>
        /// <param name="emitter">EcsUiEmitter instance.</param>
        /// <param name="worldName">World name.</param>
        /// <param name="skipNoExists">Not throw exception if named action not registered in emitter.</param>
        /// <param name="skipDelHere">Skip DelHere() registration.</param>
        public static IEcsSystems InjectUgui (this IEcsSystems ecsSystems, EcsUguiEmitter emitter, string worldName = null, bool skipNoExists = false, bool skipDelHere = false) {
            if (!skipDelHere) {
                AddDelHereSystems (ecsSystems, worldName);
            }
            emitter.SetWorld (ecsSystems.GetWorld (worldName));
            var uiNamedType = typeof (EcsUguiNamedAttribute);
            var goType = typeof (GameObject);
            var componentType = typeof (Component);
            var emitterType = typeof (EcsUguiEmitter);
            foreach (var system in ecsSystems.GetAllSystems ()) {
                var systemType = system.GetType ();
                foreach (var f in systemType.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                    // skip statics.
                    if (f.IsStatic) {
                        continue;
                    }
                    // emitter.
                    if (f.FieldType == emitterType) {
                        f.SetValue (system, emitter);
                        continue;
                    }
                    // skip fields without [EcsUiNamed] attribute.
                    if (!Attribute.IsDefined (f, uiNamedType)) {
                        continue;
                    }
                    var name = ((EcsUguiNamedAttribute) Attribute.GetCustomAttribute (f, uiNamedType)).Name;
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
                    if (string.IsNullOrEmpty (name)) { throw new Exception ($"Cant Inject field \"{f.Name}\" at \"{systemType}\" due to [EcsUiNamed] \"Name\" parameter is invalid."); }
                    if (!(f.FieldType == goType || componentType.IsAssignableFrom (f.FieldType))) {
                        throw new Exception ($"Cant Inject field \"{f.Name}\" at \"{systemType}\" due to [EcsUiNamed] attribute can be applied only to GameObject or Component type.");
                    }
                    if (!skipNoExists && !emitter.GetNamedObject (name)) { throw new Exception ($"Cant Inject field \"{f.Name}\" at \"{systemType}\" due to there is no UI action with name \"{name}\"."); }
#endif
                    var go = emitter.GetNamedObject (name);
                    // GameObject.
                    if (f.FieldType == goType) {
                        f.SetValue (system, go);
                        continue;
                    }
                    // Component.
                    if (componentType.IsAssignableFrom (f.FieldType)) {
                        f.SetValue (system, go != null ? go.GetComponent (f.FieldType) : null);
                    }
                }
            }
            return ecsSystems;
        }

        static void AddDelHereSystems (IEcsSystems ecsSystems, string worldName) {
            ecsSystems.DelHere<EcsUguiDragStartEvent> (worldName);
            ecsSystems.DelHere<EcsUguiDragMoveEvent> (worldName);
            ecsSystems.DelHere<EcsUguiDragEndEvent> (worldName);
            ecsSystems.DelHere<EcsUguiDropEvent> (worldName);
            ecsSystems.DelHere<EcsUguiClickEvent> (worldName);
            ecsSystems.DelHere<EcsUguiDownEvent> (worldName);
            ecsSystems.DelHere<EcsUguiUpEvent> (worldName);
            ecsSystems.DelHere<EcsUguiEnterEvent> (worldName);
            ecsSystems.DelHere<EcsUguiExitEvent> (worldName);
            ecsSystems.DelHere<EcsUguiScrollViewEvent> (worldName);
            ecsSystems.DelHere<EcsUguiSliderChangeEvent> (worldName);
            ecsSystems.DelHere<EcsUguiTmpDropdownChangeEvent> (worldName);
            ecsSystems.DelHere<EcsUguiTmpInputChangeEvent> (worldName);
            ecsSystems.DelHere<EcsUguiTmpInputEndEvent> (worldName);
        }
    }
}