# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Контекст проекта

Шаблон Unity (`2022.3.16f1`) для разработки игр. Стек:

- **Геймплей** — Custom ECS framework
- **DI** — Custom DIContainer
- **Асинхронное программирование** — Coroutines
- **Реактивное программирование** — Custom (_Project\Develop\Runtime\Utilities\Reactive)
- **Менеджмент контента** — Resources Folder
- **Анимации кодом** — DoTween.
- **Текст** — TMP

Дополнительно: рендеринг — URP, NuGet-пакеты подключаются через NuGetForUnity.

> При добавлении новой технологии/фреймворка в проект пользователь будет расширять этот раздел. Если видишь в коде технологию, которой нет в этом списке — спроси у пользователя, нужно ли её сюда внести.

Сообщения коммитов в репозитории — **на английском**. Соблюдай этот стиль.

## Где живёт код

`Assets/_Project/` — папка проекта со **всем пользовательским кодом и ресурсами**. Всё остальное под `Assets/` — это импортированные пакеты и плагины (`DOTween`, `TextMesh Pro`, `Thirdparty`, `Newtonsoft.Json`, `Settings`, …); такое разделение нужно, чтобы свои ассеты не путались с пакетными. Подчёркивание в имени держит `_Project` наверху Project-окна — сохраняй его и в namespace.

```
Assets/_Project/
├── Develop/                    # ВЕСЬ код проекта
│   ├── Editor/                 # Editor-only тулинг
│   └── Runtime/                # Не-редакторский код, попадает в билд
│       ├── Infrastructure/     # Самописный DI контейнер, EntryPoint
│       ├── Configs/            # ScriptableObject скрипты
│       ├── Gameplay/           # КОР игры
│       │   ├── Infrastructure/ # GameplayBootstrap, GameplayInstaller
│       │   └── Features/       # По одной папке на фичу — см. «Шаблон фичи» ниже
│       ├── Meta/               # МЕТА-механики (по той же схеме, что и Gameplay/)
│       │   ├── Infrastructure/ # MainMenuBootstrap и пр.
│       │   └── Features/       # Прогресс уровней, кошелек и т.п.
│       │── UI/                 # UI скрипты
│       └── Utilities/          # Сервисы — утилиты
├── Resources/                  # Только то, что грузится на старте (.prefab)
├── Scenes/                     # GameEntryPoint, EmptyScene, MainMenuScene, GameplayScene
└── Art/                        # Художественные ассеты
```

**Параллелизм `Gameplay/` и `Meta/`** — это два домена игры одного уровня. Оба следуют одной схеме: подпапка `Infrastructure/` для бутстрапов/инсталлеров своей сцены и подпапка `Features/` для разбиения на фичи. Когда в игре появятся мета-механики (валюты, прогрессия, баттл-пассы) — их фичи кладём в `Meta/Features/`, по тому же шаблону, что и геймплейные. Не смешиваем мета-логику в `Gameplay/Features/` и наоборот.

## Рантайм-пайплайн (прочитай до изменений сцен/DI)

### Порядок сцен и загрузки

`GameEntryPoint.cs` (сцена `EntryPoint`) — точка входа: создаёт корневой `DIContainer`, прогоняет `ProjectContextRegistrations.Process(container)`, вызывает `container.Initialize()` и стартует корутину загрузки (конфиги → сейв → `SceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu)`).

Любое переключение сцены идёт **только** через `SceneSwitcherService.ProcessSwitchTo(sceneName, sceneArgs)`, никогда напрямую через `SceneManager`:
1. Показывает `ILoadingScreen`, диспоузит DI-контейнер предыдущей сцены.
2. Грузит пустую сцену `Empty` (гарантированно выгружает предыдущую сцену), затем целевую.
3. Находит на сцене `SceneBootstrap`, создаёт для неё **дочерний** `DIContainer` (parent = корневой проектный контейнер) → `bootstrap.ProcessRegistration(container, sceneArgs)` → `container.Initialize()` → `bootstrap.Initialize()` (корутина) → прячет loading screen → `bootstrap.Run()`.

