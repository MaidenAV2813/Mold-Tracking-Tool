using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IUsers_Services
    {
        Task<IEnumerable<UserEntity>> Get();
        Task<UserEntity> GetById(UserEntity entity);
        Task<DBEntity> Create(UserEntity entity);
        Task<DBEntity> Update(UserEntity entity);
        Task<DBEntity> Delete(UserEntity entity);
        
       
    }
}
