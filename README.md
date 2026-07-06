# About

This repository was created to follow tutorial at https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&amp;tabs=visual-studio and play with other aspects of an API implementation.

This code is not intended for production usage and it may contain errors. It is also not intended to be used for anyone else other than being a educational reference, if any, for interested developers.

# The idea

After initial creation of the project - to follow the tutorial as mentioned above - a small experiment to proof a concept before applying to an enterprise application in Production, a new controller with a single GET endpoint was added: `SecretExpirationDateController`

This controller enables that a single application registration retrieves start and expiration dates of a secret of the same application. The details of the application are present in appsettings.Development.json via the object below:

```json
...
    "MicrosoftEntraId": {
        "TenantId": "YOUR_TENANT_ID",
        "ClientId": "YOUR_CLIENT_ID",
        "ClientSecretId": "YOUR_CLIENT_SECRET_ID",
        "ClientSecret": "YOUR_CLIENT_SECRET"
    },
...
```

In the real scenario, a second application registration in Microsoft Entra Id would be used to acquire an access token and then the main application registration used by this API will retrieve the client id from the access token and get the secret id passed as a parameter in the route. With these two informations, and the appropriate permissions granted to the main application by an Entra ID administrator, like `Application.Read.All`, another request will be sent to Microsoft Entra ID using Graph SDK to retrive password credentials - like secret ID, start date and end date of the secret whose secret ID was provided in the request.

```bash
# Example of a request
curl -X 'GET' \
  'https://localhost:7006/api/SecretExpirationDate/fea8421a-be16-447d-8955-ee640b5e1e7f' \
  -H 'accept: text/plain'
```

Example of a response
```json
{
  "secretId": "fea8421a-be16-447d-8955-ee640b5e1e7f",
  "startDate": "2026-07-06T12:15:19.852",
  "expirationDate": "2026-10-04T12:15:19.852"
}
```
