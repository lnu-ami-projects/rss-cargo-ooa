# Third Task

## Опис:

Кожне завдання задане в окремому файлі. Нижче описано кожен із них та наведені відповідні посилання.

---

## Структура папки

### Основні файли

1. **[analytics_model.md](./analytics_model.md)**:

   - Містить таблицю з **15 функціональних метрик**.
   - Описує **2 funnel-аналітики**, що охоплюють основні процеси системи.

2. **[data_model.md](./data_model.md)**:

   - Включає **ER-діаграми** структури бази даних.
   - Деталізує опис **5 сутностей** із визначенням атрибутів і зв’язків.
   - Включає політику зберігання даних ("data retention policy") з описом **5 типів даних**.

3. **[deployment_model.md](./deployment_model.md)**:

   - Інфраструктурна діаграма проекту з Azure.
   - Документує **6 основних типів ресурсів** (сервери, бази даних, кеш тощо).

4. **[monitoring_alerting_model.md](./monitoring_alerting_model.md)**:

   - Містить таблицю з **15 операційних метрик** для моніторингу.
   - Вказує мінімальні та максимальні значення для **10 критичних метрик**.
   - Описує план дій (Mitigation Plan) для найважливіших збоїв.

5. **[resiliency_model.md](./resiliency_model.md)**:

   - Містить **CID-діаграму** (Component Interaction Diagram).
   - Описує **RMA-діаграму** (Recovery Management Architecture) для забезпечення стійкості.

6. **[security_model.md](./security_model.md)**:
   - Включає **threat model** для основних потоків системи.
   - Містить **10 загроз** із детальним Mitigation Plan для кожної.

---

### Діаграми

Папка **[diagrams_code](./diagrams_code/)** містить вихідний код для генерації діаграм у форматі PlantUML:

- **azure_deployment.puml**: Діаграма деплойменту на Azure.
- **cid.puml**: Component Interaction Diagram (CID).
- **rma.puml**: Recovery Management Architecture (RMA).

---

## Використання

1. Для перегляду моделей відкривайте відповідні файли Markdown у цій папці.
2. Для перевірки коду діаграм, що знаходяться у папці `diagrams_code`, використайте [PlantUML](https://plantuml.com/).

---
