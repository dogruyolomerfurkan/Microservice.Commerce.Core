namespace Commerce.Core.Contracts;

public record ProductCreated(Guid productId, string Name, string Description);
public record ProductUpdated(Guid productId, string Name, string Description);
public record ProductDeleted(Guid productId);