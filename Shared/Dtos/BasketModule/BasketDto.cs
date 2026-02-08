namespace Shared.Dtos.BasketModule
{
    public record BasketDto
    {
        public string Id { get; init; }
        public ICollection<BasketItemDto> BasketItems { get; init; } = [];
    }
}
