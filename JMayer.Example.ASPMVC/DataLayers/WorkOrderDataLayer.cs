using JMayer.Data.Database.DataLayer.MemoryStorage;
using JMayer.Example.ASPMVC.Models;

namespace JMayer.Example.ASPMVC.DataLayers;

/// <summary>
/// The class manages CRUD interactions with the database for a work order.
/// </summary>
public class WorkOrderDataLayer : StandardCRUDDataLayer<WorkOrder>, IWorkOrderDataLayer
{
    /// <summary>
    /// The default constructor.
    /// </summary>
    public WorkOrderDataLayer()
    {
        IsOldDataObjectDetectionEnabled = true;
        IsUniqueNameRequired = true;
    }
}
