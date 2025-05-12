# Blind Typing Trainer

Тренажер сліпого друку з веб-інтерфейсом та збереженням результатів у MySQL.

---

## Зміст

- [Опис функціональності](#опис-функціональності)  
- [Запуск локально](#запуск-локально)  
- [Programming Principles](#programming-principles)  
- [Design Patterns](#design-patterns)  
- [Refactoring Techniques](#refactoring-techniques)  

---

## Опис функціональності

1. **Користувачі та авторизація**  
   - Реєстрація та вхід у систему ([AccountController.cs](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Controllers/AccountController.cs)).  
   - Ролі: **Admin** і звичайний користувач.  
   - Доступ до адміністративної панелі лише для Admin.  

2. **Управління уроками (Admin)**  
   - Додавання/редагування/видалення уроків із вказанням **складності** ([AdminController.cs](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Controllers/AdminController.cs), [Views/Admin](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Views/Admin)).  

3. **Перелік уроків та фільтрація**  
   - Список усіх уроків із можливістю фільтрувати за складністю (легкий/середній/важкий/дуже важкий)  
     ([HomeController.cs](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Controllers/HomeController.cs), [Views/Home/Index.cshtml](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Views/Home/Index.cshtml)).  

4. **Тренування**  
   - Крокова подача тексту (етапи) з real-time підсвічуванням правильних/неправильних символів  
     ([wwwroot/js/typing-feedback.js](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/wwwroot/js/typing-feedback.js), [TypingController.cs](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Controllers/TypingController.cs), [Views/Typing/Index.cshtml](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Views/Typing/Index.cshtml)).  
   - Таймер починає відлік з першого натискання клавіші.  
   - Автоматичний перехід між етапами.  
   - Підрахунок загального часу, WPM (слова за хвилину), точності (%) та помилок.  

5. **Профіль користувача**  
   - Перегляд історії завершених сесій з інформацією: урок, дата, час у секундах, точність, помилки  
     ([AccountController.cs → Profile](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Controllers/AccountController.cs), [Views/Account/Profile.cshtml](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Views/Account/Profile.cshtml)).  
   - Редагування даних профілю (логін, email, пароль) ([Views/Account/EditProfile.cshtml](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Views/Account/EditProfile.cshtml)).  

6. **Збереження даних**  
   - Використовує EF Core + Pomelo MySQL  
   - Міграції та сід дані у методі `SeedData.Initialize()` ([SeedData.cs](lab-6/BlindTypingTrainer.Web/BlindTypingTrainer.Web/Data/SeedData.cs)).  

---

## Запуск локально

1. **Клонуйте репозиторій**  
   ```bash
   git clone <your-repo-url>
   cd BlindTypingTrainer.Web
2. **Налаштуйте рядок з’єднання**

   Відредагуйте appsettings.json:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;database=typing_db;user=root;password=yourpassword;"
   }
4. **Виконайте міграції та сідування**
   ```bash
   dotnet ef database update
5. **Запустіть проект**
   ```bash
   dotnet run

---

## Programming Principles

1. Single Responsibility Principle
   - Кожен клас робить лише одну річ (наприклад, `TypingService` відповідає лише за логіку сесії).

2. Open/Closed Principle
   - Додаємо нові стратегії фільтрації уроків через інтерфейс `ILessonFilterStrategy` без змін в `HomeController`.

3. Liskov Substitution Principle
   - Використовуємо інтерфейси `IReadRepository<T>` / `IWriteRepository<T>` замість конкретних класів.

4. Interface Segregation Principle
   - Чітко розділено читання (`IReadRepository`) і запис (`IWriteRepository`).

5. Dependency Inversion Principle
   - Всі залежності (репозиторії, сервіси) інжектяться через DI-контейнер.

---

## Design Patterns

| Патерн          | Де застосовано                                | Навіщо                                                                |
|-----------------|-----------------------------------------------|-----------------------------------------------------------------------|
| Repository      | `LessonRepository`, `TypingSessionRepository` | Ізоляція доступу до БД та уніфікований інтерфейс CRUD                 |
| Dependency Injection | `Program.cs` та всі контролери через конструктор | Зменшення зв'язності, просте тестування                       |
| Strategy        | `ILessonFilterStrategy` + `EasyStrategy` тощо (Filters) | Динамічна фільтрація уроків за складністю                   |
| Observer        | `typing-feedback.js` слухає події DOM (`keydown`, `input`) | Live-feedback при введенні символів                      |
| Decorator (UI)  | Razor `_Layout.cshtml` + секції `@RenderSection` | Розширюваність макету та підключення скриптів/стилів               |

---

## Refactoring Techniques

1. Extract Class
   - Винесено логіку фільтрації в окремі класи-стратегії (`EasyStrategy`, `MediumStrategy`, …).

2. Extract Method
   - Методи `startTimer()`, `stopTimer()`, `showSummary()` в `typing-feedback.js`.

3. Rename
   - Інформативні імена змінних/методів: `TypingService`, `LessonRepository`, `Stage`, `TypingViewModel`.

4. Introduce Parameter Object
   - Використання `ViewModels` (`RegisterVM`, `LoginVM`, `EditProfileVM`) замість численних параметрів в методах.

5. Remove Dead Code
   - Видалено застарілі залежності та непотрібні поля після інтеграції Identity і розділення репозиторіїв.
