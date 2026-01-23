# ASP.NET Core MVC with Syncfusion Example Project

**Warning!!! This example project requires a syncfusion key in order to run. On startup, Program.cs will read the license key from a text file located at "C:\GitHub\Syncfusion License Key.txt."** [Syncfusion](https://www.syncfusion.com/)

This example project is a simplified work order management of equipment in a system and it would be part of a CMMS software (computerized maintenance management system). The example has a single page, work orders.

## Work Orders
The work orders page allows the user to add/edit/delete work orders. Work orders are divided into Inspection, Routine, Reactive and Other and each will have a status, Open, In-Progress, Resolved or Closed. The user can move the work order to the next status or set a specific one. When the status is set to Resolved, the user will be asked to optionally enter 
a problem and resolution. The rows will be colored red or yellow to highlight if the work order is critically due or is close to being due. Critical is based on if the due by is today or past today. Warning is based on if today is at most 7 days out from the due by.

<img width="1913" height="641" alt="image" src="https://github.com/user-attachments/assets/7fe1d925-b741-4fa8-901b-e5faf9dd3bd8" />

### Add / Edit

On the work orders page, the user can create a new work order or edit an existing work order.

* Name - A friendly name for the work order; required and must be unique.
* Description - A description about the work needed to be performed; optional.
* Service Type - The type of service that will be done; required. Options are Inspection, Routine, Reactive and Other.
* Other Type of Service - A friendly name to describe the other type of service; only enabled when the Service Type is Other.
* Priority - The urgency of completion for the work order; required. Options are Low, Normal and High.
* Due By - When the work order is expected to be done by; optional.

<img width="377" height="326" alt="image" src="https://github.com/user-attachments/assets/ae4f5b4c-fe41-4298-a635-f334af83802c" />

<img width="380" height="326" alt="image" src="https://github.com/user-attachments/assets/f72385f3-f91d-478e-b968-07451aec748c" />

When editing and the status is resolved or closed, a description of the problem can be entered and/or a description of the resolution to the problem can be entered; both are optional.

<img width="373" height="463" alt="image" src="https://github.com/user-attachments/assets/927f3e30-6039-49e2-bf49-18312936735f" />

### Delete

On the work orders page, the user can delete a work order. The user will be required to confirm the deletion or cancel. On confirmed, the work order will be deleted.

<img width="270" height="129" alt="image" src="https://github.com/user-attachments/assets/8cfd16bc-dc65-407d-98f4-8810fabfc083" />
