using System.Runtime.Serialization;

namespace JMayer.Example.ASPMVC.Models;

/// <summary>
/// The enumeration for the statuses of the work order.
/// </summary>
public enum WorkOrderStatus
{
    /// <summary>
    /// No one has started the work order.
    /// </summary>
    [EnumMember(Value = "Open")]
    Open = 0,

    /// <summary>
    /// Someone is working on the work order.
    /// </summary>
    [EnumMember(Value = "In Progress")]
    InProgress,

    /// <summary>
    /// The work has finished and it needs a final signoff.
    /// </summary>
    [EnumMember(Value = "Verify")]
    Verify,

    /// <summary>
    /// The work order is no longer active.
    /// </summary>
    [EnumMember(Value = "Closed")]
    Closed
}
