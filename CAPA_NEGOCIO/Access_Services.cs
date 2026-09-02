using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Access_Services : IAccess_Services

    {

        private readonly IDataAccess sql;

        public Access_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud

        //Metodo Get


        public async Task<IEnumerable<AccessEntity>> Get()

        {

            try

            {

                var result = sql.QueryAsync<AccessEntity>("sp_Access_List");

                return await result;

            }

            catch (Exception)

            {

                throw;

            }


        }

        //Metodo GetById

        public async Task<AccessEntity> GetById(AccessEntity entity)

        {

            try

            {

                var result = sql.QueryFirstAsync<AccessEntity>("sp_Access_GetById", new

                { entity.AccessID });

                return await result;

            }

            catch (Exception)

            {

                throw;

            }

        }

        //Metodo Create

        public async Task<DBEntity> Create(AccessEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Access_Insert", new
            {
                entity.RolID,
                entity.AccessDescription,
                entity.DateCreation,
                entity.DateModification,
                entity.CreatedBy,
                entity.ModifiedBy
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(AccessEntity entity)

        {

            try

            {

                var result = sql.ExecuteAsync("sp_Access_Update", new

                {

                    entity.RolID,
                    entity.AccessDescription,
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

        public async Task<DBEntity> DeleteByRol(AccessEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Access_DeleteByRol", new
            {
                entity.RolID
            });

            return await result;
        }

        public async Task<DBEntity> Delete(AccessEntity entity)
        {
            return await Task.FromResult(new DBEntity());
        }


        #endregion

    }


}
