using System.Text.Json.Serialization;

namespace TT.Application.Common.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortOrder
{
    ASC,
    DESC
}