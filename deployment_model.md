# Deployment Model for RSS Cargo on Azure

## Інфраструктурна діаграма

### Опис

Діаграма зображає основні компоненти інфраструктури проекту RSS Cargo, розгорнутого в хмарному середовищі Azure. Вона показує взаємодію між рівнями веб-додатків, аплікаційного сервера, бази даних, кешування, зберігання статичного контенту, і балансування навантаження.

### Посилання на діаграму

![Azure Deployment Diagram](https://www.plantuml.com/plantuml/png/ZLInZjD04Etz5K-feBW2IWfqHop1ILm4COZYv69YJt8hRNR3UWmAwFuTU-qiWt4aTpjltdkZ6J_vjX4AqXvj9aOiOriKM5CuUEHyilvyP2Tuy3LRU8UxNstWBAkiRskixfrmS2mh_Cw0nwyDXn8tDs_n67d8DqMPFMTPu4h87InZDJJktJvG8w6jf2i7-UYPb0CAxQMzxqudQoeIuntUa_1FEfSPq4q3rBoEXeB33rDnSt5qlkuwttY7BuR3WAf9VQENlIhdyTS3_goyEdZUIusV-kcUzfnka9EGDaVT0JjgqZ6Bpn-HNx0Ndu8vKZY_N86QlZNM2vHRZXTDoh6Eokyjjqcsfkg9iVS1MwvD4_dn5HkN5HhnWGv9zCxw7Oe1YueU6El_sUjy4qcnwef9YXwyCvt0k0F8rTXuCVin3F4hfnkFn38XwU8VcAmy_IkDORsUBvq_bPBbYfIVgOVcJJCpLL_b9b-DZjuw2RFIAUdlS-YaEE0DjfmMlJvH9M9-ufGqVnETalbBQ8PwnrwApy9lbFAoWByWJRViwlxF-WS0)

---

## Опис компонентів

### **1. Azure DNS**

- **Призначення**: Забезпечує доменне ім’я для програми та направляє запити до Azure Application Gateway.
- **Роль**: Взаємодія з інтернет-користувачами через глобальний DNS-сервіс.

### **2. Azure Application Gateway**

- **Призначення**: Балансувальник навантаження для розподілу HTTP/HTTPS-трафіку між веб-серверами.
- **Роль**: Забезпечує масштабованість та рівномірне завантаження ресурсів.

### **3. Web Tier**

- **Компоненти**: Azure Web Apps (Razor Pages).
- **Призначення**: Обробка HTTP-запитів користувачів.
- **Роль**: Відображення UI та передача запитів на аплікаційний сервер.

### **4. App Tier**

- **Компоненти**: Azure App Services.
- **Призначення**: Виконання бізнес-логіки програми.
- **Роль**: Взаємодія з базою даних та кешем для швидкої відповіді користувачам.

### **5. Data Tier**

- **Компоненти**:
  - **Azure SQL Database (Primary)**: Основна база даних для зберігання структури додатку.
  - **Azure SQL Database (Replica)**: Репліка бази даних для високої доступності.
  - **Azure Cache for Redis**: Кешування для прискорення доступу до даних.
- **Призначення**: Зберігання, реплікація і швидкий доступ до даних.

### **6. Azure Blob Storage**

- **Призначення**: Зберігання статичного контенту (зображення, файли, резервні копії).
- **Роль**: Інтеграція з Azure CDN для швидкої доставки контенту.

### **7. Azure CDN**

- **Призначення**: Глобальне кешування статичних даних для швидкого доступу.
- **Роль**: Зменшення затримок при завантаженні контенту користувачами.

### **8. Azure Monitor**

- **Призначення**: Моніторинг продуктивності додатку.
- **Роль**: Виявлення проблем у реальному часі.

### **9. Azure Service Health Notifications**

- **Призначення**: Відправлення сповіщень про стан інфраструктури.
- **Роль**: Попередження про збої або оновлення сервісів.
