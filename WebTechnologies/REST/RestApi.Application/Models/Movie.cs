﻿using System.Text.RegularExpressions;

namespace RestApi.Application.Models;

public partial class Movie
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public string Slug => GenerateSlug();

    private string GenerateSlug()
    {
        var slugTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{slugTitle}-{YearOfRelease}";
    }

    public required int YearOfRelease { get; set; }
    public required List<string> Genres { get; init; } = new();

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}
