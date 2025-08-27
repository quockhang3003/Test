using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    [Flags]
    public enum Permission : long
    {
        none = 0,

        CreateApplication = 1L << 0,
        ViewAdminPage = 1L << 1,
        ManagePermission = 1L << 2,


        basesicUser = CreateApplication,
        admin = ViewAdminPage | ManagePermission
    }
    public static class HasPermission
    {
        public static bool Has(this long owned, Permission required) => (owned & (long)required) == (long)required;
    }
}
