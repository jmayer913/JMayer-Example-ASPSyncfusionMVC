using JMayer.Example.ASPMVC.Models;

namespace TestProject.Test.EqualityComparer;

/// <summary>
/// The class manages testing the work order equality comparer.
/// </summary>
public class WorkOrderEqualityComparerUnitTest
{
    /// <summary>
    /// The constant for the description.
    /// </summary>
    private const string Description = "Description";

    /// <summary>
    /// The constant for the ID.
    /// </summary>
    private const long ID = 1;

    /// <summary>
    /// The constant for the name.
    /// </summary>
    private const string Name = "Name";

    /// <summary>
    /// The constant for the other type of service.
    /// </summary>
    private const string OtherTypeOfService = "Other";

    /// <summary>
    /// The constant for the problem.
    /// </summary>
    private const string Problem = "A Problem";

    /// <summary>
    /// The constant for the resolution.
    /// </summary>
    private const string Resolution = "A Resolution";

    /// <summary>
    /// The method verifies equality failure when two nulls are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureBothNull() => Assert.False(new WorkOrderEqualityComparer().Equals(null, null));

    /// <summary>
    /// The method verifies equality failure when the CreatedOn property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureCreatedOn()
    {
        WorkOrder workOrder1 = new()
        {
            CreatedOn = DateTime.Now,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the DueBy property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDueBy()
    {
        WorkOrder workOrder1 = new()
        {
            DueBy = DateTime.Today.AddDays(7),
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the Description property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureDescription()
    {
        WorkOrder workOrder1 = new()
        {
            Description = Description,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the ID property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureID()
    {
        WorkOrder workOrder1 = new()
        {
            Integer64ID = ID,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the LastEditedOn property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureLastEditedOn()
    {
        WorkOrder workOrder1 = new()
        {
            LastEditedOn = DateTime.Now,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the Name property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureName()
    {
        WorkOrder workOrder1 = new()
        {
            Name = Name,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when an object and null are compared.
    /// </summary>
    [Fact]
    public void VerifyFailureOneIsNull()
    {
        WorkOrder asset = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            DueBy = DateTime.Today.AddDays(7),
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            OtherTypeOfService = OtherTypeOfService,
            Priority = WorkOrderPriority.High,
            Problem = Problem,
            Resolution = Resolution,
            ServiceType = WorkOrderServiceType.Other,
            Status = WorkOrderStatus.InProgress,
        };

        Assert.False(new WorkOrderEqualityComparer().Equals(asset, null));
        Assert.False(new WorkOrderEqualityComparer().Equals(null, asset));
    }

    /// <summary>
    /// The method verifies equality failure when the OtherTypeOfService property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureOtherTypeOfService()
    {
        WorkOrder workOrder1 = new()
        {
            OtherTypeOfService = OtherTypeOfService,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the Priority property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailurePriority()
    {
        WorkOrder workOrder1 = new()
        {
            Priority = WorkOrderPriority.High,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the Problem property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureProblem()
    {
        WorkOrder workOrder1 = new()
        {
            Problem = Problem,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the Resolution property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureResolution()
    {
        WorkOrder workOrder1 = new()
        {
            Resolution = Resolution,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the ServiceType property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureServiceType()
    {
        WorkOrder workOrder1 = new()
        {
            ServiceType = WorkOrderServiceType.Other,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality failure when the Status property is different between the two objects.
    /// </summary>
    [Fact]
    public void VerifyFailureStatus()
    {
        WorkOrder workOrder1 = new()
        {
            Status = WorkOrderStatus.InProgress,
        };
        WorkOrder workOrder2 = new();

        Assert.False(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared.
    /// </summary>
    [Fact]
    public void VerifySuccess()
    {
        WorkOrder workOrder1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            DueBy = DateTime.Today.AddDays(7),
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            OtherTypeOfService = OtherTypeOfService,
            Priority = WorkOrderPriority.High,
            Problem = Problem,
            Resolution = Resolution,
            ServiceType = WorkOrderServiceType.Other,
            Status = WorkOrderStatus.InProgress,
        };
        WorkOrder workOrder2 = new(workOrder1);

        Assert.True(new WorkOrderEqualityComparer().Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeCreatedOn()
    {
        WorkOrder workOrder1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            DueBy = DateTime.Today.AddDays(7),
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            OtherTypeOfService = OtherTypeOfService,
            Priority = WorkOrderPriority.High,
            Problem = Problem,
            Resolution = Resolution,
            ServiceType = WorkOrderServiceType.Other,
            Status = WorkOrderStatus.InProgress,
        };
        WorkOrder workOrder2 = new(workOrder1)
        {
            CreatedOn = DateTime.MinValue,
        };

        Assert.True(new WorkOrderEqualityComparer(true, false, false).Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the Integer64ID property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeID()
    {
        WorkOrder workOrder1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            DueBy = DateTime.Today.AddDays(7),
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            OtherTypeOfService = OtherTypeOfService,
            Priority = WorkOrderPriority.High,
            Problem = Problem,
            Resolution = Resolution,
            ServiceType = WorkOrderServiceType.Other,
            Status = WorkOrderStatus.InProgress,
        };
        WorkOrder workOrder2 = new(workOrder1)
        {
            Integer64ID = ID + 1,
        };

        Assert.True(new WorkOrderEqualityComparer(false, true, false).Equals(workOrder1, workOrder2));
    }

    /// <summary>
    /// The method verifies equality success when two objects (different references) are compared
    /// but the LastEditedOn property is excluded from the check.
    /// </summary>
    [Fact]
    public void VerifySuccessExcludeLastEditedOn()
    {
        WorkOrder workOrder1 = new()
        {
            CreatedOn = DateTime.Now,
            Description = Description,
            DueBy = DateTime.Today.AddDays(7),
            Integer64ID = ID,
            LastEditedOn = DateTime.Now,
            Name = Name,
            OtherTypeOfService = OtherTypeOfService,
            Priority = WorkOrderPriority.High,
            Problem = Problem,
            Resolution = Resolution,
            ServiceType = WorkOrderServiceType.Other,
            Status = WorkOrderStatus.InProgress,
        };
        WorkOrder workOrder2 = new(workOrder1)
        {
            LastEditedOn = DateTime.MinValue,
        };

        Assert.True(new WorkOrderEqualityComparer(false, false, true).Equals(workOrder1, workOrder2));
    }
}
