using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Categorization_Services : ICategorization_Services

    {

        private readonly IDataAccess sql;

        public Categorization_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        //Metodo Get

        public async Task<IEnumerable<CategorizationMoldEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<CategorizationMoldEntity>("sp_Categorization_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<CategorizationMoldEntity?> GetById(CategorizationMoldEntity entity)
        {
            var result = await sql.QueryFirstAsync<CategorizationMoldEntity>("sp_Categorization_GetById", new
                {
                CategorizationID = entity.CategorizationID
            });

            return result;
        }

        //Metodo Create

        public async Task<DBEntity> Create(CategorizationMoldEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Categorization_Insert", new
            {
                entity.CategorizationType,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy,
                entity.CategorizationStatus
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(CategorizationMoldEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Categorization_Update", new

                {
                    entity.CategorizationID,
                    entity.CategorizationStatus,
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
        //public async Task<DBEntity> Delete(CategorizationMoldEntity entity)

        //{
        //    try

        //    {
        //        var result = sql.ExecuteAsync("sp_Categorization_DeleteByGateID", new
        //        {
        //            entity.CategorizationID,
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
