using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebHooks.Data.Extensions
{
    /// <summary>
    /// JSON值转换器
    /// </summary>
    public static class JsonValueConversionExtension
    {
        /// <summary>
        /// 设置JSON值转换
        /// </summary>
        /// <typeparam name="TProvider"></typeparam>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public static PropertyBuilder<TProvider> HasJsonConversion<TProvider>(this PropertyBuilder<TProvider> propertyBuilder) 
        {
            var serializeOption = new JsonSerializerOptions();
            return propertyBuilder.HasConversion(
                x => JsonSerializer.Serialize(x, serializeOption),
                y => JsonSerializer.Deserialize<TProvider>(y, serializeOption) ?? default!);
        }
    }
}
