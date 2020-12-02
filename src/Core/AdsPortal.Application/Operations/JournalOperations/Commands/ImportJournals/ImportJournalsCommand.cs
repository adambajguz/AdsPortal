namespace AdsPortal.Application.Operations.JournalOperations.Commands.ImportJournals
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
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Models.JournalsImporter;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using SerilogTimings;

    public class ImportJournalsCommand : IRequest<ImportJournalsResponse>
    {
        public IFormFile? File { get; set; }

        private class Handler : IRequestHandler<ImportJournalsCommand, ImportJournalsResponse>
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

            public async Task<ImportJournalsResponse> Handle(ImportJournalsCommand command, CancellationToken cancellationToken)
            {
                await new ImportJournalsValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

                using (Operation.Time("Importing journals ({Guid}) from {Filename} by {UserId}", Guid.NewGuid(), command.File!.FileName, _currentUser.UserId))
                {
                    List<JournalsImporterRecord> records;
                    using (StreamReader? stream = new StreamReader(command.File!.OpenReadStream()))
                        records = await _csvBuilder.GetRecordsAsync<JournalsImporterRecord, JournalsImporterRecordMap>(stream);

                    foreach (JournalsImporterRecord record in records)
                    {
                        Journal entity = _mapper.Map<Journal>(record);
                        _uow.Journals.Add(entity);
                    }

                    int saved = await _uow.SaveChangesAsync(cancellationToken);

                    return new ImportJournalsResponse(saved / 2);
                }
            }
        }
    }
}
