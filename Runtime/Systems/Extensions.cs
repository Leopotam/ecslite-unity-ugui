// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using System.Reflection;
using UnityEngine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsLite.Unity.Ugui {
    public sealed class EcsUguiNamedAttribute : Attribute {
        public readonly string Name;

        public EcsUguiNamedAttribute (string name) {
            Name = name;
        }
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    sealed class DelUguiEventSystem<T> : IEcsRunSystem where T : struct {
        readonly EcsFilter _filter;
        readonly EcsPool<T> _pool;

        public DelUguiEventSystem (EcsWorld world) {
            _filter = world.Filter<T> ().End ();
            _pool = world.GetPool<T> ();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _filter) {
                _pool.Del (entity);
            }
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
            var world = ecsSystems.GetWorld (worldName);
            if (!skipDelHere) {
                AddDelHereSystems (ecsSystems, world);
            }
            emitter.SetWorld (world);
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

        static void AddDelHereSystems (IEcsSystems ecsSystems, EcsWorld world) {
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiDragStartEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiDragMoveEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiDragEndEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiDropEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiClickEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiDownEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiUpEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiEnterEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiExitEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiScrollViewEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiSliderChangeEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiTmpDropdownChangeEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiTmpInputChangeEvent> (world));
            ecsSystems.Add (new DelUguiEventSystem<EcsUguiTmpInputEndEvent> (world));
        }
    }
}

#if ENABLE_IL2CPP
// Unity IL2CPP performance optimization attribute.
namespace Unity.IL2CPP.CompilerServices {
    enum Option {
        NullChecks = 1,
        ArrayBoundsChecks = 2
    }

    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    class Il2CppSetOptionAttribute : Attribute {
        public Option Option { get; private set; }
        public object Value { get; private set; }

        public Il2CppSetOptionAttribute (Option option, object value) { Option = option; Value = value; }
    }
}
#endif