# Contributing Guideline

## Directory Structure

- `LyricDb.Web` : The main application
  - `ClientApp` : The frontend (maybe changed in future)
  - `Endpoints` : Just like controllers, but for minimal api
- `LyricDb.Worker` : The worker for background tasks (mail sending, lyric validation), enforced by wolverine
  - `Handlers`: The handlers for main application's jobs
- `LyricDb.Contracts` : The common contracts for main application and worker

You can checkout our [Project dashboard](https://github.com/users/kengwang/projects/2)