Feature: TodoItems
Simle CRUD API for managing TODOs

Scenario: Get list of existing TODOs
    Given standard API client
    When get list of TODOs
    Then the response http status code should be 200
    And the response Content-Type header should contain application/json
    
Scenario: Create new TODO item
    Given standard API client
    When create new TODO item with "Test TODO from specs" name
    Then the response http status code should be 201
    And the response body should contain valid TodoItemDto response
    And the new TODO item name should match "Test TODO from specs"
    And the new TODO item should not be completed
    And the new TODO item should be available by provided id
    And the new TODO item should be presented in the all TODOs list
    
Scenario: Get non-existing TODO item by id
    Given standard API client
    When get TODO item with id -1
    Then the response http status code should be 404
    
    