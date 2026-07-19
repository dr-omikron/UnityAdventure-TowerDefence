---
paths: ["Assets/_Project/Develop/Runtime/Gameplay/Features/**/*.cs"]
---

# Структура ECS-фичи

Применяется к файлам внутри `Gameplay/Features/<FeatureName>/`.

> Это конвенция именно для Gameplay-фич, завязанных на Entity/Component/System. `Meta/Features/` устроен иначе (плоские сервисы + сейв/лоад, без Entity) — см. `.claude/rules/meta-feature.md`.

## Плоская структура папки

Внутри папки фичи **нет** обязательных подпапок `Components/`/`Systems/` — всё лежит плоско рядом:

- **`{FeatureName}Components.cs`** — один файл, в котором лежат **все** классы-компоненты фичи (несколько небольших классов в одном файле). Пример: `LifeCycleComponents.cs` содержит `CurrentHealth`, `MaxHealth`, `IsDead`, `MustDie`, `MustSelfRelease` и т.д.
- **Один файл на систему**, имя `{Поведение}System.cs` (`DeathSystem.cs`, `StartAttackSystem.cs`, `AttackCooldownTimerSystem.cs`) — один класс на файл.
- **`{FeatureName}Factory.cs`** — если фича комбинирует ECS-entity с чем-то вне ECS (AI-мозг, команда, звук) или диспетчеризует создание по типу конфига (см. `EnemiesFactory`, `StagesFactory`).
- **`{Purpose}Registrator.cs`** — `MonoEntityRegistrator`, если фиче нужно затащить префаб-данные (Transform-ы, коллайдеры) в компоненты в момент линковки Mono-объекта с Entity (см. `ShootPointEntityRegistrator`).
- Простые сервисы/enum/интерфейсы, не являющиеся ни компонентом, ни системой, тоже лежат прямо в папке фичи без доп. вложенности (`IStage.cs`, `StageResult.cs`, `TeamsFeature/Teams.cs`).
- Подпапки (`Attack/Shoot/`, `Attack/AreaAttack/`) появляются **только** для именованных вариантов одной и той же фичи — не создавай их «на всякий случай».

## Компоненты

- `IEntityComponent` — пустой маркерный интерфейс. Компонент — простой класс с публичными полями, обычно одно поле `Value` типа `ReactiveVariable<T>`, `ReactiveEvent`/`ReactiveEvent<T>` или `ICompositeCondition`.
- Не пиши руками методы `entity.AddX(...)`/`entity.X`/`entity.TryGetX(...)` — они генерируются в `Generated/EntityAPI.cs` через `Tools/GenerateEntityAPI` (`Assets/_Project/Develop/Editor/EntityAPIGenerator.cs`). После добавления/переименования компонента запусти этот пункт меню в Unity, иначе новый `AddX` не появится.
- `Generated/EntityAPI.cs` — машинный вывод, не редактируется руками и не попадает под общий `code-style.md`.

## Системы

- Реализуют нужное подмножество `IInitializableSystem` (`OnInit(Entity)`), `IUpdateableSystem` (`OnUpdate(float deltaTime)`), `IDisposableSystem` (`OnDispose()`) — только то, что реально нужно.
- В `OnInit` — получить нужные компоненты через сгенерированные акцессоры, подписаться на реактивные события/переменные, закэшировать ссылки в полях. В `OnUpdate` — работать с закэшированным, не резолвить компоненты заново каждый кадр. В `OnDispose` — диспоузить все подписки, оформленные в `OnInit`.
- **Системы никогда не вызывают друг друга напрямую.** Всё взаимодействие — через реактивные компоненты entity (`ReactiveVariable<T>`/`ReactiveEvent`), на которые системы подписываются. Если нужна новая связка «система A должна отреагировать на систему B» — заведи между ними реактивный компонент-событие, а не прямую ссылку.
- Порядок навешивания систем в фабрике имеет значение, когда одна система реагирует на событие, которое инициирует другая (детект контакта → фильтр по команде → нанесение урона → реакция на смерть → таймер → релиз) — сохраняй этот порядок при добавлении новых систем в цепочку.

## Условия (`ICompositeCondition`)

Условия входа/выхода (`canX`/`mustX`) собираются в `EntitiesFactory` как замыкания над реактивными полями конкретной entity (`new CompositeCondition().Add(new FuncCondition(() => ...))`), а затем добавляются как обычный компонент (`entity.AddCanX(condition)`). Системы читают их через `condition.Evaluate()`, не хардкодят условие внутри себя — это позволяет разным архетипам entity переиспользовать одну и ту же систему с разными условиями.
