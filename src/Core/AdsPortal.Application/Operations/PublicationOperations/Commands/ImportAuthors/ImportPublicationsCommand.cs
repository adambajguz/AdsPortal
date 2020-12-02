namespace AdsPortal.Application.Operations.AuthorOperations.Commands.ImportAuthors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Media;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Application.OperationsModels.Importer;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Models.AuthorImporter;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Serilog;
    using SerilogTimings;

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
                //TODO: Add validation, file format validaiton (same process but wihtout adding)
                await new ImportPublicationsValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

                using (Operation.Time("Importing publications ({Guid}) from {Filename} by {UserId}", Guid.NewGuid(), command.File!.FileName, _currentUser.UserId))
                {
                    List<PublicationsImporterRecord> records;
                    using (StreamReader? stream = new StreamReader(command.File!.OpenReadStream()))
                        records = await _csvBuilder.GetRecordsAsync<PublicationsImporterRecord, PublicationsImporterRecordMap>(stream);

                    ImportPublicationsResponse response = new ImportPublicationsResponse();

                    foreach (PublicationsImporterRecord record in records)
                    {
                        Journal journal = await GetJournalAsync(record);
                        Publication publication = await AddPublicationAsync(response, record, journal.Id, cancellationToken);

                        await AddOrUpdateAuthorAsync(response, record, publication.Id, cancellationToken);
                    }

                    int saved = await _uow.SaveChangesAsync(cancellationToken);

                    return response;
                }
            }

            private async Task<Journal> GetJournalAsync(PublicationsImporterRecord record)
            {
                string journalName = record.Journal.ToUpper().Trim();

                Log.ForContext<ImportPublicationsCommand>().Information("'{No}' '{Name}'", record.No, record.Journal);
                Journal entity = await _uow.Journals.SingleAsync(x => x.Name.ToUpper() == journalName || x.NameAlt.ToUpper() == journalName);

                return entity;
            }

            private async Task<Publication> AddPublicationAsync(ImportPublicationsResponse response, PublicationsImporterRecord record, Guid journalId, CancellationToken cancellationToken)
            {
                Publication entity = _mapper.Map<Publication>(record);
                entity.JournalId = journalId;

                _uow.Publications.Add(entity);
                ++response.Publications.Created;

                await _uow.SaveChangesAsync(cancellationToken);

                return entity;
            }

            private async Task AddOrUpdateAuthorAsync(ImportPublicationsResponse response, PublicationsImporterRecord record, Guid publicationId, CancellationToken cancellationToken)
            {
                string[]? authors = record.Authors;
                foreach (string author in authors)
                {
                    string[] nameSurname = author.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string name = nameSurname[1];
                    string surname = nameSurname[0];

                    Log.ForContext<ImportPublicationsCommand>().Information("'{Name}' '{Surname}'", name, surname);

                    Author entity = await _uow.Authors.SingleAsync(x => x.Name == name && x.Surname == surname);

                    PublicationAuthor publicationAuthor = new PublicationAuthor
                    {
                        AuthorId = entity.Id,
                        PublicationId = publicationId
                    };

                    ++response.AuthorsInPublications.Created;

                    _uow.PublicationAuthors.Add(publicationAuthor);
                }

                await _uow.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
