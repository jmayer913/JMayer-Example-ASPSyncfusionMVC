using JMayer.Data.Data;
using JMayer.Example.ASPMVC.DataLayers;
using JMayer.Example.ASPMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace JMayer.Example.ASPMVC.Controllers;

/// <summary>
/// The class manages HTTP requests for views and actions associated with work orders.
/// </summary>
public class WorkOrderController : SyncFusionModelViewController<WorkOrder, WorkOrderDataLayer>
{
    /// <inheritdoc/>
    public WorkOrderController(IWorkOrderDataLayer dataLayer, ILogger<WorkOrderController> logger) : base(dataLayer, logger)
    {
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden so the service types are added to the ViewBag.
    /// </remarks>
    public override async Task<IActionResult> GetAddPartialAsync()
    {
        //Create the service types to be displayed in the dropdown.
        ViewBag.ServiceTypes = GetServiceTypes();

        //Create the priorities to be displayed in the dropdown.
        ViewBag.Priorities = GetPriorities();

        return await base.GetAddPartialAsync();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden so the service types and priorities are added to the ViewBag.
    /// </remarks>
    public override Task<IActionResult> GetEditPartialAsync(long id)
    {
        //Create the service types to be displayed in the dropdown.
        ViewBag.ServiceTypes = GetServiceTypes();

        //Create the priorities to be displayed in the dropdown.
        ViewBag.Priorities = GetPriorities();

        return base.GetEditPartialAsync(id);
    }

    /// <summary>
    /// The method returns the priorties.
    /// </summary>
    /// <returns>A list of priorities.</returns>
    private static List<ListView> GetPriorities()
    {
        return
        [
            new() { Name = "Low", Integer64ID = (long)WorkOrderPriority.Low },
            new() { Name = "Normal", Integer64ID = (long)WorkOrderPriority.Normal },
            new() { Name = "High", Integer64ID = (long)WorkOrderPriority.High },
        ];
    }
    
    /// <summary>
    /// The method returns the service types.
    /// </summary>
    /// <returns>A list of service types.</returns>
    private static List<ListView> GetServiceTypes()
    {
        return
        [
            new() { Name = "Inspection", Integer64ID = (long)WorkOrderServiceType.Inspection },
            new() { Name = "Routine", Integer64ID = (long)WorkOrderServiceType.Routine },
            new() { Name = "Reactive", Integer64ID = (long)WorkOrderServiceType.Reactive },
            new() { Name = "Other", Integer64ID = (long)WorkOrderServiceType.Other },
        ];
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to include dropdown lists in the ViewBag.
    /// </remarks>
    public override async Task<IActionResult> IndexAsync()
    {
        //For testing purposes only. Remove once example data is auto generated.
        if (await DataLayer.CountAsync() == 0)
        {
            await DataLayer.CreateAsync([
                new WorkOrder() { Name = "Work Order 1", Description = "A description for work order 1.", DueBy = DateTime.Today.AddDays(-2) },
                new WorkOrder() { Name = "Work Order 2", Description = "A description for work order 2.", DueBy = DateTime.Today },
                new WorkOrder() { Name = "Work Order 3", Description = "A description for work order 3.", DueBy = DateTime.Today.AddDays(3) },
                new WorkOrder() { Name = "Work Order 4", Description = "A description for work order 4.", DueBy = DateTime.Today.AddDays(8) },
                new WorkOrder() { Name = "Work Order 5", Description = "A description for work order 5." },
            ]);
        }

        ////Create the statuses to be displayed in the dropdown.
        //ViewBag.Statuses = new List<dynamic>()
        //{
        //    new { Text = "Open", Value = WorkOrderStatus.Open },
        //    new { Text = "In Progress", Value = WorkOrderStatus.InProgress },
        //    new { Text = "Verify", Value = WorkOrderStatus.Verify },
        //    new { Text = "Closed", Value = WorkOrderStatus.Closed },
        //};

        return await base.IndexAsync();
    }
}
