using System.Runtime.Serialization;

namespace JMayer.Example.ASPMVC.Models;

/// <summary>
/// The enumeration for the work order priority.
/// </summary>
public enum WorkOrderPriority
{
    /// <summary>
    /// The work order needs to be addressed after other higher priorities have been taken care of.
    /// </summary>
    [EnumMember(Value = "Low")]
    Low = 0,

    /// <summary>
    /// The work order needs to be addressed in a reasonable amount of time.
    /// </summary>
    [EnumMember(Value = "Normal")]
    Normal,

    /// <summary>
    /// The work order needs to be addressed immediately.
    /// </summary>
    [EnumMember(Value = "High")]
    High,
}
