# dotnet graphql
+ One to Many Relationships in Entity Framework Core
+ Many to Many Relationships in Entity Framework Core
+ Seed Data
+ GraphQL


### Prerequisites
+ Asp Net Core 3.0
+ Entity Framework
+ Sql Server or PostgresQL
+ Docker & Docker Compose


### Issues
+ Error with Complex Input Types "Unable to parse input as a type. Did you provide a List or Scalar value accidentally?
    - Fixed: https://github.com/graphql-dotnet/graphql-dotnet/issues/816

+ The collection type 'Newtonsoft.Json.Linq.JToken' is not supported(Dotnet 3.0)
    - Fixed: https://stackoverflow.com/questions/58072703/jsonresultobject-causes-the-collection-type-newtonsoft-json-linq-jtoken-is
    - Add a package reference to Microsoft.AspNetCore.Mvc.NewtonsoftJson
    - Update Startup.ConfigureServices to call AddNewtonsoftJson
        ```
        services.AddMvc().AddNewtonsoftJson();
        ```

+ Error: Microsoft.AspNetCore.Routing.EndpointMiddleware: Information: Executing endpoint '405 HTTP Method Not Supported' in AspNet 3.0
    - Has the Issue with the Endpoint
    - Fixed: Add GraphQL.Server.Transports.AspNetCore and GraphQL.Server.Ui.Playground packages
    - And then: Run on UI playground http://localhost:5000/ui/playground
    - Using http://localhost:5000/graphql it still doesn't work.

### Usages

