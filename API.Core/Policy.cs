using API.Core.Validators;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core
{
    [Validator(typeof(PolicyValidator))]
    public class Policy
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TypeCover TypeCover { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TypeRisk TypeRisk { get; set; }
        public decimal PercentageCoverage { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public decimal Price { get; set; }
    }

    public enum TypeCover
    {
        Earthquake,
        Fire,
        Stole,
        Lost,
        Others
    }

    public enum TypeRisk
    {
        Low,
        Medium,
        MediumHigh,
        High,
    }
}
