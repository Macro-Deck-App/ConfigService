# Macro Deck Config Service (Internal tool)

This service provides the configurations for our infrastructure.

### How it works
The any service in our infrastructure requests the config from this service via a REST api.
Configs are stored as base64 string in a database.
Each config has it's own authentication token which is required to get the config.

### Endpoints

| Method | Endpoint                  | Description                                                                                                                                           | Authentication        |
|--------|---------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------|
| GET    | `/config/encoded`         | Gets a base64 encoded config. The config name is set in the `x-config-name` header and the authentication token in the `x-config-access-token` header | x-config-access-token |
| GET    | `/config/version`         | Gets the version of a config. The config name is set in the `x-config-name` header and the authentication token in the `x-config-access-token` header | x-config-access-token |
| POST   | `/config/{name}`          | Creates/Updates a config. The request body is used for the config data                                                                                | x-admin-access-token  |
| GET    | `/config/{name}`          | Gets a decoded json config                                                                                                                            | x-admin-access-token  |
| POST   | `/config/{name}/settoken` | Sets the access token for a config                                                                                                                    | x-admin-access-token  |
