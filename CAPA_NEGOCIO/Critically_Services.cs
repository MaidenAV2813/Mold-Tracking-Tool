using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Critically_Services : ICritically_Services

    {

        private readonly IDataAccess sql;

        public Critically_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        //Metodo Get

        public async Task<IEnumerable<CriticallyMoldEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<CriticallyMoldEntity>("sp_Critically_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<CriticallyMoldEntity?> GetById(CriticallyMoldEntity entity)
        {
            var result = await sql.QueryFirstAsync<CriticallyMoldEntity>(
                "sp_Critically_GetById",
                new
                {
                    CriticallyID = entity.CriticallyID
                });

            return result;
        }

        //Metodo Create

        public async Task<DBEntity> Create(CriticallyMoldEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Critically_Insert", new
            {
                entity.CriticallyType,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy,
                entity.CriticallyStatus
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(CriticallyMoldEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Critically_Update", new

                {
                    entity.CriticallyID,
                    entity.CriticallyStatus,
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
        //public async Task<DBEntity> Delete(CriticallyMoldEntity entity)

        //{
        //    try

        //    {
        //        var result = sql.ExecuteAsync("sp_Critically_DeleteByGateID", new
        //        {
        //            entity.CriticallyID,
        //        });

        //        return await result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        
    }

}
