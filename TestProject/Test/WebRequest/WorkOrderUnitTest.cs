using JMayer.Data.HTTP.Details;
using JMayer.Example.ASPMVC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Syncfusion.EJ2.Base;
using System.Net.Http.Json;

namespace TestProject.Test.WebRequest;

/// <summary>
/// The class manages tests the work order controller using both a http client and the server.
/// </summary>
/// <remarks>
/// The example web server creates default data objects and the unit tests
/// uses this already existing data.
/// </remarks>
public class WorkOrderUnitTest : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// The factory for the web application.
    /// </summary>
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// The dependency injection constructor.
    /// </summary>
    /// <param name="factory">The factory for the web application.</param>
    public WorkOrderUnitTest(WebApplicationFactory<Program> factory) => _factory = factory;

    /// <summary>
    /// The method creates a work order on the remote web server.
    /// </summary>
    /// <param name="httpClient">Used to communicate with the web server.</param>
    /// <param name="name">The name of the work order.</param>
    /// <returns>The created work order.</returns>
    private static async Task<WorkOrder?> CreateWorkOrderAsync(HttpClient httpClient, string name)
    {
        CRUDModel<WorkOrder> model = new()
        {
            Value = new WorkOrder()
            {
                Name = name,
            },
        };

        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Create", model);

        if (httpResponseMessage.IsSuccessStatusCode is false)
        {
            return null;
        }

        WorkOrder? workOrder = await httpResponseMessage.Content.ReadFromJsonAsync<WorkOrder>();

        return workOrder;
    }

    /// <summary>
    /// The method verifies the work order controller can return the add partial view when requested by the user.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyAddPartialView()
    {
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage httpResponseMessage = await client.GetAsync("WorkOrder/AddPartialView");

        Assert.True(httpResponseMessage.IsSuccessStatusCode);

        string html = await httpResponseMessage.Content.ReadAsStringAsync();

        Assert.NotEmpty(html); //HTML must have been returned.
    }

    /// <summary>
    /// The method verifies the work order controller can create a work order when requested by the user.
    /// </summary>
    /// <param name="name">The friendly name for the work order.</param>
    /// <param name="description">A description for the work order.</param>
    /// <param name="serviceType">The type of service for the work order.</param>
    /// <param name="otherTypeOfService">A custom description for the type when the standard (Inspection, Routine & Reactive) does not apply.</param>
    /// <param name="priority">The priority for the work order.</param>
    /// <param name="dueby">When the work order should be done by.</param>
    /// <param name="status">The status of the work on the order.</param>
    /// <param name="problem">A description of the problem.</param>
    /// <param name="resolution">A description of the resolution.</param>
    /// <returns>A Task for the async.</returns>
    [Theory]
    [InlineData("Create Work Order Test Inspection", "Insepction Description", WorkOrderServiceType.Inspection, null, WorkOrderPriority.Low, null, WorkOrderStatus.Open, null, null)]
    [InlineData("Create Work Order Test Routine", "Routine Description", WorkOrderServiceType.Routine, null, WorkOrderPriority.Normal, null, WorkOrderStatus.InProgress, null, null)]
    [InlineData("Create Work Order Test Reactive", "Reactive Description", WorkOrderServiceType.Reactive, null, WorkOrderPriority.High, null, WorkOrderStatus.Resolved, "Motor Starter Broke", "Replaced Component")]
    [InlineData("Create Work Order Test Other", "Other Description", WorkOrderServiceType.Other, "Other", WorkOrderPriority.High, null, WorkOrderStatus.Resolved, "A Problem", "A Resolution")]
    public async Task VerifyCreateWorkOrder(string name, string description, WorkOrderServiceType serviceType, string? otherTypeOfService, WorkOrderPriority priority, DateTime? dueby, WorkOrderStatus status, string? problem, string? resolution)
    {
        WorkOrder workOrder = new()
        {
            Description = description,
            DueBy = dueby,
            Name = name,
            OtherTypeOfService = otherTypeOfService,
            Priority = priority,
            Problem = problem,
            Resolution = resolution,
            ServiceType = serviceType,
            Status = status,
        };

        CRUDModel<WorkOrder> model = new()
        {
            Value = workOrder,
        };

        HttpClient httpClient = _factory.CreateClient();
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Create", model);

        Assert.True(httpResponseMessage.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.

        WorkOrder? returnedWorkOrder = await httpResponseMessage.Content.ReadFromJsonAsync<WorkOrder>();

        Assert.NotNull(returnedWorkOrder); //The data object must have been returned.
        Assert.True(new WorkOrderEqualityComparer(true, true, true).Equals(workOrder, returnedWorkOrder), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
    }

    /// <summary>
    /// The method verifies the work order controller will return a validation problem when it receives a create requested 
    /// but another work order has the same name.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyCreateWorkOrderDuplicateFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        WorkOrder? workOrder = await CreateWorkOrderAsync(httpClient, "Create Work Order Duplicate Test");

        if (workOrder is null)
        {
            Assert.Fail("Failed to create a work order for the test.");
        }

        WorkOrder duplicateWorkOrder = new()
        {
            Name = "Create Work Order Duplicate Test",
        };

        CRUDModel<WorkOrder> model = new()
        {
            Value = duplicateWorkOrder,
        };

        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Create", model);

        Assert.False(httpResponseMessage.IsSuccessStatusCode, "The operation should have failed."); //The operation must have failed.

        ValidationProblemDetails? validationProblemDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(validationProblemDetails); //The model state dictionary must have been returned.
        Assert.Contains(validationProblemDetails.Errors, obj => obj.Key == nameof(WorkOrder.Name)); //There must be a Name key.
        Assert.Single(validationProblemDetails.Errors[nameof(WorkOrder.Name)]); //Name must have a single error.
        Assert.Equal($"The {workOrder.Name} name already exists in the data store.", validationProblemDetails.Errors[nameof(WorkOrder.Name)][0]); //Confirm the correct error message.
    }

    /// <summary>
    /// The method verifies the work order controller will return a validation problem when it receives a create request 
    /// by the user and the work order has no name.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyCreateWorkOrderNameRequiredValidationFailure()
    {
        CRUDModel<WorkOrder> model = new()
        {
            Value = new WorkOrder(),
        };

        HttpClient httpClient = _factory.CreateClient();
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Create", model);

        Assert.False(httpResponseMessage.IsSuccessStatusCode, "The operation should have failed."); //The operation must have failed.

        ValidationProblemDetails? validationProblemDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(validationProblemDetails); //The model state dictionary must have been returned.
        Assert.Contains(validationProblemDetails.Errors, obj => obj.Key == $"Value.{nameof(WorkOrder.Name)}"); //There must be a Name key.
        Assert.Single(validationProblemDetails.Errors[$"Value.{nameof(WorkOrder.Name)}"]); //Name must have a single error.
        Assert.Equal("The Name field is required.", validationProblemDetails.Errors[$"Value.{nameof(WorkOrder.Name)}"][0]); //Confirm the correct error message.
    }

    /// <summary>
    /// The method verifies the work order controller can delete a work order when requested by the user.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task VerifyDeleteWorkOrder()
    {
        HttpClient httpClient = _factory.CreateClient();
        WorkOrder? workOrder = await CreateWorkOrderAsync(httpClient, "Delete Work Order Test");
        
        if (workOrder is null)
        {
            Assert.Fail("Failed to create a work order for the test.");
        }

        CRUDModel<WorkOrder> model = new()
        {
            Key = workOrder.Integer64ID,
            KeyColumn = nameof(WorkOrder.Integer64ID),
            Value = workOrder,
        };

        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Delete", model);

        Assert.True(httpResponseMessage.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.

        CRUDModel<WorkOrder>? returnedWorkOrder = await httpResponseMessage.Content.ReadFromJsonAsync<CRUDModel<WorkOrder>>();

        Assert.NotNull(returnedWorkOrder); //The data object must have been returned.
        Assert.True(new WorkOrderEqualityComparer(true, true, true).Equals(workOrder, returnedWorkOrder.Value), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
    }

    /// <summary>
    /// The method verifies the work order controller can return the edit partial view when requested.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyEditPartialView()
    {
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage httpResponseMessage = await client.GetAsync("WorkOrder/EditPartialView/1");

        Assert.True(httpResponseMessage.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.

        string html = await httpResponseMessage.Content.ReadAsStringAsync();

        Assert.NotEmpty(html); //HTML must have been returned.
    }

    /// <summary>
    /// The method verifies the work order controller can return the index view.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyIndexView()
    {
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage httpResponseMessage = await client.GetAsync("WorkOrder/Index");

        Assert.True(httpResponseMessage.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.

        string html = await httpResponseMessage.Content.ReadAsStringAsync();

        Assert.NotEmpty(html); //HTML must have been returned.
    }

    /// <summary>
    /// The method verifies the work order controller can update a work order when requested by the user.
    /// </summary>
    /// <param name="name">The friendly name for the work order.</param>
    /// <param name="description">A description for the work order.</param>
    /// <param name="serviceType">The type of service for the work order.</param>
    /// <param name="otherTypeOfService">A custom description for the type when the standard (Inspection, Routine & Reactive) does not apply.</param>
    /// <param name="priority">The priority for the work order.</param>
    /// <param name="status">The status of the work on the order.</param>
    /// <param name="problem">A description of the problem.</param>
    /// <param name="resolution">A description of the resolution.</param>
    /// <returns>A Task for the async.</returns>
    [Theory]
    [InlineData("Update Work Order Test Inspection", "Update Work Order Test Inspection Renamed", "Insepction Description", WorkOrderServiceType.Inspection, null, WorkOrderPriority.Low, WorkOrderStatus.Open, null, null)]
    [InlineData("Update Work Order Test Routine", "Update Work Order Test Routine Renamed", "Routine Description", WorkOrderServiceType.Routine, null, WorkOrderPriority.Normal, WorkOrderStatus.InProgress, null, null)]
    [InlineData("Update Work Order Test Reactive", "Update Work Order Test Reactive Renamed", "Reactive Description", WorkOrderServiceType.Reactive, null, WorkOrderPriority.High, WorkOrderStatus.Resolved, "Motor Starter Broke", "Replaced Component")]
    [InlineData("Update Work Order Test Other", "Update Work Order Test Other Renamed", "Other Description", WorkOrderServiceType.Other, "Other", WorkOrderPriority.High, WorkOrderStatus.Resolved, "A Problem", "A Resolution")]
    public async Task VerifyUpdateWorkOrder(string originalName, string newName, string description, WorkOrderServiceType serviceType, string? otherTypeOfService, WorkOrderPriority priority, WorkOrderStatus status, string? problem, string? resolution) 
    {
        HttpClient client = _factory.CreateClient();
        WorkOrder? workOrder = await CreateWorkOrderAsync(client, originalName);

        if (workOrder is null)
        {
            Assert.Fail("Failed to create a work order for the test.");
        }

        workOrder.Name = newName;
        workOrder.Description = description;
        workOrder.DueBy = DateTime.Today.AddDays(7);
        workOrder.ServiceType = serviceType;
        workOrder.OtherTypeOfService = otherTypeOfService;
        workOrder.Priority = priority;
        workOrder.Status = status;
        workOrder.Problem = problem;
        workOrder.Resolution = resolution;

        CRUDModel<WorkOrder> model = new()
        {
            Key = workOrder.Integer64ID,
            KeyColumn = nameof(WorkOrder.Integer64ID),
            Value = workOrder,
        };

        HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync("WorkOrder/Update", model);

        Assert.True(httpResponseMessage.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.

        WorkOrder? returnedWorkOrder = await httpResponseMessage.Content.ReadFromJsonAsync<WorkOrder>();

        Assert.NotNull(returnedWorkOrder); //The data object must have been returned.
        Assert.True(new WorkOrderEqualityComparer(true, true, true).Equals(workOrder, returnedWorkOrder), "The data object sent should be the same as the data object returned."); //Original and return must be equal.
    }

    /// <summary>
    /// The method verifies the work order controller will return a validation problem when it receives an update request 
    /// by the user and another work order has the same name.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyUpdateWorkOrderDuplicateFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        WorkOrder? workOrder = await CreateWorkOrderAsync(httpClient, "Update Work Order Duplicate Test 1");

        if (workOrder is null)
        {
            Assert.Fail("Failed to create a work order for the test.");
        }

        workOrder = await CreateWorkOrderAsync(httpClient, "Update Work Order Duplicate Test 2");

        if (workOrder is null)
        {
            Assert.Fail("Failed to create a work order for the test.");
        }

        workOrder.Name = "Update Work Order Duplicate Test 1";

        CRUDModel<WorkOrder> model = new()
        {
            Value = workOrder,
        };

        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Create", model);

        Assert.False(httpResponseMessage.IsSuccessStatusCode, "The operation should have failed."); //The operation must have failed.

        ValidationProblemDetails? validationProblemDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(validationProblemDetails); //The model state dictionary must have been returned.
        Assert.Contains(validationProblemDetails.Errors, obj => obj.Key == nameof(WorkOrder.Name)); //There must be a Name key.
        Assert.Single(validationProblemDetails.Errors[nameof(WorkOrder.Name)]); //Name must have a single error.
        Assert.Equal($"The {workOrder.Name} name already exists in the data store.", validationProblemDetails.Errors[nameof(WorkOrder.Name)][0]); //Confirm the correct error message.
    }

    /// <summary>
    /// The method verifies the work order controller will return a validation problem when it receives an update request 
    /// by the user and the work order has no name.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyUpdateWorkOrderNameRequiredValidationFailure()
    {
        HttpClient httpClient = _factory.CreateClient();
        WorkOrder? workOrder = await CreateWorkOrderAsync(httpClient, "Update Name Required Test");

        if (workOrder is null)
        {
            Assert.Fail("Failed to create a work order for the test.");
        }

        workOrder.Name = string.Empty;

        CRUDModel<WorkOrder> model = new()
        {
            Key = workOrder.Integer64ID,
            KeyColumn = nameof(WorkOrder.Integer64ID),
            Value = workOrder,
        };
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Create", model);

        Assert.False(httpResponseMessage.IsSuccessStatusCode, "The operation should have failed."); //The operation must have failed.

        ValidationProblemDetails? validationProblemDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(validationProblemDetails); //The model state dictionary must have been returned.
        Assert.Contains(validationProblemDetails.Errors, obj => obj.Key == $"Value.{nameof(WorkOrder.Name)}"); //There must be a Name key.
        Assert.Single(validationProblemDetails.Errors[$"Value.{nameof(WorkOrder.Name)}"]); //Name must have a single error.
        Assert.Equal("The Name field is required.", validationProblemDetails.Errors[$"Value.{nameof(WorkOrder.Name)}"][0]); //Confirm the correct error message.
    }

    /// <summary>
    /// The method verifies the work order controller will return a not found when it receives an update request 
    /// by the user and the work order doesn't exist.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyUpdateWorkOrderNotFound()
    {
        WorkOrder workOrder = new()
        {
            Integer64ID = 9999,
            Name = "Update Work Order Not Found Test",
        };

        CRUDModel<WorkOrder> model = new()
        {
            Key = workOrder.Integer64ID,
            KeyColumn = nameof(WorkOrder.Integer64ID),
            Value = workOrder,
        };

        HttpClient httpClient = _factory.CreateClient();
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Update", model);

        Assert.False(httpResponseMessage.IsSuccessStatusCode, "The operation should have failed."); //The operation must have failed.

        NotFoundDetails? notFoundDetails = await httpResponseMessage.Content.ReadFromJsonAsync<NotFoundDetails>();

        Assert.NotNull(notFoundDetails); //The conflict detail must have been returned.
        Assert.Equal("Work Order Update Error - Not Found", notFoundDetails.Title); //Confirm the correct title.
        Assert.Equal("The Work Order record was not found; please refresh the page because another user may have deleted it.", notFoundDetails.Detail); //Confirm the correct user message.
    }

    /// <summary>
    /// The method verifies the work order controller will return a conflict when two users try to update the same
    /// work order.
    /// </summary>
    /// <returns>A Task for the async.</returns>
    [Fact]
    public async Task VerifyUpdateWorkOrderOldDataConflict()
    {
        HttpClient httpClient = _factory.CreateClient();
        WorkOrder? workOrder = await CreateWorkOrderAsync(httpClient, "Update Work Order Old Data Conflict Test");

        if (workOrder is null)
        {
            Assert.Fail("Failed to create a work order for the test.");
        }

        workOrder.Status = WorkOrderStatus.InProgress;
        WorkOrder conflictWorkOrder = new(workOrder);

        CRUDModel<WorkOrder> model = new()
        {
            Key = workOrder.Integer64ID,
            KeyColumn = nameof(WorkOrder.Integer64ID),
            Value = workOrder,
        };
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Update", model);

        Assert.True(httpResponseMessage.IsSuccessStatusCode, "The operation should have been successful."); //The operation must have been successful.

        model.Value = conflictWorkOrder;
        httpResponseMessage = await httpClient.PostAsJsonAsync("WorkOrder/Update", model);

        Assert.False(httpResponseMessage.IsSuccessStatusCode, "The operation should have failed."); //The operation must have failed.

        ConflictDetails? conflictDetails = await httpResponseMessage.Content.ReadFromJsonAsync<ConflictDetails>();

        Assert.NotNull(conflictDetails); //The conflict dynamic type must have been returned.
        Assert.Equal("Work Order Update Error - Data Conflict", conflictDetails.Title); //Confirm the correct title.
        Assert.Equal("The submitted Work Order data was detected to be out of date; please refresh the page and try again.", conflictDetails.Detail); //Confirm the correct user message.
    }
}
