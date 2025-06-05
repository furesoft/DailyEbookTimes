using PolyType;

namespace Moss.NET.Sdk.DataSources.Crypto;

[GenerateShape]
public partial class CryptoPrice
{
    [PropertyShape(Name = "id")] public string Id { get; set; } = string.Empty;
    [PropertyShape(Name = "symbol")] public string Symbol { get; set; } = string.Empty;
    [PropertyShape(Name = "name")] public string Name { get; set; } = string.Empty;
    [PropertyShape(Name = "image")] public string Image { get; set; } = string.Empty;
    [PropertyShape(Name = "current_price")] public decimal CurrentPrice { get; set; }
    [PropertyShape(Name = "market_cap")] public decimal? MarketCap { get; set; }
    [PropertyShape(Name = "market_cap_rank")] public int MarketCapRank { get; set; }

    [PropertyShape(Name = "fully_diluted_valuation")]
    public decimal FullyDilutedValuation { get; set; }

    [PropertyShape(Name = "total_volume")] public decimal? TotalVolume { get; set; }
    [PropertyShape(Name = "high_24h")] public decimal? High24h { get; set; }
    [PropertyShape(Name = "low_24h")] public decimal? Low24h { get; set; }
    [PropertyShape(Name = "price_change_24h")] public decimal? PriceChange24h { get; set; }

    [PropertyShape(Name = "price_change_percentage_24h")]
    public decimal? PriceChangePercentage24h { get; set; }

    [PropertyShape(Name = "market_cap_change_24h")]
    public decimal? MarketCapChange24h { get; set; }

    [PropertyShape(Name = "market_cap_change_percentage_24h")]
    public decimal? MarketCapChangePercentage24h { get; set; }

    [PropertyShape(Name = "circulating_supply")]
    public decimal? CirculatingSupply { get; set; }

    [PropertyShape(Name = "total_supply")] public decimal? TotalSupply { get; set; }
    [PropertyShape(Name = "max_supply")] public decimal? MaxSupply { get; set; }
    [PropertyShape(Name = "ath")] public decimal? Ath { get; set; }

    [PropertyShape(Name = "ath_change_percentage")]
    public decimal? AthChangePercentage { get; set; }

    [PropertyShape(Name = "ath_date")] public DateTime AthDate { get; set; }
    [PropertyShape(Name = "atl")] public decimal? Atl { get; set; }

    [PropertyShape(Name = "atl_change_percentage")]
    public decimal? AtlChangePercentage { get; set; }

    [PropertyShape(Name = "atl_date")] public DateTime AtlDate { get; set; }

    [PropertyShape(Name = "last_updated")] public DateTime LastUpdated { get; set; }
}

