# LeoECS Lite uGui Bindings - поддержка событий uGui в ECS-мире
Интеграция событий uGui в ECS-мир.

> Проверено на Unity 2020.3 (зависит от Unity) и содержит asmdef-описания для компиляции в виде отдельных сборок и уменьшения времени рекомпиляции основного проекта.

> **ВАЖНО!** Зависит от [LeoECS Lite](https://github.com/Leopotam/ecslite) - зависимость должна быть установлена до установки этого модуля.

* [Установка](#Установка)
    * [В виде unity модуля](#В-виде-unity-модуля)
    * [В виде исходников](#В-виде-исходников)
* [Классы](#Классы)
    * [EcsUguiEmitter](#EcsUguiEmitter)
    * [EcsUguiCallbackSystem](#EcsUguiCallbackSystem)
    * [Действия](#Действия)
    * [Компоненты](#Компоненты)
* [Лицензия](#Лицензия)

# Социальные ресурсы
[![discord](https://img.shields.io/discord/404358247621853185.svg?label=enter%20to%20discord%20server&style=for-the-badge&logo=discord)](https://discord.gg/5GZVde6)

# Установка

## В виде unity модуля
Поддерживается установка в виде unity-модуля через git-ссылку в PackageManager или прямое редактирование `Packages/manifest.json`:
```
"com.leopotam.ecslite.unity.ugui": "https://github.com/Leopotam/ecslite-unity-ugui.git",
```
По умолчанию используется последняя релизная версия. Если требуется версия "в разработке" с актуальными изменениями - следует переключиться на ветку `develop`:
```
"com.leopotam.ecslite.unity.ugui": "https://github.com/Leopotam/ecslite-unity-ugui.git#develop",
```

## В виде исходников
Код так же может быть склонирован или получен в виде архива со страницы релизов.

# Классы

## EcsUguiEmitter
`EcsUiEmitter` является `MonoBehaviour`-классом, отвечающим за генерацию ECS-событий на основе uGui-событий (нажатие, отпускание, перетаскивание и т.п).
Должен быть размещен на корневом `GameObject`-е UI-иерархии (или хотя бы на корневом `Canvas`-е) и подключен к ECS-инфраструктуре через инспектор:
```c#
using Leopotam.EcsLite.Unity.Ugui;

public class Startup : MonoBehaviour {
    // Поле должно быть проинициализировано в инспекторе средствами редактора Unity.
    [SerializeField] EcsUguiEmitter _uguiEmitter;

    IEcsSystems _systems;

    void Start () {
        _systems = new EcsSystems (new EcsWorld ());
        _systems
            .Add (new Test1System ())
            .Add (new Test2System ())
            // Этот вызов должен быть размещен после всех систем,
            // в которых есть зависимость от uGui-событий.
            .InjectUgui (_uguiEmitter)
            .Init ();
    }
    
    void Update () {
        _systems?.Update ();
    }
    
    void OnDestroy () {
        if (_systems != null) {
            _systems.GetWorld ("ugui-events").Destroy ();
            _systems.Destroy ();
            _systems.GetWorld ().Destroy ();
            _systems = null;
        }
    }
}

public class Test1System : IEcsInitSystem {
    // Это поле будет автоматически инициализировано
    // ссылкой на экземпляр эмиттера на сцене.
    readonly EcsUguiEmitter _ugui = default;
    
    GameObject _btnGo;
    Transform _btnTransform;
    Button _btn;

    public void Init (IEcsSystems systems) {
        // Получение ссылки на виджет-действие с именем "MyButton". 
        _btnGo = _ugui.GetNamedObject ("MyButton");
        // Чтение Transform-компонента с него.
        _btnTransform = _ugui.GetNamedObject ("MyButton").GetComponent<Transform> ();
        // Чтение Button-компонента с него.
        _btn = _ugui.GetNamedObject ("MyButton").GetComponent<Button> ();
    }
}
```
Пример выше можно упростить через `[EcsUguiNamedAttribute]`:
```c#
using Leopotam.EcsLite.Unity.Ugui;

public class Test2System : IEcsInitSystem {
    // Все поля будут автоматически заполнены ссылками
    // на соответствующие компоненты с именованного виджета-действия.
    [EcsUguiNamed("MyButton")] GameObject _btnGo;
    [EcsUguiNamed("MyButton")] Transform _btnTransform;
    [EcsUguiNamed("MyButton")] Button _btn;

    public void Init (IEcsSystems systems) {
        // Все поля инициализированы и могут быть использованы здесь.
    }
}
```

## EcsUguiCallbackSystem
Эта система дает возможность напрямую подписываться на uGui-события без дополнительного кода:
```c#
using Leopotam.EcsLite.Unity.Ugui;

public class TestUguiClickEventSystem : EcsUguiCallbackSystem {
    [Preserve] // Этот атрибут необходим для сохранения этого метода для il2cpp.
    [EcsUguiClickEvent]
    void OnAnyClick (in EcsUguiClickEvent evt) {
        Debug.Log ("Im clicked!", evt.Sender);
    }
    
    // Этот метод будет вызван при нажатии на виджет с действием, имеющим имя "exit-button". 
    [Preserve]
    [EcsUguiClickEvent("exit-button")]
    void OnExitButtonClicked (in EcsUguiClickEvent evt) {
        Debug.Log ("exit-button clicked!", evt.Sender);
    }
}
```
Список поддерживаемых атрибутов действий (событий uGui):
```c#
[EcsUguiClickEvent]
[EcsUguiUpEvent]
[EcsUguiDownEvent]
[EcsUguiDragStartEvent]
[EcsUguiDragMoveEvent]
[EcsUguiDragEndEvent]
[EcsUguiEnterEvent]
[EcsUguiExitEvent]
[EcsUguiScrollViewEvent]
[EcsUguiSliderChangeEvent]
[EcsUguiTmpDropdownChangeEvent]
[EcsUguiTmpInputChangeEvent]
[EcsUguiTmpInputEndEvent]
[EcsUguiDropEvent]
```
## Действия
Действия (классы `xxxAction`) - это `MonoBehaviour`-компоненты, которые слушают события uGui виджетов, ищут `EcsUiEmitter` по иерархии вверх и вызывают генерацию соответствующих событий для ECS-мира.

## Компоненты
ECS-компоненты, описывающие события: `EcsUguiClickEvent`, `EcsUguiBeginDragEvent`, `EcsUguiEndDragEvent` и т.д. - все они являются стандартными ECS-компонентами и могут быть отфильтрованы с помощью `EcsFilter`:
```c#
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

public class TestUguiClickEventSystem : IEcsInitSystem, IEcsRunSystem {
    EcsPool<EcsUguiClickEvent> _clickEventsPool;
    EcsFilter _clickEvents;
    
    public void Init (IEcsSystems systems) {
        var world = systems.GetWorld ();
        _clickEventsPool = world.GetPool<EcsUguiClickEvent> (); 
        _clickEvents = world.Filter<EcsUguiClickEvent> ().End ();
    }

    public void Run (IEcsSystems systems) {
        foreach (var entity in _clickEvents) {
            ref EcsUguiClickEvent data = ref _clickEventsPool.Get (entity);
            Debug.Log ("Im clicked!", data.Sender);
        }
    }
}
```

# Лицензия
Фреймворк выпускается под двумя лицензиями, [подробности тут](./LICENSE.md).

В случаях лицензирования по условиям MIT-Red не стоит расчитывать на
персональные консультации или какие-либо гарантии.