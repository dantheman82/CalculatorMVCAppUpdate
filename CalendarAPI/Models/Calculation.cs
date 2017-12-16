using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CalculatorAPI.Models
{
    public class Calculation
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Operator { get; set; }
        public string Operand1 { get; set; }
        public string Operand2 { get; set; }
        public string Result { get; set; }
        public override string ToString()
        {
            return string.Format(Operand1 + " " + Operator + " " + Operand2);
        }
    }
}