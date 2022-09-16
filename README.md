# Pipeline Assessment

## Elsa

Elsa is an Open Source workflow library, which was chosen as a tool to help us build the *self service* functionality.

The project website with Elsa documentation is located [here](https://elsa-workflows.github.io/elsa-core/).

An `Admin` user can create Elsa workflows using Elsa Dashboard. Workflow building blocks (referred to as `Activities`) represent UI components rendered to an `Assessor` user in the Pipeline Assessment UI (or any other project in the future consuming our Elsa SDK).

## Elsa Custom Workflow SDK

## Elsa Custom Activities



## EF Migrations
Package manager consolse

1. Elsa.Server needs to be set as Startup Project
1. Then run the following command in Package manager consolse window 
```
Add-Migration [MigrationName] -Project Elsa.CustomInfrastructure -Context Elsa.CustomInfrastructure.Data.PipelineAssessmentContext"
```