# Pipeline Assessment

## Elsa

Elsa is an Open Source workflow library, which was chosen as a tool to help us build the *self service* functionality.

The project website with Elsa documentation is located [here](https://elsa-workflows.github.io/elsa-core/).

An `Admin` user can create Elsa workflows using Elsa Dashboard. Workflow building blocks (referred to as `Activities`) represent UI components rendered to an `Assessor` user in the Pipeline Assessment UI (or any other project in the future consuming our Elsa SDK).

## Elsa Server

The Elsa Server project makes use of Elsa nuget packages to provide the out of the box Elsa functionality.
We extend the existing Elsa functionality and add our own end points to allow the UI project to start, load and progress through a workflow.

## Elsa Dashboard

The Elsa Dashboard project uses Elsa nuget packages to serve up the self service front end to allow users to create and monitor workflows. Our custom activies will appear in the dashboard and users will be able to select these to start to build assessment stages.

Elsa Dashboard uses [stencil.js](https://stenciljs.com/) components to extend functionlaity for any custom activives.

We use `jest` to unit test our Typescript components. Currently unit tests can be triggered manually by running the following command (set in `package.json`), which is also available via Task Runner Explorer:

`npm run unit-test`

## Elsa Custom Activities

The Elsa Custom Acitives project is used to create new Elsa activies, which will be added to Elsa server start up code to make the new activies available in Elsa Dashboard for user selection.

## Elsa Custom Infrastructure

The Elsa Custom Infrastructure project is used to create an Entity Framework database which we will use additional to the out of the box Elsa workflow database, to allow us to support our specific use case in suspending activies, and navigating backwards and forwards between activies.

### EF Migrations
To apply EF migrations to the Custom Elsa database in Package manager consolse.

1. Elsa.Server needs to be set as Startup Project
1. Then run the following command in Package manager consolse window 
```
Add-Migration Test -Project Elsa.CustomInfrastructure -Context Elsa.CustomInfrastructure.Data.ElsaCustomContext
```

To apply EF migrations to the Pipeline Assessment database in Package manager consolse.

1. He.PipelineAssessment.UI needs to be set as Startup Project
1. Then run the following command in Package manager consolse window 
```
Add-Migration [MigrationName] -Project He.PipelineAssessment.Infrastructure -Context He.PipelineAssessment.Infrastructure.Data.PipelineAssessmentContext
```

## Elsa Custom Workflow SDK

Elsa Custom Workflow SDK project is intended to later become a nuget package of its own to allow any number of custom UI projects to interface with our customised version of Elsa. This currently provides a http client class and a number of dto objects that allow the UI project to interact with Elsa.

## Pipeline Assessment UI

Pipeline Assessment UI project is the custom UI for the Pipeline Assessment, this will interface with Elsa using the Elsa Custom Workflow SDK and allow users to progress screen by screen and compelte the relevant information for the current assessment stage.

This uses [GOV.UK Design System](https://design-system.service.gov.uk/) for its style and layout.

