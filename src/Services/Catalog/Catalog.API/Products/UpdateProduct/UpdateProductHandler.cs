
using Catalog.API.Exceptions;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description,string ImageFile, decimal Price):ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    internal class UpdateProductCommandHandler
        (IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (result is null)
            {
                throw new ProductNotFoundException();
            }

            result.Name = command.Name;
            result.Category = command.Category;
            result.Description = command.Description;
            result.ImageFile = command.ImageFile;
            result.Price = command.Price;

            session.Update(result);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
