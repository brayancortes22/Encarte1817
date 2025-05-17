using Data.Implements.RolUserData;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRolUserData : IBaseData<RolUser>
    {
        Task<bool> UpdatePartial(RolUser rolUser);
    }
}
