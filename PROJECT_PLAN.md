# InventoryManagement.Web - Project Plan

## Objectives
- Provide a responsive inventory management experience for warehouse managers and purchasing teams.
- Offer clear low-stock visibility and streamlined supplier coordination.
- Ship a production-ready ASP.NET Core application with automated regression coverage.

## Scope Summary
- Inventory tracking (products, suppliers, stock adjustments, low-stock alerts).
- Dashboard analytics (KPI cards, trend charts, replenishment queues).
- Secure authentication/authorization based on existing Identity setup.
- Test automation coverage for services and critical controllers.

## Assumptions
- Team of 3 developers + 1 QA + 1 product owner is available full time.
- Dependencies: SQL Server instance, Azure DevOps pipelines, Bootstrap UI kit already approved.
- Target production window: end of Week 9 (latest sprint in this plan).

## Timeline & Milestones
| Week | Milestone | Key Tasks | Deliverables |
|------|-----------|-----------|--------------|
| 1 | Kickoff & Foundations | Finalize requirements, confirm architecture, set up repos & CI | Architecture decision record, CI pipeline skeleton |
| 2 | Data Layer Ready | Implement EF Core models, migrations, seed data, context unit tests | Passing data-layer tests, reviewed migration |
| 3 | Core Services | Build product, supplier, dashboard services + coverage | Service interfaces, unit tests green |
| 4 | API Layer | Implement REST controllers, validation, DTO mapping | Authenticated APIs with swagger doc |
| 5 | Web UI Pass 1 | Razor views for dashboard, product CRUD, supplier CRUD | Usable UI walkthrough |
| 6 | Integration & Hardening | Wire DI, logging, error handling, perf profiling | Load test report, telemetry wiring |
| 7 | QA Sprint | Regression suite, bug fixing, accessibility checks | Test summary, resolved backlog |
| 8 | Holiday Buffer & Release Prep | Triage bugs, polish UX, prep deployment scripts, keep skeleton crew on call | Hardened build, updated runbooks |
| 9 | UAT & Deploy | Stakeholder validation, final deployment rehearsal, go-live checklist | Approved UAT signoff, release candidate |

## Workstreams & Owners
### Backend/API (Owner: Dev A)
- Maintain `ApplicationDbContext`, migrations, and seeding logic.
- Ensure `IProductService`, `ISupplierService`, `IDashboardService` cover filtering, pagination, and KPIs.

### Web UI (Owner: Dev B)
- Build Razor views under `Views/Products`, `Views/Suppliers`, `Views/Dashboard` with responsive layouts.
- Integrate client-side validation and chart widgets.

### Quality & Tooling (Owner: Dev C + QA)
- Expand test suite in `InventoryManagement.Web.Tests` (service, API, and integration tests).
- Automate `dotnet test` in CI and enforce coverage gates.

### DevOps (Owner: Shared)
- Maintain deployment slots, secrets, monitoring dashboards.
- Script database migrations for staging/prod releases.

## Dependencies & Checkpoints
- ✅ EF Core 8, ASP.NET Core Identity already configured.
- ☐ Confirm Azure SQL connection string and managed identity.
- ☐ Approve licensing for any third-party UI/chart packages.

## Risk Register
| Risk | Impact | Likelihood | Mitigation |
|------|--------|------------|------------|
| Requirements churn for analytics widgets | Medium | Medium | Lock scope during the Week 1 workshop; use feature toggles. |
| Test data inconsistency between environments | High | Low | Provide shared seed scripts plus nightly refresh job. |
| Performance degradation under large catalogs | Medium | Medium | Enable caching, add indexes, complete load tests by Week 6. |
| Deployment window conflicts with other teams | Low | High | Coordinate via shared release calendar, reserve the Week 9 slot before Week 5. |


## Definition of Done Checklist
- Business acceptance criteria met and peer-reviewed.
- Unit/integration tests updated; `dotnet test` passes in CI.
- Logging/monitoring entries verified in staging.
- Documentation updated (README, API reference, release notes).
