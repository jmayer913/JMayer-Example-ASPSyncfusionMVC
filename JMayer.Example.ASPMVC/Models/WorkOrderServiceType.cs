using System.Runtime.Serialization;

namespace JMayer.Example.ASPMVC.Models;

/// <summary>
/// The enumeration for type of work order services.
/// </summary>
public enum WorkOrderServiceType
{
    /// <summary>
    /// Insepction of an equipment.
    /// </summary>
    [EnumMember(Value = "Insepction")]
    Inspection = 0,

    /// <summary>
    /// Routine maintenance done on an equipment.
    /// </summary>
    [EnumMember(Value = "Routine")]
    Routine,

    /// <summary>
    /// Maintenance done in response to an equipment failure.
    /// </summary>
    [EnumMember(Value = "Reactive")]
    Reactive,

    /// <summary>
    /// Any other type of service.
    /// </summary>
    [EnumMember(Value = "Other")]
    Other
}
