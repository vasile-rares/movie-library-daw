using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.Attributes;

public class SafeUrlAttribute : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
    {
      return ValidationResult.Success; // Allow null/empty, use [Required] separately
    }

    var url = value.ToString()!;

    // Check if URL is valid
    if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
    {
      return new ValidationResult("Invalid URL format.");
    }

    // Only allow HTTP and HTTPS schemes
    if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
    {
      return new ValidationResult("Only HTTP and HTTPS URLs are allowed.");
    }

    // Check for dangerous patterns
    var lowerUrl = url.ToLowerInvariant();
    if (lowerUrl.Contains("javascript:") || lowerUrl.Contains("data:") || lowerUrl.Contains("vbscript:"))
    {
      return new ValidationResult("URL contains potentially dangerous content.");
    }

    return ValidationResult.Success;
  }
}
