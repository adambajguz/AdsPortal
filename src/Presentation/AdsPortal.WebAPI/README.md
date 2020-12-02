# Presentation Layer :: WebAPI

This layer contains API (controllers .etc)

## File endpoint example

```
public class ImportPublicationsCommand : IOperation<ImportPublicationsResponse>
{
    public IFormFile? File { get; set; }

    private class Handler : IRequestHandler<ImportPublicationsCommand, ImportPublicationsResponse>
    {
        private readonly IAppRelationalUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICsvBuilderService _csvBuilder;
        private readonly ICurrentUserService _currentUser;

        public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, ICsvBuilderService csvBuilder, ICurrentUserService currentUser)
        {
            _uow = uow;
            _mapper = mapper;
            _csvBuilder = csvBuilder;
            _currentUser = currentUser;
        }

        public async Task<ImportPublicationsResponse> Handle(ImportPublicationsCommand command, CancellationToken cancellationToken)
        {
            await new ImportPublicationsValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

            using (Operation.Time("Importing publications ({Guid}) from {Filename} by {UserId}", Guid.NewGuid(), command.File!.FileName, _currentUser.UserId))
            {
                List<PublicationsImporterRecord> records;
                using (StreamReader? stream = new StreamReader(command.File!.OpenReadStream()))
                    records = await _csvBuilder.GetRecordsAsync<PublicationsImporterRecord, PublicationsImporterRecordMap>(stream);

                (...)

                return response;
            }
        }
```

```
[CustomAuthorize(Roles.Admin)]
[HttpPost("import")]
[SwaggerOperation(
    Summary = "Import publications from csv file",
    Description = "Imports publications from csv file")]
[SwaggerResponse(StatusCodes.Status200OK, "Publications imported", typeof(ImportPublicationsResponse))]
[SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
[SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
public async Task<IActionResult> ImportPublications([FromForm] ImportPublicationsCommand request)
{
    return Ok(await Mediator.Send(request));
}
```