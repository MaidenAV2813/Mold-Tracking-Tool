using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;


namespace CAPA_NEGOCIO
{
    public class Users_Services : IUsers_Services

    {
        private readonly IDataAccess sql;

        public Users_Services(IDataAccess _sql)

        {
            sql = _sql;
        }

        #region MetodosCrud

        //Metodo Get

        public async Task<IEnumerable<UserEntity>> Get()

        {
            try

            {
                var result = sql.QueryAsync<UserEntity>("sp_User_List");

                return await result;
            }

            catch (Exception)

            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<UserEntity> GetById(UserEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<UserEntity>("sp_User_GetById", new

                { entity.UserID });

                return await result;
            }

            catch (Exception)

            {
                throw;
            }
        }

        //Metodo Create
        public async Task<DBEntity> Create(UserEntity entity)
        {
            var result = sql.ExecuteAsync("sp_User_Insert", new
            {
                entity.RolID,
                entity.Username,
                entity.EmpName,
                entity.UserStatus,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(UserEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_User_Update", new
                {
                    entity.UserID,
                    entity.RolID,
                    entity.Username,
                    entity.EmpName,
                    entity.UserStatus,
                    entity.DateModification,
                    entity.ModifiedBy
                });
                return await result;
            }

            catch (Exception)

            {
                throw;
            }
        }

        //Metodo Delete

        public async Task<DBEntity> Delete(UserEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("", new

                {
                    entity.UserID,

                });

                return await result;
            }

            catch (Exception)

            {
                throw;
            }
        }
        #endregion
    }
}
