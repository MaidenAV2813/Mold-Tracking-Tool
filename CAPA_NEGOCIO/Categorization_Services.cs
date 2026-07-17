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

        //public async Task<GateTypeEntity> GetById(GateTypeEntity entity)

        //{
        //    try

        //    {
        //        var result = sql.QueryFirstAsync<GateTypeEntity>("sp_Gates_GetById", new

        //        { entity.GateID });

        //        return await result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //Metodo Create

        public async Task<DBEntity> Create(CategorizationMoldEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Categorization_Insert", new
            {
                entity.CategorizationType,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy
            });

            return await result;
        }

        //Metodo Update

        //public async Task<DBEntity> Update(GateTypeEntity entity)

        //{
        //    try

        //    {
        //        var result = sql.ExecuteAsync("sp_Gates_Update", new

        //        {
        //            entity.GateID,
        //            entity.GateType,
        //            entity.DateModification,
        //            entity.ModifiedBy
        //        });

        //        return await result;
        //    }

        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //Metodo Delete
        public async Task<DBEntity> Delete(CategorizationMoldEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Categorization_DeleteByGateID", new
                {
                    entity.CategorizationID,
                });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }

}
