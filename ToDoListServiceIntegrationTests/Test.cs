using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace ToDoListServiceIntegrationTests;

public class Test(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Get_SuccessResponseForAllTodoItems()
    {
        var client = factory.CreateClient();
        
        var response = await client.GetAsync("/api/TodoItems");
        
        response.EnsureSuccessStatusCode();
    }
}