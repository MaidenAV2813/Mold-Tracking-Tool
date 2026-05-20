using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Gates_Services : IGates_Services

    {

        private readonly IDataAccess sql;

        public Gates_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        //Metodo Get

        public async Task<IEnumerable<GateTypeEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<GateTypeEntity>("sp_Gates_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<GateTypeEntity> GetById(GateTypeEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<GateTypeEntity>("sp_Gates_GetById", new

                { entity.GateID });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Create

        public async Task<DBEntity> Create(GateTypeEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Gates_Insert", new
            {
                entity.GateType,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(GateTypeEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Gates_Update", new

                {
                    entity.GateID,
                    entity.GateType,
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
        public async Task<DBEntity> Delete(GateTypeEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("", new
                {
                    entity.GateID,
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
