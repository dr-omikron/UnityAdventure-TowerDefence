---
paths: ["**/*ContextRegistrations.cs", "**/*Bootstrap.cs"]
---

# DI-регистрация и бутстрап сцены

Применяется к `...ContextRegistrations.cs` (`ProjectContextRegistrations`, `GameplayContextRegistrations`, `MainMenuContextRegistrations`) и к `...Bootstrap.cs` (`GameplayBootstrap`, `MainMenuBootstrap`).

## `...ContextRegistrations.cs`

- Статический класс с единственным методом `Process(DIContainer container, ...)`.
- На каждый сервис — пара: `container.RegisterAsSingle<TKey>(CreateX);` + приватный статический фабричный метод `private static TConcrete CreateX(DIContainer c) => new TConcrete(c.Resolve<Dep1>(), c.Resolve<Dep2>(), ...);`. Внедрение зависимостей всегда явное — через `c.Resolve<T>()` внутри `CreateX`, без рефлексии.
- **Ключ регистрации — интерфейс**, только если сервису реально нужна подмена реализации или он выступает abstraction-точкой (`ICoroutinesPerformer`, `ILoadingScreen`, `IInputService`, `ISaveLoadService`). Остальные геймплейные сервисы регистрируй по конкретному типу — не добавляй интерфейс «на будущее».
- **`.NonLazy()`** — ставь, когда у сервиса есть побочный эффект в конструкторе, который должен произойти независимо от того, резолвит ли его кто-то явно (подписка на события, инстанс UI-рута из Resources, регистрация в реестре). Примеры: `MonoEntityFactory`, `WalletService`, `StationHolderService`.
- Порядок вызовов внутри `Process` — от сервисов без зависимостей к фабрикам и далее к контекстам, которые их используют (регистрация оборачивается в замыкание и резолвится лениво, но читаемость выигрывает от такого порядка).
- Разделение по контексту жёсткое: глобальные/сквозные сервисы — только в `ProjectContextRegistrations`; геймплейные — только в `GameplayContextRegistrations`; меню/мета — только в `MainMenuContextRegistrations`. Не регистрируй геймплейный сервис в проектном контексте, даже если он используется в двух сценах — вместо этого зарегистрируй его дважды в своих контекстах или подними в проектный, только если это осознанное решение.

## `...Bootstrap.cs` (`SceneBootstrap`)

- Реализует `ProcessRegistration(DIContainer container, IInputSceneArgs sceneArgs)` (кастует `sceneArgs` к своему типу и зовёт `Process` из соответствующего `ContextRegistrations`), `Initialize()` (корутина: резолвит контексты сцены, создаёт стартовые entity), `Run()` (входит в стейт-машину/стартует сцену), `Update()`.
- Если у сцены есть пер-кадровая симуляция (Gameplay) — `Update()` бутстрапа вручную тикает все контексты сцены (`AIBrainContext`, `EntitiesLifeContext`, `GameplayStatesContext`, ...) через `Time.deltaTime`. Другого центрального игрового цикла в проекте нет — не заводи ещё один `Update` где-то ещё для той же цели.
- Контейнер сцены создаётся `SceneSwitcherService` как **дочерний** от корневого проектного контейнера (`new DIContainer(_projectContainer)`) и диспоузится при следующем переключении сцены — не храни в бутстрапе состояние, которое должно пережить смену сцены (для этого используй проектный контейнер/сервисы).