+ GraphQL(http://localhost:5000/ui/playground)
    + Check GraphQL schema
        ```
        {
            __schema {
                types {
                    name
                }
            }
        }
        ```

    + Query Book by Id
        GraphQL

        ```
        query APIQuery($id: Int) {
            book(id: $id) {
                id
                bookName
                price
                author {
                    id
                    firstName
                }
                bookCategories {
                    bookId
                    categoryId
                    category {
                        categoryId
                        categoryName
                    }
                }
            }
        }


        Query variable
        {
            "id": 1
        }
        ```

        Output
        ```
        {
            "data": {
                "book": {
                    "id": 1,
                    "bookName": "Quantum Networking",
                    "price": 220,
                    "author": {
                        "id": 1,
                        "firstName": "Nick"
                    },
                    "bookCategories": [
                        {
                            "bookId": 1,
                            "categoryId": 1,
                                "category": {
                                    "categoryId": 1,
                                    "categoryName": "Network"
                                }
                        }
                    ]
                }
            }
        }
        ```

    + Query All Books
        GraphQL
        ```
        query APIQuery {
            books {
                id
                bookName
                price
                author {
                    id
                    firstName
                }
                bookCategories {
                    bookId
                    categoryId
                    category {
                        categoryId
                        categoryName
                    }
                }
            }
        }
        ```

        Output
        ```
        {
            "data": {
                "books": [
                {
                    "id": 1,
                    "bookName": "Quantum Networking",
                    "price": 220,
                    "author": {
                        "id": 1,
                        "firstName": "Nick"
                    },
                    "bookCategories": [
                        {
                            "bookId": 1,
                            "categoryId": 1,
                            "category": {
                                "categoryId": 1,
                                "categoryName": "Network"
                            }
                        }
                    ]
                },
                {
                    "id": 2,
                    "bookName": "Advance C#",
                    "price": 110,
                    "author": {
                        "id": 2,
                        "firstName": "David"
                    },
                    "bookCategories": [
                        {
                            "bookId": 2,
                            "categoryId": 2,
                            "category": {
                                "categoryId": 2,
                                "categoryName": "Programming"
                            }
                        }
                    ]
                }
                ]
            }
        }
        ```

    + Create Product
        Graphql
        ```
        mutation ($product: ProductInput!) {
            createProduct(product: $product) {
                name
                description
                availableStock
                price
            }
        }
        ```

        Query Vairable
        ```
        {
            "product": {
                "name": "Product 1",
                "description": "description product 1",
                "availableStock": 100,
                "price": 11.8,
                "productTypeId": 1,
                "productBrandId": 1
            }
        }
        ```

        Output
        ```
        {
            "data": {
                "createProduct": {
                    "name": "Product 1",
                    "description": "description product 1",
                    "availableStock": 100,
                    "price": 11.8
                }
            }
        }
        ```

    + Create Product with Sizes
        GraphQL
        ```
        mutation ($product: ProductInput!) {
            createProduct(product: $product) {
                name
                description
                availableStock
                price
                sizes {
                    name
                    code
                }
            }
        }
        ```

        Query Vairable
        ```
        {
            "product": {

                "name": "Product TShirt sport",
                "description": "description TShirt sport",
                "availableStock": 100,
                "price": 18.8,
                "productTypeId": 1,
                "productBrandId": 1,
                "sizes": [ "X", "XL"]
            }
        }
        ```

        Output
        ```
        {
            "data": {
                "createProduct": {
                    "name": "Product TShirt sport",
                    "description": "description TShirt sport",
                    "availableStock": 100,
                    "price": 18.8,
                    "sizes": [
                        {
                            "name": "X",
                            "code": "X"
                        },
                        {
                            "name": "XL",
                            "code": "XL"
                        }
                    ]
                }
            }
        }
        ```

    + Create User
        GraphQL
        ```
        mutation ($user: UserInput!) {
            createUser(user: $user) {
                firstName
                lastName
                username
                password
            }
        }
        ```

        Query Vairable
        ```
        {
            "user":
            {
                "firstName": "First",
                "lastName": "Last",
                "username": "firstlasttest",
            }
        }
        ```

        Output
        ```
        {
            "data": {
                "createUser": {
                    "firstName": "First",
                    "lastName": "Last",
                    "username": "firstlasttest",
                    "password": "Abcde@123"
                }
            }
        }
        ```

+ Using Postman
    + Get Token:
        ```
        POST http://localhost:5000/users/authenticate
        Body:
        {
            "username": "test",
            "password": "Abcde@123"
        }
        ```

    + Authentication

        ```
        POST http://localhost:5000/graphql/
        Authoriation: Bearer [Token]

        Body:
            {
                "operationName": "APIQuery",
                "variables": { "id": 1 },
                "query": " query APIQuery { products { name } }"
            }

        Output:
            {
                "data": {
                    "products": [
                        {
                            "name": "White Sneaker 11"
                        },
                        {
                            "name": "Kudu Purple Hoodie"
                        },
                        {
                            "name": "Roslyn Red T-Shirt"
                        },
                        {
                            "name": " Blue Hoodie"
                        },
                        {
                            "name": "Roslyn Red trousers pants"
                        },
                        {
                            "name": "Foundation T-shirt"
                        }
                    ]
                }
            }
        ```

### References
+ [Configuring Many To Many Relationships in Entity Framework Core](https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration)
+ [Configuring One To Many Relationships in Entity Framework Core](https://www.learnentityframeworkcore.com/configuration/one-to-many-relationship-configuration)
+ [Update many to many relationships in Entity Framework Core](https://www.thereformedprogrammer.net/updating-many-to-many-relationships-in-entity-framework-core/)
+ [Building a GraphQL API with ASP.NET Core 2 and Entity Framework Core](https://fullstackmark.com/post/17/building-a-graphql-api-with-aspnet-core-2-and-entity-framework-core)
+ [Build a GraphQL API with ASP.NET Core](https://developer.okta.com/blog/2019/04/16/graphql-api-with-aspnetcore)
+ [Building GraphQL APIs with ASP.NET Core](https://medium.com/volosoft/building-graphql-apis-with-asp-net-core-419b32a5305b)
+ [GraphQL API Authorization](https://learnmoreseekmore.blogspot.com/2020/01/dotnet-core-graphql-api-authorization.html)