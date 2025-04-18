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
        ViewBag.ServiceTypes = new List<dynamic>()
        {
            new { Text = "Inspection", Value = WorkOrderServiceType.Inspection },
            new { Text = "Routine", Value = WorkOrderServiceType.Routine },
            new { Text = "Reactive", Value = WorkOrderServiceType.Reactive },
            new { Text = "Other", Value = WorkOrderServiceType.Other },
        };

        return await base.GetAddPartialAsync();
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
                new WorkOrder() { Name = "Work Order 1", Description = "A description for work order 1." },
                new WorkOrder() { Name = "Work Order 2", Description = "A description for work order 2." },
                new WorkOrder() { Name = "Work Order 3", Description = "A description for work order 3." },
                new WorkOrder() { Name = "Work Order 4", Description = "A description for work order 4." },
            ]);
        }

        ////Create the priorities to be displayed in the dropdown.
        //ViewBag.Priorities = new List<dynamic>()
        //{
        //    new { Text = "Low", Value = WorkOrderPriority.Low },
        //    new { Text = "Normal", Value = WorkOrderPriority.Normal },
        //    new { Text = "High", Value = WorkOrderPriority.High },
        //};

        ////Create the service types to be displayed in the dropdown.
        //ViewBag.ServiceTypes = new List<dynamic>()
        //{
        //    new { Text = "Inspection", Value = WorkOrderServiceType.Inspection },
        //    new { Text = "Routine", Value = WorkOrderServiceType.Routine },
        //    new { Text = "Reactive", Value = WorkOrderServiceType.Reactive },
        //    new { Text = "Other", Value = WorkOrderServiceType.Other },
        //};

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
