## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Provider Request Apprentice Training Web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/das-provider-rat-web?branchName=main)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/results?buildId=763384&view=results)
[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=SkillsFundingAgency_das-provider-rat-web)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-provider-rat-web)
[![Confluence Page](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/4249845867/QT2+-+Request+Apprenticeship+Training+aka+AEDv2)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This web solution is part of Request Apprentice Training (RAT) project. Here the provider users can search for apprentice training requests created by employers and optionally respond to them.

## How It Works
Users are expected to have an account on the DfE Sign In which allows them to access the Request Training service 
When running this locally, with stub sign-in enabled, the launch url should be `https://localhost:8801/`

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* Optionally an Azure Active Directory account with the appropriate roles.
* The Outer API [das-apim-endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/ProviderRequestApprenticeTraining) should be available either running locally or accessible in an Azure tenancy.

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-provider-rat-web/SFA.DAS.ProviderRequestApprenticeTraining.Web.json)

In the web project, if not exist already, add `AppSettings.Development.json` file with following content:
```json
{
  "EnvironmentName": "LOCAL",
  "ResourceEnvironmentName": "LOCAL",
  "StubProviderAuth": false,
  "UseStubProviderValidation": false,
} 
```

## Technologies
* .NetCore 8.0
* NUnit
* Moq
* FluentAssertions
* RestEase
* MediatR
