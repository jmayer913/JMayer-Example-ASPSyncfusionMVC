using JMayer.Data.Data;
using JMayer.Example.ASPMVC.DataLayers;
using JMayer.Example.ASPMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace JMayer.Example.ASPMVC.Controllers;

/// <summary>
/// The class manages HTTP requests for views and actions associated with work orders.
/// </summary>
public class WorkOrderController : SyncFusionModelViewController<WorkOrder, IWorkOrderDataLayer>
{
    /// <inheritdoc/>
    public WorkOrderController(IWorkOrderDataLayer dataLayer, ILogger<WorkOrderController> logger) : base(dataLayer, logger) { }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden so the service types are added to the ViewBag.
    /// </remarks>
    public override async Task<IActionResult> AddPartialViewAsync()
    {
        //Create the service types to be displayed in the dropdown.
        ViewBag.ServiceTypes = GetServiceTypes();

        //Create the priorities to be displayed in the dropdown.
        ViewBag.Priorities = GetPriorities();

        return await base.AddPartialViewAsync();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden so the service types and priorities are added to the ViewBag.
    /// </remarks>
    public override async Task<IActionResult> EditPartialViewAsync(long id)
    {
        //Create the service types to be displayed in the dropdown.
        ViewBag.ServiceTypes = GetServiceTypes();

        //Create the priorities to be displayed in the dropdown.
        ViewBag.Priorities = GetPriorities();

        return await base.EditPartialViewAsync(id);
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
}
