# PaymentsProcessingSystem

## Description

A .NET 10 learning project focused on exploring AWS services and integrating them into modern .NET applications.

The project aims to demonstrate practical AWS integration using C#, the AWS SDK for .NET, dependency injection, and professional software development practices.

The initial implementation focuses on connecting to Amazon DynamoDB using the AWS SDK and retrieving payment records.

### Current Progress

- AWS account and IAM development user configured.
- AWS CLI authentication using temporary credentials.
- DynamoDB `Payments` table created.
- AWS SDK for .NET integrated into the application.
- AWS configuration externalised into `appsettings.json`.
- Dependency injection configured for AWS DynamoDB.
- Successful AWS credentials resolution and DynamoDB client registration.

## Prerequisites

The following software and AWS resources are required:

- Visual Studio 2026.
- .NET 10 SDK.
- AWS CLI v2.32.0 or later.
- An AWS account.
- An IAM development user with appropriate permissions.
- An Amazon DynamoDB table named `Payments` in the `us-east-1` region.

### AWS Configuration

The project uses an AWS CLI profile named `aws-learning`.

The IAM development user requires:
- `SignInLocalDevelopmentAccess` permission for local AWS CLI authentication.
- Appropriate DynamoDB permissions for the `Payments` table.

The AWS CLI should be authenticated before running the application.

## Installing

### 1. Clone the repository

```bash
git clone <repository-url>
```

Open `PaymentsProcessingSystem.slnx` in Visual Studio 2026.

### 2. Configure AWS authentication

Authenticate using the configured AWS CLI profile:

```bash
aws login --profile aws-learning
```

Configure the AWS region:

```bash
aws configure set region us-east-1 --profile aws-learning
```

Verify the authenticated identity:

```bash
aws sts get-caller-identity --profile aws-learning
```

### 3. Configure application settings

Update `appsettings.json` with the appropriate AWS configuration:

```json
{
  "AWS": {
    "Profile": "aws-learning",
    "Region": "us-east-1"
  }
}
```

**Note:** AWS credentials are not stored in `appsettings.json`. Authentication is handled through the AWS CLI profile and temporary credentials.

### 4. Run the application

Build and run the project using Visual Studio 2026.

The application should successfully resolve the configured AWS credentials and register the DynamoDB client through dependency injection.

## Author

Sashen Govender
