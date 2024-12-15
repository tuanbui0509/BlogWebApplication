using Blog.Application.Common.Interfaces.Repositories;
using Blog.Application.Business.Posts.Commands.CreatePost;
using Blog.Domain.Entities;
using MediatR;
using System.Text.Json;
using Blog.Application.Business.Posts.Queries.GetAllPosts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.Application.Business.Posts.Handlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        private readonly Serilog.ILogger _logger;


        public CreatePostCommandHandler(IUnitOfWork unitOfWork, IDistributedCache cache, Serilog.ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Handle tags (ensure they exist or create new ones)
                var tags = await GetOrCreateTagsAsync(command.Tags, command.UserId, cancellationToken);
                // Map command to entity
                var entity = MapToPost(command, tags);
                // caching
                var cachePosts = await _cache.GetStringAsync("Posts");
                if (cachePosts == null)
                {
                    cachePosts = JsonSerializer.Serialize(new List<GetAllPostsDto>());
                }
                var posts = JsonSerializer.Deserialize<List<GetAllPostsDto>>(cachePosts);
                posts.Add(new GetAllPostsDto()
                {
                    Title = command.Title,
                    Slug = command.Slug,
                    PostContents = command.PostContents,
                    IsPublished = command.IsPublished,
                    AuthorId = command.UserId,
                    Tags = command.Tags,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedBy = command.UserName,
                    UpdatedBy = command.UserName
                });
                await _cache.SetStringAsync($"Posts", JsonSerializer.Serialize(posts));

                await _unitOfWork.Repository<Post>().AddAsync(entity);
                entity.AddDomainEvent(new PostCreatedEvent(entity));
                await _unitOfWork.SaveChangesAsync(cancellationToken);
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
                Slug = command.Slug,
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
            // Fetch existing tags
            // var existingTags = _unitOfWork.Repository<Tag>().Entities.AsEnumerable()
            //     .Where(t => command.Tags.Contains(t.TagName))
            //     .ToList();
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