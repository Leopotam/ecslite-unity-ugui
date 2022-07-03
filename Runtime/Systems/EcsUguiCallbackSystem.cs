// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Leopotam.EcsLite.Unity.Ugui {
    [AttributeUsage (AttributeTargets.Method)]
    public abstract class EcsUguiEventAttribute : Attribute {
        public readonly string WidgetName;
        public readonly string WorldName;

        public EcsUguiEventAttribute (string widgetName = default, string worldName = default) {
            WidgetName = widgetName;
            WorldName = worldName;
        }
    }

    public sealed class EcsUguiClickEventAttribute : EcsUguiEventAttribute {
        public EcsUguiClickEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiUpEventAttribute : EcsUguiEventAttribute {
        public EcsUguiUpEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiDownEventAttribute : EcsUguiEventAttribute {
        public EcsUguiDownEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiDragStartEventAttribute : EcsUguiEventAttribute {
        public EcsUguiDragStartEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiDragMoveEventAttribute : EcsUguiEventAttribute {
        public EcsUguiDragMoveEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiDragEndEventAttribute : EcsUguiEventAttribute {
        public EcsUguiDragEndEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiEnterEventAttribute : EcsUguiEventAttribute {
        public EcsUguiEnterEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiExitEventAttribute : EcsUguiEventAttribute {
        public EcsUguiExitEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiScrollViewEventAttribute : EcsUguiEventAttribute {
        public EcsUguiScrollViewEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiSliderChangeEventAttribute : EcsUguiEventAttribute {
        public EcsUguiSliderChangeEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiTmpDropdownChangeEventAttribute : EcsUguiEventAttribute {
        public EcsUguiTmpDropdownChangeEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiTmpInputChangeEventAttribute : EcsUguiEventAttribute {
        public EcsUguiTmpInputChangeEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiTmpInputEndEventAttribute : EcsUguiEventAttribute {
        public EcsUguiTmpInputEndEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public sealed class EcsUguiDropEventAttribute : EcsUguiEventAttribute {
        public EcsUguiDropEventAttribute (string widgetName = default, string worldName = default) : base (widgetName, worldName) { }
    }

    public delegate void UserCallback<T> (in T e) where T : struct;

    public abstract class EcsUguiCallbackSystem : IEcsPreInitSystem, IEcsRunSystem {
        class UguiEventDesc<T> where T : struct {
            public readonly EcsFilter Filter;
            public readonly EcsPool<T> Pool;
            public readonly string WidgetName;
            public readonly UserCallback<T> Callback;

            public UguiEventDesc (EcsFilter filter, EcsPool<T> pool, string widgetName, UserCallback<T> cb) {
                Filter = filter;
                Pool = pool;
                WidgetName = widgetName;
                Callback = cb;
            }
        }

        List<UguiEventDesc<EcsUguiClickEvent>> _clicks;
        List<UguiEventDesc<EcsUguiUpEvent>> _ups;
        List<UguiEventDesc<EcsUguiDownEvent>> _downs;
        List<UguiEventDesc<EcsUguiDragStartEvent>> _dragStarts;
        List<UguiEventDesc<EcsUguiDragMoveEvent>> _dragMoves;
        List<UguiEventDesc<EcsUguiDragEndEvent>> _dragEnds;
        List<UguiEventDesc<EcsUguiEnterEvent>> _enters;
        List<UguiEventDesc<EcsUguiExitEvent>> _exits;
        List<UguiEventDesc<EcsUguiScrollViewEvent>> _scrollViews;
        List<UguiEventDesc<EcsUguiSliderChangeEvent>> _sliderChanges;
        List<UguiEventDesc<EcsUguiTmpDropdownChangeEvent>> _tmpDropdownChanges;
        List<UguiEventDesc<EcsUguiTmpInputChangeEvent>> _tmpInputChanges;
        List<UguiEventDesc<EcsUguiTmpInputEndEvent>> _tmpInputEnds;
        List<UguiEventDesc<EcsUguiDropEvent>> _drops;

        public void PreInit (IEcsSystems systems) {
            foreach (var m in GetType ().GetMethods (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (m.IsStatic) { continue; }

                CheckAttribute<EcsUguiClickEventAttribute, EcsUguiClickEvent> (m, systems, this, ref _clicks);
                CheckAttribute<EcsUguiUpEventAttribute, EcsUguiUpEvent> (m, systems, this, ref _ups);
                CheckAttribute<EcsUguiDownEventAttribute, EcsUguiDownEvent> (m, systems, this, ref _downs);
                CheckAttribute<EcsUguiDragStartEventAttribute, EcsUguiDragStartEvent> (m, systems, this, ref _dragStarts);
                CheckAttribute<EcsUguiDragMoveEventAttribute, EcsUguiDragMoveEvent> (m, systems, this, ref _dragMoves);
                CheckAttribute<EcsUguiDragEndEventAttribute, EcsUguiDragEndEvent> (m, systems, this, ref _dragEnds);
                CheckAttribute<EcsUguiEnterEventAttribute, EcsUguiEnterEvent> (m, systems, this, ref _enters);
                CheckAttribute<EcsUguiExitEventAttribute, EcsUguiExitEvent> (m, systems, this, ref _exits);
                CheckAttribute<EcsUguiScrollViewEventAttribute, EcsUguiScrollViewEvent> (m, systems, this, ref _scrollViews);
                CheckAttribute<EcsUguiSliderChangeEventAttribute, EcsUguiSliderChangeEvent> (m, systems, this, ref _sliderChanges);
                CheckAttribute<EcsUguiTmpDropdownChangeEventAttribute, EcsUguiTmpDropdownChangeEvent> (m, systems, this, ref _tmpDropdownChanges);
                CheckAttribute<EcsUguiTmpInputChangeEventAttribute, EcsUguiTmpInputChangeEvent> (m, systems, this, ref _tmpInputChanges);
                CheckAttribute<EcsUguiTmpInputEndEventAttribute, EcsUguiTmpInputEndEvent> (m, systems, this, ref _tmpInputEnds);
                CheckAttribute<EcsUguiDropEventAttribute, EcsUguiDropEvent> (m, systems, this, ref _drops);
            }
        }

        public virtual void Run (IEcsSystems systems) {
            if (_clicks != null) {
                foreach (var item in _clicks) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_ups != null) {
                foreach (var item in _ups) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_downs != null) {
                foreach (var item in _downs) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_dragStarts != null) {
                foreach (var item in _dragStarts) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_dragMoves != null) {
                foreach (var item in _dragMoves) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_dragEnds != null) {
                foreach (var item in _dragEnds) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_enters != null) {
                foreach (var item in _enters) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_exits != null) {
                foreach (var item in _exits) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_scrollViews != null) {
                foreach (var item in _scrollViews) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_sliderChanges != null) {
                foreach (var item in _sliderChanges) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_tmpDropdownChanges != null) {
                foreach (var item in _tmpDropdownChanges) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_tmpInputChanges != null) {
                foreach (var item in _tmpInputChanges) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_tmpInputEnds != null) {
                foreach (var item in _tmpInputEnds) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
            if (_drops != null) {
                foreach (var item in _drops) {
                    foreach (var entity in item.Filter) {
                        ref var e = ref item.Pool.Get (entity);
                        CallUserMethod (e, e.WidgetName, item);
                    }
                }
            }
        }

        static void CheckAttribute<T1, T2> (MethodInfo m, IEcsSystems systems, EcsUguiCallbackSystem system, ref List<UguiEventDesc<T2>> list)
            where T1 : EcsUguiEventAttribute where T2 : struct {
            var attrType = typeof (T1);
            if (Attribute.IsDefined (m, attrType)) {
                RegisterCallback ((T1) Attribute.GetCustomAttribute (m, attrType), m, systems, system, ref list);
            }
        }

        static void RegisterCallback<T> (EcsUguiEventAttribute attr, MethodInfo methodInfo, IEcsSystems systems, EcsUguiCallbackSystem system, ref List<UguiEventDesc<T>> list)
            where T : struct {
            var world = systems.GetWorld (attr.WorldName);
            var name = string.IsNullOrEmpty (attr.WidgetName) ? null : attr.WidgetName;
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (world == null) { throw new Exception ($"World for \"{typeof (T).Name}\" event in system \"{system.GetType ().Name}\" for widget \"{name ?? "<ANY>"}\" not exist."); }
#endif
            var cb = (UserCallback<T>) Delegate.CreateDelegate (
                typeof (UserCallback<T>),
                system,
                methodInfo, false);
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (cb == null) { throw new Exception ($"Callback method for \"{typeof (T).Name}\" event in system \"{system.GetType ().Name}\" for widget \"{attr.WidgetName}\" not compatible, should be \"void MethodName(in {typeof (T).Name} eventName) {{}}\"."); }
#endif
            list ??= new List<UguiEventDesc<T>> ();
            list.Add (new UguiEventDesc<T> (world.Filter<T> ().End (), world.GetPool<T> (), name, cb));
        }

        static void CallUserMethod<T> (in T e, string widgetName, UguiEventDesc<T> desc) where T : struct {
            if (desc.WidgetName == null || string.CompareOrdinal (widgetName, desc.WidgetName) == 0) {
                desc.Callback (e);
            }
        }
    }
}