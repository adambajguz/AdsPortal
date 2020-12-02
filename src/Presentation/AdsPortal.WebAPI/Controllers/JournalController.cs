namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.JournalOperations.Commands.CreateJournal;
    using AdsPortal.Application.Operations.JournalOperations.Commands.DeleteJournal;
    using AdsPortal.Application.Operations.JournalOperations.Commands.ImportJournals;
    using AdsPortal.Application.Operations.JournalOperations.Commands.UpdateJournal;
    using AdsPortal.Application.Operations.JournalOperations.Queries.GetJournalDetails;
    using AdsPortal.Application.Operations.JournalOperations.Queries.GetJournalsList;
    using AdsPortal.Application.Operations.JournalOperations.Queries.GetPagedJournalsList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/journal")]
    [SwaggerTag("Create, update, and get journal")]
    public class JournalController : BaseController
    {
        public const string Import = nameof(Import);
        //public const string ImportFromExistingFile = nameof(ImportFromExistingFile);
        public const string Create = nameof(CreateJournal);
        public const string GetDetails = nameof(GetJournalDetails);
        public const string Update = nameof(UpdateJournal);
        public const string Delete = nameof(DeleteJournal);
        public const string GetAll = nameof(GetJournalsList);
        public const string GetPaged = nameof(GetPagedJournalsList);

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("import")]
        [SwaggerOperation(
            Summary = "Import journals from csv file",
            Description = "Imports journals from csv file")]
        [SwaggerResponse(StatusCodes.Status200OK, "Journals imported", typeof(ImportJournalsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> ImportJournals([FromForm] ImportJournalsCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        //[HttpPost("import-from-existing-file", Name = ImportFromExistingFile)]
        //public async Task<IActionResult> ImportJournalsFromExistingFile([FromForm] FileRequest file)
        //{
        //    byte[] bytes = await file.File.GetBytesAsync();

        //    return Ok();
        //}

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new journal",
            Description = "Creates a new journal")]
        [SwaggerResponse(StatusCodes.Status200OK, "Journal created", typeof(ImportJournalsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateJournal([FromBody] CreateJournalCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("get/{id:guid}", Name = GetDetails)]
        [SwaggerOperation(
            Summary = "Get journal details",
            Description = "Gets journal details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetJournalDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetJournalDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetJournalDetailsQuery(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated journal details",
            Description = "Updates journal details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Journal details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateJournal([FromBody] UpdateJournalCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete journal",
            Description = "Deletes journal")]
        [SwaggerResponse(StatusCodes.Status200OK, "Journal deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteJournal([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteJournalCommand(id)));
        }

        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all journals",
            Description = "Gets a list of all journals")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetJournalsListResponse>))]
        public async Task<IActionResult> GetJournalsList()
        {
            return Ok(await Mediator.Send(new GetJournalsListQuery()));
        }

        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get paged journals",
            Description = "Gets a paged list of journals")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetJournalsListResponse>))]
        public async Task<IActionResult> GetPagedJournalsList([FromQuery] GetPagedJournalsListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
