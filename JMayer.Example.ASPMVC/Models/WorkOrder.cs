using JMayer.Data.Data;
using System.ComponentModel.DataAnnotations;

namespace JMayer.Example.ASPMVC.Models;

/// <summary>
/// The class represents a work order.
/// </summary>
public class WorkOrder : UserEditableDataObject
{
    /// <summary>
    /// The property gets/sets when the work order is expected to done by.
    /// </summary>
    public DateTime? DoneBy { get; set; }

    /// <summary>
    /// The property gets/sets the user defined type of service when Other is selected for the service.
    /// </summary>
    public string OtherTypeOfService { get; set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the priority of the work order.
    /// </summary>
    [Required]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public WorkOrderPriority Priority { get; set; }

    /// <summary>
    /// The property gets/sets the resolution to the work order.
    /// </summary>
    public string Resolution { get; set; } = string.Empty;

    /// <summary>
    /// The property gets/sets the type of service to be done.
    /// </summary>
    [Required]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public WorkOrderServiceType ServiceType { get; set; }

    /// <summary>
    /// The property get/sets the status of the work order.
    /// </summary>
    [Required]
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public WorkOrderStatus Status { get; set; }

    /// <inheritdoc/>
    public override void MapProperties(DataObject dataObject)
    {
        base.MapProperties(dataObject);

        if (dataObject is WorkOrder workOrder)
        {
            DoneBy = workOrder.DoneBy;
            OtherTypeOfService = workOrder.OtherTypeOfService;
            Priority = workOrder.Priority;
            Resolution = workOrder.Resolution;
            ServiceType = workOrder.ServiceType;
            Status = workOrder.Status;
        }
    }
}
