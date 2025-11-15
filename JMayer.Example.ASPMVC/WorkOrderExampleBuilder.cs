using JMayer.Example.ASPMVC.DataLayers;
using JMayer.Example.ASPMVC.Models;

namespace JMayer.Example.ASPMVC;

/// <summary>
/// The class is used to generate example data for BHS work orders.
/// </summary>
public class WorkOrderExampleBuilder
{
    /// <summary>
    /// The property gets/sets the data layer used to interact with work orders.
    /// </summary>
    public IWorkOrderDataLayer WorkOrderDataLayer { get; set; } = new WorkOrderDataLayer();

    /// <summary>
    /// The method builds the work order example data.
    /// </summary>
    public void Build()
    {
        BuildOpenWorkOrders();
        BuildInProgressWorkOrders();
        BuildResolvedWorkOrders();
        BuildClosedWorkOrders();
    }

    /// <summary>
    /// The method builds the closed work orders.
    /// </summary>
    private void BuildClosedWorkOrders()
    {
        WorkOrderDataLayer.CreateAsync([
            new WorkOrder()
            {
                Description = "Various electrical tests.",
                DueBy = DateTime.Today.AddDays(-7),
                Name = $"Electrical Inspection {DateTime.Today.AddDays(-14)}",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.Normal,
                Problem = "No Issues Found",
                Resolution = "No Issues Found",
                ServiceType = WorkOrderServiceType.Inspection,
                Status = WorkOrderStatus.Closed,
            },
            new WorkOrder()
            {
                Description = "Each photoeye needs to be wiped of dust to ensure optimal function.",
                DueBy = DateTime.Today.AddDays(-7),
                Name = $"Photoeye Clean {DateTime.Today.AddDays(-14)}",
                OtherTypeOfService = null,
                Problem = "None",
                Resolution = "Clean Completed",
                Priority = WorkOrderPriority.Normal,
                ServiceType = WorkOrderServiceType.Routine,
                Status = WorkOrderStatus.Closed,
            },
            new WorkOrder()
            {
                Description = "An overload and failed to start alarm has been reported by the system for ML1-30.",
                DueBy = DateTime.Today.AddDays(-7),
                Name = "ML1-30 Motor Issue",
                OtherTypeOfService = null,
                Problem = "The conveyor's motor has burned out.",
                Resolution = "The motor has been replaced.",
                Priority = WorkOrderPriority.High,
                ServiceType = WorkOrderServiceType.Reactive,
                Status = WorkOrderStatus.Closed,
            },
        ]).Wait();
    }

    /// <summary>
    /// The method builds the in progress work orders.
    /// </summary>
    private void BuildInProgressWorkOrders()
    {
        WorkOrderDataLayer.CreateAsync([
            new WorkOrder()
            {
                Description = "Various electrical tests.",
                DueBy = DateTime.Today.AddDays(7),
                Name = $"Electrical Inspection {DateTime.Today.AddDays(-7)}",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.Normal,
                Problem = null,
                Resolution = null,
                ServiceType = WorkOrderServiceType.Inspection,
                Status = WorkOrderStatus.InProgress,
            },
            new WorkOrder()
            {
                Description = "Each photoeye needs to be wiped of dust to ensure optimal function.",
                DueBy = DateTime.Today.AddDays(7),
                Name = $"Photoeye Clean {DateTime.Today.AddDays(-7)}",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.Normal,
                Problem = null,
                Resolution = null,
                ServiceType = WorkOrderServiceType.Routine,
                Status = WorkOrderStatus.InProgress,
            },
            new WorkOrder()
            {
                Description = "Water has been reported near B-7 PWC.",
                DueBy = DateTime.Today,
                Name = "B-7 PWC Leaking Water Hose",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.High,
                Problem = null,
                Resolution = null,
                ServiceType = WorkOrderServiceType.Reactive,
                Status = WorkOrderStatus.InProgress,
            },
        ]).Wait();
    }

    /// <summary>
    /// The method builds the open work orders.
    /// </summary>
    private void BuildOpenWorkOrders()
    {
        WorkOrderDataLayer.CreateAsync([
            new WorkOrder() 
            {
                Description = "Various electrical tests.",
                DueBy = DateTime.Today.AddDays(14),
                Name = $"Electrical Inspection {DateTime.Today.AddDays(7)}",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.Normal,
                Problem = null,
                Resolution = null,
                ServiceType = WorkOrderServiceType.Inspection,
                Status = WorkOrderStatus.Open,
            },
            new WorkOrder()
            {
                Description = "Each photoeye needs to be wiped of dust to ensure optimal function.",
                DueBy = DateTime.Today.AddDays(14),
                Name = $"Photoeye Clean {DateTime.Today.AddDays(7)}",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.Normal,
                Problem = null,
                Resolution = null,
                ServiceType = WorkOrderServiceType.Routine,
                Status = WorkOrderStatus.Open,
            },
            new WorkOrder()
            {
                Description = "The system is continously triggering a SL1-13 encoder underspeed alarm.",
                DueBy = DateTime.Today,
                Name = "SL1-13 Encoder Issue",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.High,
                Problem = null,
                Resolution = null,
                ServiceType = WorkOrderServiceType.Reactive,
                Status = WorkOrderStatus.Open,
            },
            new WorkOrder()
            {
                Description = "Contractor will be inspecting the system for an upcoming job and they require a TSA security escort.",
                DueBy = DateTime.Today.AddDays(14),
                Name = $"TSA Security Escort {DateTime.Today.AddDays(14)}",
                OtherTypeOfService = "TSA Security Escort",
                Problem = null,
                Resolution = null,
                ServiceType = WorkOrderServiceType.Other,
                Status = WorkOrderStatus.Open,
            },
        ]).Wait();
    }

    /// <summary>
    /// The method builds the resolved work orders.
    /// </summary>
    private void BuildResolvedWorkOrders()
    {
        WorkOrderDataLayer.CreateAsync([
            new WorkOrder()
            {
                Description = "A VFD alarm has been reported by the system for SSL1-04.",
                DueBy = DateTime.Today,
                Name = "SS1-04 VFD Failure",
                OtherTypeOfService = null,
                Priority = WorkOrderPriority.High,
                Problem = "The I/O module failed.",
                Resolution = "The I/O Module was replaced.",
                ServiceType = WorkOrderServiceType.Reactive,
                Status = WorkOrderStatus.Resolved,
            },
        ]).Wait();
    }
}
