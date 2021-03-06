# .NET Core AWS Serverless Application Project

## Serverless Template

* Patterned off of Serverless example, [aws-dotnet-rest-api-with-dynamodb](https://github.com/serverless/examples/tree/master/aws-dotnet-rest-api-with-dynamodb), in Github.

## Key Configuration Elements

* **serverless.template** - an AWS CloudFormation Serverless Application Model template file for declaring your Serverless functions and other AWS resources. This demo deploys lambdas for CRU operations, as well as defines a DynamoDB table called ChangeRequest
* **aws-lambda-tools-defaults.json** - default argument settings for use with Visual Studio and command line deployment tools for AWS

## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Execute unit tests
```
    cd "ProjectMgmtLambda/test/ProjectMgmtLambda.Tests"
    dotnet test
```

Deploy application
```
    cd "ProjectMgmtLambda/src/ProjectMgmtLambda"
    dotnet lambda deploy-serverless
```