using System.ComponentModel;
using System.Reflection;
using TT.Application.Common.Enums;

namespace TT.Application.Common.Extensions;

public static class EnumExtension
{
    public static string GetEnumDescription(this Enum currency)
    {
        var fieldInfo = currency.GetType().GetField(currency.ToString())!;
        var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        return attribute != null ? attribute.Description : currency.ToString();
    }
}