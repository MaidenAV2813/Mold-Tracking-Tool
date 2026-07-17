using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;

namespace CAPA_NEGOCIO
{
    public class Mold_Services : IMold_Services

    {

        private readonly IDataAccess sql;

        public Mold_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud


        //Metodo Get

        public async Task<IEnumerable<MoldEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<MoldEntity>("sp_Mold_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<MoldEntity> GetById(MoldEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<MoldEntity>("sp_Mold_SelectById", new

                { entity.MoldID });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Create

        public async Task<DBEntity> Create(MoldEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Mold_Insert", new
            {
                entity.CriticallyID,
                entity.GateID,
                entity.CastingID,
                entity.ActuatorID,
                entity.CategorizationID,
                entity.MoldAssetNumber,
                entity.MoldNumber,
                entity.MoldDescription,
                entity.MoldStatus,
                entity.MoldOrigin,
                entity.DigitalPlane,
                entity.CavityQty,
                entity.BlockCavityQty,
                entity.HaveCounter,
                entity.CounterType,
                entity.ThreeLayer,
                entity.InitialCount,
                entity.DateCreation,
                entity.DateModification,
                entity.CreatedBy,
                entity.ModifiedBy

            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(MoldEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Mold_Update", new

                {
                    entity.MoldID,
                    entity.CriticallyID,
                    entity.GateID,
                    entity.CastingID,
                    entity.ActuatorID,
                    entity.CategorizationID,
                    entity.MoldAssetNumber,
                    entity.MoldNumber,
                    entity.MoldDescription,
                    entity.MoldStatus,
                    entity.MoldOrigin,
                    entity.DigitalPlane,
                    entity.CavityQty,
                    entity.BlockCavityQty,
                    entity.HaveCounter,
                    entity.CounterType,
                    entity.ThreeLayer,
                    entity.InitialCount,
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
        public async Task<DBEntity> Delete(MoldEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Mold_Update", new
                {
                    entity.MoldID,
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
