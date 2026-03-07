namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Defines common CRUD operations for repository implementations.
/// </summary>
public interface IRepository<T>
{
    /// <summary>
    /// Returns all stored entities.
    /// </summary>
    List<T> GetAll();

    /// <summary>
    /// Returns one entity by ID when found.
    /// </summary>
    T? GetById(Guid id);

    /// <summary>
    /// Adds an entity to storage.
    /// </summary>
    void Add(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Removes an entity by ID.
    /// </summary>
    void Remove(Guid id);
}
