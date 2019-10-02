﻿using dotnet_graphql.GraphQL;
using dotnet_graphql.Models;
using dotnet_graphql.Services;
using GraphQL.Types;

namespace dotnet_graphql.Queries
{
    public class ProductMutation : ObjectGraphType
    {
        public ProductMutation(ProductService productService)
        {
            Name = "Mutation";

            Field<GraphQL.ProductType>(
                "createProduct",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ProductInputType>> { Name = "product" }
            ),
            resolve: context =>
            {
                ProductViewModel product = context.GetArgument<ProductViewModel>("product");
                return productService.Create(product);
            });

            Field<GraphQL.ProductType>(
                "updateProduct",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ProductInputType>> { Name = "product" }
            ),
            resolve: context =>
            {
                ProductViewModel product = context.GetArgument<ProductViewModel>("product");
                return productService.UpdateProductAsync(product);
            });
        }
    }
}