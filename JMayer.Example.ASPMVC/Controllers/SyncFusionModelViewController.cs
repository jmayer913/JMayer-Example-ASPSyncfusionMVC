using JMayer.Data.Data;
using JMayer.Data.Database.DataLayer;
using JMayer.Web.Mvc.Controller.Mvc;
using JMayer.Web.Mvc.Extension;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

namespace JMayer.Example.ASPMVC.Controllers;

/// <summary>
/// The class manages Syncfusion HTTP requests for CRUD operations associated with a data object and a data layer.
/// </summary>
/// <typeparam name="T">Must be a DataObject since the data layer requires this.</typeparam>
/// <typeparam name="U">Must be an IStandardCRUDDataLayer so the controller can interact with the collection/table associated with it.</typeparam>
public class SyncFusionModelViewController<T, U> : StandardModelViewController<T, U>
    where T : DataObject
    where U : IStandardCRUDDataLayer<T>
{
    /// <inheritdoc/>
    public SyncFusionModelViewController(U dataLayer, ILogger<SyncFusionModelViewController<T, U>> logger) : base(dataLayer, logger) 
    {
        IsCUDActionRedirectedOnSuccess = false;
        IsDetailsIncludedInNegativeResponse = true;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overridden so the NonAction attribute is applied to it. Syncfusion uses a CRUDModel object so a custom CreateAsync() 
    /// needed to be created in the SyncFusionModelViewController and this causes a mapping conflict in asp.net core between the
    /// parent and child classes. The NonAction attributes tells asp.net core to ignore the base CreateAsync().
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> CreateAsync([FromBody] T dataObject)
    {
        return base.CreateAsync(dataObject);
    }

    /// <summary>
    /// The method creates a data object using the data layer.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The created data object or a negative status code.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] CRUDModel<T> model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                model.Value = await DataLayer.CreateAsync(model.Value);
                Logger.LogInformation("The {Type} was successfully created.", DataObjectTypeName);
                return Json(model.Value);
            }
            else
            {
                Logger.LogWarning("Failed to create the {Type} because of a model validation error.", DataObjectTypeName);
                return ValidationProblem(ModelState);
            }
        }
        catch (DataObjectValidationException ex)
        {
            Logger.LogWarning(ex, "Failed to create the {Type} because of a server-side validation error.", DataObjectTypeName);
            ex.CopyToModelState(ModelState);
            return ValidationProblem(ModelState);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to create the {Type}.", DataObjectTypeName);
            return Problem(detail: "Failed to create the record because of an error on the server.");
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overridden so the NonAction attribute is applied to it. Syncfusion uses a CRUDModel object so a custom DeleteAsync() 
    /// needed to be created in the SyncFusionModelViewController and this causes a mapping conflict in asp.net core between the
    /// parent and child classes. The NonAction attributes tells asp.net core to ignore the base DeleteAsync().
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> DeleteAsync(long id)
    {
        return base.DeleteAsync(id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overridden so the NonAction attribute is applied to it. Syncfusion uses a CRUDModel object so a custom DeleteAsync() 
    /// needed to be created in the SyncFusionModelViewController and this causes a mapping conflict in asp.net core between the
    /// parent and child classes. The NonAction attributes tells asp.net core to ignore the base DeleteAsync().
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> DeleteAsync(string id)
    {
        return base.DeleteAsync(id);
    }

    /// <summary>
    /// The method deletes a data object using the data layer.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The deleted data object or a negative status code.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> DeleteAsync([FromBody] CRUDModel<T> model)
    {
        try
        {
            if (model.Key is null)
            {
                Logger.LogWarning("Failed to delete the {Type} because a key was not defined.", DataObjectTypeName);
                return NotFound(new { UserMessage = "The record could not be found because a key was not provided." });
            }
            else if (model.KeyColumn == nameof(DataObject.Integer64ID))
            {
                await DataLayer.DeleteAsync(obj => obj.Integer64ID == Convert.ToInt64(model.Key.ToString()));
                Logger.LogInformation("The {Key} for the {Type} was successfully deleted.", model.Key.ToString(), DataObjectTypeName);
            }
            else if (model.KeyColumn == nameof(DataObject.StringID))
            {
                await DataLayer.DeleteAsync(obj => obj.StringID == model.Key.ToString());
                Logger.LogInformation("The {Key} for the {Type} was successfully deleted.", model.Key.ToString(), DataObjectTypeName);
            }

            return Json(model);
        }
        catch (DataObjectDeleteConflictException ex)
        {
            Logger.LogError(ex, "Failed to delete the {Key} {Type} because of a data conflict.", model.Key.ToString(), DataObjectTypeName);
            return Conflict(new { UserMessage = "The record has a dependency that prevents it from being deleted." });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete the {Key} {Type}.", model.Key.ToString(), DataObjectTypeName);
            return Problem(detail: "Failed to delete the record because of an error on the server.");
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Overridden so the NonAction attribute is applied to it. Syncfusion uses a CRUDModel object so a custom UpdateAsync() 
    /// needed to be created in the SyncFusionModelViewController and this causes a mapping conflict in asp.net core between the
    /// parent and child classes. The NonAction attributes tells asp.net core to ignore the base UpdateAsync().
    /// </remarks>
    [NonAction]
    public override Task<IActionResult> UpdateAsync([FromBody] T dataObject)
    {
        return base.UpdateAsync(dataObject);
    }

    /// <summary>
    /// The method updates a data object using the data layer.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The updated data object or a negative status code.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> UpdateAsync([FromBody] CRUDModel<T> model)
    {
        try
        {
            if (model.Key is null)
            {
                Logger.LogWarning("Failed to update the {Type} because a key was not defined.", DataObjectTypeName);
                return NotFound(new { UserMessage = "The record could not be found because a key was not provided." });
            }
            else if (ModelState.IsValid)
            {
                UpdateUTCTimesToLocal(model);
                model.Value = await DataLayer.UpdateAsync(model.Value);
                Logger.LogInformation("The {Key} for the {Type} was successfully updated.", model.Key.ToString(), DataObjectTypeName);
                return Json(model.Value);
            }
            else
            {
                Logger.LogWarning("Failed to update the {Key} {Type} because of a model validation error.", model.Key.ToString(), DataObjectTypeName);
                return ValidationProblem(ModelState);
            }
        }
        catch (DataObjectUpdateConflictException ex)
        {
            Logger.LogWarning(ex, "Failed to update {Key} {Type} because the data was considered old.", model.Key.ToString(), DataObjectTypeName);
            return Conflict(new { UserMessage = "The submitted data was detected to be out of date; please refresh the page and try again." });
        }
        catch (DataObjectValidationException ex)
        {
            Logger.LogWarning(ex, "Failed to update the {Key} {Type} because of a server-side validation error.", model.Key.ToString(), DataObjectTypeName);
            ex.CopyToModelState(ModelState);
            return ValidationProblem(ModelState);
        }
        catch (IDNotFoundException ex)
        {
            Logger.LogWarning(ex, "Failed to update the {Key} {Type} because it was not found.", model.Key.ToString(), DataObjectTypeName);
            return NotFound(new { UserMessage = "The record was not found; please refresh the page because another user may have deleted it." });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update the {Type} for {Key}.", DataObjectTypeName, model.Key.ToString());
            return Problem(detail: "Failed to update the record because of an error on the server.");
        }
    }

    /// <summary>
    /// The method converts the CreatedOn and LastEditedOn properties in the model to the local time.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <remarks>
    /// Syncfusion stores date times as UTC and sends them to the server as UTC so 
    /// at least, the LastEditedOn field needs to be converted to the local so it doesn't 
    /// trigger conflict detection.
    /// </remarks>
    protected virtual void UpdateUTCTimesToLocal(CRUDModel<T> model)
    {
        if (model.Value is UserEditableDataObject dataObject)
        {
            dataObject.CreatedOn = dataObject.CreatedOn.ToLocalTime();
            dataObject.LastEditedOn = dataObject.LastEditedOn?.ToLocalTime();
        }
    }
}
