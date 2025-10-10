using System.Diagnostics.CodeAnalysis;

namespace JMayer.Example.ASPMVC.Models;

/// <summary>
/// The class manages comparing two WorkOrder objects.
/// </summary>
public class WorkOrderEqualityComparer : IEqualityComparer<WorkOrder>
{
    /// <summary>
    /// Excludes the CreatedOn property from the equals check.
    /// </summary>
    private readonly bool _excludeCreatedOn;

    /// <summary>
    /// Excludes the ID property from the equals check.
    /// </summary>
    private readonly bool _exlucdeID;

    /// <summary>
    /// Excludes the LastEditedOn property from the equals check.
    /// </summary>
    private readonly bool _excludeLastEditedOn;

    /// <summary>
    /// The default constructor.
    /// </summary>
    public WorkOrderEqualityComparer() { }

    /// <summary>
    /// The property constructor.
    /// </summary>
    /// <param name="excludeCreatedOn">Excludes the CreatedOn property from the equals check.</param>
    /// <param name="exlucdeID">Excludes the ID property from the equals check.</param>
    /// <param name="excludeLastEditedOn">Excludes the LastEditedOn property from the equals check.</param>
    public WorkOrderEqualityComparer(bool excludeCreatedOn, bool exlucdeID, bool excludeLastEditedOn)
    {
        _excludeCreatedOn = excludeCreatedOn;
        _exlucdeID = exlucdeID;
        _excludeLastEditedOn = excludeLastEditedOn;
    }

    /// <inheritdoc/>
    public bool Equals(WorkOrder? x, WorkOrder? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return (_excludeCreatedOn || x.CreatedOn == y.CreatedOn)
            && x.Description == y.Description
            && x.DueBy == y.DueBy
            && (_exlucdeID || x.Integer64ID == y.Integer64ID)
            && (_excludeLastEditedOn || x.LastEditedOn == y.LastEditedOn)
            && x.Name == y.Name
            && x.OtherTypeOfService == y.OtherTypeOfService
            && x.Priority == y.Priority
            && x.Problem == y.Problem
            && x.Resolution == y.Resolution
            && x.ServiceType == y.ServiceType
            && x.Status == y.Status;
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] WorkOrder obj)
    {
        throw new NotImplementedException();
    }
}
