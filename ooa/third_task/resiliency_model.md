# Resiliency Model for RSS Cargo

## Component Interaction Diagram (CID)

### Опис

CID (Component Interaction Diagram) демонструє основні компоненти системи RSS Cargo та їхню взаємодію. Це допомагає зрозуміти архітектуру додатка та його зв’язки із зовнішніми системами.

![Component Interaction Diagram](https://www.plantuml.com/plantuml/png/ZLEzRjim4Dxr50TDpf1cpr34jMFKG54ebeiCGOSBUN68ogHxFAK_HO7-37s7FaNHgLEVGNgZ7kb6y2B0ayJZn-_dUBo8WYpjjYd4IKCm2zjTyEG5bbw8qOWB7kOEDunRcCoMynEu2mohkeOPyYR0Yg9h77dpeIWKhdkl8Z5WXBBxsVtjltH_-g_zj_vxzxlxrVye2nyiGNbDjr0JFn2N2QlB_Ge-5G0t2mugxorCLlXHAIlSK3nv3QSslFEdJ-7c6PetUNzQLqzUNgxLE9C2pk7vUbsfgRSjHJbJSnedKl7vl-nqjskSmPJgK5xB1_AZx8Fy70LlCH9CgX1bmrHVlSZg-eMhbjXHF8feHTP5xIkQ-xtz_AV-qPvTinCwYkNolH1xRFArf23EK4niqshvs8iuz63fxnYZS6kaPIfpvB54WqGjWVKI4cc2GaKSNHJo9biyp35oR9yiPMbtwiKqeRLGFbeVR6S_ev46ubHU45ceE3muIvppBaZCVQwdk4MQnE78w52TGy8T3dnneJ1z5Fy0)

---

## RMA Workbook for RSS Cargo Resiliency

### Interactions

| Interaction ID | From                   | To                     | Description                                                            |
| -------------- | ---------------------- | ---------------------- | ---------------------------------------------------------------------- |
| 1              | User                   | Frontend (Razor Pages) | User interacts with the frontend to initiate requests.                 |
| 2              | Frontend (Razor Pages) | Backend (ASP.NET Core) | Frontend sends HTTP requests (GET/POST) to the backend for processing. |
| 3              | Backend (ASP.NET Core) | Database (PostgreSQL)  | Backend queries the database for requested data.                       |
| 4              | Backend (ASP.NET Core) | Redis (Cache)          | Backend reads/writes data to/from Redis for caching purposes.          |
| 5              | Backend (ASP.NET Core) | External RSS Services  | Backend sends API requests to external RSS services to fetch feeds.    |
| 6              | Database (PostgreSQL)  | Storage                | Database writes data to persistent storage for backup and recovery.    |
| 7              | Redis (Cache)          | Storage                | Redis stores backup of cached data to persistent storage.              |
| 8              | External RSS Services  | RSS Feed Providers     | External services provide RSS feed data to be consumed by the backend. |

### Possible Failures and Responses to Them

| Interaction ID | Possible Failure       | Description                                                                                                               | Response/Triggered Action                                                                                                   |
| -------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------- |
| 1              | User Request Timeout   | The web server takes too long to respond to user interaction due to high traffic or system overload.                      | Notify the user of a delay and retry automatically. Monitor load and scale servers if needed.                               |
| 2              | Database Query Failure | Database fails to return requested data due to a connection issue or resource unavailability.                             | Retry query with exponential backoff. If failure persists, serve a cached response or display "data unavailable."           |
| 3              | Cache Read/Write Error | Redis is unavailable, leading to failed read or write operations, which affects response times.                           | Fallback to direct database queries. Alert infrastructure team and attempt cache restoration.                               |
| 4              | API Request Timeout    | External RSS services fail to respond within the expected time frame, possibly due to service issues or network problems. | Retry the API request. If it fails multiple times, notify users about limited data availability and alert the support team. |
| 5              | Storage Write Failure  | Failed attempt to write data to persistent storage, possibly due to hardware issues or storage limits.                    | Trigger automatic failover to a backup storage. Alert the storage management team and perform data integrity checks.        |

### Rate Phase: Analyze Failures

| Interaction ID | Interaction                     | Impact | Likelihood | Time to Detect (TTD) | Time to Recover (TTR) | Risk (Impact × Likelihood) |
| -------------- | ------------------------------- | ------ | ---------- | -------------------- | --------------------- | -------------------------- |
| 1              | User → Frontend (Razor Pages)   | Medium | High       | < 1 minute           | 5 minutes             | High                       |
| 2              | Backend → Database (PostgreSQL) | High   | Medium     | < 5 minutes          | 15 minutes            | Medium                     |
| 3              | Backend → Redis (Cache)         | Medium | Low        | < 1 minute           | 10 minutes            | Low                        |
| 4              | Backend → External RSS Services | Low    | High       | < 10 minutes         | 20 minutes            | Medium                     |
| 5              | Database → Storage              | High   | Low        | < 5 minutes          | 30 minutes            | Medium                     |

### Act Phase: Mitigation Strategies

1. **Scaling for High Traffic**:

   - Implement auto-scaling for Web Server and Application Server to handle peak traffic.

2. **Fallback Mechanisms for Database and Cache**:

   - Maintain a warm backup of the database for quick failover in case of a failure.

3. **External Service Failure Handling**:

   - Implement retries with exponential backoff for API requests to External RSS Services.

4. **Storage Redundancy**:

   - Use replicated storage to prevent data loss due to hardware failures.

5. **Monitoring and Alerting**:
   - Set up monitoring for key metrics such as response times, error rates, and resource usage.
   - Configure alerts for incidents like server overload, cache failures, and database connection issues to ensure prompt action.
