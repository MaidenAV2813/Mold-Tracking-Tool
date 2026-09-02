using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class InventoryBOH_Services : IInventoryBOH_Services

    {

        private readonly IDataAccess sql;

        public InventoryBOH_Services(IDataAccess _sql)

        {

            sql = _sql;

        }

        #region MetodosCrud


        //Metodo Get

        public async Task<IEnumerable<InventoryBOHEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<InventoryBOHEntity>("sp_InventoryBOH_List");

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo GetById

        public async Task<InventoryBOHEntity> GetById(InventoryBOHEntity entity)

        {
            try

            {
                var result = sql.QueryFirstAsync<InventoryBOHEntity>("", new

                { entity.BOHID });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo Create

        public async Task<DBEntity> Create(InventoryBOHEntity entity)
        {
            var result = sql.ExecuteAsync("sp_InventoryBOH_Insert", new
            {
                entity.ItemNumberID,
                entity.ItemNumber,
                entity.LocationID,
                entity.LocationNumber,
                entity.QtyOnHand,
                entity.UOM,
                entity.ItemStatus,
                entity.ActualSupplier,
                entity.DateCreation,
                entity.DateModification,
                entity.CreatedBy,
                entity.ModifiedBy

            });

            return await result;
        }

        #endregion
    }

}
