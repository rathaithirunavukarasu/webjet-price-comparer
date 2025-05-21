using WebjetPriceComparer.Domain.Enums;

namespace WebjetPriceComparer.Application.Dtos
{
    /// <summary>
    /// Data Transfer Object representing a provider and its price for a movie.
    /// </summary>
    public class ProviderPriceDto
    {
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the price offered by the provider.
        /// </summary>
        public string Price { get; set; }
    }
}
