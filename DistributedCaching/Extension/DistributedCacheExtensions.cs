using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace DistributedCaching.Extension
{
    public static class DistributedCacheExtensions
    {
        // Serializer options to customize the JSON serialization and deserialization behavior.
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null, 
            WriteIndented = true,
            AllowTrailingCommas = true, 
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull 
        };

        /// <summary>
        /// Sets an object in the distributed cache using default expiration settings.
        /// </summary>
        /// <typeparam name="T">The type of the object to cache.</typeparam>
        /// <param name="cache">The distributed cache instance.</param>
        /// <param name="key">The key under which the object will be stored.</param>
        /// <param name="value">The object to store in the cache.</param>
        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
        {
            return SetAsync(cache, key, value, new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)) 
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }

        /// <summary>
        /// Sets an object in the distributed cache with specified expiration settings.
        /// </summary>
        /// <typeparam name="T">The type of the object to cache.</typeparam>
        /// <param name="cache">The distributed cache instance.</param>
        /// <param name="key">The key under which the object will be stored.</param>
        /// <param name="value">The object to store in the cache.</param>
        /// <param name="options">Cache entry options that define expiration behavior.</param>
        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            // Serialize the object to JSON and convert it to a byte array.
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, serializerOptions));
            return cache.SetAsync(key, bytes, options);
        }

        /// <summary>
        /// Tries to get a value from the distributed cache.
        /// </summary>
        /// <typeparam name="T">The expected type of the cached value.</typeparam>
        /// <param name="cache">The distributed cache instance.</param>
        /// <param name="key">The key under which the object is stored.</param>
        /// <param name="value">The output parameter that will hold the deserialized object if found.</param>
        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
        {
            var val = cache.Get(key);
            value = default;

            if (val == null) return false;

            value = JsonSerializer.Deserialize<T>(val, serializerOptions);
            return true;
        }

        /// <summary>
        /// Retrieves a value from the distributed cache or sets it if it doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of the value to cache.</typeparam>
        /// <param name="cache">The distributed cache instance.</param>
        /// <param name="key">The key under which the value is stored or will be stored.</param>
        /// <param name="task">A function to retrieve the value if it's not found in the cache.</param>
        /// <param name="options">Cache entry options to define expiration behavior (optional).</param>
        public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> task, DistributedCacheEntryOptions? options = null)
        {
            if (options == null)
            {
                options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)) 
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            }

            // Attempt to get the cached value.
            if (cache.TryGetValue(key, out T? value) && value is not null)
            {
                return value;
            }

            // If the key is not found, invoke the provided function to retrieve the value.
            value = await task();

            if (value is not null)
            {
                await cache.SetAsync<T>(key, value, options);
            }
            return value;
        }
    }
}
