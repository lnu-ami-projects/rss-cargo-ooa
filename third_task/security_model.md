# Security Model for RSS Cargo

## 1. Threat Model Diagram

### Посилання на діаграму

![Threat Model Diagram](https://www.plantuml.com/plantuml/png/RP91Rziy38Rl-XNyt7EBeeTlm0v5BJjeWXfhAZi86XGO59jpX3XI9iaBic7_VP9THOMt78IO3_cIhyG7Oxcs_Q4Z_pNuJKY4TBDYiw_PS_7CuEhszjDTzl-5qUVLbpnbh--OpN92p5x88zVy0BKw79L4QKjxxX2SwDq7gh6sNBOE_2BWVfJN-rQhNZQPwfI6zNnHpKQYT8DwZD8YpPx8Rq9YULAecdUmCgXXu_ebm0qKgmrShNV2OdbG-z3ZCX4aLAkJgnYIAHjemotVSdDHBAtIl7tNAoSfcL9xWMQ0Vqsqa1QrH0lK--7wFAGjA5mLejtPL-mwTGep2LtxcdIvW4TkySJ7RFVzE95DV_OQeTAzcIYdLP2cNVyFiwmi8UEwLR36hLSE7IQcbihOLcFvKhp9V8oEzdCwbKnxktD_HCsjKF9DbgX_Y1f7uLKrdoWpR8HnCzIxCILBi_dCswCChg_l1xVm6P9vLIrBOFYzHsCJuiE1U_mH3F6S-h0R65vwTBT1SXho0s1etD06Ov6VprFl8VRoqL0RwRHudDppmKmi7-EXFlA9GBGQoSKbPV4E8YOkV7DpxtRGKx-L2FvraW2ofmZ2NXBIOOko0UtNWW2KMFTQs7FO79T4m7Lr1yC6EY7HWCQZHkCVs0vRjesvJcjK90_kw0_T7m00)

### Таблиця моделі загроз для діаграми вище

| # | Threat | Category | Description | Mitigation | Interaction | Priority | SDL Phase |
|---|---------|-----------|-------------|------------|-------------|-----------|------------|
| 1 | Man-in-the-Middle Attack | Spoofing, Tampering | Attacker intercepts communication between Browser and API Gateway | - Use strong TLS/SSL encryption (HTTPS)<br>- Certificate pinning<br>- Implement HSTS | Browser ↔ API Gateway | P0 (Critical) | Design, Implementation |
| 2 | SQL Injection | Tampering, Information Disclosure | Malicious SQL queries injected through API endpoints | - Use parameterized queries<br>- Input validation<br>- ORM with proper escaping<br>- Least privilege database user | API ↔ PostgreSQL Database | P0 (Critical) | Implementation, Verification |
| 3 | Authentication Bypass | Spoofing, Elevation of Privilege | Attacker bypasses authentication service | - Implement JWT tokens<br>- Strong session management<br>- Multi-factor authentication<br>- Rate limiting | API ↔ Auth Service | P0 (Critical) | Design, Implementation |
| 4 | Cache Poisoning | Tampering, Denial of Service | Manipulation of Redis cache data | - Input validation<br>- Proper cache invalidation<br>- Access control for cache<br>- Encryption of cached data | API ↔ Redis Cache | P1 (High) | Design, Implementation |
| 5 | Cross-Site Scripting (XSS) | Tampering, Information Disclosure | Injection of malicious scripts through browser | - Content Security Policy<br>- Input sanitization<br>- Output encoding<br>- HttpOnly cookies | Browser ↔ Local Storage | P0 (Critical) | Implementation, Verification |
| 6 | Local Storage/Cookie Theft | Information Disclosure | Unauthorized access to stored data | - Encrypt sensitive data<br>- Secure cookie flags<br>- Clear data on logout<br>- Minimal sensitive data storage | Browser ↔ Cookies/Local Storage | P1 (High) | Design, Implementation |
| 7 | API Rate Limiting Bypass | Denial of Service | Overwhelming API with requests | - Implement rate limiting<br>- API quotas<br>- Request throttling<br>- CAPTCHA for suspicious activity | Browser ↔ API Gateway | P1 (High) | Design, Implementation |
| 8 | Unauthorized Database Access | Information Disclosure, Elevation of Privilege | Direct database access bypass | - Network segmentation<br>- Firewall rules<br>- Database encryption<br>- Access control lists | API ↔ PostgreSQL Database | P0 (Critical) | Design, Implementation |
| 9 | Session Hijacking | Spoofing | Theft of user session tokens | - Secure session management<br>- Token rotation<br>- IP binding<br>- Secure cookie attributes | Browser ↔ API | P0 (Critical) | Design, Implementation |
| 10 | Data Exposure in Transit | Information Disclosure | Sensitive data exposed during transmission | - End-to-end encryption<br>- Data minimization<br>- Secure protocols<br>- Transport layer security | All Components | P1 (High) | Design, Implementation |
