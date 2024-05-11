﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.StaticWebAssets.Tasks;
using Moq;

namespace Microsoft.NET.Sdk.Razor.Tests.StaticWebAssets;

public class StaticWebAssetPathPatternTest
{
    [Fact]
    public void CanParse_PathWithNoExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site.css", "MyApp");
        var expected = new StaticWebAssetPathPattern("css/site.css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "css/site.css", IsLiteral = true }] }
            ]
        };
        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexFingerprintExpression_Middle()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("css/site#[.{fingerprint}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "css/site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexFingerprintExpression_Start()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[.{fingerprint}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexFingerprintExpression_End()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]", "MyApp");
        var expected = new StaticWebAssetPathPattern("site#[.{fingerprint}]")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexFingerprintExpression_Only()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}]", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[.{fingerprint}]")
        {
            Segments = [
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexFingerprintExpression_Multiple()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}]-#[.{version}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("css/site#[.{fingerprint}]-#[.{version}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "css/site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = "-", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "version", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexFingerprintExpression_ConsecutiveExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}]#[.{version}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("css/site#[.{fingerprint}]#[.{version}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "css/site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "version", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_SimpleFingerprintExpression_Start()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[{fingerprint}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[{fingerprint}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "fingerprint", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_SimpleFingerprintExpression_Middle()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[{fingerprint}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("css/site#[{fingerprint}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "css/site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = "fingerprint", IsLiteral = false }] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_SimpleFingerprintExpression_End()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[{fingerprint}]", "MyApp");
        var expected = new StaticWebAssetPathPattern("site#[{fingerprint}]")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = "fingerprint", IsLiteral = false }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_SimpleFingerprintExpression_Only()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[{fingerprint}]", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[{fingerprint}]")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "fingerprint", IsLiteral = false }] }
                ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexExpression_MultipleVariables()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}-{version}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("css/site#[.{fingerprint}-{version}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "css/site", IsLiteral = true }] },
                new (){ Parts = [
                    new() { Value = ".", IsLiteral = true },
                    new() { Value = "fingerprint", IsLiteral = false },
                    new() { Value = "-", IsLiteral = true },
                    new() { Value = "version", IsLiteral = false }
                ] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexExpression_MultipleConsecutiveVariables()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}{version}].css", "MyApp");
        var expected = new StaticWebAssetPathPattern("css/site#[.{fingerprint}{version}].css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "css/site", IsLiteral = true }] },
                new (){ Parts = [
                    new() { Value = ".", IsLiteral = true },
                    new() { Value = "fingerprint", IsLiteral = false },
                    new() { Value = "version", IsLiteral = false }
                ] },
                new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ComplexExpression_StartsWithVariable()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[{fingerprint}.]css", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[{fingerprint}.]css")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "fingerprint", IsLiteral = false }, new() { Value = ".", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = "css", IsLiteral = true }] }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_OptionalExpressions_End()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]?", "MyApp");
        var expected = new StaticWebAssetPathPattern("site#[.{fingerprint}]?")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }], IsOptional = true }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_OptionalPreferredExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]!", "MyApp");
        var expected = new StaticWebAssetPathPattern("site#[.{fingerprint}]!")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }], IsOptional = true, IsPreferred = true }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_OptionalExpressions_Start()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}]?site", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[.{fingerprint}]?site")
        {
            Segments = [
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }], IsOptional = true },
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }]
            }]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_OptionalExpressions_Middle()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]?site", "MyApp");
        var expected = new StaticWebAssetPathPattern("site#[.{fingerprint}]?site")
        {
            Segments = [
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }], IsOptional = true },
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }]
                           }]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_OptionalExpressions_Only()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}]?", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[.{fingerprint}]?")
        {
            Segments = [
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }], IsOptional = true }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_MultipleOptionalExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}]?site#[.{version}]?", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[.{fingerprint}]?site#[.{version}]?")
        {
            Segments = [
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }], IsOptional = true },
                new (){ Parts = [ new() { Value = "site", IsLiteral = true }], IsOptional = false },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "version", IsLiteral = false }], IsOptional = true }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanParse_ConsecutiveOptionalExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}]?#[.{version}]?", "MyApp");
        var expected = new StaticWebAssetPathPattern("#[.{fingerprint}]?#[.{version}]?")
        {
            Segments = [
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }], IsOptional = true },
                new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "version", IsLiteral = false }], IsOptional = true }
            ]
        };

        Assert.Equal(expected, pattern);
    }

    [Fact]
    public void CanReplaceTokens_PathWithNoExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site.css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("css/site.css", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexFingerprintExpression_Middle()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("css/site.asdf1234.css", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexFingerprintExpression_Start()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal(".asdf1234.css", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexFingerprintExpression_End()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("site.asdf1234", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexFingerprintExpression_Only()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[.{fingerprint}]", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal(".asdf1234", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexFingerprintExpression_Multiple()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}]-#[.{version}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234",
        };
        var path = pattern.ReplaceTokens(
            tokens,
            CreateTestResolver(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["version"] = "v1" }));

        Assert.Equal("css/site.asdf1234-.v1.css", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexFingerprintExpression_ConsecutiveExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}]#[.{version}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234",
        };
        var path = pattern.ReplaceTokens(
            tokens,
            CreateTestResolver(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["version"] = "v1" }));

        Assert.Equal("css/site.asdf1234.v1.css", path);
    }

    [Fact]
    public void CanReplaceTokens_SimpleFingerprintExpression_Start()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[{fingerprint}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("asdf1234.css", path);
    }

    [Fact]
    public void CanReplaceTokens_SimpleFingerprintExpression_Middle()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[{fingerprint}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("css/siteasdf1234.css", path);
    }

    [Fact]
    public void CanReplaceTokens_SimpleFingerprintExpression_End()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[{fingerprint}]", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("siteasdf1234", path);
    }

    [Fact]
    public void CanReplaceTokens_SimpleFingerprintExpression_Only()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[{fingerprint}]", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("asdf1234", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexExpression_MultipleVariables()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}-{version}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234",
        };
        var path = pattern.ReplaceTokens(
            tokens,
            CreateTestResolver(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["version"] = "v1" }));

        Assert.Equal("css/site.asdf1234-v1.css", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexExpression_MultipleConsecutiveVariables()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}{version}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234",
        };
        var path = pattern.ReplaceTokens(
            tokens,
            CreateTestResolver(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["version"] = "v1" }));

        Assert.Equal("css/site.asdf1234v1.css", path);
    }

    [Fact]
    public void CanReplaceTokens_ComplexExpression_StartsWithVariable()
    {
        var pattern = StaticWebAssetPathPattern.Parse("#[{fingerprint}.]css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("asdf1234.css", path);
    }

    [Fact]
    public void CanReplaceTokens_ThrowsException_IfRequiredExpressionIsValue()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}].css", "MyApp");
        var tokens = new StaticWebAsset();
        var exception = Assert.Throws<InvalidOperationException>(() => pattern.ReplaceTokens(tokens, CreateTestResolver()));
        Assert.Equal("Token 'fingerprint' not provided for 'css/site#[.{fingerprint}].css'.", exception.Message);
    }

    [Fact]
    public void CanReplaceTokens_ThrowsException_MultipleTokenComplexExpression_MissingAtLeastOneValue()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}-{version}].css", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var exception = Assert.Throws<InvalidOperationException>(() => pattern.ReplaceTokens(tokens, CreateTestResolver()));
        Assert.Equal("Token 'version' not provided for 'css/site#[.{fingerprint}-{version}].css'.", exception.Message);
    }

    [Fact]
    public void CanReplaceTokens_OptionalExpression_OmittedWhenValueNotProvided()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]?", "MyApp");
        var tokens = new StaticWebAsset();
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("site", path);
    }

    [Fact]
    public void CanReplaceTokens_OptionalMultipleTokenComplexExpression_OmittedWhenMissingAtLeastOneValue()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}.{version}]?", "MyApp");
        var tokens = new StaticWebAsset
        {
            Fingerprint = "asdf1234"
        };
        var path = pattern.ReplaceTokens(tokens, CreateTestResolver());

        Assert.Equal("site", path);
    }

    [Fact]
    public void CanExpandRoutes_LiteralPatterns()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site.css", "MyApp");
        var routePatterns = pattern.ExpandRoutes();

        Assert.Equal([pattern], routePatterns);
    }

    [Fact]
    public void CanExpandRoutes_SingleRequiredExpression()
    {
        var pattern = StaticWebAssetPathPattern.Parse("css/site#[.{fingerprint}].css", "MyApp");
        var routePatterns = pattern.ExpandRoutes();

        Assert.Equal([pattern], routePatterns);
    }

    [Fact]
    public void CanExpandRoutes_SingleOptionalExpression()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]?.css", "MyApp");
        var routePatterns = pattern.ExpandRoutes();

        var expected = new[]
        {
            new StaticWebAssetPathPattern("site.css")
            {
                Segments =
                [
                    new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                    new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
            },
            new StaticWebAssetPathPattern("site#[.{fingerprint}].css")
            {
                Segments =
                [
                    new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                    new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] },
                    new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
            }
        };

        Assert.Equal(expected, routePatterns);
    }

    [Fact]
    public void CanExpandRoutes_MultipleOptionalExpressions()
    {
        var pattern = StaticWebAssetPathPattern.Parse("site#[.{fingerprint}]?#[.{version}]?.css", "MyApp");
        var routePatterns = pattern.ExpandRoutes();

        var expected = new[]
        {
            new StaticWebAssetPathPattern("site.css")
            {
                Segments =
                [
                    new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                    new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
            },
            new StaticWebAssetPathPattern("site#[.{fingerprint}].css")
            {
                Segments =
                [
                    new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                    new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] },
                    new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
            },
            new StaticWebAssetPathPattern("site#[.{version}].css")
            {
                Segments =
                [
                    new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                    new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "version", IsLiteral = false }] },
                    new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
            },
            new StaticWebAssetPathPattern("site#[.{fingerprint}]#[.{version}].css")
            {
                Segments =
                [
                    new (){ Parts = [ new() { Value = "site", IsLiteral = true }] },
                    new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "fingerprint", IsLiteral = false }] },
                    new (){ Parts = [ new() { Value = ".", IsLiteral = true }, new() { Value = "version", IsLiteral = false }] },
                    new (){ Parts = [ new() { Value = ".css", IsLiteral = true }] }
                ]
            }
        };

        Assert.Equal(expected, routePatterns);
    }

    private StaticWebAssetTokenResolver CreateTestResolver(Dictionary<string, string> additionalTokens = null) => new(additionalTokens);
}
