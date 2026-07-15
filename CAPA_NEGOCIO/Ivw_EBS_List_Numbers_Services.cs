using System.Collections.Generic;
using System.Threading.Tasks;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface Ivw_EBS_List_Numbers_Services
    {
        Task<IEnumerable<vw_EBS_List_NumbersEntity>> 
            GetByListnumber(string listnumber);
    }
}
