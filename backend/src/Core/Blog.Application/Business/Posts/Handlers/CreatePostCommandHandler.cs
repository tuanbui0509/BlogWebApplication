using Blog.Application.Common.Interfaces.Repositories;
using Blog.Application.Business.Posts.Commands.CreatePost;
using Blog.Domain.Entities;
using MediatR;
using System.Text.Json;
using Blog.Application.Business.Posts.Queries.GetAllPosts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace Blog.Application.Business.Posts.Handlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IUnitOfWork unitOfWork, IDistributedCache cache, Serilog.ILogger logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var tags = await GetOrCreateTagsAsync(command.Tags, command.UserId, cancellationToken);
                // Map command to entity
                var entity = MapToPost(command, tags);

                await _unitOfWork.Repository<Post>().AddAsync(entity);
                entity.AddDomainEvent(new PostCreatedEvent(entity));
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.Information("Create post successful!");
                // caching
                await _cache.RemoveAsync("Posts", cancellationToken);
                var posts = await _unitOfWork.Repository<Post>()
                            .Entities
                            .AsQueryable()
                            .Include(post => post.PostTags)
                            .ThenInclude(postTag => postTag.Tag) // Navigate through PostTags to Tags
                            .ProjectTo<GetAllPostsDto>(_mapper.ConfigurationProvider)
                            .AsNoTracking()
                            .OrderByDescending(post => post.UpdatedDate)
                            .ToListAsync(cancellationToken);
                await _cache.SetStringAsync($"Posts", JsonSerializer.Serialize(posts));
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating post");
                throw;
            }
        }

        private static Post MapToPost(CreatePostCommand command, List<Tag> existingTags)
        {
            return new Post
            {
                Title = command.Title,
                Slug = GenerateSlug(command.Slug),
                PostContents = command.PostContents,
                CreatedBy = command.UserName,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = command.UserName,
                UpdatedDate = DateTime.UtcNow,
                IsPublished = command.IsPublished,
                AuthorId = Guid.Parse(command.UserId),
                PostTags = command.Tags.Select(tagName =>
                {
                    var tag = existingTags.FirstOrDefault(t => t.TagName.Equals(tagName, StringComparison.OrdinalIgnoreCase));
                    return new PostTag
                    {
                        Tag = tag ?? new Tag { TagName = tagName, Slug = GenerateSlug(tagName) },
                        CreatedBy = command.UserName,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = command.UserName,
                        UpdatedDate = DateTime.UtcNow,
                    };
                }).ToList()
            };
        }

        private async Task<List<Tag>> GetOrCreateTagsAsync(
            List<string> tagNames,
            string userId,
            CancellationToken cancellationToken)
        {
            var existingTags = await _unitOfWork.Repository<Tag>().GetAllAsync();
            existingTags = existingTags.Where(t => tagNames.Contains(t.TagName)).ToList();
            var newTagNames = tagNames
                .Except(existingTags.Select(t => t.TagName))
                .ToList();

            var newTags = newTagNames.Select(tagName => new Tag
            {
                TagName = tagName,
                Slug = GenerateSlug(tagName),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            }).ToList();

            if (newTags.Any())
            {
                await _unitOfWork.Repository<Tag>().AddRangeAsync(newTags);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return existingTags.Concat(newTags).ToList();
        }

        private static string GenerateSlug(string input)
        {
            return input.ToLower().Replace(" ", "-");
        }
    }
}