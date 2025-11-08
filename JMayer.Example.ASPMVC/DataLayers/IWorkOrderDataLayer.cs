using JMayer.Data.Database.DataLayer;
using JMayer.Example.ASPMVC.Models;

namespace JMayer.Example.ASPMVC.DataLayers;

/// <summary>
/// The interface for interacting with a work order collection in a database using CRUD operations.
/// </summary>
public interface IWorkOrderDataLayer : IStandardCRUDDataLayer<WorkOrder>
{
}
