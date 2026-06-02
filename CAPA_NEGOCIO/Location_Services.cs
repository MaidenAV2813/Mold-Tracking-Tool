using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class Location_Services : ILocation_Services

    {

        private readonly IDataAccess sql;

        public Location_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud

        //Metodo Get

        public async Task<IEnumerable<LocationEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<LocationEntity>("sp_Location_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<LocationEntity> GetById(LocationEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<LocationEntity>("", new

                { entity.LocationID });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Create

        public async Task<DBEntity> Create(LocationEntity entity)
        {
            var result = sql.ExecuteAsync("sp_Location_Insert", new
            {
                entity.LocationNumber,
                entity.LocationStatus,
                entity.CreatedBy,
                entity.DateCreation,
                entity.DateModification,
                entity.ModifiedBy
            });

            return await result;
        }

        //Metodo Update

        public async Task<DBEntity> Update(LocationEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("sp_Location_Update", new

                {
                    entity.LocationID,
                    entity.LocationNumber,
                    entity.LocationStatus,
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
        public async Task<DBEntity> Delete(LocationEntity entity)

        {
            try

            {
                var result = sql.ExecuteAsync("", new
                {
                    entity.LocationID,
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
