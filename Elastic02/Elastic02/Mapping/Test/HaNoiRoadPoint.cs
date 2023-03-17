using Elastic02.Models.Test;
using Nest;

namespace Elastic02.Mapping.Test
{
    public static partial class Mapping
    {
        public static CreateIndexDescriptor HaNoiRoadPointMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<HaNoiRoadPoint>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.id))
                .Text(t => t.Name(n => n.name))
                .Text(t => t.Name(n => n.extend))
                .Text(t => t.Name(n => n.keywords))
                .Number(t => t.Name(n => n.lat))
                .Number(t => t.Name(n => n.lng))
                .GeoPoint(t => t.Name(n => n.location))
                //.GeoShape(t => t.Name(n ))
                )

            );
        }

        public static IndexSettingsDescriptor MapIndexSettings(this IndexSettingsDescriptor descriptor)
        {
            return new IndexSettingsDescriptor()
            .Analysis(a => a
                .CharFilters(cf => cf
                    .Mapping("programming_language", mca => mca
                        .Mappings(new[]
                        {
                            "c# => csharp",
                            "C# => Csharp"
                        })
                    )
                )
                .TokenFilters(tf => tf
                    .AsciiFolding("ascii_folding", tk => new AsciiFoldingTokenFilter
                    {
                        PreserveOriginal = true
                    })
                )
                .Analyzers(an => an
                    .Custom("my_analyzer", ca => ca
                        .CharFilters("programming_language")
                        .Tokenizer("standard")
                        .Filters("lowercase")
                    )
                    .Custom("vi_analyzer", ca => ca
                        .CharFilters("programming_language")
                        .Tokenizer("vi_tokenizer")
                        .Filters("lowercase", "icu_folding", "ascii_folding")
                    )
                    .Custom("my_keyword_analyzer", ca => ca
                        .Tokenizer("keyword")
                        .Filters("lowercase")
                    )
                    .Custom("my_combined_analyzer", ca => ca
                        .Tokenizer("standard")
                        .Filters("lowercase", "my_keyword_analyzer", "vi_analyzer")
                    )
                )
            );
        }

    }

}
