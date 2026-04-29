using JMayer.Data.Data;
using JMayer.Example.ASPMVC.DataLayers;
using JMayer.Example.ASPMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

namespace JMayer.Example.ASPMVC.Controllers;

/// <summary>
/// The class manages HTTP requests for views and actions associated with work orders.
/// </summary>
public class WorkOrderController : SyncFusionModelViewController<WorkOrder, IWorkOrderDataLayer>
{
    /// <summary>
    /// The priorties.
    /// </summary>
    private static readonly List<ListView> _priorities =
        [
            new() { Name = "Low", Integer64ID = (long)WorkOrderPriority.Low },
            new() { Name = "Normal", Integer64ID = (long)WorkOrderPriority.Normal },
            new() { Name = "High", Integer64ID = (long)WorkOrderPriority.High },
        ];

    /// <summary>
    /// The service types.
    /// </summary>
    private static readonly List<ListView> _serviceTypes =
        [
            new() { Name = "Inspection", Integer64ID = (long)WorkOrderServiceType.Inspection },
            new() { Name = "Routine", Integer64ID = (long)WorkOrderServiceType.Routine },
            new() { Name = "Reactive", Integer64ID = (long)WorkOrderServiceType.Reactive },
            new() { Name = "Other", Integer64ID = (long)WorkOrderServiceType.Other },
        ];

    /// <inheritdoc/>
    public WorkOrderController(IWorkOrderDataLayer dataLayer, ILogger<WorkOrderController> logger) : base(dataLayer, logger) { }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden so the service types are added to the ViewBag.
    /// </remarks>
    public override async Task<IActionResult> AddPartialViewAsync()
    {
        //Create the service types to be displayed in the dropdown.
        ViewBag.ServiceTypes = new List<ListView>(_serviceTypes);

        //Create the priorities to be displayed in the dropdown.
        ViewBag.Priorities = new List<ListView>(_priorities);

        return await base.AddPartialViewAsync();
    }

    /// <summary>
    /// The method clears the other type of service if the service type is not other.
    /// </summary>
    /// <param name="model">The model which contains the work order.</param>
    private static void ClearOtherTypeOfService(CRUDModel<WorkOrder> model)
    {
        if (model.Value.ServiceType is not WorkOrderServiceType.Other)
        {
            model.Value.OtherTypeOfService = string.Empty;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to clear the OtherTypeOfService property if the ServiceType is not Other.
    /// </remarks>
    public override async Task<IActionResult> CreateAsync([FromBody] CRUDModel<WorkOrder> model)
    {
        ClearOtherTypeOfService(model);
        return await base.CreateAsync(model);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden so the service types and priorities are added to the ViewBag.
    /// </remarks>
    public override async Task<IActionResult> EditPartialViewAsync(long id)
    {
        //Create the service types to be displayed in the dropdown.
        ViewBag.ServiceTypes = new List<ListView>(_serviceTypes);

        //Create the priorities to be displayed in the dropdown.
        ViewBag.Priorities = new List<ListView>(_priorities);

        return await base.EditPartialViewAsync(id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overriden to clear the OtherTypeOfService property if the ServiceType is not Other.
    /// </remarks>
    public override async Task<IActionResult> UpdateAsync([FromBody] CRUDModel<WorkOrder> model)
    {
        ClearOtherTypeOfService(model);
        return await base.UpdateAsync(model);
    }
}
