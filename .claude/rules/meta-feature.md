---
paths: ["Assets/_Project/Develop/Runtime/Meta/Features/**/*.cs"]
---

# Структура Meta-фичи

Применяется к файлам внутри `Meta/Features/<FeatureName>/`. В отличие от `Gameplay/Features/` здесь **нет** Entity/Component/System — Meta-фича почти всегда обычный сервис с состоянием, персистентным между сценами.

## Эталон

`WalletService` (`Meta/Features/Wallet/WalletService.cs`) и `LevelsProgressionService` (`Meta/Features/LevelsProgression/LevelsProgressionService.cs`) — канонический вид Meta-фичи:

- Простой класс без `IEntityComponent`/`IEntitySystem` — публичные методы-запросы (`IsLevelCompleted`, `Enough`) и методы-мутаторы (`AddLevelToCompleted`, `Add`, `Spend`), состояние — обычные поля/коллекции (не `ReactiveVariable`, если снаружи не нужна подписка на изменения).
- Реализует `IDataReader<PlayerData>` и `IDataWriter<PlayerData>` (`ReadFrom`/`WriteTo`) — это точка сохранения/загрузки. В конструкторе сам регистрируется в провайдере: `playerDataProvider.RegisterReader(this); playerDataProvider.RegisterWriter(this);`.
- Мутаторы валидируют входные данные через исключения (`ArgumentOutOfRangeException`, `InvalidOperationException`) — Meta-сервисы это точка входа с внешней стороны (UI, геймплей), поэтому проверяют аргументы, в отличие от внутренней ECS-логики.
- Для типов-значений заводи отдельный enum/файл рядом (`CurrencyType.cs`), а не свойство `Value` компонента.

## Регистрация в DI

Несмотря на то что код лежит в `Meta/Features/`, `WalletService` и `LevelsProgressionService` зарегистрированы как `.NonLazy()` в **`ProjectContextRegistrations.cs`** (глобальный контейнер), а не в `MainMenuContextRegistrations.cs` — потому что их состояние должно пережить переход `MainMenu → Gameplay` (кошелёк и прогресс уровней нужны в обеих сценах). `.NonLazy()` — чтобы регистрация в `PlayerDataProvider` произошла сразу при старте, а не только когда кто-то явно резолвит сервис.

Если новая Meta-фича нужна только в рамках одной сцены (например, что-то сугубо про UI главного меню) — регистрируй её в `MainMenuContextRegistrations.cs`, а не в проектном контексте. Регистрируй в проектном контейнере только то, что должно быть доступно из нескольких сцен и/или пережить их смену.
