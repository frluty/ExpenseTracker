using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TT.Application.Common.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Currencies
{
    [Description("Euro")]
    EUR,
    [Description("Dollar américain")]
    USD,
    [Description("Livre sterling")]
    GBP,
    [Description("Yen japonais")]
    JPY,
    [Description("Franc suisse")]
    CHF,
    [Description("Dollar canadien")]
    CAD,
    [Description("Rouble russe")]
    RUB
}