using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Actuator_Services : IActuator_Services

    {

        private readonly IDataAccess sql;

        public Actuator_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        //Metodo Get

        public async Task<IEnumerable<ActuatorTypeEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<ActuatorTypeEntity>("sp_ActuatorType_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<ActuatorTypeEntity?> GetById(ActuatorTypeEntity entity)
        {
            var result = await sql.QueryFirstAsync<ActuatorTypeEntity>("sp_Actuator_GetById",new
                {
                    ActuatorID = entity.ActuatorID
                });

            return result;
        }

        //Metodo Create

        public async Task<DBEntity> Create(ActuatorTypeEntity entity)
        {
            var result = sql.ExecuteAsync("sp_ActuatorType_Insert", new
            {
                entity.ActuatorType,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy,
                entity.ActuatorStatus
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(ActuatorTypeEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Actuator_Update", new

                {
                    entity.ActuatorID,
                    entity.ActuatorStatus,
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
        //public async Task<DBEntity> Delete(ActuatorTypeEntity entity)

        //{
        //    try

        //    {
        //        var result = sql.ExecuteAsync("sp_ActuatorType_DeleteByGateID", new
        //        {
        //            entity.ActuatorID,
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