Схема: `EntryPoint` → `Empty` → `MainMenu` ⇄ (через `Empty`) ⇄ `Gameplay`.

- `GameplayBootstrap : SceneBootstrap` — регистрирует геймплейные сервисы (`GameplayContextRegistrations.Process`), в `Initialize()` создаёт entity станции (`StationFactory.Create`), в `Run()` запускает `GameplayStatesContext`. **`Update()` этого бутстрапа — единственный тик ECS в проекте**: вручную обновляет `AIBrainContext`, `EntitiesLifeContext`, `GameplayStatesContext` каждый кадр через `Time.deltaTime`.
- `MainMenuBootstrap : SceneBootstrap` — регистрирует `MainMenuContextRegistrations.Process`; без пер-кадровой симуляции — меню событийное/UI-driven.

Глобальные (сквозные для всех сцен) сервисы регистрируются в `ProjectContextRegistrations.cs`. Геймплейные — в `GameplayContextRegistrations.cs`. Сервисы меню и мета-геймплея — в `MainMenuContextRegistrations.cs`. Конвенции самой регистрации (`RegisterAsSingle`/`CreateX`, `.NonLazy()`, интерфейс vs конкретный тип) — см. `.claude/rules/di-registration.md`.

### Как добавить фичу в Gameplay (данные/система для Entity)

1. **Компоненты**: создай `{FeatureName}Components.cs` с классами-данными, реализующими `IEntityComponent`.
2. **Кодогенерация**: после добавления/переименования компонента запусти `Tools/GenerateEntityAPI` в Unity — иначе `entity.AddX(...)`/`entity.X`/`entity.TryGetX(...)` не появятся в `Generated/EntityAPI.cs`.
3. **Система**: создай `{Behavior}System.cs`, реализующий нужное подмножество `IInitializableSystem`/`IUpdateableSystem`/`IDisposableSystem`, использующий данные из шага 1.
4. **Подключение**: в `EntitiesFactory` (`Gameplay/EntitiesCore/EntitiesFactory.cs`) для нужного архетипа Entity добавь `entity.AddX(...)` (данные), собери `ICompositeCondition`, если нужны условия (`canX`/`mustX`), и `entity.AddSystem(new XSystem(...))` (система) — порядок навешивания систем важен, если одна реагирует на событие другой.
5. **Сервис** (если фича не про данные Entity, а про отдельный сервис) — зарегистрируй его в подходящем контексте DI (см. выше).

Детальные конвенции по структуре ECS-фичи — `.claude/rules/ecs-feature.md`.

## Шаблон фичи

Эталоны — при реализации новой похожей фичи опирайся на эти файлы, а не изобретай структуру заново:

