using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ToDoListService.DTOs;
using Xunit;

namespace ToDoListServiceSpecs.Steps;

[Binding]
public class TodoItemsStepDefinitions(ScenarioContext scenarioContext, WebApplicationFactory<Program> factory)
{
    [Given(@"standard API client")]
    public void GivenStandardApiConsumer()
    {
        HttpClient client = factory.CreateClient();
        scenarioContext.Set(client);
    }

    [When(@"get request to (.*) is sent")]
    public async Task WhenGetRequestToUrlIsSent(string url)
    {
        var client = scenarioContext.Get<HttpClient>();
        HttpResponseMessage response = await client.GetAsync(url);
        scenarioContext.Set(response);
    }
    
    [When(@"get list of TODOs")]
    public async Task WhenGetListOfTodos(string url)
    {
        var client = scenarioContext.Get<HttpClient>();
        HttpResponseMessage response = await client.GetAsync("api/TodoItems");
        scenarioContext.Set(response);
    }

    [Then(@"the response http status code should be (.*)")]
    public void ThenTheResponseCodeShouldBe(int httpStatusCode)
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        Assert.Equal(httpStatusCode, (int) response.StatusCode);
    }

    [Then(@"the response (.*) header should contain (.*)")]
    public void ThenTheResponseHeaderShouldContain(string headerName, string value)
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        var headerValues = response.Content.Headers.GetValues(headerName);
        Console.WriteLine(headerValues);
        Assert.Contains(value.ToLower(), string.Join("; ", headerValues).ToLower());
    }

    [When("""create new TODO item with "(.*)" name""")]
    public async Task WhenCreateNewTodoItemWithName(string todoName)
    {
        var httpClient = scenarioContext.Get<HttpClient>();
        var todoItemDto = new TodoItemDto
        {
            Id = 0,
            Name = todoName,
            IsComplete = false
        };
        var httpResponseMessage = await httpClient.PostAsJsonAsync("/api/TodoItems", todoItemDto);
        scenarioContext.Set(httpResponseMessage);
    }

    [Then(@"the response body should contain valid TodoItemDto response")]
    public async Task ThenTheResponseBodyShouldContainValidTodoItemDtoResponse()
    {
        var httpResponseMessage = scenarioContext.Get<HttpResponseMessage>();
        var todoItemDto = await httpResponseMessage.Content.ReadFromJsonAsync<TodoItemDto>();
        Assert.NotNull(todoItemDto);
        Assert.True(todoItemDto.Id > 0);
        scenarioContext.Set(todoItemDto);
    }

    [Then(@"the new TODO item should be available by provided id")]
    public async Task ThenTheNewTodoItemShouldBeAvailableByProvidedId()
    {
        var httpClient = scenarioContext.Get<HttpClient>();
        var todoItemDto = scenarioContext.Get<TodoItemDto>();
        var todoItemDtoFetched = await httpClient.GetFromJsonAsync<TodoItemDto>($"api/TodoItems/{todoItemDto.Id}");
        Assert.Equal(todoItemDto, todoItemDtoFetched);
    }

    [Then("""
          the new TODO item name should match "(.*)"
          """)]
    public void ThenTheNewTodoItemNameShouldMatch(string todoName)
    {
        var todoItemDto = scenarioContext.Get<TodoItemDto>();
        Assert.Equal(todoName, todoItemDto.Name);
    }

    [Then(@"the new TODO item should not be completed")]
    public void ThenTheNewTodoItemShouldNotBeCompleted()
    {
        var todoItemDto = scenarioContext.Get<TodoItemDto>();
        Assert.False(todoItemDto.IsComplete);
    }

    [Then(@"the new TODO item should be presented in the all TODOs list")]
    public async Task ThenTheNewTodoItemShouldBePresentedInTheAllTodOsList()
    {
        var todoItemDto = scenarioContext.Get<TodoItemDto>();
        var httpClient = scenarioContext.Get<HttpClient>();
        var allTodoItemDtos = await httpClient.GetFromJsonAsync<IEnumerable<TodoItemDto>>("api/TodoItems");
        Assert.NotNull(allTodoItemDtos);
        Assert.Contains(todoItemDto, allTodoItemDtos);
    }

    [When(@"get TODO item with id (.*)")]
    public async Task WhenGetTodoItemWithId(int id)
    {
        var httpClient = scenarioContext.Get<HttpClient>();
        var httpResponseMessage = await httpClient.GetAsync($"api/TodoItems/{id}");
        scenarioContext.Set(httpResponseMessage);
    }
}