using CalculatorAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace CalculatorAPI.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EFContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CalculatorAPI.Models.EFContext";
        }

        protected override void Seed(EFContext context)
        {
            context.Calculations.AddOrUpdate(
              p => p.Id,
              new Calculation { Operator = "+", Operand1 = "0", Operand2 = "0" }
            );
        }
    }
}