using JMayer.Data.Data;
using JMayer.Data.Database.DataLayer;
using JMayer.Data.HTTP.DataLayer;
using JMayer.Web.Mvc.Controller;
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
    where U : Data.Database.DataLayer.IStandardCRUDDataLayer<T>
{
    /// <summary>
    /// The constant for the conflict message key.
    /// </summary>
    private const string ConflictMessageKey = "ConflictMessage";

    /// <inheritdoc/>
    public SyncFusionModelViewController(Data.Database.DataLayer.IStandardCRUDDataLayer<T> dataLayer, ILogger<SyncFusionModelViewController<T, U>> logger) : base(dataLayer, logger)
    {
    }

    /// <summary>
    /// The method creates a data object using the data layer.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The created data object.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] CRUDModel<T> model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                model.Value = await DataLayer.CreateAsync(model.Value);
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
            ServerSideValidationResult serverSideValidationResult = new(ex.ValidationResults);
            Logger.LogWarning(ex, "Failed to create the {Type} because of a server-side validation error.", DataObjectTypeName);
            return BadRequest(serverSideValidationResult);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to create the {Type}.", DataObjectTypeName);
            return Problem(detail: "Failed to create the record because of an error on the server.");
        }
    }

    /// <summary>
    /// The method deletes a data object using the data layer.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The model.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> DeleteAsync([FromBody] CRUDModel<T> model)
    {
        try
        {
            if (model.KeyColumn == nameof(DataObject.Integer64ID))
            {
                await DataLayer.DeleteAsync(obj => obj.Integer64ID == (long)model.Key);
            }
            else if (model.KeyColumn == nameof(DataObject.StringID))
            {
                await DataLayer.DeleteAsync(obj => obj.StringID == (string)model.Key);
            }

            return Json(model);
        }
        catch (DataObjectDeleteConflictException ex)
        {
            ModelState.AddModelError(ConflictMessageKey, "The record has a dependency that prevents it from being deleted.");
            Logger.LogError(ex, "Failed to delete the {Key} {Type} because of a data conflict.", model.Key.ToString(), DataObjectTypeName);
            return Conflict(ModelState);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete the {Key} {Type}.", model.Key.ToString(), DataObjectTypeName);
            return Problem(detail: "Failed to delete the record because of an error on the server.");
        }
    }

    /// <summary>
    /// The method updates a data object using the data layer.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The updated data object.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> UpdateAsync([FromBody] CRUDModel<T> model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                model.Value = await DataLayer.UpdateAsync(model.Value);
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
            ModelState.AddModelError(ConflictMessageKey, "The submitted data was detected to be out of date; please refresh the page and try again.");
            Logger.LogWarning(ex, "Failed to update {Key} {Type} because the data was considered old.", model.Key.ToString(), DataObjectTypeName);
            return Conflict(ModelState);
        }
        catch (DataObjectValidationException ex)
        {
            ServerSideValidationResult serverSideValidationResult = new(ex.ValidationResults);
            Logger.LogWarning(ex, "Failed to update the {Key} {Type} because of a server-side validation error.", model.Key.ToString(), DataObjectTypeName);
            return BadRequest(serverSideValidationResult);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update the {Type} for {Key}.", DataObjectTypeName, model.Key.ToString());
            return Problem(detail: "Failed to update the record because of an error on the server.");
        }
    }
}