- **Цикл действия по таймеру с отменой** (общий ECS-кейс): фича `Attack` (`Gameplay/Features/Attack/`) — цепочка реактивных систем `StartAttackSystem` → `AttackProcessTimerSystem` → `EndAttackSystem` → `AttackCooldownTimerSystem` (+ `AttackCanceledSystem` для отмены), все данные в одном `AttackComponents.cs`. Подключение — `EntitiesFactory.CreateTurret(...)`. Этот же цикл без изменений переиспользуют станция и стреляющий враг.
- **Мгновенный эффект по реактивному триггеру + спавн новых Entity из системы**: `Attack/Shoot/InstantShootSystem.cs` — подписывается на `StartAttackEvent` и создаёт снаряды через внедрённую `EntitiesFactory`. Рядом — `ShootPointEntityRegistrator.cs` (`MonoEntityRegistrator`): эталон того, как затащить префаб-данные (точки выстрела) в компонент Entity в момент линковки, без участия фабрики.
- **Фабрика, комбинирующая ECS-entity с логикой вне ECS**: `EnemiesFactory` (`Gameplay/Features/Enemies/EnemiesFactory.cs`) — диспетчер по типу конфига (`switch (entityConfig) { case SimpleEnemyConfig ... }`), создаёт «сырую» Entity через `EntitiesFactory`, затем навешивает кросс-каттинг логику (AI-мозг через `BrainsFactory`, команда) и сам регистрирует entity в `EntitiesLifeContext`. Используй эту схему, когда фиче нужно объединить Entity с чем-то внешним (AI, звук, VFX).
- **Условие победы/поражения (Stage)**: `ClearAllEnemiesStage` (`Gameplay/Features/StagesFeature/ClearAllEnemiesStage.cs`) — реализация `IStage`: в `Start()` спавнит нужное, отслеживает завершение подпиской на реактивные компоненты созданных Entity (без опроса каждый кадр), сообщает о завершении через `ReactiveEvent Completed`, диспоузит все подписки в `Cleanup`/`Dispose`. Новый тип стадии = новый `StageConfig` + ветка в `StagesFactory.Create` + класс по этой же схеме.
- **Meta-фича (без ECS, персистентное состояние)**: `WalletService`/`LevelsProgressionService` (`Meta/Features/Wallet/`, `Meta/Features/LevelsProgression/`) — обычный сервис (не Entity/Component/System) с методами-запросами и мутаторами, реализует `IDataReader<PlayerData>`/`IDataWriter<PlayerData>` и сам регистрируется в `PlayerDataProvider` в конструкторе. Несмотря на папку `Meta/Features/`, регистрируется `.NonLazy()` в глобальном `ProjectContextRegistrations.cs`, а не в `MainMenuContextRegistrations.cs` — потому что должен пережить переход между сценами.

Структура папки Gameplay-фичи (что в `{FeatureName}Components.cs`, что отдельным файлом на систему, когда нужна `{FeatureName}Factory.cs`/`{Purpose}Registrator.cs`) — `.claude/rules/ecs-feature.md`. Структура Meta-фичи (без Entity, сейв/лоад через `IDataReader`/`IDataWriter`) — `.claude/rules/meta-feature.md`.

Конкретные конвенции для каждого типа файла подгружаются автоматически из соответствующего правила в `.claude/rules/` при работе с этими файлами.

## Index правил `.claude/rules/`

Модульные конвенции с lazy-загрузкой по `paths`. **При добавлении/переименовании файла в `.claude/rules/` обнови этот index.**

| Файл | Когда подгружается | О чём |
|---|---|---|
| `code-style.md` | `**/*.cs` | Структура файла/класса, именование (camelCase/PascalCase/`_camelCase`/`UPPER_SNAKE_CASE`), форматирование, содержимое |
| `ecs-feature.md` | `Gameplay/Features/**/*.cs` | Структура папки Gameplay-фичи, конвенции компонентов/систем/фабрик/регистраторов, `ICompositeCondition` |
| `meta-feature.md` | `Meta/Features/**/*.cs` | Meta-фича как плоский сервис без ECS: `IDataReader`/`IDataWriter<PlayerData>`, где регистрировать (проектный vs меню-контекст) |
| `di-registration.md` | `**/*ContextRegistrations.cs`, `**/*Bootstrap.cs` | Паттерн регистрации сервисов в DI (`RegisterAsSingle`/`CreateX`, `.NonLazy()`), структура `SceneBootstrap` |

## Git / IDE

- `.idea/` (JetBrains Rider) КОММИТИТСЯ — Rider основной IDE. `.DotSettings.user` — в gitignore.
- VS Code открыт параллельно на той же папке как хост для Claude Code (рефакторинг/дебаг/навигация остаются в Rider). Если правишь код через агент в VS Code — Rider подхватит изменения; не держи один файл несохранённым в обеих IDE одновременно.
- `*.csproj` и `*.sln` — в gitignore (Unity их регенерирует).
- Не коммить ничего из `Library/`, `Temp/`, `Logs/`, `UserSettings/` — уже покрыто `.gitignore`.
