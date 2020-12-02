namespace AdsPortal.Application.Operations.AuthorOperations.Commands.ImportAuthors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Media;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Application.OperationsModels.Importer;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Models.AuthorImporter;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using SerilogTimings;

    public class ImportAuthorsCommand : IOperation<ImportAuthorsResponse>
    {
        public IFormFile? File { get; set; }

        private class Handler : IRequestHandler<ImportAuthorsCommand, ImportAuthorsResponse>
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

            public async Task<ImportAuthorsResponse> Handle(ImportAuthorsCommand command, CancellationToken cancellationToken)
            {
                await new ImportAuthorsValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

                using (Operation.Time("Importing authors ({Guid}) from {Filename} by {UserId}", Guid.NewGuid(), command.File!.FileName, _currentUser.UserId))
                {
                    List<AuthorsImporterRecord> records;
                    using (StreamReader? stream = new StreamReader(command.File!.OpenReadStream()))
                        records = await _csvBuilder.GetRecordsAsync<AuthorsImporterRecord, AuthorsImporterRecordMap>(stream);

                    ImportAuthorsResponse response = new ImportAuthorsResponse();

                    foreach (AuthorsImporterRecord record in records)
                    {
                        Guid degreeId = await GetDegreeOrAddAsync(response, record.Degree, cancellationToken);
                        Guid departmentId = await GetDepartmentOrAddAsync(response, record.Department, cancellationToken);

                        await AddOrUpdateAuthorAsync(response, record, degreeId, departmentId);
                    }

                    int saved = await _uow.SaveChangesAsync(cancellationToken);

                    return response;
                }
            }

            private async Task AddOrUpdateAuthorAsync(ImportAuthorsResponse response, AuthorsImporterRecord record, Guid degreeId, Guid departmentId)
            {
                Author? entity = await _uow.Authors.SingleOrDefaultAsync(x => x.Name == record.Name && x.Surname == record.Surname);

                if (entity is null)
                {
                    entity = _mapper.Map<Author>(record);
                    entity.DegreeId = degreeId;
                    entity.DepartmentId = departmentId;

                    _uow.Authors.Add(entity);
                    ++response.Authors.Created;
                }
                else
                {
                    _mapper.Map(record, entity);
                    entity.DegreeId = degreeId;
                    entity.DepartmentId = departmentId;

                    _uow.Authors.Update(entity);
                    ++response.Authors.Updated;
                }
            }

            private async Task<Guid> GetDegreeOrAddAsync(ImportAuthorsResponse response, string name, CancellationToken cancellationToken)
            {
                Degree? entity = await _uow.Degrees.SingleOrDefaultAsync(x => x.Name == name, noTracking: true);

                if (entity is null)
                {
                    Guid id = _uow.Degrees.Add(new Degree
                    {
                        Name = name
                    }).Id;

                    await _uow.SaveChangesAsync(cancellationToken);
                    ++response.Degrees.Created;

                    return id;
                }
                ++response.Degrees.Skipped;

                return entity.Id;
            }

            private async Task<Guid> GetDepartmentOrAddAsync(ImportAuthorsResponse response, string name, CancellationToken cancellationToken)
            {
                Department? entity = await _uow.Departments.SingleOrDefaultAsync(x => x.Name == name, noTracking: true);

                if (entity is null)
                {
                    Guid id = _uow.Departments.Add(new Department
                    {
                        Name = name
                    }).Id;

                    await _uow.SaveChangesAsync(cancellationToken);
                    ++response.Departments.Created;

                    return id;
                }
                ++response.Departments.Skipped;

                return entity.Id;
            }
        }
    }
}
