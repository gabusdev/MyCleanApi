namespace Application.Common.Caching;

public interface ICacheService
{
    /// <summary>
    /// Returns the <c>T</c> object from the Cache or <c>null</c>
    /// </summary>
    /// <typeparam name="T">Type of the Object</typeparam>
    /// <param name="key">Key value of the object to search for in Cache</param>
    /// <returns>The Object if found, else, <c>null</c></returns>
    T? Get<T>(string key);
    /// <summary>
    /// Returns the <c>T</c> object from the Cache or <c>null</c> in asyncronous way
    /// </summary>
    /// <typeparam name="T">Type of the Object</typeparam>
    /// <param name="key">Key value of the object to search for in Cache</param>
    /// <returns>The Object if found, else, <c>null</c></returns>
    Task<T?> GetAsync<T>(string key, CancellationToken token = default);

    /// <summary>
    /// Refreshes the expiracy time of the object in Cache
    /// </summary>
    /// <param name="key">Key value of the object to search for in Cache</param>
    void Refresh(string key);
    /// <summary>
    /// Refreshes the expiracy time of the object in Cache in asyncronous way
    /// </summary>
    /// <param name="key">Key value of the object to search for in Cache</param>
    Task RefreshAsync(string key, CancellationToken token = default);

    /// <summary>
    /// Removes the Object from Cache
    /// </summary>
    /// <param name="key">>Key value of the object to delete for in Cache</param>
    void Remove(string key);
    /// <summary>
    /// Removes the Object from Cache in asyncronous way
    /// </summary>
    /// <param name="key">>Key value of the object to delete for in Cache</param>
    Task RemoveAsync(string key, CancellationToken token = default);

    /// <summary>
    /// Inserts a <c>T</c> object into Cache
    /// </summary>
    /// <typeparam name="T">Type of the Object to insert</typeparam>
    /// <param name="key">Key value of the object to insert into Cache</param>
    /// <param name="value">The object</param>
    /// <param name="slidingExpiration">Slinding expiracy time for the object. Wich refreshes after being used</param>
    /// <param name="absoluteExpiration">Absolute expiracy time for the object. Overrides <c>slidingExpiration</c> time</param>
    void Set<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null);
    /// <summary>
    /// Inserts a <c>T</c> object into Cache in asyncronous way
    /// </summary>
    /// <typeparam name="T">Type of the Object to insert</typeparam>
    /// <param name="key">Key value of the object to insert into Cache</param>
    /// <param name="value">The object</param>
    /// <param name="slidingExpiration">Slinding expiracy time for the object. Wich refreshes after being used</param>
    /// <param name="absoluteExpiration">Absolute expiracy time for the object. Overrides <c>slidingExpiration</c> time</param>
    Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default);
}