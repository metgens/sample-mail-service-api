﻿using AutoMapper;
using MailService.Common.Bus.Query;
using MailService.Common.Pagination;
using MailService.Contracts.DTOs;
using MailService.Contracts.Queries;
using MailService.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MailService.Domain.Handlers
{
    public class GetAllMailsQueryHandler : IQueryHandler<GetAllMailsQuery, PagedResult<MailDto>>
    {
        private readonly IMailReadRepository _mailRepository;
        private readonly IMapper _mapper;

        public GetAllMailsQueryHandler(IMailReadRepository mailRepository, IMapper mapper)
        {
            _mailRepository = mailRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<MailDto>> Handle(GetAllMailsQuery request, CancellationToken cancellationToken)
        {
            var results = await _mailRepository.GetAllAsync(request);

            return _mapper.Map<PagedResult<MailDto>>(results);
        }

        public class GetAllMailsQueryProfile : Profile
        {
            public GetAllMailsQueryProfile()
            {
                CreateMap<PagedResult<Mail>, PagedResult<MailDto>>();
                CreateMap<Mail, MailDto>();
                CreateMap<MailAttachment, MailAttachmentWithoutContentDto>();
            }
        }

    }
}
