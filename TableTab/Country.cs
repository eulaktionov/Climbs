using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

using static Climbs.Properties.Resources;


namespace Climbs.TableTab
{
    internal class Country : Base
    {
        public Country(SqlConnection connection) : base(connection, CountriesTable)
        { 
        }
    }

    internal class Mountain : Base
    {
        public Mountain(SqlConnection connection) : base(connection, MountainsTable)
        {
        }
    }

    internal class Climb : Base
    {
        public Climb(SqlConnection connection) : base(connection, ClimbsTable)
        {
        }
    }

    internal class Climbers : Base
    {
        public Climbers(SqlConnection connection) : base(connection, ClimbersTable)
        {
        }
    }

    internal class ClimbClimbers : Base
    {
        public ClimbClimbers(SqlConnection connection) : base(connection, ClimbClimbersTable)
        {
        }
    }
}
