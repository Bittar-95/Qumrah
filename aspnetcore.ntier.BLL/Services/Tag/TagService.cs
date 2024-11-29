using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagEntity = aspnetcore.ntier.DAL.Entities.Tag;
using MultimediaEntity = aspnetcore.ntier.DAL.Entities.Multimedia;
using aspnetcore.ntier.DAL.Entities;
using AutoMapper;
using aspnetcore.ntier.BLL.Services.IServices;

namespace aspnetcore.ntier.BLL.Services.Tag
{
    public class TagService : ITagService
    {
        private readonly IGenericRepository<TagEntity> _tagRepository;
        private readonly IGenericRepository<MultimediaEntity> _multimediaRepository;
        private readonly IGenericRepository<MultimediaTag> _multimediaTagRepository;
        private readonly IMapper _mapper;

        public TagService(IGenericRepository<TagEntity> tagRepository, IGenericRepository<MultimediaEntity> multimediaRepository, IGenericRepository<MultimediaTag> multimediaTagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _multimediaRepository = multimediaRepository;
            _multimediaTagRepository = multimediaTagRepository;
            _mapper = mapper;
        }

        public async Task<List<TagDto>> GetAll(bool Top10 = false)
        {
            List<TagEntity> Tags = new List<TagEntity>();
            if (Top10)
            {
                var ids = (await _multimediaRepository.GetListAsync()).OrderByDescending(x => x.TotalDownloads).Take(10).Select(i => i.Id);
                var tagIds = (await _multimediaTagRepository.GetListAsync(x => ids.Contains(x.MultimediaId))).Select(mt => mt.TagId);
                Tags = await _tagRepository.GetListAsync(t => tagIds.Contains(t.Id));

            }
            else
            {
                Tags = await _tagRepository.GetListAsync();
            }
            return _mapper.Map<List<TagDto>>(Tags);
        }
    }
}
