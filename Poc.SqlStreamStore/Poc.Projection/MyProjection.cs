using System;
using System.Collections.Generic;
using System.Text;
using Projac.Sql;
using Projac.SqlClient;

namespace Poc.Projection
{
    public class MyProjection   : SqlProjection
    {
        public const string FundProjectorId = "FundProjection";
        public static readonly SqlClientSyntax Sql = new SqlClientSyntax();

        public MyProjection()
        {

        }
    }
}
